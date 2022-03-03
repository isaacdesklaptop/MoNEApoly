using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class PlayerEntity : MonoBehaviour
{
    public float moveSpeed = 0f;
    public int currentPosition = 0;
    public GameManager GameManager;
    public string userName;
    public double balance;
    public TextMeshProUGUI purchReqText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI balanceText;
    public PhotonView photonView;

    public int getOutJailCardsCount;
    public bool playerInJail;
    public int rollsTakenFromJail;

    // HUD
    public TextMeshProUGUI balanceDText;
    bool escapeMenuOpen;

    // Tab Screen
    public TextMeshProUGUI playerTabNamePrefab;
    public GameObject playerTabNameListing;
    public Canvas TabScreenCanvas;

    void Start()
    {
        balanceDText = GameObject.Find("BalanceDText").GetComponent<TextMeshProUGUI>();
        this.SetPlayerBalance(1500, 1);
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        photonView = GetComponent<PhotonView>();
        userName = photonView.Owner.NickName;
        balanceDText = GameObject.Find("BalanceDText").GetComponent<TextMeshProUGUI>();
        TabScreenCanvas = GameObject.Find("TabScreenCanvas").GetComponent<Canvas>();
        playerTabNameListing = GameObject.Find("PlayerListing");
        GameManager.photonView.RPC("UpdatePlayerDicts", RpcTarget.AllBuffered, userName, photonView.ViewID);
        GameManager.escMenuNameText.text = $"{userName}";
        GameManager.escMenuRoomText.text = $"Room: {PhotonNetwork.CurrentRoom.Name}";
    }

    void Update()
    {
        GameManager.UpdateBalanceText();

        // Input Checks
        if (Input.GetKey("tab"))
        {
            TabScreenCanvas.enabled = true;
        }
        else
        {
            TabScreenCanvas.enabled = false;
        }

        if (Input.GetKeyDown("escape"))
        {
            if (escapeMenuOpen)
            {
                GameManager.ShowEscMenu();
                escapeMenuOpen = false;
            }
            else
            {
                GameManager.HideEscMenu();
                escapeMenuOpen = true;
            }
        }
    }

    public GameObject GetObjectFromID(int viewID)
    {
        GameObject playerFromID = PhotonView.Find(viewID).gameObject;
        return playerFromID;
    }

    public void UpdatePlayerTabCanvas(string newName, double balance) 
    {
        TextMeshProUGUI newPlayerTabName = Instantiate(playerTabNamePrefab, playerTabNameListing.GetComponent<Transform>());
        newPlayerTabName.text = newName;
        
        GameObject thisPlayerListing = Instantiate(GameManager.playerListingObjectPrefab, GameManager.playerListingListing.GetComponent<Transform>());
        thisPlayerListing.GetComponent<Transform>().GetChild(1).GetComponent<TextMeshProUGUI>().text = newName;
        thisPlayerListing.GetComponent<Transform>().GetChild(2).GetComponent<TextMeshProUGUI>().text = $"${balance}";
    }

    public void PayOtherPlayer(int propertyIndex)
    {
        double rentForPayment = GameManager.propertyArray[propertyIndex].rent;
        Player transferTarget = GetObjectFromID(GameManager.playersDict[GameManager.propertyArray[propertyIndex].owner]).GetComponent<PhotonView>().Owner;
        Debug.Log(transferTarget.NickName);
        photonView.RPC("TransferMoneyTo", transferTarget, rentForPayment, 1);
        this.balance -= rentForPayment;
        Debug.Log("Payment Complete");
        GameManager.purchaseRequestCanvas.SetActive(false);
    }

    void ShowTabListing()
    {
        TabScreenCanvas.enabled = true;
    }

    public void SetPlayerBalance(double amount, int addOrMinus)
    {
        if (addOrMinus == 1) 
        {
            balance += amount;
        }
        else
        {
            if (addOrMinus == -1)
            {
                balance -= amount;
            }
            else
            {
                Debug.Log("addOrMinus pass-in for SetPlayerBalance was not '-1' or '1'");
            }
        }
    }

    public void GoToJail()
    {
        transform.position = GameManager.propertiesArray[10].transform.position;
        currentPosition = 10;
        playerInJail = true;
        GameManager.doubleCounter = 0;
    }

    public void Move(int roll)
    {
        if (photonView.IsMine)
        {
            if (playerInJail && GameManager.doubleCounter == 0 && rollsTakenFromJail != 3)
            {
                rollsTakenFromJail++;
                return;
            }
            rollsTakenFromJail = 0;
           
            // Move Player
            if (currentPosition + roll > 39)
            {
                currentPosition = (currentPosition += roll) - 40;
            }
            else
            {
                currentPosition += roll;
            }
            transform.position = GameManager.propertiesArray[currentPosition].transform.position;

            // If the position is chance or community chest
            if (currentPosition == 2 || currentPosition == 17 || currentPosition == 33)
            {
                string currentChestText = $"{GameManager.communityChestTextsArray[GameManager.communityChanceManager.chestsNums[0]]}";
                GameManager.communityChanceManager.RunChestMethod(GameManager.communityChanceManager.chestsNums[0], this);
                GameManager.communityChanceManager.Reorder("chest");
                GameManager.chanceChestText.text = $"{currentChestText}";
                GameManager.ShowChanceChest();
            }

            if (currentPosition == 7 || currentPosition == 22 || currentPosition == 36)
            {
                string currentChanceText = $"{GameManager.chanceTextsArray[GameManager.communityChanceManager.chancesNus[0]]}";
                GameManager.communityChanceManager.RunChanceMethod(GameManager.communityChanceManager.chancesNus[0], this);
                GameManager.communityChanceManager.Reorder("chance");
                GameManager.chanceChestText.text = $"{currentChanceText}";
                GameManager.ShowChanceChest();
            }

            // If the position is owned by another player:
            if (GameManager.propertyArray[currentPosition].owned == true && GameManager.propertyArray[currentPosition].owner != photonView.Owner.NickName)
            {
                GameManager.rentCostText.text = $"This property has a rent of {GameManager.propertyArray[currentPosition].rent}"; 
                GameManager.purchReqText.text = $"\"{GameManager.propertyArray[this.currentPosition].name}\" is owned by {GameManager.propertyArray[this.currentPosition].owner}"; 
                GameManager.showRentPay();
            }

            // If the position is owned by the current player:
            if (GameManager.propertyArray[currentPosition].owned == true && GameManager.propertyArray[currentPosition].owner == photonView.Owner.NickName)
            {
                GameManager.purchReqText.text = $"You already own \"{GameManager.propertyArray[this.currentPosition].name}\"!";
                PurchaseRequestNotBuyable();
            }

            // If the position is buyable, and not owned already:
            if (GameManager.propertyArray[currentPosition].forPurchase == true && GameManager.propertyArray[currentPosition].owned == false)
            {
                if (this.balance >= GameManager.propertyArray[this.currentPosition].purchaseCost)
                {
                    GameManager.purchReqText.text = $"Would you like to purchase \"{GameManager.propertyArray[this.currentPosition].name}\"?";
                    GameManager.costText.text = $"The property costs: {GameManager.propertyArray[this.currentPosition].purchaseCost}";
                    GameManager.balanceText.text = $"Your balance is: {this.balance}";
                    PurchaseRequestBuyable();
                }
                else
                {
                    GameManager.purchReqText.text = $"\"{GameManager.propertyArray[this.currentPosition].name}\" is too expensive!";
                    PurchaseRequestNotBuyable();
                }
            }
        }
    }

    [PunRPC]
    void TransferMoneyTo(double amount, int addOrMinus)
    {
        Debug.Log("RPC CALLED SUCCESS");
        if (addOrMinus == -1)
        {
            balance -= amount;
        }
        if (addOrMinus == 1)
        {
            balance += amount;
        }
        Debug.Log($"should have payed player {amount}");
    }

    public void PurchaseRequestBuyable()
    {
        GameManager.showPurchaseRequestBuyable();
    }

    public void PurchaseRequestNotBuyable()
    {
        GameManager.showPurchaseRequestNotBuyable();
    }
}
