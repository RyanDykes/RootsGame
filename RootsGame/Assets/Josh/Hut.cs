using UnityEngine;

public class Hut : MonoBehaviour
{
    public float MinSpawnDelay = 3.0f;
    public float MaxSpawnDelay = 10.0f;

    public void StartSpawning()
    {
        Invoke(nameof(InstantiateObject), Random.Range(MinSpawnDelay, 0.0f));
    }

    void InstantiateObject()
    {
        var enemy = WaveCoordinatorSingleton.Instance.GetEnemy();
        if (enemy != null)
        {
            if (!PlayerSingleton.Instance.GamePaused)
            {
                Instantiate(enemy, transform.position, transform.rotation);
            }
            Invoke(nameof(InstantiateObject), Random.Range(MinSpawnDelay, MaxSpawnDelay));
        } 
        else
        {
            WaveCoordinatorSingleton.Instance.FinishSpawner();
        }
    }
}
