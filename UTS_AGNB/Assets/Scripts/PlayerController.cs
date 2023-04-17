using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    // Global variables
    public CharacterController controls;
    public HeadBobControls headBobControls;
    public GameObject stopwatch;
    public Animator Animator;
    public CanvasGroup minimap;
    public AudioSource map_sfx;
    public AudioSource timer_sfx;

    public AudioManager audioManager;

    float movement_speed;
    public float walking_speed;
    public float running_speed;
    public float crouching_speed;

    private bool toggle_map = false;
    private bool running = false;


    public bool getRunning()
    {
        return running && Input.GetKey(KeyCode.LeftShift);
    }

    private void toggleMap(bool on)
    {
        if (on)
        {
            map_sfx.Play();
            minimap.alpha = 1;
        }
        else
        {
            if (map_sfx.isPlaying)
            {
                map_sfx.Stop();
            }
            minimap.alpha = 0;
        }
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Gets movement inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        running = (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0);
        Animator.SetBool("running", running);

        //use running speeed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement_speed = running_speed;
        }
        //use crouching speed
        else if(Input.GetKey(KeyCode.LeftControl))
        {
            movement_speed = crouching_speed;
        }
        //use walking speed
        else
        {
            movement_speed = walking_speed;
        }
        Vector3 direction = transform.right * horizontalInput + transform.forward * verticalInput;


        // if right click is pressed
        if (Input.GetButtonDown("Fire2"))
        {
            timer_sfx.volume = 1;
            Animator.SetTrigger("pullingout");
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            Animator.SetTrigger("pullingback");
            timer_sfx.volume = 0;
        }

        if (Input.GetButtonDown("Minimap"))
        {
            toggle_map = !toggle_map;
            toggleMap(toggle_map);
        }

        if (!toggle_map)
        {
            controls.Move(direction * movement_speed * Time.deltaTime);
        }
        
    }
}
