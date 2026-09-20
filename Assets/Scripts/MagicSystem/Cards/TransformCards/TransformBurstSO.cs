using UnityEngine;

// 3. [변형] 범위 확산: 폭발 (Burst)
[CreateAssetMenu(fileName = "Card_Transform_Burst", menuName = "MagicSystem/Transform/Burst")]
public class TransformBurstSO : TransformCardSO
{
    public float radius = 3f;

    public override void ExecuteMagic(Transform casterTransform, ElementCardSO element)
    {
        if (element == null || element.baseParticlePrefab == null) return;

        // 전방 좌표에 범위 폭발 생성
        Vector3 spawnPos = casterTransform.position + (casterTransform.right * 2f);
        GameObject burstObj = Instantiate(element.baseParticlePrefab, spawnPos, Quaternion.identity);
        burstObj.transform.localScale = Vector3.one * radius;

        Debug.Log($"[{element.cardName}] 속성의 [폭발] 마법 발동!");
    }
}