using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public AudioManager manager;
    public SFXManager sfxmanager;
    private IEnumerator fadeAudio(int start, int end)
    {
        int iterator = (end-start) / Mathf.Abs(end - start);
        for (int i = start; i != end; i += iterator)
        {
            manager.changeVolume(i);
            yield return null;
        }
    }

    public void pullingOutStopwatch()
    {
        StartCoroutine(fadeAudio(0, 100));
        //add
    }

    public void pullingBackStopwatch()
    {
        Debug.Log("stopwatch pulled back");
        manager.changeVolume(0);
    }

    public void SFX(SFXManager.clips clip)
    {
        sfxmanager.playSFX(clip);
    }
}
