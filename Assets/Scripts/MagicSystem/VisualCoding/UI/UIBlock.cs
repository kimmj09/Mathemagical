using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum BlockType
{
    Element,
    Transform,
    Parameter
}

public class UIBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Block Data")]
    public BlockType blockType;
    public ScriptableObject cardData;

    [Header("UI Visuals")]
    [SerializeField] private TMP_Text blockNameText;
    [SerializeField] private Image blockIconImage;

    [Header("Magnetic Snap")]
    public float snapRadius = 60f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas mainCanvas;
    private Vector2 originalPosition;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
    }

    /// <summary>
    /// 외부 스크립트에서 ScriptableObject 데이터를 넘겨받아 블록을 세팅하는 함수
    /// </summary>
    public void SetupBlock(ScriptableObject data, BlockType type)
    {
        cardData = data;
        blockType = type;

        if (data is ElementCardSO element)
        {
            if (blockNameText != null) blockNameText.text = element.cardName;
            if (blockIconImage != null && element.cardIcon != null) blockIconImage.sprite = element.cardIcon;
        }
        else if (data is TransformCardSO transformCard)
        {
            if (blockNameText != null) blockNameText.text = transformCard.cardName;
            if (blockIconImage != null && transformCard.cardIcon != null) blockIconImage.sprite = transformCard.cardIcon;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        if (mainCanvas != null)
        {
            transform.SetParent(mainCanvas.transform, true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (mainCanvas == null) return;

        // 마우스 좌표 어긋남 문제 해결 (RectTransformUtility 사용)
        RectTransform canvasRect = mainCanvas.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            mainCanvas.worldCamera,
            out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        UIBlockSlot nearestSlot = FindNearestSlot();

        if (nearestSlot != null && nearestSlot.CanAcceptBlock(this))
        {
            nearestSlot.AttachBlock(this);
        }
        else
        {
            // 슬롯에 붙지 못했을 때 원래 부모와 위치로 복귀
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private UIBlockSlot FindNearestSlot()
    {
        // Unity 6 경고 메시지(Warning CS0618) 해결을 위해 FindObjectsByType 사용
        UIBlockSlot[] slots = Object.FindObjectsByType<UIBlockSlot>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        UIBlockSlot closestSlot = null;
        float minDistance = snapRadius;

        foreach (var slot in slots)
        {
            float distance = Vector2.Distance(rectTransform.position, slot.GetComponent<RectTransform>().position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlot = slot;
            }
        }

        return closestSlot;
    }
}