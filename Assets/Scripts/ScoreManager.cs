using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text textScore;
    public float score;

    void Start()
    {
        score = 0f;
        textScore.text = "Score: " + score.ToString();
    }

    void Update()
    {
        textScore.text = "Score: " + ((int)score).ToString();
    }
}
