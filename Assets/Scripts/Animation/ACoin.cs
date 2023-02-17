using UnityEngine;

public class ACoin : MonoBehaviour
{
    [Header("Coin rotation")]
    [SerializeField] private Vector3 direction;

    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private float enemyDistanceSpawn = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Todo: pos Player + distance du player
            Vector3 enemySpawnPosition = other.transform.position + enemyDistanceSpawn * Random.insideUnitSphere;
            _waveManager.SpawnEnemy(enemySpawnPosition);
        }
    }
}
