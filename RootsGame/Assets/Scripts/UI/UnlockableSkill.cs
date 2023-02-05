using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnlockableSkill : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int requiredEXP = 250;

    [SerializeField] private bool increaseAmount = false;
    [SerializeField] private int rootIncreaseAmount = 0;

    [SerializeField] private bool lowerCooldown = false;
    [SerializeField] private float reduceRootCooldownAmount = 0;

    [SerializeField] private Image abilityIcon = null;
    [SerializeField] private TreeAbilities ability = null;

    private bool isActive = false;

    private void Start()
    {
        Color alpha = abilityIcon.color;
        alpha.a = 0.4f;
        abilityIcon.color = alpha;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isActive)
            return;

        if (PlayerSingleton.Instance.Experience >= requiredEXP)
        {
            isActive = false;
            Color alpha = abilityIcon.color;
            alpha.a = 1f;
            abilityIcon.color = alpha;

            if (ability != null)
                TreeController.Instance.UnlockNewAbility(ability);

            if(increaseAmount)
                TreeController.Instance.IncreaseRootAmount(rootIncreaseAmount);

            if (lowerCooldown)
                TreeController.Instance.LowerRootCooldown(reduceRootCooldownAmount);

            PlayerSingleton.Instance.RecieveExp(-requiredEXP);
        }
    }
}
