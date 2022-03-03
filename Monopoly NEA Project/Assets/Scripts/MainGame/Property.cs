using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property
{
    public string name { get; set; } // Property name
    public bool forPurchase { get; set; } // Can a player buy the property?
    public bool owned { get; set; } // Is the property owned by a player?
    public bool isMine { get; set; } // Is the property owned by the current player?
    public string owner { get; set; } // Which player owns the property
    public double rent { get; set; } // Rent when landed on
    public double purchaseCost { get; set; } // Cost to buy property
    public PlayerEntity ownerEntity { get; set; } // PlayerEntity owner
}
