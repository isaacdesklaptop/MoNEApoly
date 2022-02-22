using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    // Logic Setup
    public PhotonView photonView;
    public int playerTurn;
    public TextMeshProUGUI rolledDisplay;
    public TextMeshProUGUI currentRollTextDisplay;
    public int myPlayerNum;
    public GameObject thisPlayer;
    public int manageDiceRoll;

    // Board Setup
    public GameObject[] propertiesArray = new GameObject[40];
    public Property[] propertyArray = new Property[40];
    public Sprite[] titleDeedSpriteArray = new Sprite[40];

    // Room
    public Dictionary<string, int> playersDict = new Dictionary<string, int>();

    // PurchaseRequest   
    public GameObject purchaseRequestCanvas;
    public TextMeshProUGUI purchReqText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI balanceText;
    public GameObject purchaseRequestBuyableCanvas;
    public GameObject purchaseRequestNotBuyableCanvas;
    public GameObject purchaseRequestRentCanvas;
    public TextMeshProUGUI rentCostText;
    // TitleDeed display
    public GameObject titleDeedListing;
    public Image titleDeedObjectSprite;

    // HUD
    public TextMeshProUGUI balanceDText;
    public GameObject escMenu;
    public TextMeshProUGUI escMenuNameText;
    public TextMeshProUGUI escMenuRoomText;

    // Dice
    //public Image diceSprite;
    public GameObject dice;

    // Start is called before the first frame update
    void Start()
    {
        thisPlayer = GameObject.Find("Player Object(Clone)");
        photonView = GetComponent<PhotonView>();
        DisableDice();
        LoadProperties(propertyArray);
        hidePurchaseRequest();
        HideEscMenu();
        playerTurn = 1;
        myPlayerNum = thisPlayer.GetComponent<PhotonView>().ViewID / 1000;
        CallTurnRPC();
        currentRollTextDisplay.text = $"{thisPlayer.GetComponent<PlayerEntity>().GetObjectFromID((playerTurn * 1000) + 1).GetComponent<PlayerEntity>().userName}'s turn to roll!";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTurn == myPlayerNum)
        {
            EnableDice();
        }
    }

    public void PlayerBuy(int targetProperty, string targetPlayer)
    {
        propertyArray[targetProperty].owned = true;
        photonView.RPC("UpdatePropertyOwnership", RpcTarget.AllBuffered, targetProperty, true, targetPlayer);
    }

    public void UpdateRollText(string prevUserName, int prevRoll)
    {
        currentRollTextDisplay.text = $"{thisPlayer.GetComponent<PlayerEntity>().GetObjectFromID((playerTurn * 1000) + 1).GetComponent<PlayerEntity>().userName}'s turn to roll!";
        rolledDisplay.text = $"{prevUserName} rolled: {prevRoll}";
    }

    public void UpdateTitleDeedSprite(int posInArray)
    {
        titleDeedObjectSprite.sprite = titleDeedSpriteArray[posInArray];
    }

    public void EnableDice()
    {
        dice.GetComponent<Button>().interactable = true;
    }

    public void DisableDice()
    {
        dice.GetComponent<Button>().interactable = false;
        //dice.SetActive(false);
    }

    //public void ChangeDiceSprite(Sprite[] diceSides, int randomDiceSide)
    //{
    //    dice = GameObject.Find("Dice Button");
    //    diceSprite = dice.GetComponent<Image>();
    //    diceSprite.sprite = diceSides[randomDiceSide];
    //}

    public void UpdateBalanceText()
    {
        balanceDText.text = $"${thisPlayer.GetComponent<PlayerEntity>().balance}";
    }
    
    public void CallTurnRPC()
    {
        photonView.RPC("ChangePlayerTurn", RpcTarget.AllBuffered, thisPlayer.GetComponent<PlayerEntity>().userName, manageDiceRoll);
    }

    public void hidePurchaseRequest()
    {
        purchaseRequestCanvas.SetActive(false);
        titleDeedListing.SetActive(false);
    }
    
    public void showPurchaseRequestBuyable()
    {
        purchaseRequestCanvas.SetActive(true);
        purchaseRequestBuyableCanvas.SetActive(true);
        purchaseRequestNotBuyableCanvas.SetActive(false);
        purchaseRequestRentCanvas.SetActive(false);
        titleDeedListing.SetActive(true);
    }
    
    public void showPurchaseRequestNotBuyable()
    {
        purchaseRequestCanvas.SetActive(true);
        purchaseRequestBuyableCanvas.SetActive(false);
        purchaseRequestRentCanvas.SetActive(false);
        purchaseRequestNotBuyableCanvas.SetActive(true);
        titleDeedListing.SetActive(true);
    }

    public void showRentPay()
    {
        purchaseRequestCanvas.SetActive(true);
        purchaseRequestBuyableCanvas.SetActive(false);
        purchaseRequestRentCanvas.SetActive(true);
        purchaseRequestNotBuyableCanvas.SetActive(false);
        titleDeedListing.SetActive(true);
    }

    public void ShowEscMenu()
    {
        escMenu.SetActive(true);
    }

    public void HideEscMenu()
    {
        escMenu.SetActive(false);
    }

    private void LoadProperties(Property[] propertyArray) // WOULD LIKE TO DO FROM A TEXT FILE WHEN FIGURED OUT.
    {
        string[] propertyNamesArray = new string[40] {"Go" , "Old Kent Road" , "Community Chest" , "Whitechapel Road" , 
        "Income Tax" , "King's Cross Station" , "The Angel Islington" , "Chance" , "Euston Road" , "Pentonville Road" ,
        "In Jail / Just Visiting" , "Pall Mall" , "Electric Company" , "Whitehall" , "Northumberland Avenue", "Marylebone Station" , "Bow Street" ,
        "Community Chest" , "Marlborough Street" , "Vine Street" , "Free Parking" , "Strand" , "Chance" , "Fleet Street" ,
        "Trafalgar Square" , "Fenchurch St. Station" , "Leicester Square" , "Coventry Steet" , "Water Works" , "Piccadilly" ,
        "Go To Jail" , "Regent Street" , "Oxford Street" , "Community Chest" , "Bond Street" , "Liverpool Street Station" ,
        "Chance" , "Park Lane" , "Super Tax" , "Mayfair"
        };

        double[] propertyRentsArray = new double[40] {0, 2, 0, 4, 0, 25, 6, 0, 6, 8, 0, 10, 0, 10, 12, 25, 14, 0, 14, 16, 0, 18, 0, 18, 20, 25, 22, 22, 0, 22, 0, 26, 26, 0, 28, 25, 0, 35, 0, 50};

        double[] propertyCostsArray = new double[40] {0, 60, 0, 60, 200, 200, 100, 0, 100, 120, 0, 140, 150, 140,160,200,180,0,180,200,0,220,0,220,240,200,260, 260, 150, 280, 0, 300, 300, 0, 320, 200, 0, 350, 100, 400};

         for (int currentProperty = 0; currentProperty < 40; currentProperty++)
         {
            string propertyName = propertyNamesArray[currentProperty];
            Property property = new Property();
            // set name            
            property.name = propertyNamesArray[currentProperty];
            // set forPurchase
            property.forPurchase = true;
            if (propertyName == "Go" || propertyName == "Community Chest" || propertyName == "Income Tax" || propertyName == "Chance" || propertyName == "In Jail / Just Visiting" || propertyName == "Go To Jail" || propertyName == "Super Tax" || propertyName == "Free Parking")
            {
                property.forPurchase = false;
            }
            // set purchaseCost
            property.purchaseCost = propertyCostsArray[currentProperty];
            // set initialRent
            property.initalRent = propertyRentsArray[currentProperty];

            // asign property into Property array
            propertyArray[currentProperty] = property;
         }
    }

    [PunRPC]
    void UpdatePropertyOwnership(int propertyToUpdate, bool owned, string newOwner)
    {
        propertyArray[propertyToUpdate].owned = owned;
        propertyArray[propertyToUpdate].owner = newOwner;
    }

    [PunRPC]
    void UpdatePlayerDicts(string userName, int viewID)
    {
        playersDict.Add(userName, viewID);
        Debug.Log($"Add comp");
        thisPlayer.GetComponent<PlayerEntity>().UpdatePlayerTabCanvas(userName);
    }

    [PunRPC]
    void ChangePlayerTurn(string userName, int roll)
    {
        if (playerTurn < playersDict.Count)
        {
            playerTurn += 1;
        }
        else
        {
            playerTurn = 1;
        }
        UpdateRollText(userName, roll);
        DisableDice();
    }
}
