using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameManager gameManager;

    public void OnClick()
    {
        gameManager.HideEscMenu();
    }
}
