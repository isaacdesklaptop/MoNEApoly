using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class MasterGameManager : MonoBehaviour
{
    public PhotonView photonView;
    public Property[] propertyArray = new Property[40];
    int latestPropertyUpdated;
    bool latestBool;
    
    private void Update()
    {
        photonView.RPC("UpdatePropertyArray", RpcTarget.AllBuffered, latestPropertyUpdated, true);
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        LoadProperties(propertyArray);
    }

    [PunRPC]
    void UpdateMasterPropertyArray(int propertyToUpdate, bool owned)
    {
        propertyArray[propertyToUpdate].owned = owned;
        latestPropertyUpdated = propertyToUpdate;
        latestBool = owned;
        Debug.Log("Master Updated of Purchase");
    }
   
    [PunRPC]
    void UpdatePropertyArray(int propertyToUpdate, bool owned)
    {

    }

    private void LoadProperties(Property[] propertyArray) // WOULD LIKE TO DO FROM A TEXT FILE WHEN FIGURED OUT.
    {
        #region Name Array

        string[] propertyNamesArray = new string[40] {"Go" , "Old Kent Road" , "Community Chest" , "Whitechapel Road" ,
        "Income Tax" , "King's Cross Station" , "The Angel Islington" , "Chance" , "Euston Road" , "Pentonville Road" ,
        "In Jail / Just Visiting" , "Pall Mall" , "Electric Company" , "Whitehall" , "Northumberland Avenue", "Marylebone Station" , "Bow Street" ,
        "Community Chest" , "Marlborough Street" , "Vine Street" , "Free Parking" , "Strand" , "Chance" , "Fleet Street" ,
        "Trafalgar Square" , "Fenchurch St. Station" , "Leicester Square" , "Coventry Steet" , "Water Works" , "Piccadilly" ,
        "Go To Jail" , "Regent Street" , "Oxford Street" , "Community Chest" , "Bond Street" , "Liverpool Street Station" ,
        "Chance" , "Park Lane" , "Super Tax" , "Mayfair"
        };

        #endregion
        
        double[] propertyCostsArray = new double[40] { 0, 60, 0, 60, 200, 200, 100, 0, 100, 120, 0, 140, 150, 140, 160, 200, 180, 0, 180, 200, 0, 220, 0, 220, 240, 200, 260, 260, 150, 280, 0, 300, 300, 0, 320, 200, 0, 350, 100, 400 };

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
}
