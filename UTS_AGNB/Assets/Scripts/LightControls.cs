using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControls : MonoBehaviour
{
    public float chance;
    public float time;

    private Light lights;
    private float timer;

    private bool on = true;

    private void Start()
    {
        timer = time;
        lights = GetComponent<Light>();
    }

    void Update()
    {
        if (timer <= 0)
        {
            float rng = Random.Range(0, 100)/100f;
            Debug.Log(rng);
            
            if (rng < chance && on)
            {
                lights.enabled = false;
                on = false;
            }
            else
            {
                lights.enabled = true;
                on = true;
                timer = time;
            }
        }

        timer -= Time.deltaTime;
    }
}
