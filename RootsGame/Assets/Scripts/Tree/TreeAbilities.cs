using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAbilities : MonoBehaviour
{
    public bool IsActive { get; set; } = false;
    public Sprite TreeAbilitySprite => treeAbilitySprite;

    [SerializeField] protected float spawnSpeed = 1f;
    [SerializeField] protected Vector3 spawnOffset = Vector3.zero;
    [SerializeField] protected AnimationCurve spawnCurve = null;
    [SerializeField] protected Sprite treeAbilitySprite = null;

    protected Coroutine spawnCoroutine = null;

    public virtual void Spawn(Vector3 spawnPosition)
    {
        TreeController.Instance.SpawnRoot();
    }
}
