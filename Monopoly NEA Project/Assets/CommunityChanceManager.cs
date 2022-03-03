using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityChanceManager
{
    public GameManager gameManager;

    public int[] chestsNums = new int[15] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    public int[] chancesNus = new int[15] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    int tempValue;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public int GetCard(string arrayFrom)
    {
        if (arrayFrom == "chest")
        {
            return chestsNums[0];
        }
        if (arrayFrom == "chance")
        {
            return chancesNus[0];
        }
        return -1;
    }

    public void RunChestMethod(int cardNum, PlayerEntity targetPlayer)
    {
        switch (cardNum)
        {
            case 0:
                //GenericBalanceMinus();
                break;
            default:
                break;
        }
    }

    public void RunChanceMethod(int cardNum, PlayerEntity targetPlayer)
    {
        switch (cardNum)
        {
            case 0:
                //GenericBalanceMinus();
                break;
            default:
                break;
        }
    }

    public void Reorder(string arrayToReorder)
    {
        if (arrayToReorder == "chest")
        {
            tempValue = chestsNums[0];
            for (int i = 0; i < chestsNums.Length-2; i++)
            {
                chestsNums[i] = chestsNums[i + 1];
            }
            chestsNums[14] = tempValue;
        }

        if (arrayToReorder == "chance")
        {
            tempValue = chancesNus[0];
            for (int i = 0; i < chancesNus.Length - 2; i++)
            {
                chancesNus[i] = chancesNus[i + 1];
            }
            chancesNus[14] = tempValue;
        }
    }

    public void GenericBalanceMinus(PlayerEntity targetPlayer, double amountToChange)
    {
        targetPlayer.balance -= amountToChange;
    }

    public void GenericBalanceAdd(PlayerEntity targetPlayer, double amountToChange)
    {
        targetPlayer.balance += amountToChange;
    }

    public void ChangePlayerPosition(PlayerEntity targetPlayer, int newPosition)
    {
        targetPlayer.currentPosition = newPosition;
        targetPlayer.transform.position = gameManager.propertiesArray[newPosition].transform.position;
    }

    public void GrantJailCard(PlayerEntity targetPlayer)
    {
        targetPlayer.getOutJailCardsCount++;
    }

    public void PayPerHouse(PlayerEntity targetPlayer)
    {
        // needs house logic
    }

}
