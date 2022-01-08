using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dice : MonoBehaviour
{
    public Player Player;
    public TextMeshProUGUI rolledDisplay;
    public AudioSource buttonAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        int diceRoll = Random.Range(1,7);
        Player.Move(diceRoll);
        rolledDisplay.text = $"You rolled: {diceRoll}";
        buttonAudio.Play();
    }

}
