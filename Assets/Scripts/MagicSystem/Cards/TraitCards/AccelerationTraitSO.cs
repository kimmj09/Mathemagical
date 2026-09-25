using UnityEngine;

namespace MagicSystem.Core
{
    [CreateAssetMenu(fileName = "Card_Trait_Acceleration", menuName = "MagicSystem/Trait Card/Acceleration")]
    public class AccelerationTraitSO : TraitCardSO
    {
        public float accelRate = 10f; // 초당 가속도

        public override void ApplyTrait(GameObject projectileObj, MagicProjectile projectileScript)
        {
            var behavior = projectileObj.AddComponent<AccelerationBehavior>();
            behavior.accelRate = accelRate;
            behavior.targetProjectile = projectileScript;
        }
    }

    public class AccelerationBehavior : MonoBehaviour
    {
        public float accelRate = 10f;
        public MagicProjectile targetProjectile;

        private void Update()
        {
            if (targetProjectile != null)
            {
                targetProjectile.moveSpeed += accelRate * Time.deltaTime;
            }
        }
    }
}