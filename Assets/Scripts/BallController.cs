using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    //private RectTransform strengthBar;
    private Transform trajectory;
    private Transform[] points;
    private Text powerText;
    private AudioSource chargeSound;

    public int currentScore;
    public int levelScore;

    private ScoreTracker scoreTracker;
    private Transform multiplierTracker;

    private Transform info;
    private Animator hand;

    public bool wonLevel;

    void Start()
    {
        charging = false;
        anti = false;
        readyForLaunch = true;
        power = 100;
        maxPower = 480;
        rb = GetComponent<Rigidbody2D>();
        //strengthBar = transform.GetChild(1).GetComponent<RectTransform>();
        trajectory = transform.GetChild(3);
        points = new Transform[trajectory.childCount];
        for(int i = 0; i < trajectory.childCount; i++)
        {
            points[i] = trajectory.GetChild(i);
            points[i].GetComponent<SpriteRenderer>().enabled = false;
        }
        chargeSound = GetComponent<AudioSource>();
        powerText = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        scoreTracker = FindObjectOfType<ScoreTracker>();
        info = GameObject.Find("Info").transform;
        hand = GameObject.Find("Hand").GetComponent<Animator>();
        GetComponent<SpriteRenderer>().enabled = false;
        multiplierTracker = GameObject.Find("MultiplierTracker").transform;

        wonLevel = false;
        
        rb.gravityScale = 0;

        currentScore = 0;
        levelScore = 1000;
        info.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score: " + scoreTracker.totalScore;
        info.GetChild(1).GetComponent<TextMeshProUGUI>().text = scoreTracker.nextLevel;
        info.GetChild(2).GetComponent<TextMeshProUGUI>().text = scoreTracker.levelTracker;
    }


    void FixedUpdate()
    {
        if (Input.anyKey && readyForLaunch)
        {
            foreach (Transform point in points)
            {
                point.GetComponent<SpriteRenderer>().enabled = true;
            }
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
                //strengthBar.localPosition = new Vector3(0, 0.7f + 1.5f * (power - 100f)/(maxPower - 100f), 0);
                int index = 0;
                for (float i = 2f; index < points.Length; i -= 0.4f)//0.9 1.8 2.7 3.6 4.5
                {
                    points[index].localPosition = new Vector3(0, i * (power - 100f) / (maxPower - 100f), 0);
                    index++;
                }
                
            }
        }
        else if (charging)
        {
            chargeSound.Stop();
            charging = false;
            readyForLaunch = false;
            rb.gravityScale = 1f;
            powerText.text = "";
            hand.SetTrigger("throw");
            GetComponent<SpriteRenderer>().enabled = true;
            rb.AddForce(Vector2.up * power);
            //strengthBar.localPosition = new Vector3(0, 0.7f, 0);
            trajectory.GetComponentInChildren<SpriteRenderer>().enabled = false;
            foreach (Transform point in points)
            {
                point.localPosition = new Vector3(0, 0, 0);
                point.GetComponent<SpriteRenderer>().enabled = false;
            }
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
        
        if (GetComponent<SpriteRenderer>().enabled)
        {
            readyForLaunch = true;
            hand.SetTrigger("reset");
        }
        GetComponent<SpriteRenderer>().enabled = false;
        transform.position = new Vector2(0, -2.8f);
        levelScore = levelScore > 100 ? levelScore - 100 : 100;
        for (int i = 9; i > (levelScore/100) - 1; i--)
        {
            multiplierTracker.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void checkWin()
    {
        bool won = true;
        currentScore += levelScore;
        levelScore += 100;

        info.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score: " + (scoreTracker.totalScore + currentScore);

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
            wonLevel = true;
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        //toLowerMultiplier = false;
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
