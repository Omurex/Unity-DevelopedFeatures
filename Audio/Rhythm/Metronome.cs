using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Extremely important class that keeps entire game in time
public class Metronome : MonoBehaviour
{
    const float BEAT_DELAY = 1.2f;


    public static Metronome singleton { get; private set; }


    public delegate void OnBeat();
    public event OnBeat BeatHappened;
    public event OnBeat SongStarted;


    [SerializeField] AudioSource music;
    
    [SerializeField] int startBPM;
    

    int bpm;
    bool bpmChanged = false;


    void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
        SetBPM(startBPM);
    }


    void Start()
    {
        float startDelay = 60f / bpm;
        Invoke("StartSong", startDelay);
        InvokeRepeating("BeatHit", startDelay * BEAT_DELAY, startDelay);
    }


    void StartSong()
    {
        music.Play();
        SongStarted?.Invoke();

        float invokeRepeatTime = 60f / bpm;
    }


    void BeatHit()
    {
        BeatHappened?.Invoke();

        if(bpmChanged)
        {
            float invokeRepeatTime = 60f / bpm;

            CancelInvoke("BeatHit");
            InvokeRepeating("BeatHit", invokeRepeatTime, invokeRepeatTime);
            bpmChanged = false;
        }
    }


    void OnDestroy()
    {
        if(singleton != null && singleton == this) 
        {
            singleton = null;
        }
    }


    public int GetBPM() { return bpm; }
    public float GetBPS() { return BPMinToSec(bpm); }


    public void SetBPM(int newBPM) 
    { 
        bpmChanged = true;
        bpm = newBPM; 

        BeatGenerator.UpdateBeatSpeed();
    }


    public void SetBPS(float newBPS) 
    { 
        bpmChanged = true;
        bpm = BPSecToMin(newBPS); 

        BeatGenerator.UpdateBeatSpeed();
    }


    public static float BPMinToSec(int bpm) { return bpm / 60f; }
    public static int BPSecToMin(float bps) { return (int) (bps * 60f + .1f); }
}
