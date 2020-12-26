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

    private bool up;

    void Start()
    {
        scoreTracker = FindObjectOfType<ScoreTracker>();
        level = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        score = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        if (scoreTracker.nextLevel != "Level 1")
        {
            for (int i = 0; i < scoreTracker.levelTracker.Length; i++)
            {
                if (scoreTracker.levelTracker[i] + "" == "•")
                {
                    if (i < scoreTracker.levelTracker.Length + 1)
                    {
                        if (scoreTracker.levelTracker[i + 1] + "" == "|")
                        {
                            scoreTracker.levelTracker = scoreTracker.levelTracker.Substring(0, i) + "·|•" + scoreTracker.levelTracker.Substring(i + 3);
                        }
                        else
                        {
                            scoreTracker.levelTracker = scoreTracker.levelTracker.Substring(0, i) + "·•" + scoreTracker.levelTracker.Substring(i + 2);
                        }
                    }
                    break;
                }
            }
        }

        GameObject.Find("ScenePieces").transform.Find("Info").GetChild(2).GetComponent<TextMeshProUGUI>().text = scoreTracker.levelTracker;

        level.text = scoreTracker.nextLevel;
        score.text = "Score: " + scoreTracker.totalScore;
        up = false;
    }

    void Update()
    {
        if (Input.anyKey)
        {
            up = true;
        }
        else if (up)
        {
            SceneManager.LoadScene("Scenes/" + scoreTracker.nextLevel);
        }
    }
}
