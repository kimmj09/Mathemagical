using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicCraftingUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject craftingUIRoot;      // UI 전체 루트 (MagicCraftingPanel)
    public Transform cardListContainer;    // [1] 마법 카드 버튼들이 나열될 좌측 목록 패널
    public Transform paletteContainer;     // [2] 카드를 눌렀을 때 UIBlock이 생성될 우측 팔레트 패널

    [Header("UI Prefabs")]
    public GameObject cardButtonPrefab;     // 카드 버튼 프리팹
    public UIBlock uiBlockBasePrefab;      // 마그네틱 블록 프리팹 (UIBlock_Base)

    [Header("Magic Databases")]
    public List<ElementCardSO> allElementCards;           // 보유한 원소 카드 목록
    public List<TransformCardSO> commonTransformCards;    // 공통 변형 카드
    public List<TransformCardSO> atmosphereTransformCards;// 대기 전용 변형 카드
    public List<TransformCardSO> lightTransformCards;     // 빛 전용 변형 카드

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
        ClearContainer(paletteContainer);
    }

    public void CloseCraftingUI()
    {
        if (cardListContainer != null) cardListContainer.gameObject.SetActive(false);
        
        ClearContainer(cardListContainer);
        ClearContainer(paletteContainer);

        if (craftingUIRoot != null) craftingUIRoot.SetActive(false);
    }

    // 1. 탭 버튼(Atmosphere, Light 등)을 눌렀을 때: 카드 목록(CardListContainer) 생성
    public void OnSelectElementTab(int elementTypeIndex)
    {
        if (cardListContainer != null && !cardListContainer.gameObject.activeSelf)
        {
            cardListContainer.gameObject.SetActive(true);
        }

        ClearContainer(cardListContainer);
        ElementType selectedType = (ElementType)elementTypeIndex;

        // 원소 카드 버튼 생성
        ElementCardSO targetElement = allElementCards.Find(e => e.elementType == selectedType);
        if (targetElement != null)
        {
            CreateCardButton(targetElement, BlockType.Element);
        }

        // 공통 변형 카드 버튼 생성
        foreach (var transCard in commonTransformCards)
        {
            if (transCard != null) CreateCardButton(transCard, BlockType.Transform);
        }

        // 전용 변형 카드 버튼 생성
        List<TransformCardSO> exclusiveCards = GetExclusiveCards(selectedType);
        if (exclusiveCards != null)
        {
            foreach (var transCard in exclusiveCards)
            {
                if (transCard != null) CreateCardButton(transCard, BlockType.Transform);
            }
        }
    }

    // 2. 카드 버튼 생성 및 클릭 이벤트 연결
    private void CreateCardButton(ScriptableObject cardData, BlockType blockType)
    {
        if (cardButtonPrefab == null || cardListContainer == null)
        {
            Debug.LogError("[MagicCraftingUI] cardButtonPrefab 또는 cardListContainer가 연결되지 않았습니다!");
            return;
        }

        GameObject btnObj = Instantiate(cardButtonPrefab, cardListContainer);
    
        string cardName = cardData is ElementCardSO element ? element.cardName : ((TransformCardSO)cardData).cardName;
    
        // TMP_Text 참조 안전하게 가져오기
        TextMeshProUGUI btnText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null) btnText.text = $"[{blockType}] {cardName}";

        Button btn = btnObj.GetComponent<Button>();
        if (btn != null)
        {
            // Listener 등록 전 기존 이벤트를 모두 제거해 중복/오작동 방지
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => {
                Debug.Log($"[UI] 마법 카드 클릭됨: {cardName} ({blockType})");
                SpawnBlockToPalette(cardData, blockType);
            });
        }
    }

    // 3. 카드 버튼 클릭 시 PaletteContainer에 마그네틱 블록(UIBlock) 스폰
    private void SpawnBlockToPalette(ScriptableObject cardData, BlockType blockType)
    {
        if (uiBlockBasePrefab == null)
        {
            Debug.LogError("[MagicCraftingUI] uiBlockBasePrefab (UIBlock_Base) 참조가 비어 있습니다!");
            return;
        }

        if (paletteContainer == null)
        {
            Debug.LogError("[MagicCraftingUI] paletteContainer 참조가 비어 있습니다!");
            return;
        }

        // 1. 블록 생성
        UIBlock newBlock = Instantiate(uiBlockBasePrefab, paletteContainer);
        
        // 2. 스폰된 블록의 Scale 및 Position 초기화 (UI 스케일 깨짐 방지)
        RectTransform rect = newBlock.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localScale = Vector3.one;
            rect.anchoredPosition = Vector2.zero;
        }

        // 3. 데이터 주입
        newBlock.SetupBlock(cardData, blockType);

        Debug.Log($"[UI] PaletteContainer에 블록 생성 완료: {cardData.name}");
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