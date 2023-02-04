using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    [SerializeField] private Flower flowerPrefab = null;

    private Coroutine spawningCoroutine = null;
    private const float treeRootSize = 4.5f;
    private float spawnRange = 10f;

    private void Start()
    {
        if (spawningCoroutine != null) StopCoroutine(spawningCoroutine);
        spawningCoroutine = StartCoroutine(SpawningCoroutine());
    }

    public void SetSpawnRange(float newSpawnRange)
    {
        spawnRange = newSpawnRange;
    }

    private IEnumerator SpawningCoroutine()
    {
        while (true)
        {
            SpawnNewFlower();

            yield return new WaitForSeconds(Random.Range(5f, 7f));
        }
    }

    private void SpawnNewFlower()
    {
        Vector3 randomPosition = (Random.insideUnitCircle * spawnRange);

        Debug.DrawLine(new Vector3(randomPosition.x, 0f, randomPosition.y), new Vector3(randomPosition.x, 0f, randomPosition.y) + Vector3.up, Color.yellow, 10f);

        Vector3 normalizedPosition = (randomPosition - transform.position).normalized * treeRootSize;

        Debug.DrawLine(new Vector3(normalizedPosition.x, 0f, normalizedPosition.y), new Vector3(normalizedPosition.x, 0f, normalizedPosition.y) + Vector3.up, Color.red, 10f);
        Vector3 spawnPosition = new Vector3(randomPosition.x + normalizedPosition.x, 0f, randomPosition.y + normalizedPosition.y);
        Quaternion targetRotation = Quaternion.LookRotation(spawnPosition - transform.position);
        Flower newFlower = Instantiate(flowerPrefab, spawnPosition, targetRotation);
        newFlower.Initialize();
    }
}
