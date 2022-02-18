using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public PhotonView photonView;
    
    // Board Setup
    public GameObject[] propertiesArray = new GameObject[40];
    public Property[] propertyArray = new Property[40];

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

    // HUD
    public TextMeshProUGUI balanceDText;
    public GameObject escMenu;
    public TextMeshProUGUI escMenuNameText;
    public TextMeshProUGUI escMenuRoomText;

    // Dice
    public Image diceSprite;
    public GameObject dice;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        LoadProperties(propertyArray);
        hidePurchaseRequest();
        HideEscMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerBuy(int targetProperty, string targetPlayer)
    {
        propertyArray[targetProperty].owned = true;
        photonView.RPC("UpdatePropertyOwnership", RpcTarget.AllBuffered, targetProperty, true, targetPlayer);
    }

    public void ChangeDiceSprite(Sprite[] diceSides, int randomDiceSide)
    {
        dice = GameObject.Find("Dice Button");
        diceSprite = dice.GetComponent<Image>();
        diceSprite.sprite = diceSides[randomDiceSide];
    }

    public void UpdateBalanceText()
    {
        balanceDText.text = $"${GameObject.Find("Player Object(Clone)").GetComponent<PlayerEntity>().balance}";
    }
    

    public void hidePurchaseRequest()
    {
        purchaseRequestCanvas.SetActive(false);
    }
    
    public void showPurchaseRequestBuyable()
    {
        purchaseRequestCanvas.SetActive(true);
        purchaseRequestBuyableCanvas.SetActive(true);
        purchaseRequestNotBuyableCanvas.SetActive(false);
        purchaseRequestRentCanvas.SetActive(false);
    }
    
    public void showPurchaseRequestNotBuyable()
    {
        purchaseRequestCanvas.SetActive(true);
        purchaseRequestBuyableCanvas.SetActive(false);
        purchaseRequestRentCanvas.SetActive(false);
        purchaseRequestNotBuyableCanvas.SetActive(true);
    }

    public void showRentPay()
    {
        purchaseRequestCanvas.SetActive(true);
        purchaseRequestBuyableCanvas.SetActive(false);
        purchaseRequestRentCanvas.SetActive(true);
        purchaseRequestNotBuyableCanvas.SetActive(false);
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
        GameObject.Find("Player Object(Clone)").GetComponent<PlayerEntity>().UpdatePlayerTabCanvas(userName);
    }
}
