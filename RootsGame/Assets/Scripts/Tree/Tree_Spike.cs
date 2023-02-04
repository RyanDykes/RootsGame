using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Spike : TreeAbilities
{
    public bool IsActive { get; set; } = false;

    [SerializeField] private List<Transform> impaleTransforms = null;

    private Coroutine enemyCoroutine = null;

    public override void Spawn(Vector3 spawnPosition)
    {
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnCoroutine(spawnPosition));
    }

    private IEnumerator SpawnCoroutine(Vector3 spawnPosition)
    {
        float time = 0f;
        float duration = spawnSpeed;
        const float heightMultiplier = 0.3f;
        const float scaleMultiplier = 0.3f;
        Vector3 startPosition = spawnPosition + (spawnOffset);
        Vector3 targetPosition = spawnPosition;
        spawnPosition.y = 0f;
        Vector3 baseScale = Vector3.one;

        IsActive = true;

        while (time < duration)
        {
            float T = time / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, spawnCurve.Evaluate(T)) + (Mathf.Sin(Mathf.PI * T) * heightMultiplier * Vector3.up);
            transform.localScale = baseScale + (Mathf.Sin(Mathf.PI * T) * scaleMultiplier * Vector3.up);

            time += Time.deltaTime;
            yield return null;
        }

        IsActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActive)
            return;

        if (other.CompareTag("Enemy"))
        {
            //RD_TODO: Add enemy call to stop moving
            Transform impaleTransform = impaleTransforms[Random.Range(0, impaleTransforms.Count)];
            if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
            enemyCoroutine = StartCoroutine(SetPlayerPositionCoroutine(other.transform, impaleTransform));
        }
    }

    private IEnumerator SetPlayerPositionCoroutine(Transform enemy, Transform target)
    {
        while (true)
        {
            enemy.transform.SetPositionAndRotation(target.position, target.rotation);
            yield return null;
        }
    }
}
