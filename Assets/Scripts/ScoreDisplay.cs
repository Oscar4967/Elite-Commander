﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{

    Text scoreText;
    GameSession gameSession;

    void Start()
    {
        scoreText = GetComponent<Text>();
        gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        if (gameSession)
        {
            scoreText.text = gameSession.GetScore().ToString();
        }
        else
        {
            scoreText.text = "N/A";
        }
    }
}
