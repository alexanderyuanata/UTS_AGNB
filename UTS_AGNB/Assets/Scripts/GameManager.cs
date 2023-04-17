using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public Timer timerscript;
    public static GameManager instance;

    public float initial_time;
    public float time_mult;
    public GameObject player;
    public GameObject flowers;
    public TextOverlayManager textOverlay;
    public GameObject flower_holder;
    public SFXManager sfxmanager;
    public EnemyScript enemy;
    public GameObject pause_screen;
    public GameObject gameover_screen;
    public TextMeshProUGUI number_dis;
    public TextMeshProUGUI rounds_dis;

    public GameObject[] spawns;

    public int numberof_rounds;
    private int rounds;

    private float current_time;

    public int COINS_AMOUNT;

    int flowers_left = 0;

    private bool playing = true;
    private int flowers_gained = 0;

    private float previousTimeScale;
    private bool paused = false;

    private float[] axisValues;

    class spawnpoints
    {
        Transform spawn;
        float distance;

        public spawnpoints(Transform a, float b)
        {
            spawn = a;
            distance = b;
        }
        public float getDist()
        {
            return distance;
        }

        public Transform getSpawn()
        {
            return spawn;
        }
    }

    public void pause()
    {
        previousTimeScale = Time.timeScale;
        AudioListener.pause = true;
        Input.ResetInputAxes();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Time.timeScale = 0f;
        pause_screen.SetActive(true);
        paused = true;
    }

    public void resume()
    {
        Cursor.visible = false;
        paused = false;
        Time.timeScale = previousTimeScale;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        pause_screen.SetActive(false);
    }

    public bool getPlaying()
    {
        return playing;
    }

    public void gameover()
    {
        if (playing)
        {
            previousTimeScale = 1f;
            playing = false;
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
        
        
        number_dis.text = flowers_gained.ToString();
        rounds_dis.text = rounds.ToString();
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        gameover_screen.SetActive(true);
    }

    public void decrementFlowers()
    {
        flowers_gained++;
        flowers_left--;
        if (flowers_left <= 0)
        {
            rounds++;
            generateCoins(COINS_AMOUNT);
        }
    }

    public int getFlowersLeft()
    {
        return flowers_left;
    }

    //generates N objects that act as collectible coins at random with the same transform as preset objects
    public void generateCoins(int amount)
    {
        sfxmanager.playSFX(SFXManager.clips.BELL);
        flowers_left = COINS_AMOUNT;
        List<GameObject> spawnpoints = new List<GameObject>(spawns);
        GameObject spawnpoint;
        int rand;
        for (int i = 0; i < amount; i++)
        {
            rand = UnityEngine.Random.Range(0, spawnpoints.Count);
            spawnpoint = spawnpoints[rand];
            spawnpoints.RemoveAt(rand);
            Instantiate(flowers, spawnpoint.transform.position, spawnpoint.transform.rotation, flower_holder.transform);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rounds = 1;
        playing = true;
        current_time = initial_time;
        generateCoins(COINS_AMOUNT);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);
        if (current_time <= 0 && playing)
        {
            gameover();
        }
        if (playing) {
            if (Input.GetButtonDown("Cancel"))
            {
                if (!paused)
                {
                    pause();
                }
                else
                {
                    resume();
                }
            }

            if (!paused)
            {
                current_time -= Time.deltaTime;
                timerscript.updateTimer(current_time, initial_time);
            } 
        }
        
    }


}
