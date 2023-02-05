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

    private float cooldownTime = 0;
    [SerializeField] private float maxCooldownTime = 2;

    public bool IsAbilityActive => (ActiveAbility != null);
    public TreeAbilities ActiveAbility { get; private set; } = null;

    [SerializeField] private List<Image> rootsUI = null;
    [SerializeField] private Image rootsUIPrefab = null;
    private List<TreeAbilities> unlockedTreeAbilities = new List<TreeAbilities>();
    [SerializeField] private List<TreeAbilities> allAbilities = null;

    private List<TreeAbilities> activeTreeAbilities = new List<TreeAbilities>();
    

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cooldownTime = maxCooldownTime;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PlayerSingleton.Instance.RecieveExp(500);
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
        RootAmount = (int)Mathf.Clamp(RootAmount, 0f, rootsUI.Count  -1);
        for (int i = 0; i < RootAmount; i++)
        {
            rootsUI[i].gameObject.SetActive(true);
            SetAlpha(rootsUI[i], 1f);
        }

        for (int i = 0; i < RootCount; i++)
        {
            SetAlpha(rootsUI[RootAmount - i], 0.5f);
        }
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
