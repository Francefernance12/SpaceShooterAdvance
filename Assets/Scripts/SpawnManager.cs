using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject _astroidPrefab;
    [SerializeField] private GameObject[] powerups;
    [Header("Spawning")]
    [SerializeField] private bool _stopSpawning = false;
    [SerializeField] private float _enemySpawnRate = 1.0f;
    [SerializeField] private float _astroidSpawnRate = 10.0f;
    [SerializeField] private float _powerupSpawnRate = 10.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        // Enemy spawn absolute time.
        StartCoroutine(SpawnEnemyRoutine());
        // Powerup spawn random time.
        StartCoroutine(SpawnPowerUpRoutine());
        // Astroid spawn random time.
        StartCoroutine(SpawnAstroidRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-8.5f, 8.5f), 7.95f, 0);
            // Instantiate
            GameObject newEnemy = Instantiate(_enemyPrefab, randomPos, Quaternion.identity);

            // Add to enemy container
            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_enemySpawnRate);
            
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-8.5f, 8.5f), 7.95f, 0);
            // Randomly select a powerup
            int randomIndex = Random.Range(0, powerups.Length);
            GameObject randomPowerup = powerups[randomIndex];
            
            // Instantiate
            GameObject newPowerUp = Instantiate(randomPowerup, randomPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(_powerupSpawnRate, _powerupSpawnRate + 5.0f));
            
        }
    }

    IEnumerator SpawnAstroidRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 randomPos = new Vector3(Random.Range(-8.5f, 8.5f), 7.95f, 0);
            // Instantiate
            GameObject newAstroid = Instantiate(_astroidPrefab, randomPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(_astroidSpawnRate, _astroidSpawnRate + 5.0f));
            
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
