using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityOption : MonoBehaviour, IPointerClickHandler
{
    public TreeAbilities ActiveAbility { get; private set; } = null;
    public bool IsAbilityActive => (ActiveAbility != null) && (AbilityController.Instance.ActiveAbilityOption == this);

    [SerializeField] private Image imageUI = null;

    private Color alpha;

    private void Start()
    {
        alpha = imageUI.color;
    }

    private void Update()
    {
        imageUI.raycastTarget = IsAbilityActive;

        alpha.a = IsAbilityActive ? 1f : 0.4f;
        imageUI.color = alpha;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AbilityController.Instance.SetActiveOption(this);
        TreeController.Instance.SetActiveAbility(ActiveAbility);
    }

    public void SetAbilityOption(TreeAbilities newAbility)
    {
        ActiveAbility = newAbility;
        imageUI.sprite = newAbility.TreeAbilitySprite;
    }
}
