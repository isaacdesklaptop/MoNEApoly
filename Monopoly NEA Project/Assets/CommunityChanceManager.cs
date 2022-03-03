using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunityChanceManager
{
    public GameManager gameManager;
    public PlayerEntity thisPlayerEntity;

    public int[] chestsNums = new int[15] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    public int[] chancesNus = new int[15] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
    int tempValue;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        thisPlayerEntity = gameManager.thisPlayer.GetComponent<PlayerEntity>();
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
                GenericBalanceMinus(thisPlayerEntity, 100);
                break;
            case 1:
                GenericBalanceAdd(thisPlayerEntity, 10);
                break;
            case 2:
                GenericBalanceMinus(thisPlayerEntity, 10);
                break;
            case 3:
                ChangePlayerPosition(thisPlayerEntity, 1);
                break;
            case 4:
                GenericBalanceAdd(thisPlayerEntity, 25);
                break;
            case 5:
                GenericBalanceAdd(thisPlayerEntity, 10);
                break;
            case 6:
                GrantJailCard(thisPlayerEntity);
                break;
            case 7:
                GenericBalanceAdd(thisPlayerEntity, 100);
                break;
            case 8:
                GenericBalanceMinus(thisPlayerEntity, 50);
                break;
            case 9:
                GenericBalanceAdd(thisPlayerEntity, 20);
                break;
            case 10:
                thisPlayerEntity.GoToJail();
                break;
            case 11:
                GenericBalanceMinus(thisPlayerEntity, 50);
                break;
            case 12:
                GenericBalanceAdd(thisPlayerEntity, 50);
                break;
            case 13:
                GenericBalanceAdd(thisPlayerEntity, 200);
                break;
            case 14:
                ChangePlayerPosition(thisPlayerEntity, 0);
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
                GenericBalanceMinus(thisPlayerEntity, 50);
                break;
            case 1:
                GenericBalanceMinus(thisPlayerEntity, 25);
                break;
            case 2:
                ChangePlayerPosition(thisPlayerEntity, 24);
                break;
            case 3:
                GenericBalanceMinus(thisPlayerEntity, 150);
                break;
            case 4:
                GenericBalanceMinus(thisPlayerEntity, 40);
                break;
            case 5:
                ChangePlayerPosition(thisPlayerEntity, 0);
                break;
            case 6:
                GrantJailCard(thisPlayerEntity);
                break;
            case 7:
                GenericBalanceMinus(thisPlayerEntity, 15);
                break;
            case 8:
                ChangePlayerPosition(thisPlayerEntity, thisPlayerEntity.currentPosition - 3);
                break;
            case 9:
                GenericBalanceAdd(thisPlayerEntity, 150);
                break;
            case 10:
                GenericBalanceAdd(thisPlayerEntity, 50);
                break;
            case 11:
                ChangePlayerPosition(thisPlayerEntity, 39);
                break;
            case 12:
                thisPlayerEntity.GoToJail();
                break;
            case 13:
                GenericBalanceAdd(thisPlayerEntity, 100);
                break;
            case 14:
                GenericBalanceAdd(thisPlayerEntity, 100);
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
}
