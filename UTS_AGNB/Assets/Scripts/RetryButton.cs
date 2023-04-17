using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Retry(string scene)
    {
        GameManager.instance.resume();
        SceneManager.LoadScene(scene);
    }
}
