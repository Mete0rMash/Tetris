using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    private void Update()
    {
        if (TetrisBlock.addScore)
        {
            score += 250;
            TetrisBlock.addScore = false;
            scoreText.text = score.ToString();

        }
    }
}
