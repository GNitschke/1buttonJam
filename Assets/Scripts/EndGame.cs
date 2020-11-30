using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private ScoreTracker scoreTracker;

    private TextMeshProUGUI score;

    void Start()
    {
        scoreTracker = FindObjectOfType<ScoreTracker>();
        score = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        score.text = "Final Score: " + scoreTracker.totalScore;
    }
}
