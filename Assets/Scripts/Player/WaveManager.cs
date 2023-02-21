using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefab;

    [Header("Coin")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Spawn Scenario")]
    [SerializeField] private Scenario scenario;

    public static WaveManager instance;

    void Awake()
    {
        instance = this;
    }

    private void CreateEnemy(Vector3 position)
    {
        Instantiate(enemyPrefab[0], position, Quaternion.identity);
    }

    private void CreateCoin(Vector3 position)
    {
        Instantiate(coinPrefab, position, Quaternion.identity);
    }

    public void SpawnCoin(Vector3 position)
    {
        CreateCoin(position);
    }
    public void SpawnEnemy(Vector3 position)
    {
        CreateEnemy(position);
    }


}
