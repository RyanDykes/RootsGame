using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public static TreeController Instance = null;

    public bool IsAbilityActive => (ActiveAbility != null);
    public TreeAbilities ActiveAbility { get; private set; } = null;

    [SerializeField] private List<TreeAbilities> unlockedTreeAbilities = null;

    private List<TreeAbilities> activeTreeAbilities = new List<TreeAbilities>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetActiveAbility(unlockedTreeAbilities[0]);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (unlockedTreeAbilities.Count > 0)
                SetActiveAbility(unlockedTreeAbilities[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(unlockedTreeAbilities.Count > 1)
                SetActiveAbility(unlockedTreeAbilities[1]);
        }
    }

    public void SetActiveAbility(TreeAbilities newActiveAbility)
    {
        ActiveAbility = newActiveAbility;
    }

    public void UnlockNewAbility(TreeAbilities newUnlock)
    {
        unlockedTreeAbilities.Add(newUnlock);
    }

    public void SpawnAbility(Vector3 spawnPosition)
    {
        TreeAbilities spawnedAbility = Instantiate(ActiveAbility, spawnPosition, Quaternion.identity);
        activeTreeAbilities.Add(spawnedAbility);
        spawnedAbility.Spawn(spawnPosition);
    }
}
