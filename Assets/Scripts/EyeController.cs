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

    public Sprite[] masks;
    private SpriteMask blinkMask;

    public bool startOpen;
    public float closedTime;
    public float openTime;

    private AudioSource openSound;
    
    void Start()
    {
        index = 0;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer bsr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        openSound = GetComponent<AudioSource>();
        
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
            case Eye.Anti:
                index = 4;
                break;
        }
        sr.sprite = sprites[index];
        bsr.color = beams[index];

        blinkMask = transform.GetChild(1).GetComponent<SpriteMask>();
        if(closedTime == 0)
        {
            blinkMask.sprite = masks[5];
            open = true;
        }
        else if (!startOpen)
        {
            blinkMask.sprite = masks[0];
            open = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Open());
        }
        else
        {
            open = true;
            StartCoroutine(Close());
        }

        //open = false;
        //transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        //StartCoroutine(Open());
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
                        if(!open)
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
        //if (!startOpen)
        yield return new WaitForSeconds(closedTime);
        SpriteRenderer beam = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //openSound.Play();
        beam.enabled = true;
        beam.color = new Color(beam.color.r, beam.color.g, beam.color.b, 0);
        for (int i = 0; i < masks.Length; i++)
        {
            blinkMask.sprite = masks[i];
            //yield return new WaitForFixedUpdate();
            beam.color = new Color(beam.color.r, beam.color.g, beam.color.b, i/(masks.Length-1f) * 0.5f);
            yield return new WaitForFixedUpdate();
        }
        open = true;
        //yield return new WaitForSeconds(openTime);
        if (closedTime > 0)
            StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(openTime);
        SpriteRenderer beam = transform.GetChild(0).GetComponent<SpriteRenderer>();
        for (int i = masks.Length - 1; i >= 0; i--)
        {
            blinkMask.sprite = masks[i];
            //yield return new WaitForFixedUpdate();
            beam.color = new Color(beam.color.r, beam.color.g, beam.color.b, i / (masks.Length - 1f) * 0.5f);
            yield return new WaitForFixedUpdate();
        }
        open = false;
        beam.enabled = false;

        //if(startOpen)
        //    yield return new WaitForSeconds(closedTime);
        StartCoroutine(Open());
    }
}
