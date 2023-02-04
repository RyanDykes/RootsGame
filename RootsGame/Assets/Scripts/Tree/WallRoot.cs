using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRoot : TreeAbilities
{
    [SerializeField] private Transform wallRoot1Prefab = null;
    [SerializeField] private Transform wallRoot2Prefab = null;
    [SerializeField] private Transform wallRoot3Prefab = null;

    public override void Spawn(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        transform.rotation = Quaternion.LookRotation(TreeController.Instance.transform.position - transform.position);
        StartCoroutine(SpawnDelayCoroutine());
    }

    public void DestroyWallRoot()
    {
        Destroy(gameObject);
    }

    private readonly WaitForSeconds rootDelay = new WaitForSeconds(0.23f);
    private IEnumerator SpawnDelayCoroutine()
    {
        StartCoroutine(SpawnCoroutine(wallRoot1Prefab));

        yield return rootDelay;

        StartCoroutine(SpawnCoroutine(wallRoot2Prefab));

        yield return rootDelay;

        StartCoroutine(SpawnCoroutine(wallRoot3Prefab));
    }

    private IEnumerator SpawnCoroutine(Transform root)
    {
        float time = 0f;
        float duration = spawnSpeed;
        Vector3 startPosition = root.position;
        Vector3 targetPosition = new Vector3(startPosition.x, 0f, startPosition.z);

        while (time < duration)
        {
            float T = time / duration;
            root.position = Vector3.Lerp(startPosition, targetPosition, spawnCurve.Evaluate(T));

            IsActive = (T > 0.6f);

            time += Time.deltaTime;
            yield return null;
        }
        
        IsActive = true;
        root.position = targetPosition;
    }
}
