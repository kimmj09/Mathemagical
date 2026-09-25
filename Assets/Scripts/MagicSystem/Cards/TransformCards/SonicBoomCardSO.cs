using UnityEngine;
using MagicSystem.Core;

[CreateAssetMenu(fileName = "Card_Transform_SonicBoom", menuName = "MagicSystem/Transform Card/Sonic Boom")]
public class SonicBoomCardSO : TransformCardSO
{
    [Header("Sonic Boom Options")]
    public float baseStunDuration = 1.5f;

    public override void ExecuteMagic(Transform casterTransform, ElementCardSO element)
    {
        float spectrum = 0.5f;
        var craftingUI = Object.FindAnyObjectByType<MagicSystem.UI.MagicCraftingUI>();
        if (craftingUI != null && craftingUI.tuningSelector != null)
        {
            spectrum = craftingUI.tuningSelector.CurrentSpectrum;
        }

        Debug.Log($"[시전] {element.cardName} 소닉붐! (튜닝 스펙트럼: {spectrum:F2})");
    }
}