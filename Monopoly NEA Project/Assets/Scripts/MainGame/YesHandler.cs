using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesHandler : MonoBehaviour
{
    public PlayerEntity currentPlayer;
    public GameManager GameManager;
    public AudioSource buttonAudio;

    public void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnClick()
    {
        currentPlayer = GameObject.Find("Player Object(Clone)").GetComponent<PlayerEntity>();
        GameManager.PlayerBuy(currentPlayer.currentPosition, currentPlayer.userName);
        GameManager.hidePurchaseRequest();
        buttonAudio.Play();
        currentPlayer.SetPlayerBalance(GameManager.propertyArray[currentPlayer.currentPosition].purchaseCost, -1);
    }
}
