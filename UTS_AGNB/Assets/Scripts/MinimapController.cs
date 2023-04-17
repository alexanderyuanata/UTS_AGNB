using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3 (player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
    }
}
