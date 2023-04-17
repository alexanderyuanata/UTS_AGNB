using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAction : MonoBehaviour
{
    public SFXManager sfxmanager;
    public TextOverlayManager textoverlaymanager;
    public GameManager gamemanager;

    private void OnTriggerEnter(Collider other)
    {
        //when collides with player
        if (other.name == "Player")
        {
            gamemanager.decrementFlowers();
            if (gamemanager.getFlowersLeft() > 0)
            {
                sfxmanager.playSFX(SFXManager.clips.DING);
                textoverlaymanager.startTextOverlay(gamemanager.getFlowersLeft().ToString());
            }

            //destroy self
            Destroy(gameObject);
        }
    }
}
