using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Dice : MonoBehaviour
{
    public GameManager GameManager;
    public PlayerEntity Player;
    public AudioSource buttonAudio;
    public int maxOneDice;
    public Sprite[] diceSides = new Sprite[6];
    public Button diceButton;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        diceButton = this.GetComponent<Button>();
    }

    public void OnClick()
    {
        Player = GameObject.Find("Player Object(Clone)").GetComponent<PlayerEntity>();
        int diceRoll1 = Random.Range(1, maxOneDice + 1);
        int diceRoll2 = Random.Range(1, maxOneDice + 1);
        int diceRoll = diceRoll1 + diceRoll2;

        GameManager.manageDiceRoll = diceRoll;
        GameManager.UpdateRollText(GameManager.thisPlayer.GetComponent<PlayerEntity>().userName, GameManager.manageDiceRoll);

        if (diceRoll1 == diceRoll2)
        {
            diceButton.interactable = true;
            GameManager.doubleCounter++;
        }
        else
        {
            GameManager.rollTaken = true;
            GameManager.endTurnButton.GetComponent<Button>().interactable = true;
        }

        Player.Move(diceRoll);
        buttonAudio.Play();
        GameManager.titleDeedObjectSprite.sprite = GameManager.titleDeedSpriteArray[GameManager.thisPlayer.GetComponent<PlayerEntity>().currentPosition];
    }
}
