using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Slider volumeSlider;
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        //for slider events
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(volumeSlider.value); });
    }

    public void ChangeVolume(float sliderValue)
    {
        audioSource.volume = sliderValue;
        RunStats.volume = sliderValue;
        
    }
}
