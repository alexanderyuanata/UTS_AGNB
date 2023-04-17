using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public enum clips
    {
        BEASTGROWL = 0,
        WOMANSCREAM = 1,
        MONSTERGROWL = 2,
        DING = 3,
        STEP1 = 4,
        STEP2 = 5,
        BELL = 6,
        CACKLE = 7,
    }

    public AudioClip[] sfx_clips;

    private AudioSource sfx;

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    public void playSFX(clips index)
    {
        sfx.clip = sfx_clips[(int)index];
        sfx.Play();
    }
}
