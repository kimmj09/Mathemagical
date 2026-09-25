using UnityEngine;
using MagicSystem.Core;

[CreateAssetMenu(fileName = "Card_Transform_WindCutter", menuName = "MagicSystem/Transform Card/Wind Cutter")]
public class WindCutterCardSO : TransformCardSO
{
    [Header("Wind Cutter Options")]
    public float baseDamageMultiplier = 1.2f;

    public override void ExecuteMagic(Transform casterTransform, ElementCardSO element)
    {
        if (magicPrefab == null) return;

        // 현재 스펙트럼 값 가져오기 (0.0: 극단적 집중, 0.5: 균형, 1.0: 극단적 확산)
        float spectrum = 0.5f;
        var craftingUI = Object.FindAnyObjectByType<MagicSystem.UI.MagicCraftingUI>();
        if (craftingUI != null && craftingUI.tuningSelector != null)
        {
            spectrum = craftingUI.tuningSelector.CurrentSpectrum;
        }

        Vector3 castDirection = casterTransform.localScale.x < 0 ? Vector3.left : Vector3.right;
        Vector3 spawnPosition = casterTransform.position + castDirection * 0.8f;

        // -------------------------------------------------------------
        // 스펙트럼 연산 (0.0 -> 1.0)
        // 크기(Scale): 2.2배 -> 0.6배
        // 속도(Speed): 22f -> 7f
        // 발사 개수(Count): 1개 -> 5개
        // -------------------------------------------------------------
        float sizeFactor = Mathf.Lerp(2.2f, 0.6f, spectrum);
        float speedFactor = Mathf.Lerp(22f, 7f, spectrum);
        int projectileCount = Mathf.RoundToInt(Mathf.Lerp(1f, 5f, spectrum));

        if (projectileCount <= 1)
        {
            // 집중형: 대형 단일 참격
            GameObject projObj = Object.Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
            projObj.transform.localScale *= sizeFactor;
            
            var proj = projObj.GetComponent<MagicProjectile>() ?? projObj.AddComponent<MagicProjectile>();
            proj.Init(castDirection, speedFactor);
        }
        else
        {
            // 확산형: 각도 분할 다중 발사 (스펙트럼이 높을수록 각도 범위가 넓어짐)
            float totalAngle = Mathf.Lerp(15f, 60f, spectrum);
            float startAngle = -totalAngle / 2f;
            float angleStep = totalAngle / (projectileCount - 1);

            for (int i = 0; i < projectileCount; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Vector3 dir = Quaternion.Euler(0, 0, currentAngle) * castDirection;

                GameObject projObj = Object.Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
                projObj.transform.localScale *= sizeFactor;

                var proj = projObj.GetComponent<MagicProjectile>() ?? projObj.AddComponent<MagicProjectile>();
                proj.Init(dir, speedFactor);
            }
        }
    }
}