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
    public bool rollTaken;
    public int doubleCounter;

    // Board Setup
    public GameObject[] propertiesArray = new GameObject[40];
    public Property[] propertyArray = new Property[40];
    public Sprite[] titleDeedSpriteArray = new Sprite[40];
    public string[] communityChestTextsArray = new string[15];
    public string[] chanceTextsArray = new string[15];
    public CommunityChanceManager communityChanceManager = new CommunityChanceManager();

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
    public GameObject tradeButton;
    public GameObject endTurnButton;
    // ChanceChest HUD
    public GameObject chanceChestEmpty;
    public TextMeshProUGUI chanceChestText;
    
    public GameObject playerListingObjectPrefab;
    public GameObject playerListingListing;

    // Dice
    public GameObject dice;

    // Start is called before the first frame update
    void Start()
    {
        thisPlayer = GameObject.Find("Player Object(Clone)");
        photonView = GetComponent<PhotonView>();
        LoadProperties(propertyArray);
        hidePurchaseRequest();
        HideEscMenu();
        DisableTurnButtons();
        HideChanceChest();
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
            if (!rollTaken)
            {
                EnableTurnButtons();
            }
            else
            {
                dice.GetComponent<Button>().interactable = false;
            }
        }

        if (doubleCounter % 3 == 0 && doubleCounter != 0)
        {
            thisPlayer.GetComponent<PlayerEntity>().GoToJail();
        }
    }

    public void PlayerBuy(int targetProperty, string targetPlayer)
    {
        propertyArray[targetProperty].owned = true;
        propertyArray[targetProperty].isMine = true;
        photonView.RPC("UpdatePropertyOwnership", RpcTarget.AllBuffered, targetProperty, true, targetPlayer);
    }

    public void UpdateRollText(string prevUserName, int prevRoll)
    {
        currentRollTextDisplay.text = $"{thisPlayer.GetComponent<PlayerEntity>().GetObjectFromID((playerTurn * 1000) + 1).GetComponent<PlayerEntity>().userName}'s turn to roll!";
        rolledDisplay.text = $"{prevUserName} rolled: {prevRoll}";
    }

    public void DisableTurnButtons()
    {
        endTurnButton.GetComponent<Button>().interactable = false;
        dice.GetComponent<Button>().interactable = false;
    }

    public void EnableTurnButtons()
    {
        dice.GetComponent<Button>().interactable = true;
    }

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

    public void ShowChanceChest()
    {
        chanceChestEmpty.SetActive(true);
    }

    public void HideChanceChest()
    {
        chanceChestEmpty.SetActive(false);
    }

    private void LoadProperties(Property[] propertyArray)
    {
        string[] propertyNamesArray = new string[40] {"Go" , "Old Kent Road" , "Community Chest" , "Whitechapel Road" , 
        "Income Tax" , "King's Cross Station" , "The Angel Islington" , "Chance" , "Euston Road" , "Pentonville Road" ,
        "In Jail / Just Visiting" , "Pall Mall" , "Electric Company" , "Whitehall" , "Northumberland Avenue", "Marylebone Station" , "Bow Street" ,
        "Community Chest" , "Marlborough Street" , "Vine Street" , "Free Parking" , "Strand" , "Chance" , "Fleet Street" ,
        "Trafalgar Square" , "Fenchurch St. Station" , "Leicester Square" , "Coventry Steet" , "Water Works" , "Piccadilly" ,
        "Go To Jail" , "Regent Street" , "Oxford Street" , "Community Chest" , "Bond Street" , "Liverpool Street Station" ,
        "Chance" , "Park Lane" , "Super Tax" , "Mayfair"
        };

        #region Community Chests Array Initialization
        communityChestTextsArray = new string[15] { "Pay hospital fee of $100." , "It's your birthday! Collect $10 from the bank",
            "Pay a fine of $10", "Go back to \"Old Kent Road\"." , "Recieve interest on 7% preference shares, $25." ,
            "You have won second prize in a beauty contest, collect $10.", "GET OUT OF JAIL FREE." , "You inherited $100." , "Doctor's fee, pay $50.",
            "Income Tax refund, collect $20." , "Go directly to \"Jail\"." , "Pay your insurance premium of $50.", "From sale of stock you get $50.",
            "Bank error in  your favour, collect $200.", "Advance to \"Go\"." };
        #endregion

        #region Chances Array Initialiaztion
        chanceTextsArray = new string[15] { "Drunk driving fine, pay $20.", "Make general repairs on houses, pay $25.",
            "Advance to \"Trafalgar Square\".", "Pay school fees of $150.", "Street repairs, pay $40.",
            "Advance to \"Go\".", "GET OUT OF JAIL FREE.", "Speeding fine, pay $15.", "Go back three spaces.", "Your building loan matures, recieve $150.", "Bank pays you dividend of $50.",
            "Advance to \"Mayfair\".", "Go directly to \"Jail\".", "You won a crossword competition, collect $100.", "Annuity matures, collect $100." };
        #endregion

        double[] propertyRentsArray = new double[40] {0, 2, 0, 4, 0, 25, 6, 0, 6, 8, 0, 10, 0, 10, 12, 25, 14, 0, 14, 16, 0, 18, 0, 18, 20, 25, 22, 22, 0, 22, 0, 26, 26, 0, 28, 25, 0, 35, 0, 50};

        double[] propertyCostsArray = new double[40] {0, 60, 0, 60, 200, 200, 100, 0, 100, 120, 0, 140, 150, 140,160,200,180,0,180,200,0,220,0,220,240,200,260, 260, 150, 280, 0, 300, 300, 0, 320, 200, 0, 350, 100, 400};

        for (int currentProperty = 0; currentProperty < 40; currentProperty++)
        {
            string propertyName = propertyNamesArray[currentProperty];
            Property property = new Property();         
            property.name = propertyNamesArray[currentProperty];
            property.forPurchase = true;
            if (propertyName == "Go" || propertyName == "Community Chest" || propertyName == "Income Tax" || propertyName == "Chance" || propertyName == "In Jail / Just Visiting" || propertyName == "Go To Jail" || propertyName == "Super Tax" || propertyName == "Free Parking")
            {
                property.forPurchase = false;
            }
            property.purchaseCost = propertyCostsArray[currentProperty];
            property.rent = propertyRentsArray[currentProperty];
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
        thisPlayer.GetComponent<PlayerEntity>().UpdatePlayerTabCanvas(userName, thisPlayer.GetComponent<PlayerEntity>().balance);
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
        DisableTurnButtons();
    }
}
