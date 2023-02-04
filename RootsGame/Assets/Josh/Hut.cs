using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hut : MonoBehaviour
{
    public float MinSpawnDelay = 3.0f;
    public float MaxSpawnDelay = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(InstantiateObject), Random.Range(MinSpawnDelay, 0.0f));
    }

    void InstantiateObject()
    {
        var enemy = WaveCoordinatorSingleton.Instance.GetEnemy();
        if (enemy != null)
        {
            Instantiate(enemy, transform.position, transform.rotation);
            Invoke(nameof(InstantiateObject), Random.Range(MinSpawnDelay, MaxSpawnDelay));
        } 
        else
        {
            WaveCoordinatorSingleton.Instance.FinishSpawner();

            // Inefficient have Wave Coordinator start this again
            Invoke(nameof(InstantiateObject), Random.Range(MinSpawnDelay, MaxSpawnDelay));
        }
    }
}
