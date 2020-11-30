using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public bool caught;

    private Animator animator;
    private Transform canvas;
    private AudioSource catchSound;
    private AudioSource scoreSound;

    private Text scoreText;
    private Text scoreTextColored;

    void Start()
    {
        caught = false;
        animator = GetComponent<Animator>();
        canvas = transform.GetChild(0);
        canvas.Rotate(0, 0, -transform.rotation.eulerAngles.z);
        scoreText = canvas.GetChild(1).GetComponent<Text>();
        scoreTextColored = canvas.GetChild(0).GetComponent<Text>();
        catchSound = GetComponent<AudioSource>();
        scoreSound = transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject ball = collision.gameObject;

        if (ball.name == "Ball" && !caught)
        {
            caught = true;
            catchSound.Play();
            StartCoroutine(Score(ball.GetComponent<BallController>().levelScore));
            animator.SetTrigger("caught");
            ball.GetComponent<BallController>().checkWin();
        }
    }

    IEnumerator Score(int levelScore)
    {
        yield return new WaitForSeconds(0.12f);
        scoreText.text = "" + levelScore;
        scoreTextColored.text = "" + levelScore;
        scoreTextColored.GetComponent<Animator>().SetTrigger("caught");
        scoreSound.Play();
        for (int i = 0; i < 50; i++)
        {
            yield return new WaitForFixedUpdate();
            scoreText.transform.localPosition = new Vector3(scoreText.transform.localPosition.x, scoreText.transform.localPosition.y + 0.0001f * i, 0);
        }
        scoreText.text = "";
        scoreTextColored.text = "";
    }
}
