using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Text healthText;
    int playerHealth;
    Player player;

    void Start()
    {
        healthText = GetComponent<Text>();
        player = FindObjectOfType<Player>();        
    }

    void Update()
    {
        if (player)
        {
            playerHealth = player.GetHealth();
            healthText.text = ("LIVES: " + playerHealth.ToString());
        }
        else
        {
            healthText.text = "LIVES: 0";
        }
    }
}
