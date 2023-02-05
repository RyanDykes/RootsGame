using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeController : MonoBehaviour
{
    public static TreeController Instance = null;

    public List<TreeAbilities> UnlockedTreeAbilities => unlockedTreeAbilities;

    public int RootCount = 0;
    public int RootAmount = 5;

    private float cooldownTime = 2;
    [SerializeField] private float maxCooldownTime = 2;

    public bool IsAbilityActive => (ActiveAbility != null);
    public TreeAbilities ActiveAbility { get; private set; } = null;

    [SerializeField] private List<Image> rootsUI = null;
    [SerializeField] private List<TreeAbilities> unlockedTreeAbilities = null;
    [SerializeField] private List<TreeAbilities> allAbilities = null;

    private List<TreeAbilities> activeTreeAbilities = new List<TreeAbilities>();
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rootsUI.ForEach(r => r.gameObject.SetActive(false));
        for(int i = 0; i < RootAmount; i++)
        {
            rootsUI[i].gameObject.SetActive(true);
        }

        AbilityController.Instance.SetNewAbility(allAbilities[0]);
        SetActiveAbility(allAbilities[0]);
        AbilityController.Instance.SetActiveOption(AbilityController.Instance.AbilityOptions[0]);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void SetActiveAbility(TreeAbilities newActiveAbility)
    {
        ActiveAbility = newActiveAbility;
    }

    public void UnlockNewAbility(TreeAbilities newUnlock)
    {
        if (unlockedTreeAbilities.Contains(newUnlock))
            return;

        unlockedTreeAbilities.Add(newUnlock);
    }

    public void SpawnAbility(Vector3 spawnPosition)
    {
        TreeAbilities spawnedAbility = Instantiate(ActiveAbility, spawnPosition, Quaternion.identity);
        activeTreeAbilities.Add(spawnedAbility);
        spawnedAbility.Spawn(spawnPosition);
    }

    public void IncreaseRootAmount(int increaseAmount)
    {
        RootAmount += increaseAmount;
    }

    public void LowerRootCooldown(float decreaseAmount)
    {
        cooldownTime -= decreaseAmount;
        cooldownTime = Mathf.Clamp(cooldownTime, 0f, maxCooldownTime);
    }

    public void SpawnRoot()
    {
        RootCount++;
        RootCount = Mathf.Clamp(RootCount, 0, RootAmount);
        SetAlpha(rootsUI[RootAmount - RootCount], 0.5f);
        StartCoroutine(RootCooldownCoroutine());
    }

    private IEnumerator RootCooldownCoroutine()
    {
        yield return new WaitForSeconds(cooldownTime);

        RootCount--;
        RootCount = Mathf.Clamp(RootCount, 0, RootAmount);
        SetAlpha(rootsUI[(RootAmount - RootCount) - 1], 1f);
    }

    private void SetAlpha(Image rootImage, float value)
    {
        Color alpha = rootImage.color;
        alpha.a = value;
        rootImage.color = alpha;
    }
}
