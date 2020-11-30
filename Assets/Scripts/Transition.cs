using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private ScoreTracker scoreTracker;

    private TextMeshProUGUI level;
    private TextMeshProUGUI score;

    void Start()
    {
        scoreTracker = FindObjectOfType<ScoreTracker>();
        level = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        score = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        level.text = scoreTracker.nextLevel;
        score.text = "Score: " + scoreTracker.totalScore;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Scenes/" + scoreTracker.nextLevel);
        }
    }
}
