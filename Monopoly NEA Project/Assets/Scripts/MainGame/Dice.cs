using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dice : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Player = GameObject.Find("Player Object(Clone)").GetComponent<PlayerEntity>();
        int diceRoll1 = Random.Range(1,maxOneDice+1);
        int diceRoll2 = Random.Range(1, maxOneDice + 1);
        int diceRoll = diceRoll1 + diceRoll2;
        Player.Move(diceRoll);
        rolledDisplay.text = $"You rolled: {diceRoll}";
        buttonAudio.Play();
    }

    private IEnumerator DiceRollAnimation()
    {
        int randomDiceSide = 0;
        for (int i = 0; i < 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            diceSprite.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
