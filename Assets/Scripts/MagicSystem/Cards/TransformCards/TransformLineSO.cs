using UnityEngine;

// 2. [변형] 1차 함수: 직선 (Line / 발사기)
[CreateAssetMenu(fileName = "Card_Transform_Line", menuName = "MagicSystem/Transform/Line")]
public class TransformLineSO : TransformCardSO
{
    public float speed = 15f;
    public float spawnOffset = 1.2f; // 플레이어와 겹치지 않게 떨어뜨릴 거리

    public override void ExecuteMagic(Transform casterTransform, ElementCardSO element)
    {
        if (element == null || element.baseParticlePrefab == null) return;

        // 1. 플레이어가 바라보는 방향 판단 (오른쪽: +1, 왼쪽: -1)
        float direction = 1f;
        PlayerController player = casterTransform.GetComponent<PlayerController>();

        if (player != null)
        {
            direction = player.isFacingRight ? 1f : -1f;
        }
        else
        {
            direction = Mathf.Sign(casterTransform.localScale.x);
        }

        Vector2 launchDirection = new Vector2(direction, 0f);

        // 2. 플레이어 중심에서 바라보는 방향으로 일정 거리(spawnOffset)만큼 떨어진 위치 계산
        Vector3 spawnPosition = casterTransform.position + new Vector3(direction * spawnOffset, 0f, 0f);

        // 3. 스케일을 음수로 만드는 대신 회전(Rotation)으로 방향 지정
        Quaternion spawnRotation = (direction < 0f) ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;

        // 4. 투사체 생성
        GameObject projectile = Instantiate(element.baseParticlePrefab, spawnPosition, spawnRotation);

        // 5. 플레이어와 투사체 간의 물리 충돌을 코드상에서 강제로 무시 처리
        Collider2D playerCollider = casterTransform.GetComponent<Collider2D>();
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        if (playerCollider != null && projectileCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, projectileCollider);
        }

        // 6. 물리 속도 적용
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = launchDirection * speed;
        }

        Debug.Log($"[{element.cardName}] 속성의 [직선] 마법이 {(direction > 0 ? "오른쪽" : "왼쪽")}으로 발사!");
    }
}