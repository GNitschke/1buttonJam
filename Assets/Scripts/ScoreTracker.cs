using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public int totalScore;
    public string nextLevel;
    public string levelTracker;

    void Start()
    {
        totalScore = 0;
        DontDestroyOnLoad(this);
        nextLevel = "Level 1";
        levelTracker = "•|···|···|···|···";
    }
}
