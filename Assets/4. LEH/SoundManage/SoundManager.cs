using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource bgm;
    AudioSource sfx;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    public AudioClip[] audio_clips;
    public void PlaySound(string type)
    {
        int index = 0;

        switch (type)
        {
            case "pieceUp": index = 0; break;
            case "pieceDown": index = 1; break;
            case "combatStart": index = 2; break;
            case "boxBell": index = 3; break;
            case "lock": index = 4; break;
            case "playerHeat": index = 5; break;
            case "smash1": index = 6; break;
            case "smash2": index = 7; break;
        }

        sfx.clip = audio_clips[index];
        sfx.PlayOneShot(sfx.clip);
    }
}
