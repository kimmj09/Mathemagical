using UnityEngine;
using MagicSystem.Core;

[CreateAssetMenu(fileName = "Card_Transform_WindBuster", menuName = "MagicSystem/Transform Card/Wind Buster")]
public class WindBusterCardSO : TransformCardSO
{
    [Header("Wind Buster Options")]
    public float basePushForce = 15f;

    public override void ExecuteMagic(Transform casterTransform, ElementCardSO element)
    {
        if (magicPrefab == null) return;

        float spectrum = 0.5f;
        var craftingUI = Object.FindAnyObjectByType<MagicSystem.UI.MagicCraftingUI>();
        if (craftingUI != null && craftingUI.tuningSelector != null)
        {
            spectrum = craftingUI.tuningSelector.CurrentSpectrum;
        }

        Vector3 castDirection = casterTransform.localScale.x < 0 ? Vector3.left : Vector3.right;
        Vector3 spawnPosition = casterTransform.position + castDirection * 1.0f;

        // 스펙트럼 기반 넉백/범위 연산
        float scale = Mathf.Lerp(2.0f, 0.8f, spectrum);
        float speed = Mathf.Lerp(18f, 6f, spectrum);

        if (spectrum < 0.7f)
        {
            // 정면 충격파 발사
            GameObject projObj = Object.Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
            projObj.transform.localScale *= scale;

            var proj = projObj.GetComponent<MagicProjectile>() ?? projObj.AddComponent<MagicProjectile>();
            proj.Init(castDirection, speed);
        }
        else
        {
            // 극단적 확산: 전방위 360도 8방향 압력 방출
            int count = 8;
            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count);
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;

                GameObject projObj = Object.Instantiate(magicPrefab, casterTransform.position + dir * 0.5f, Quaternion.identity);
                projObj.transform.localScale *= scale;

                var proj = projObj.GetComponent<MagicProjectile>() ?? projObj.AddComponent<MagicProjectile>();
                proj.Init(dir, speed);
            }
        }
    }
}