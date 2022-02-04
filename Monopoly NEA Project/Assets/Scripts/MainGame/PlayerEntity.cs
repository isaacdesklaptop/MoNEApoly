using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerEntity : MonoBehaviour
{
    public double moveSpeed = 0f;
    public int currentPosition = 0;
    public GameManager GameManager;
    public string userName { get; set; }
    public double balance { get; set; } = 0f;
    public TextMeshProUGUI purchReqText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI balanceText;
    PhotonView view;

    // Start is called before the first frame update
    void Start() 
    {
        this.SetPlayerBalance(1500, 1); // Initializes the player's balance to $1,500 at start of game   
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        view = GetComponent<PhotonView>();
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
        if (view.IsMine)
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
            // If the position is buyable, and not owned already:
            if (GameManager.propertyArray[currentPosition].forPurchase == true && GameManager.propertyArray[currentPosition].owned == false)
            {
                GameManager.purchReqText.text = $"Would you like to purchase \"{GameManager.propertyArray[this.currentPosition].name}\"?"; // Set purchase request button text 
                GameManager.costText.text = $"The property costs: {GameManager.propertyArray[this.currentPosition].purchaseCost}"; // Set property cost text
                GameManager.balanceText.text = $"Your balance is: {this.balance}"; // Set balance text
                PurchaseRequestBuyable(); // Display purchase screen
            }
            // If the position is buyable, but owned already.
            if (GameManager.propertyArray[currentPosition].forPurchase == true && GameManager.propertyArray[currentPosition].owned == true) 
            {
                GameManager.purchReqText.text = $"\"{GameManager.propertyArray[this.currentPosition].name}\"is owned already!"; // Set purchase request button text 
                PurchaseRequestNotBuyable(); // Display purchase screen
            }
        }
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
