using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hut : MonoBehaviour
{
    private float _minSpawnDelay = 3.0f;
    private float _maxSpawnDelay = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(InstantiateObject), Random.Range(_minSpawnDelay, 0.0f));
    }

    void InstantiateObject()
    {
        var enemy = WaveCoordinatorSingleton.Instance.GetEnemy();
        if (enemy != null)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            Invoke(nameof(InstantiateObject), Random.Range(_minSpawnDelay, _maxSpawnDelay));
        } 
        else
        {
            WaveCoordinatorSingleton.Instance.FinishSpawner();

            // Inefficient have Wave Coordinator start this again
            Invoke(nameof(InstantiateObject), Random.Range(_minSpawnDelay, _maxSpawnDelay));
        }
    }
}
