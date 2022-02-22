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

    // Room Setup
    //public Dictionary<string, int> playersDict = new Dictionary<string, int>();

    // HUD
    public TextMeshProUGUI balanceDText;
    bool escapeMenuOpen;
    public Image titleDeedObjectSprite;

    // Tab Screen
    public TextMeshProUGUI playerTabNamePrefab;
    public GameObject playerTabNameListing;
    public Canvas TabScreenCanvas;

    // Start is called before the first frame update
    void Start()
    {
        balanceDText = GameObject.Find("BalanceDText").GetComponent<TextMeshProUGUI>();
        titleDeedObjectSprite = GameObject.Find("TitleDeedObject").GetComponent<Image>();
        this.SetPlayerBalance(1500, 1); // Initializes the player's balance to $1,500 at start of game   
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        photonView = GetComponent<PhotonView>();
        userName = photonView.Owner.NickName;
        balanceDText = GameObject.Find("BalanceDText").GetComponent<TextMeshProUGUI>();
        //photonView.RPC("GetDict", RpcTarget.AllBuffered, userName, photonView.ViewID);
        TabScreenCanvas = GameObject.Find("TabScreenCanvas").GetComponent<Canvas>();
        playerTabNameListing = GameObject.Find("PlayerListing");
        GameManager.photonView.RPC("UpdatePlayerDicts", RpcTarget.AllBuffered, userName, photonView.ViewID);
        GameManager.escMenuNameText.text = $"{userName}";
        GameManager.escMenuRoomText.text = $"Room: {PhotonNetwork.CurrentRoom.Name}";
    }

    void Update()
    {
        GameManager.UpdateBalanceText();
        GameManager.UpdateTitleDeedSprite(this.currentPosition);

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

    public void UpdatePlayerTabCanvas(string newName) 
    {
        TextMeshProUGUI newPlayerTabName = Instantiate(playerTabNamePrefab, playerTabNameListing.GetComponent<Transform>());
        newPlayerTabName.text = newName;
    }

    public void PayOtherPlayer(int propertyIndex)
    {
        double rentForPayment = GameManager.propertyArray[propertyIndex].rent;
        Player transferTarget = GetObjectFromID(GameManager.playersDict[GameManager.propertyArray[propertyIndex].owner]).GetComponent<PhotonView>().Owner;
        Debug.Log(transferTarget.NickName);
        photonView.RPC("TransferMoneyTo", transferTarget, 1);
        this.balance -= rentForPayment;
        Debug.Log("Payment Complete");
        GameManager.purchaseRequestCanvas.SetActive(false);
    }

    void ShowTabListing()
    {
        TabScreenCanvas.enabled = true;
    }

    public void SetPlayerBalance(double amount, int addOrMinus) // Subroutine for changing player's balance ;; Takes amount to change and -1 or 1 to indicate add or subtract
    {
        if (addOrMinus == 1) 
        {
            balance += amount; // If 1 was passed in, add amount to balance of player
        }
        else
        {
            if (addOrMinus == -1)
            {
                balance -= amount; // If -1 was passed in, subtract amount from balance of player
            }
            else
            {
                Debug.Log("addOrMinus pass-in for SetPlayerBalance was not '-1' or '1'"); // If neither -1 or 1 was passed in, log error ||TEMP||
            }
        }
    }

    public void Move(int roll) // Subroutine to move player around the board ;; Takes amount rolled by dice, aka. how many positions to move player
    {
        if (photonView.IsMine)
        {
            // Move Player
            if (currentPosition + roll > 39) // If the future position would exceed squares on the board,
            {
                currentPosition = (currentPosition += roll) - 40; // Minus 40 from the future total in order to account for passing Go
            }
            else
            {
                currentPosition += roll; // Otherwise, just add dice roll
            }
            transform.position = GameManager.propertiesArray[currentPosition].transform.position; // Move player to position of waypoint at the new roll location

            // If statements for buying / paying 

            // If the position is owned by another player:
            if (GameManager.propertyArray[currentPosition].owned == true && GameManager.propertyArray[currentPosition].owner != photonView.Owner.NickName)
            {
                GameManager.rentCostText.text = $"This property has a rent of {GameManager.propertyArray[currentPosition].rent}"; 
                GameManager.purchReqText.text = $"\"{GameManager.propertyArray[this.currentPosition].name}\" is owned by {GameManager.propertyArray[this.currentPosition].owner}"; // Set purchase request button text 
                GameManager.showRentPay(); // Display rent screen
            }

            // If the position is owned by the current player:
            if (GameManager.propertyArray[currentPosition].owned == true && GameManager.propertyArray[currentPosition].owner == photonView.Owner.NickName)
            {
                GameManager.purchReqText.text = $"You already own \"{GameManager.propertyArray[this.currentPosition].name}\"!"; // Set purchase request button text 
                PurchaseRequestNotBuyable();
            }

            // If the position is buyable, and not owned already:
            if (GameManager.propertyArray[currentPosition].forPurchase == true && GameManager.propertyArray[currentPosition].owned == false)
            {
                if (this.balance >= GameManager.propertyArray[this.currentPosition].purchaseCost)
                {
                    GameManager.purchReqText.text = $"Would you like to purchase \"{GameManager.propertyArray[this.currentPosition].name}\"?"; // Set purchase request button text 
                    GameManager.costText.text = $"The property costs: {GameManager.propertyArray[this.currentPosition].purchaseCost}"; // Set property cost text
                    GameManager.balanceText.text = $"Your balance is: {this.balance}"; // Set balance text
                    PurchaseRequestBuyable(); // Display purchase screen
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

    public void PurchaseRequestBuyable() // Subroutine to display purchase screen
    {
        GameManager.showPurchaseRequestBuyable(); // Shows the empty parent containing all purchase screen assets
    }

    public void PurchaseRequestNotBuyable() // Subroutine to display purchase screen
    {
        GameManager.showPurchaseRequestNotBuyable(); // Shows the empty parent containing all purchase screen assets
    }

}
