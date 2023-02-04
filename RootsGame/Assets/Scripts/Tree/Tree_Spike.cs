using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_Spike : TreeAbilities
{
    [SerializeField] private List<Transform> impaleTransforms = null;

    private List<Enemy> impaledEnemies = new List<Enemy>();

    private Coroutine destroyCoroutine = null;
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

            IsActive = T < 0.4f;

            time += Time.deltaTime;
            yield return null;
        }

        IsActive = false;
        transform.position = targetPosition;
        transform.localScale = Vector3.one;

        if (destroyCoroutine != null) StopCoroutine(destroyCoroutine);
        destroyCoroutine = StartCoroutine(DestroyCoroutine());
    }

    readonly WaitForSeconds destroyDelay = new WaitForSeconds(1f);
    private IEnumerator DestroyCoroutine()
    {
        yield return destroyDelay;

        float time = 0f;
        float duration = 0.2f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = transform.position + (spawnOffset);

        while (time < duration)
        {
            float T = time / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, T);

            time += Time.deltaTime;
            yield return null;
        }

        for(int i = 0; i < impaledEnemies.Count; i++)
        {
            Destroy(impaledEnemies[i].gameObject);
        }
        
        Destroy(gameObject);
        //RD_NOTE: SPAWN SKULL AND REMOVE ENEMY
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsActive)
            return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();

            if (enemy.IsDead)
                return;

            impaledEnemies.Add(enemy);
            enemy.IsDead = true;
            enemy.GiveExperience();

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
