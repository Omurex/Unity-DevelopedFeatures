using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectRepository : MonoBehaviour
{
    [SerializeField] List<AudioClip> effects;
    [SerializeField] bool playOnAwake = true;


    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(playOnAwake == true) PlayRandom();
    }


    public void PlayRandom()
    {
        audioSource.clip = effects[Random.Range(0, effects.Count)];
        audioSource.Play();
    }
}
