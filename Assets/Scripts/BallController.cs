using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    private bool charging;
    public bool readyForLaunch;

    public Goal[] goals;

    public float power;
    private Rigidbody2D rb;

    public string nextLevel;

    private RectTransform strengthBar;

    void Start()
    {
        charging = false;
        readyForLaunch = true;
        power = 100;
        rb = GetComponent<Rigidbody2D>();
        strengthBar = transform.GetChild(1).GetComponent<RectTransform>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        if (Input.anyKey && readyForLaunch)
        {
            charging = true;
            if (power < 530f)
            {
                power += 2f;
                strengthBar.localPosition = new Vector3(0, 0.55f + 0.9f * (power-100f)/440f, 0);
            }
        }
        else if (charging)
        {
            charging = false;
            readyForLaunch = false;
            rb.gravityScale = 1f;
            rb.AddForce(Vector2.up * power);
            strengthBar.localPosition = new Vector3(0, 0.472f, 0);
            power = 100;
        }
    }

    public void resetBall()
    {
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        transform.position = new Vector2(0, -2.8f);
        readyForLaunch = true;
    }

    public void checkWin()
    {
        bool won = true;
        foreach(Goal goal in goals)
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
            Debug.Log("You win!");
            StartCoroutine(Win());
        }
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scenes/" + nextLevel);
    }
}
