using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpRoots : MonoBehaviour
{
    public bool IsActive { get; set; } = false;

    [SerializeField] private MeshRenderer rootRenderer = null;
    [SerializeField] private Material fillingMaterial = null;

    [SerializeField] private ExpRoots parentRoot = null;

    [SerializeField] private UnlockableSkill unlockableSkill = null;
    [SerializeField] private float startOffsetAmount = -0.4f;

    private Material activeMaterial = null;

    private void Start()
    {
        if(unlockableSkill != null)
            unlockableSkill.OnClick += GrowRoots;
        activeMaterial = new Material(fillingMaterial);
        rootRenderer.material = activeMaterial;
        activeMaterial.SetTextureOffset("_MainTex", new Vector2(0f, startOffsetAmount));
    }

    private void OnDestroy()
    {
        if (unlockableSkill != null)
            unlockableSkill.OnClick -= GrowRoots;
    }

    private void GrowRoots()
    {
        StartCoroutine(GrowRootsCoroutine());
    }

    private IEnumerator GrowRootsCoroutine()
    {
        if (parentRoot != null)
        {
            parentRoot.GrowRoots();
            while (!parentRoot.IsActive)
                yield return null;
        }

        float time = 0f;
        float duration = 0.6f;

        float startOffset = activeMaterial.GetTextureOffset("_MainTex").y;
        float targetOffset = 1f;

        while (time <= duration)
        {
            float T = time / duration;
            float offset = Mathf.Lerp(startOffset, targetOffset, T);
            activeMaterial.SetTextureOffset("_MainTex", new Vector2(0f, offset));

            time += Time.deltaTime;
            yield return null;
        }

        activeMaterial.SetTextureOffset("_MainTex", new Vector2(0f, 1f));
        IsActive = true;
    }
}
