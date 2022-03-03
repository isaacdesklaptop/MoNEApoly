using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    public GameManager gameManager;

    public void Start()
    {

    }

    public void OnClick()
    {
        gameManager.rollTaken = false;
        gameManager.CallTurnRPC();
    }
}
