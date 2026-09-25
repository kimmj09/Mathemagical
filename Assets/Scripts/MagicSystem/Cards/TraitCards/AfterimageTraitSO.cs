using UnityEngine;

namespace MagicSystem.Core
{
    [CreateAssetMenu(fileName = "Card_Trait_Afterimage", menuName = "MagicSystem/Trait Card/Afterimage")]
    public class AfterimageTraitSO : TraitCardSO
    {
        public float spawnInterval = 0.05f; // 잔상 생성 주기

        public override void ApplyTrait(GameObject projectileObj, MagicProjectile projectileScript)
        {
            var behavior = projectileObj.AddComponent<AfterimageBehavior>();
            behavior.spawnInterval = spawnInterval;
        }
    }

    public class AfterimageBehavior : MonoBehaviour
    {
        public float spawnInterval = 0.05f;
        private float timer;
        private SpriteRenderer mainRenderer;

        private void Awake()
        {
            mainRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            if (mainRenderer == null) return;

            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                timer = 0f;
                CreateGhost();
            }
        }

        private void CreateGhost()
        {
            GameObject ghost = new GameObject("Afterimage_Ghost");
            ghost.transform.position = transform.position;
            ghost.transform.rotation = transform.rotation;
            ghost.transform.localScale = transform.localScale;

            SpriteRenderer ghostSr = ghost.AddComponent<SpriteRenderer>();
            ghostSr.sprite = mainRenderer.sprite;
            ghostSr.color = new Color(mainRenderer.color.r, mainRenderer.color.g, mainRenderer.color.b, 0.5f);

            Destroy(ghost, 0.2f); // 0.2초 뒤 잔상 소멸
        }
    }
}