using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // TODO make some varied spawning routines (multiple routes in same path?)
    // --> try to add the same couple of paths repeating in the waves list


    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] List<WaveConfig> bossWaveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;

    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];

            StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave)); 
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                waveConfig.GetWaypoints()[0].transform.position,
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }

    }

    // Placeholder for pausing waves during a boss encounter
    public void PauseWaves()
    {
        looping = false;
        StopCoroutine(SpawnAllWaves());
    }

    // Placeholder for resuming waves after finishing a boss encounter
    public void ResumeWaves()
    {
        looping = true;
        Start();
    }
}
