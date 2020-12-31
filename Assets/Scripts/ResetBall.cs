using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ball = collision.gameObject;
        
        if (ball.name == "Ball" && !ball.GetComponent<BallController>().wonLevel)
        {
            ball.GetComponent<BallController>().resetBall();
        }
    }
}
