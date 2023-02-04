using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityOption : MonoBehaviour, IPointerClickHandler
{
    public bool IsAbilityActive => (activeAbility != null);

    [SerializeField] private Image imageUI = null;

    private TreeAbilities activeAbility = null;
    private Color alpha;

    private void Start()
    {
        alpha = imageUI.color;
    }

    private void Update()
    {
        imageUI.raycastTarget = IsAbilityActive;

        alpha.a = IsAbilityActive ? 1f : 0.5f;
        imageUI.color = alpha;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("click");
        TreeController.Instance.SetActiveAbility(activeAbility);
    }

    public void SetAbilityOption(TreeAbilities newAbility)
    {
        activeAbility = newAbility;
    }
}
