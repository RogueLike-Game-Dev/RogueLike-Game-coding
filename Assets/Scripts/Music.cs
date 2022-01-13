using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Slider volumeSlider;
    AudioSource audioSource;

    private static Music _instance;
    
    public static Music Instance 
    { 
        get { return _instance; } 
    } 

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Awake() 
    { 
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
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
