using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceChestButton : MonoBehaviour
{
    public GameManager gameManager;

    public void OnClick()
    {
        gameManager.HideChanceChest();
    }
}
