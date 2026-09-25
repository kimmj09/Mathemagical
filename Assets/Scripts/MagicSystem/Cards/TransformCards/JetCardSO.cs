using UnityEngine;
using MagicSystem.Core;

[CreateAssetMenu(fileName = "Card_Transform_Jet", menuName = "MagicSystem/Transform Card/Jet")]
public class JetCardSO : TransformCardSO
{
    [Header("Jet Options")]
    public float baseDashSpeed = 20f;

    public override void ExecuteMagic(Transform casterTransform, ElementCardSO element)
    {
        float spectrum = 0.5f;
        var craftingUI = Object.FindAnyObjectByType<MagicSystem.UI.MagicCraftingUI>();
        if (craftingUI != null && craftingUI.tuningSelector != null)
        {
            spectrum = craftingUI.tuningSelector.CurrentSpectrum;
        }

        Debug.Log($"[시전] {element.cardName} 제트! (튜닝 스펙트럼: {spectrum:F2})");
    }
}