using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;


public class GameManager : MonoBehaviour
{
    public GameObject[] propertiesArray = new GameObject[40];
    public Property[] propertyArray = new Property[40];
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject purchaseRequestText;
    public GameObject purchaseBG;
    public GameObject purchaseButtonsBG;

    // Start is called before the first frame update
    void Start()
    {
       LoadProperties(propertyArray);
       hidePurchaseRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hidePurchaseRequest()
    {
        yesButton.SetActive(false);
        noButton.SetActive(false);
        purchaseRequestText.SetActive(false);
        purchaseBG.SetActive(false);
        purchaseButtonsBG.SetActive(false);
    }
    public void showPurchaseRequest()
    {
        yesButton.SetActive(true);
        noButton.SetActive(true);
        purchaseRequestText.SetActive(true);
        purchaseBG.SetActive(true);
        purchaseButtonsBG.SetActive(true);
    }

    private void LoadProperties(Property[] propertyArray)
    {
        
        string[] propertyFileArray = new string[40] {"Go" , "Old Kent Road" , "Community Chest" , "Whitechapel Road" , 
        "Income Tax" , "King's Cross Station" , "The Angel Islington" , "Chance" , "Euston Road" , "Pentonville Road" ,
        "In Jail / Just Visiting" , "Pall Mall" , "Electric Company" , "Whitehall" , "Northumberland Avenue", "Marylebone Station" , "Bow Street" ,
        "Community Chest" , "Marlborough Street" , "Vine Street" , "Free Parking" , "Strand" , "Chance" , "Fleet Street" ,
        "Trafalgar Square" , "Fenchurch St. Station" , "Leicester Square" , "Coventry Steet" , "Water Works" , "Piccadilly" ,
        "Go To Jail" , "Regent Street" , "Oxford Street" , "Community Chest" , "Bond Street" , "Liverpool Street Station" ,
        "Chance" , "Park Lane" , "Super Tax" , "Mayfair"
        };

         for (int currentProperty = 0; currentProperty < 40; currentProperty++)
         {
             string propertyName = propertyFileArray[currentProperty];
             Property property = new Property();
             property.name = propertyFileArray[currentProperty];
             property.forPurchase = true;
             if (propertyName == "Go" || propertyName == "Community Chest" || propertyName == "Income Tax" || propertyName == "Chance" || propertyName == "In Jail / Just Visiting" || propertyName == "Go To Jail" || propertyName == "Super Tax")
             {
                 property.forPurchase = false;
             }
             propertyArray[currentProperty] = property;
         }

    }

    public class Property
    {
        public string name { get; set; }
        public int position { get; }
        public float purchaseCost { get; }
        public float rent { get; }
        public string owner { get; set; }
        public bool owned { get; set; }
        public string set { get; }
        public bool forPurchase { get; set; }
    }

}
