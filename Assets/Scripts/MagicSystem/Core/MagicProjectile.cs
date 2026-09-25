using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float moveSpeed = 10f;     // Inspector에서 기본 속도 조절 가능
    public float lifeTime = 3f;       // 자동 파괴 시간 (초)
    
    private Vector3 moveDirection = Vector3.right;
    private bool isInitialized = false;

    // 속도(speed)를 명시적으로 넘겨받아 설정
    public void Init(Vector3 direction, float speed)
    {
        moveDirection = direction.normalized;
        
        // 넘겨받은 speed 값이 0보다 크면 moveSpeed를 즉시 변경
        if (speed > 0f)
        {
            moveSpeed = speed;
        }

        // 진행 방향을 바라보도록 2D Z축 회전
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        isInitialized = true;
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // transform.Translate 대신 World 좌표 기준으로 정확히 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        Debug.Log($"[마법 충돌] {collision.name}에 마법이 적중했습니다.");
    }
}