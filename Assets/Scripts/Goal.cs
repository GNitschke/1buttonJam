using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool caught;

    public Sprite[] sprites;
    private SpriteRenderer sr;

    void Start()
    {
        caught = false;
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ball = collision.gameObject;

        if (ball.name == "Ball" && !caught)
        {
            caught = true;
            sr.sprite = sprites[1];
            ball.GetComponent<BallController>().checkWin();
        }
    }
}
