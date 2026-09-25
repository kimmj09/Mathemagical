using UnityEngine;

namespace MagicSystem.Core
{
    [CreateAssetMenu(fileName = "Card_Trait_Bounce", menuName = "MagicSystem/Trait Card/Bounce")]
    public class BounceTraitSO : TraitCardSO
    {
        public int maxBounces = 2; // 최대 튕김 횟수

        public override void ApplyTrait(GameObject projectileObj, MagicProjectile projectileScript)
        {
            var behavior = projectileObj.AddComponent<BounceBehavior>();
            behavior.maxBounces = maxBounces;
        }
    }

    public class BounceBehavior : MonoBehaviour
    {
        public int maxBounces = 2;
        private int currentBounces = 0;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) return;

            if (currentBounces < maxBounces)
            {
                currentBounces++;
                transform.right = -transform.right; // 반대 방향으로 반사
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}