using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YesHandler : MonoBehaviour
{
    public Player currentPlayer;
    public GameManager GameManager;
    public AudioSource buttonAudio;
 
    public void OnClick()
    {
        GameManager.propertyArray[currentPlayer.currentPosition].owned = true;
        GameManager.hidePurchaseRequest();
        buttonAudio.Play();
    }
}
