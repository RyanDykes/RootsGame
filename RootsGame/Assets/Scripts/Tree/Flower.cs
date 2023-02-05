using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Flower : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int health = 3;
    [SerializeField] private float growingTime = 5f;
    [SerializeField] private Collider clickCollider = null;
    [SerializeField] private Transform experienceOrb = null;
    [SerializeField] private AnimationCurve spawnCurve = null;

    private Coroutine growingCoroutine = null;
    private bool isReady = false;

    public void Initialize()
    {
        if (growingCoroutine != null) StopCoroutine(growingCoroutine);
        growingCoroutine = StartCoroutine(GrowingDelayCoroutine());
    }

    private void Update()
    {
        clickCollider.enabled = isReady;
    }

    public void TakeDamage()
    {
        health--;
        if (health < 1)
        {
            DestroyFlower();
        }
    }

    public void DestroyFlower()
    {
        Destroy(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isReady)
            return;

        PlayerSingleton.Instance.RecieveExp(100);
        DestroyFlower();
    }

    private IEnumerator GrowingDelayCoroutine()
    {
        yield return new WaitForSeconds(growingTime);

        if (growingCoroutine != null) StopCoroutine(growingCoroutine);
        growingCoroutine = StartCoroutine(GrowingCoroutine());
    }

    private IEnumerator GrowingCoroutine()
    {
        float time = 0f;
        float duration = 0.4f;
        Vector3 startScale = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        experienceOrb.gameObject.SetActive(true);

        while (time < duration)
        {
            float T = time / duration;
            experienceOrb.localScale = Vector3.Lerp(startScale, targetScale, T) + (Vector3.one * spawnCurve.Evaluate(T));
            isReady = (T > 0.5f);
            time += Time.deltaTime;
            yield return null;
        }

        experienceOrb.localScale = Vector3.one;
        isReady = true;
    }
}
