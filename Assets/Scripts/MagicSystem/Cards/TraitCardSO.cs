using UnityEngine;

namespace MagicSystem.Core
{
    public enum TraitType
    {
        Physics,    // 가속, 회전, 잔상, 중력 등 물리/궤적 변화
        Effect      // 반사, 분열, 유도 등 특수 마법 기믹
    }

    public abstract class TraitCardSO : ScriptableObject
    {
        [Header("Trait Card Info")]
        public string cardName;
        public Sprite cardIcon;
        [TextArea] public string description;
        public TraitType traitType;

        // 투사체 생성 시 특성 효과를 주입하는 핵심 함수
        public abstract void ApplyTrait(GameObject projectileObj, MagicProjectile projectileScript);
    }
}