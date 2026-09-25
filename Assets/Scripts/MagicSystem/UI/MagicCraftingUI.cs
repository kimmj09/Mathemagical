using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MagicSystem.Core;

namespace MagicSystem.UI
{
    public class MagicCraftingUI : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject craftingUIRoot;      // UI 전체 루트 (MagicCraftingPanel)
        public Transform cardListContainer;    // 마법 카드 버튼들이 나열될 좌측 목록 패널

        [Header("Crafting Slots & Tuning Selector")]
        public MagicCardSlot elementSlot;      // 원소 카드 장착 슬롯
        public MagicCardSlot transformSlot;    // 변형 카드 장착 슬롯
        public PneumaTuningSelector tuningSelector; // [수정] 튜닝 슬라이더 선택기

        [Header("UI Prefabs")]
        public GameObject cardButtonPrefab;     // 카드 선택 버튼 프리팹

        [Header("Magic Databases")]
        public List<ElementCardSO> allElementCards;            // 보유한 원소 카드 목록
        public List<TransformCardSO> atmosphereTransformCards; // 대기 전용 변형 카드
        public List<TransformCardSO> lightTransformCards;      // 빛 전용 변형 카드

        [Header("References")]
        public MagicCaster caster;

        private void Start()
        {
            CloseCraftingUI();
        }

        public void OpenCraftingUI()
        {
            if (craftingUIRoot != null) craftingUIRoot.SetActive(true);
            if (cardListContainer != null) cardListContainer.gameObject.SetActive(false);
            
            ClearContainer(cardListContainer);
        }

        public void CloseCraftingUI()
        {
            if (cardListContainer != null) cardListContainer.gameObject.SetActive(false);
            
            ClearContainer(cardListContainer);

            if (craftingUIRoot != null) craftingUIRoot.SetActive(false);
        }

        public void OnSelectElementTab(int elementTypeIndex)
        {
            if (cardListContainer != null && !cardListContainer.gameObject.activeSelf)
            {
                cardListContainer.gameObject.SetActive(true);
            }

            ClearContainer(cardListContainer);
            ElementType selectedType = (ElementType)elementTypeIndex;

            ElementCardSO targetElement = allElementCards.Find(e => e.elementType == selectedType);
            if (targetElement != null)
            {
                CreateCardButton(targetElement, MagicCardSlot.SlotType.Element);
            }

            List<TransformCardSO> exclusiveCards = GetExclusiveCards(selectedType);
            if (exclusiveCards != null)
            {
                foreach (var transCard in exclusiveCards)
                {
                    if (transCard != null) CreateCardButton(transCard, MagicCardSlot.SlotType.Transform);
                }
            }
        }

        private void CreateCardButton(ScriptableObject cardData, MagicCardSlot.SlotType slotType)
        {
            if (cardButtonPrefab == null || cardListContainer == null) return;

            GameObject btnObj = Instantiate(cardButtonPrefab, cardListContainer);
            string cardName = cardData is ElementCardSO element ? element.cardName : ((TransformCardSO)cardData).cardName;
        
            TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = $"[{slotType}] {cardName}";

            Button btn = btnObj.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => {
                    SelectCardToSlot(cardData, slotType);
                });
            }
        }

        private void SelectCardToSlot(ScriptableObject cardData, MagicCardSlot.SlotType slotType)
        {
            if (slotType == MagicCardSlot.SlotType.Element && elementSlot != null)
            {
                elementSlot.SetCard(cardData);
            }
            else if (slotType == MagicCardSlot.SlotType.Transform && transformSlot != null)
            {
                transformSlot.SetCard(cardData);
            }
        }

        public void OnClickSaveToQuickSlot(int slotIndex)
        {
            if (elementSlot == null || elementSlot.CurrentCardData == null) return;
            if (transformSlot == null || transformSlot.CurrentCardData == null) return;

            ElementCardSO element = elementSlot.CurrentCardData as ElementCardSO;
            TransformCardSO transform = transformSlot.CurrentCardData as TransformCardSO;
            
            // 튜닝 슬라이더 값 가져오기 (없으면 기본값 0.5f)
            float spectrum = (tuningSelector != null) ? tuningSelector.CurrentSpectrum : 0.5f;

            Debug.Log($"[퀵슬롯 저장] 슬롯 {slotIndex}번: {element.cardName} + {transform.cardName} (스펙트럼: {spectrum:F2})");
        }

        private List<TransformCardSO> GetExclusiveCards(ElementType type)
        {
            return type switch
            {
                ElementType.atmosphere => atmosphereTransformCards,
                ElementType.Light => lightTransformCards,
                _ => null
            };
        }

        private void ClearContainer(Transform container)
        {
            if (container == null) return;
            foreach (Transform child in container)
            {
                Destroy(child.gameObject);
            }
        }
    }
}