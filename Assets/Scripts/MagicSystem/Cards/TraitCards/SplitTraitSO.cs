using UnityEngine;

namespace MagicSystem.Core
{
    [CreateAssetMenu(fileName = "Card_Trait_Split", menuName = "MagicSystem/Trait Card/Split")]
    public class SplitTraitSO : TraitCardSO
    {
        public float splitDelay = 0.4f; // 분열까지 걸리는 시간

        public override void ApplyTrait(GameObject projectileObj, MagicProjectile projectileScript)
        {
            var behavior = projectileObj.AddComponent<SplitBehavior>();
            behavior.splitDelay = splitDelay;
        }
    }

    public class SplitBehavior : MonoBehaviour
    {
        public float splitDelay = 0.4f;
        private bool hasSplit = false;

        private void Start()
        {
            Invoke(nameof(ExecuteSplit), splitDelay);
        }

        private void ExecuteSplit()
        {
            if (hasSplit) return;
            hasSplit = true;

            for (int i = -1; i <= 1; i += 2)
            {
                GameObject clone = Instantiate(gameObject, transform.position, Quaternion.identity);
                Destroy(clone.GetComponent<SplitBehavior>()); // 자식 투사체 재분열 방지
                
                Vector3 newDir = Quaternion.Euler(0, 0, i * 25f) * transform.right;
                var proj = clone.GetComponent<MagicProjectile>();
                if (proj != null) proj.Init(newDir, proj.moveSpeed);
            }
        }
    }
}