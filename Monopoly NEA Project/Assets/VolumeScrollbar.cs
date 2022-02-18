using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScrollbar : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource buttonAudio;
    public Scrollbar thisScrollbar;

    public void OnSliderChanged()
    {
        buttonAudio.volume = thisScrollbar.value; 
    }
}
