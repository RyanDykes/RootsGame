using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhompingRoot : TreeAbilities
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private float grabRadius = 1f;
    [SerializeField] private SphereCollider grabCollider = null;
    [SerializeField] private Transform grabIndicator = null;
    [SerializeField] private Transform grabTransform = null;

    private Coroutine followCoroutine = null;
    private Coroutine whompCoroutine = null;
    private Coroutine enemyCoroutine = null;
    private Coroutine throwCoroutine = null;
    private Coroutine destroyCoroutine = null;

    private Enemy activeEnemy = null;
    private Enemy previousEnemy = null;

    private const float whompDelayAmount = 1f;

    private void Start()
    {
        grabIndicator.localScale = Vector3.one * grabRadius;
        grabCollider.radius = grabRadius;
    }

    public override void Spawn(Vector3 spawnPosition)
    {
        base.Spawn(spawnPosition);

        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
        spawnCoroutine = StartCoroutine(SpawnCoroutine(spawnPosition));
    }

    public void GrabEnemy()
    {
        if(activeEnemy != null)
            activeEnemy.IsGrabbed = true;

        if (followCoroutine != null) StopCoroutine(followCoroutine);
        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);
        enemyCoroutine = StartCoroutine(SetEnemyPositionCoroutine(grabTransform));
    }

    public void ThrowEnemy()
    {
        if (enemyCoroutine != null) StopCoroutine(enemyCoroutine);

        if (activeEnemy == null)
            return;

        if (throwCoroutine != null) StopCoroutine(throwCoroutine);
        throwCoroutine = StartCoroutine(ThrowEnemyCoroutine());
    }

    private IEnumerator SpawnCoroutine(Vector3 spawnPosition)
    {
        float time = 0f;
        float duration = spawnSpeed;
        const float heightMultiplier = 0.3f;
        Vector3 startPosition = spawnPosition + (spawnOffset);
        Vector3 targetPosition = spawnPosition;
        spawnPosition.y = 0f;

        while (time < duration)
        {
            float T = time / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, spawnCurve.Evaluate(T)) + (Mathf.Sin(Mathf.PI * T) * heightMultiplier * Vector3.up);

            time += Time.deltaTime;
            yield return null;
        }

        IsActive = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!IsActive || other.transform == previousEnemy || activeEnemy != null)
            return;

        if (other.CompareTag("Enemy") && activeEnemy == null && other.TryGetComponent(out Enemy enemy))
        {
            activeEnemy = enemy;
            if (followCoroutine != null) StopCoroutine(followCoroutine);
            followCoroutine = StartCoroutine(FollowCoroutine());

            if (whompCoroutine != null) StopCoroutine(whompCoroutine);
            whompCoroutine = StartCoroutine(WhompDelayCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsActive || activeEnemy == null)
            return;

        if (other.CompareTag("Enemy") && other.transform == activeEnemy.transform)
        {
            if (followCoroutine != null) StopCoroutine(followCoroutine);

            activeEnemy = null;
            if (whompCoroutine != null) StopCoroutine(whompCoroutine);
        }
    }

    private IEnumerator FollowCoroutine()
    {
        float time = 0f;
        float duration = whompDelayAmount;

        while(time < duration && activeEnemy != null)
        {
            float T = time / duration;
            Vector3 enemyPosition = activeEnemy.transform.position;
            enemyPosition.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(enemyPosition - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, T);

            time += Time.deltaTime;
            yield return null;
        }
    }

    readonly WaitForSeconds whompDelay = new WaitForSeconds(whompDelayAmount);
    private IEnumerator WhompDelayCoroutine()
    {
        yield return whompDelay;

        if (activeEnemy != null)
        {
            IsActive = false;
            animator.SetTrigger("Whomp");
        }
    }

    private IEnumerator SetEnemyPositionCoroutine(Transform target)
    {
        while (activeEnemy != null)
        {
            activeEnemy.transform.SetPositionAndRotation(target.position, target.rotation);
            yield return null;
        }
    }

    private IEnumerator ThrowEnemyCoroutine()
    {
        float time = 0f;
        float duration = 0.8f;
        float heightMultiplier = Random.Range(4f, 6f);

        Vector3 startPosition = activeEnemy.transform.position;
        Vector3 targetPosition = transform.position + (transform.forward * -25f);

        while (time < duration && activeEnemy != null)
        {
            float T = time / duration;
            activeEnemy.transform.position = Vector3.Lerp(startPosition, targetPosition, T) + (Mathf.Sin(Mathf.PI * T) * heightMultiplier * Vector3.up);

            time += Time.deltaTime;
            yield return null;
        }

        if (activeEnemy != null)
            activeEnemy.IsGrabbed = false;

        previousEnemy = activeEnemy;
        activeEnemy = null;

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

        Destroy(gameObject);
        //RD_NOTE: SPAWN SKULL AND REMOVE ENEMY
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }
#endif
}
