using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public GameObject target;
    public GameObject[] searchspot;
    public PlayerController player;
    public Animator animator;
    public AudioSource roar_player;
    private AudioSource steps;

    public float idle_time;
    public float idle_variance;
    private float idle_timer = 0;
    private bool currently_idle = false;

    private bool currently_searching = false;

    public float chasing_speed;
    public float chasing_time;
    private float chasing_timer = 0;
    private bool currently_chasing = false;

    public float tracking_time;
    private float tracking_timer;
    
    public float sight_angle;
    public float sight_range;
    public float hearing_range;

    private NavMeshAgent agent;

    public enum EnemyStates
    {
        IDLE,
        SEARCHING,
        ALERT,
        CHASING,
        DOOM
    }

    private EnemyStates state;


    public void setState(EnemyStates new_state)
    {
        state = new_state;
    }

    private Vector3 getClosestPoint()
    {
        Vector3 min_spot = searchspot[0].transform.position;
        float min = Vector3.Distance(transform.position, searchspot[0].transform.position);
        float temp;
        //iterate through all searchpoints and get the shortest one to the player
        for (int i = 1; i < searchspot.Length; i++)
        {
            temp = Vector3.Distance(target.transform.position, searchspot[i].transform.position);
            if (temp < min)
            {
                min_spot = searchspot[i].transform.position;
            }
        }

        return min_spot;
    }

    


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        state = EnemyStates.IDLE;
        steps = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //if monster is moving
        if (agent.velocity.magnitude >= 0.1f)
        {
            animator.SetBool("moving", true);
            steps.volume = 1;
        }
        else
        {
            animator.SetBool("moving", false);
            steps.volume = 0;
        }

        //if player is running within a certain distance
        if(Vector3.Distance(target.transform.position, transform.position) < hearing_range && player.getRunning() && !currently_chasing)
        {
            animator.SetTrigger("player_found");
            roar_player.Play();
            state = EnemyStates.ALERT;
        }

        //if player enters possible LOS
        if (Vector3.Angle(transform.forward, (target.transform.position - transform.position)) <= sight_angle / 2f)
        {
            //checks using raycasting if player is within los
            RaycastHit check;
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out check, sight_range))
            {
                if (check.transform.tag == "Player")
                {
                    //if not chasing then enter alert mode and roar
                    if (!currently_chasing)
                    {
                        animator.SetTrigger("player_found");
                        roar_player.Play();
                        state = EnemyStates.ALERT;
                    }
                    else
                    {
                        // refresh chase timer, track timer and current known player position
                        chasing_timer = chasing_time;
                        tracking_timer = tracking_time;
                        agent.SetDestination(target.transform.position);
                    }
                }
            }
        }

        switch (state)
        {
            //when idle
            case EnemyStates.IDLE:
                currently_chasing = false;
                if (!currently_idle) {
                    currently_idle = true;
                    //start idle timer with a small variance
                    idle_timer = idle_time + (Random.Range(-1, 1) * idle_variance) * idle_time;
                }
                else {
                    //tick down
                    idle_timer -= Time.deltaTime;
                    if (idle_timer <= 0)
                    {
                        //when time is up switch to searching
                        currently_idle = false;
                        state = EnemyStates.SEARCHING;
                    }
                }
            break;

            case EnemyStates.SEARCHING:
                if (!currently_searching)
                {
                    //pick one search target and go to it
                    GameObject search_target = searchspot[Random.Range(0, searchspot.Length-1)];
                    agent.SetDestination(search_target.transform.position);
                    currently_searching = true;
                }
                else
                {
                    //if near destination, become idle again
                    if (Vector3.Distance(transform.position, agent.destination) < 1f)
                    {
                        state = EnemyStates.IDLE;
                        currently_searching = false;
                    }
                }
            break;

            case EnemyStates.ALERT:
                currently_chasing = true;
                chasing_timer = chasing_time;
                tracking_timer = tracking_time;
                
                agent.SetDestination(target.transform.position);
                state = EnemyStates.CHASING;
            break;

            case EnemyStates.CHASING:
                if (Vector3.Distance(target.transform.position, transform.position) < 2.5f && GameManager.instance.getPlaying())
                {
                    GameManager.instance.gameover();
                }

                //enemy knows position for a short time after sighting player
                if (tracking_timer > 0)
                {
                    agent.SetDestination(target.transform.position);
                    tracking_timer -= Time.deltaTime;
                }
                //if not then
                else
                {
                    //if enemy reaches latest known position, change to the closest searchpoint to player
                    if (Vector3.Distance(transform.position, agent.destination) <= 1f)
                    {
                        agent.SetDestination(getClosestPoint());
                    }
                }

                //if the enemy doesnt find the player after a certain amount of time, enter idle mode
                if (chasing_timer <= 0)
                {
                    state = EnemyStates.IDLE;
                }
                else
                {
                    chasing_timer -= Time.deltaTime;
                }
            break;

            case EnemyStates.DOOM:
                agent.SetDestination(target.transform.position);
                agent.speed = 15;
            break;
        }

        Debug.DrawLine(transform.position, agent.destination, Color.white);
    }
}
