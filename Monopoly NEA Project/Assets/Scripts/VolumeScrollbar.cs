using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScrollbar : MonoBehaviour
{
    public AudioSource buttonAudio;
    public Scrollbar thisScrollbar;

    public void OnSliderChanged()
    {
        buttonAudio.volume = thisScrollbar.value; 
    }
}
