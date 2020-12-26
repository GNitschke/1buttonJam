using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private bool up;

    private void Start()
    {
        up = false;
    }
    void Update()
    {
        if (Input.anyKey)
        {
            up = true;
        }
        else if(up)
        {
            SceneManager.LoadScene("Scenes/TransitionLevel");
        }
    }
}
