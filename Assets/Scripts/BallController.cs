using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    private bool charging;
    public bool readyForLaunch;

    public Goal[] goals;

    private float maxPower;
    private float power;
    private Rigidbody2D rb;
    public bool anti;

    public string nextLevel;

    private RectTransform strengthBar;
    private Text powerText;
    private AudioSource chargeSound;

    public int currentScore;
    public int levelScore;

    private ScoreTracker scoreTracker;

    void Start()
    {
        charging = false;
        anti = false;
        readyForLaunch = true;
        power = 100;
        maxPower = 480;
        rb = GetComponent<Rigidbody2D>();
        strengthBar = transform.GetChild(1).GetComponent<RectTransform>();
        chargeSound = transform.GetChild(0).GetComponent<AudioSource>();
        powerText = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        scoreTracker = FindObjectOfType<ScoreTracker>();
        rb.gravityScale = 0;

        currentScore = 0;
        levelScore = 1000;
    }


    void FixedUpdate()
    {
        if (Input.anyKey && readyForLaunch)
        {
            if (!charging)
            {
                chargeSound.Play();
            }
            else if(power >= maxPower)
            {
                chargeSound.Stop();
            }
            charging = true;
            if (power < maxPower)
            {
                power += 5f;
                rb.velocity = new Vector2(0, 0);
                rb.gravityScale = 0;
                transform.position = new Vector2(0, -2.8f);
                powerText.text = "" + (int)((power-100f)/(maxPower - 100f) * 100f);
                strengthBar.localPosition = new Vector3(0, 0.7f + 1.5f * (power - 100f)/(maxPower - 100f), 0);
            }
        }
        else if (charging)
        {
            chargeSound.Stop();
            charging = false;
            readyForLaunch = false;
            rb.gravityScale = 1f;
            powerText.text = "";
            rb.AddForce(Vector2.up * power);
            strengthBar.localPosition = new Vector3(0, 0.7f, 0);
            power = 100;
        }
        else if (readyForLaunch)
        {
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
            transform.position = new Vector2(0, -2.8f);
        }
    }

    public void resetBall()
    {
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        transform.position = new Vector2(0, -2.8f);
        if(GetComponent<SpriteRenderer>().enabled)
            readyForLaunch = true;
        levelScore = levelScore > 100 ? levelScore - 100 : 100;
    }

    public void checkWin()
    {
        bool won = true;
        currentScore += levelScore;
        levelScore += 100;
        
        foreach (Goal goal in goals)
        {
            if (!goal.caught)
            {
                won = false;
                resetBall();
                break;
            }
        }
        if (won)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            levelScore = 1000;
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        scoreTracker.totalScore += currentScore;
        scoreTracker.nextLevel = nextLevel;
        yield return new WaitForSeconds(2f);
        if (nextLevel != "End")
        {
            SceneManager.LoadScene("Scenes/TransitionLevel");
        }
        else
        {
            SceneManager.LoadScene("Scenes/End");
        }
    }
}
