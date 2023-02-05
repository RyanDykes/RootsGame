using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public static AbilityController Instance = null;

    public AbilityOption ActiveAbilityOption { get; private set; } = null;

    public List<AbilityOption> AbilityOptions => abilityOptions;
    [SerializeField] private List<AbilityOption> abilityOptions = null;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (abilityOptions.Count < 1)
                return;

            SetActiveOption(abilityOptions[0]);
            TreeController.Instance.SetActiveAbility(abilityOptions[0].ActiveAbility);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (abilityOptions.Count < 2)
                return;

            SetActiveOption(abilityOptions[1]);
            TreeController.Instance.SetActiveAbility(abilityOptions[1].ActiveAbility);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (abilityOptions.Count < 3)
                return;

            SetActiveOption(abilityOptions[2]);
            TreeController.Instance.SetActiveAbility(abilityOptions[2].ActiveAbility);
        }
    }

    public void SetNewAbility(TreeAbilities newAbility)
    {
        for(int i = 0; i < abilityOptions.Count; i++)
        {
            if (abilityOptions[i].ActiveAbility == null)
            {
                abilityOptions[i].SetAbilityOption(newAbility);
                TreeController.Instance.UnlockNewAbility(newAbility);
                break;
            }
        }
    }

    public void SetActiveOption(AbilityOption activeOption)
    {
        ActiveAbilityOption = activeOption;
    }
}
