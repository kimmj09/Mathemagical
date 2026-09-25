using UnityEngine;

namespace MagicSystem.Core
{
    [System.Serializable]
    public class SavedMagicRecipe
    {
        public string recipeName;               // MagicCaster에서 참조하는 레시피 이름
        public ElementCardSO elementCard;       // 원소 카드
        public TransformCardSO transformCard;   // 변형 카드
        public float spectrum = 0.5f;           // 튜닝 슬라이더 수치 (0.0 ~ 1.0)

        // 기본 생성자
        public SavedMagicRecipe() { }

        // 매개변수를 갖는 생성자
        public SavedMagicRecipe(string recipeName, ElementCardSO element, TransformCardSO transform, float spectrumValue)
        {
            this.recipeName = recipeName;
            this.elementCard = element;
            this.transformCard = transform;
            this.spectrum = spectrumValue;
        }

        public void CastMagic(Transform casterTransform)
        {
            if (transformCard == null || elementCard == null) return;

            // 저장된 스펙트럼 값 및 원소 정보를 바탕으로 마법 실행
            transformCard.ExecuteMagic(casterTransform, elementCard);
        }
    }
}