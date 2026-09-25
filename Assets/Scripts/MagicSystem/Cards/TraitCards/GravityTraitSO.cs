using UnityEngine;

namespace MagicSystem.Core
{
    [CreateAssetMenu(fileName = "Card_Trait_Gravity", menuName = "MagicSystem/Trait Card/Gravity")]
    public class GravityTraitSO : TraitCardSO
    {
        public float gravityScale = 1.5f; // 중력 가속도 수치

        public override void ApplyTrait(GameObject projectileObj, MagicProjectile projectileScript)
        {
            var behavior = projectileObj.AddComponent<GravityBehavior>();
            behavior.gravityScale = gravityScale;
        }
    }

    public class GravityBehavior : MonoBehaviour
    {
        public float gravityScale = 1.5f;
        private Vector3 currentVelocity;

        private void Start()
        {
            var proj = GetComponent<MagicProjectile>();
            if (proj != null)
            {
                currentVelocity = transform.right * proj.moveSpeed;
            }
        }

        private void Update()
        {
            currentVelocity += Vector3.down * (9.81f * gravityScale) * Time.deltaTime;
            transform.position += currentVelocity * Time.deltaTime;
            transform.right = currentVelocity.normalized; // 진행 방향으로 회전 처리
        }
    }
}