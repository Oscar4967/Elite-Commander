using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP class for managing boss encounters
public class BossAI : MonoBehaviour
{



    // Turn off EnemySpawner
    // Activate paths
    // 1. Enter path (0)
    // Paths (1)->(2)->(3)->(4)
    // random, either keep doing current path if 1 or 3, or move to the next one (1->2, 3->4) and repeat
    // When dead, if boss --> turn on EnemySpawner again (in Enemy.cs)

    EnemySpawner enemySpawner;
    



    void Start()
    {
    }



    void Update()
    {
        
    }
}
