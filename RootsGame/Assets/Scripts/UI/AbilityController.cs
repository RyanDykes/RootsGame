using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private List<AbilityOption> abilityOptions = null;
    [SerializeField] private List<TreeAbilities> allAbilities = null;

    private void Start()
    {
        for (int i = 0; i < abilityOptions.Count; i++)
        {
            if (i >= allAbilities.Count)
                break;

            if (!abilityOptions[i].IsAbilityActive)
            {
                abilityOptions[i].SetAbilityOption(allAbilities[i]);
            }
        }
    }

    public void SetNewAbility(TreeAbilities newAbility)
    {
        for(int i = 0; i < abilityOptions.Count; i++)
        {
            if (!abilityOptions[i].IsAbilityActive)
            {
                abilityOptions[i].SetAbilityOption(newAbility);
                break;
            }
        }
    }
}
