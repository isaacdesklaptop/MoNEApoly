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
    public TextMeshProUGUI rolledDisplay;
    public AudioSource buttonAudio;
    public int maxOneDice;
    public Sprite[] diceSides = new Sprite[6];
    Image diceSprite;
    // Start is called before the first frame update
    void Start()
    {
        diceSprite = GetComponent<Image>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        //DiceRollAnimation();
        Player = GameObject.Find("Player Object(Clone)").GetComponent<PlayerEntity>();
        int diceRoll1 = Random.Range(1,maxOneDice+1);
        int diceRoll2 = Random.Range(1, maxOneDice + 1);
        int diceRoll = diceRoll1 + diceRoll2;
        Player.Move(diceRoll);
        buttonAudio.Play();
        GameManager.manageDiceRoll = diceRoll;
        GameManager.CallTurnRPC();
    }

    //private IEnumerator DiceRollAnimation()
    //{
    //    int randomDiceSide;
    //    for (int i = 0; i < 20; i++)
    //    {
    //        randomDiceSide = Random.Range(0, 6);
    //        GameManager.ChangeDiceSprite(diceSides, randomDiceSide);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //}
}
