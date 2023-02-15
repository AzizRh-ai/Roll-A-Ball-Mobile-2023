using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefab;

    [Header("Number of enemy")]
    [SerializeField] private int nbEnemy = 8;

    [Header("Spawn Scenario")]
    [SerializeField] private Scenario scenario;

    private void Start()
    {

        for (int i = 0; i < nbEnemy; i++)
        {
            //init enemy x fois en se basant sur scenario..
            Instantiate(enemyPrefab[0], scenario.EnemySpawnPosition[Random.Range(0, scenario.EnemySpawnPosition.Length)], Quaternion.identity);
        }
    }

}
