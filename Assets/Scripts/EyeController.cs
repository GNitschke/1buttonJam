using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Eye
{
    Pull,
    Push,
    LowGrav,
    HighGrav,
    Anti
}

public class EyeController : MonoBehaviour
{
    private bool open;

    public float strength;
    public Eye eyeType;
    private int index;
    public Sprite[] sprites;
    public Color[] beams;

    public bool startOpen;
    public float closedTime;
    public float openTime;
    
    void Start()
    {
        if (closedTime > 0)
        {
            if (startOpen)
            {
                open = true;
                StartCoroutine(Close());
            }
            else
            {
                open = false;
                transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(Open());
            }
        }
        else
        {
            open = true;
        }

        index = 0;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer bsr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        switch (eyeType)
        {
            case Eye.Pull:
                index = 0;
                break;
            case Eye.Push:
                index = 1;
                break;
            case Eye.LowGrav:
                index = 2;
                break;
            case Eye.HighGrav:
                index = 3;
                break;
        }
        sr.sprite = sprites[index];
        bsr.color = beams[index];
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject ball = collision.gameObject;

        if (ball.name == "Ball")
        {
            Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
            if (open && !ball.GetComponent<BallController>().anti)
            {
                switch (eyeType)
                {
                    case Eye.Pull:
                        ballRB.AddForce(transform.up * strength);
                        break;
                    case Eye.Push:
                        ballRB.AddForce(transform.up * -strength);
                        break;
                    case Eye.LowGrav:
                        ballRB.gravityScale = 0.2f;
                        //ballRB.drag = 1f;
                        break;
                    case Eye.HighGrav:
                        ballRB.gravityScale = 1.5f;
                        break;
                    case Eye.Anti:
                        ball.GetComponent<BallController>().anti = true;
                        break;
                }

            }
            else
            {
                switch (eyeType)
                {
                    case Eye.LowGrav:
                        ballRB.gravityScale = 1f;
                        break;
                    case Eye.HighGrav:
                        ballRB.gravityScale = 1f;
                        break;
                    case Eye.Anti:
                        ball.GetComponent<BallController>().anti = false;
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject ball = collision.gameObject;

        if (ball.name == "Ball")
        {
            Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
            switch (eyeType)
            {
                case Eye.LowGrav:
                    ballRB.gravityScale = 1f;
                    break;
                case Eye.HighGrav:
                    ballRB.gravityScale = 1f;
                    break;
                case Eye.Anti:
                    ball.GetComponent<BallController>().anti = false;
                    break;
            }
        }
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(closedTime);
        open = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(openTime);
        open = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(Open());
    }
}
