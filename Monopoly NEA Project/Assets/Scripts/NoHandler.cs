using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHandler : MonoBehaviour
{
    public Player currentPlayer;
    public GameManager GameManager;
    public AudioSource buttonAudio;

    public void OnClick()
    {
        GameManager.hidePurchaseRequest();
        buttonAudio.Play();
    }
}
