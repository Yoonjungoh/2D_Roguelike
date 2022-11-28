using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;
    public AudioSource audioSourceBGM;
    public AudioSource audioSourceEffect;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance!=null)
            Destroy(gameObject);
            //Destroy(Instance);  // °°À»µí

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip audioClip)
    {
        audioSourceEffect.clip = audioClip;
        audioSourceEffect.Play();
    }
    public void RandomizeSft(params AudioClip [] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPicth = Random.Range(lowPitchRange, highPitchRange);

        audioSourceEffect.pitch = randomPicth;
        audioSourceEffect.clip = clips[randomIndex];
        audioSourceEffect.Play();
    }
}
