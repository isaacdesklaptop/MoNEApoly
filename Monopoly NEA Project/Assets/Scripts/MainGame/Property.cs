using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property
{
    public string name { get; set; } // Property name
    public bool forPurchase { get; set; } // Can a player buy the property?
    public bool owned { get; set; } // Is the property owned by a player?
    public string owner { get; set; } // Which player owns the property
    public double rent { get; set; } // Rent when landed on
    public double mortgage { get; } // Money gained when mortgaged to bank
    public double purchaseCost { get; set; } // Cost to buy property
    public int houseCount { get; set; } // How many houses are on the property
    public int hotelCount { get; set; } // How many hotels are on the property	
    public string colourSet { get; set; } // Color set of property
    public PlayerEntity ownerEntity { get; set; } // PlayerEntity owner
}
