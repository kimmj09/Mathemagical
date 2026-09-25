using UnityEngine;

namespace MagicSystem.Core
{
    [CreateAssetMenu(fileName = "Card_Trait_Spiral", menuName = "MagicSystem/Trait Card/Spiral")]
    public class SpiralTraitSO : TraitCardSO
    {
        public float frequency = 12f; // 회전 속도
        public float magnitude = 0.6f; // 진폭 범위

        public override void ApplyTrait(GameObject projectileObj, MagicProjectile projectileScript)
        {
            var behavior = projectileObj.AddComponent<SpiralBehavior>();
            behavior.frequency = frequency;
            behavior.magnitude = magnitude;
        }
    }

    public class SpiralBehavior : MonoBehaviour
    {
        public float frequency = 12f;
        public float magnitude = 0.6f;
        private float timeAccumulator;

        private void Update()
        {
            timeAccumulator += Time.deltaTime;
            Vector3 offset = transform.up * Mathf.Sin(timeAccumulator * frequency) * magnitude;
            transform.position += offset * Time.deltaTime;
        }
    }
}