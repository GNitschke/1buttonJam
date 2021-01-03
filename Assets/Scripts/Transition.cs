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
    private bool exiting;

    private Animator wipe;
    private Animator info;

    void Start()
    {
        scoreTracker = FindObjectOfType<ScoreTracker>();
        wipe = GameObject.Find("ScreenWipe").GetComponent<Animator>();
        level = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        score = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        info = GameObject.Find("Info").GetComponent<Animator>();

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
        exiting = false;
    }

    void Update()
    {
        if (!exiting)
        {
            if (Input.anyKey)
            {
                up = true;
            }
            else if (up)
            {
                exiting = true;
                StartCoroutine(nextScene());
            }
        }
    }

    IEnumerator nextScene()
    {
        wipe.SetTrigger("End");
        info.SetTrigger("End");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Scenes/" + scoreTracker.nextLevel);
    }
}
