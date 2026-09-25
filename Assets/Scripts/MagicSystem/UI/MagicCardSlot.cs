using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MagicSystem.UI
{
    public class MagicCardSlot : MonoBehaviour
    {
        public enum SlotType { Element, Transform }
        public SlotType slotType;

        [Header("UI Components")]
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button removeButton;

        public ScriptableObject CurrentCardData { get; private set; }

        private void Awake()
        {
            if (removeButton != null)
                removeButton.onClick.AddListener(ClearSlot);
        }

        public void SetCard(ScriptableObject cardData)
        {
            CurrentCardData = cardData;

            if (cardData is ElementCardSO element)
            {
                if (nameText != null) nameText.text = element.cardName;
                if (iconImage != null) iconImage.sprite = element.cardIcon;
            }
            else if (cardData is TransformCardSO transformCard)
            {
                if (nameText != null) nameText.text = transformCard.cardName;
                if (iconImage != null) iconImage.sprite = transformCard.cardIcon;
            }

            if (iconImage != null) iconImage.enabled = true;
        }

        public void ClearSlot()
        {
            CurrentCardData = null;
            if (nameText != null) nameText.text = "Empty";
            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }
        }
    }
}