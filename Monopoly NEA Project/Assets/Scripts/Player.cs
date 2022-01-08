using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0f;
    public int currentPosition = 0;
    public GameManager GameManager;
    public string userName { get; set; }
    public float balance { get; set; }
    public TextMeshProUGUI purchReqText;

    public void Move(int roll)
    {
        if (currentPosition + roll > 39)
        {
            currentPosition = (currentPosition += roll) - 40;
        }
        else
        {
            currentPosition += roll;
        }
        this.transform.position = GameManager.propertiesArray[currentPosition].transform.position; //Render move to new square
        if (GameManager.propertyArray[currentPosition].forPurchase == true)
        {
            purchReqText.text = $"Would you like to purchase \"{GameManager.propertyArray[currentPosition].name}\"?";
            PurchaseRequest();
        }
    }

    public void PurchaseRequest()
    {
        GameManager.showPurchaseRequest();
    }
}
