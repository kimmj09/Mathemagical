using UnityEngine;
using MagicSystem.UI;
using MagicSystem.Core;

public class MagicCaster : MonoBehaviour
{
    [Header("Card Slots")]
    public ElementCardSO currentElementCard;      // 연성판에 올라간 원소 카드
    public TransformCardSO currentTransformCard;  // 연성판에 올라간 변형 카드

    [Header("Player Pneuma (MP)")]
    public float currentPneuma = 100f;
    public float rawCastPenaltyCost = 50f;

    [Header("Time Dilation (Casting State)")]
    public bool isCasting = false;
    public float slowTimeFactor = 0.1f;
    public float logosStat = 1f;

    public MagicCraftingUI craftingUI;

    public Animator anim;

    private void Update()
    {
        // ⓐ & ⓑ F 키 입력 제어
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isCasting)
            {
                // ⓐ 캐스팅 상태가 아닐 때: 캐스팅 상태 진입
                StartCasting();
            }
            else
            {
                // ⓑ 캐스팅 상태일 때: 연성 확인 및 시전 (조합 미완성 시 자동 취소)
                TryExecuteCraftedMagic();
            }
        }

        // ⓒ C 키 입력 제어: 캐스팅 취소
        if (isCasting && Input.GetKeyDown(KeyCode.C))
        {
            CancelCasting();
        }
    }

    // ⓐ 캐스팅 진입 (시간 팽창 & UI 오픈)
    public void StartCasting()
    {
        isCasting = true;
        float currentFactor = Mathf.Clamp(slowTimeFactor / (1f + logosStat * 0.1f), 0.02f, 0.5f);
        
        Time.timeScale = currentFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Charge 애니메이션 실행
        if (anim != null) anim.SetBool("isCasting", true);

        if (craftingUI != null) craftingUI.OpenCraftingUI();
        Debug.Log($"[캐스팅 진입] 시간이 {Time.timeScale * 100f}% 속도로 느려집니다.");
    }

    // ⓑ F키 시전 시도 (예외 처리 포함)
    private void TryExecuteCraftedMagic()
    {
        // 1. UI 슬롯에서 현재 선택된 카드 정보 가져오기
        if (craftingUI != null)
        {
            if (craftingUI.elementSlot != null)
                currentElementCard = craftingUI.elementSlot.CurrentCardData as ElementCardSO;
                
            if (craftingUI.transformSlot != null)
                currentTransformCard = craftingUI.transformSlot.CurrentCardData as TransformCardSO;
        }

        // 2. 예외 처리: 원소 카드가 선택되지 않은 경우
        if (currentElementCard == null)
        {
            Debug.Log("[연성 실패] 원소 카드가 선택되지 않아 캐스팅이 취소됩니다.");
            CancelCasting();
            return;
        }

        // 3. 변형 카드가 세팅된 경우 마법 실행
        if (currentTransformCard != null)
        {
            float requiredCost = currentTransformCard.basePneumaCost;
            if (currentPneuma >= requiredCost)
            {
                currentPneuma -= requiredCost;
                currentTransformCard.ExecuteMagic(transform, currentElementCard);
                FinishCasting();
            }
            else
            {
                Debug.LogWarning("프뉴마(MP)가 부족합니다!");
            }
        }
        // 4. 변형 카드 없이 원소 단독 시전
        else
        {
            Debug.Log($"[{currentElementCard.cardName}] 원소 단독 시전!");
            FinishCasting();
        }
    }

    // ⓒ 캐스팅 취소 (C키 또는 예외 처리)
    public void CancelCasting()
    {
        ResetTimeScale();
        if (craftingUI != null) craftingUI.CloseCraftingUI();
        Debug.Log("[캐스팅 취소] 연성을 중단하고 복귀합니다.");
    }

    // 시전 완료 후 성공적으로 복귀
    private void FinishCasting()
    {
        ResetTimeScale();
        if (craftingUI != null) craftingUI.CloseCraftingUI();
        
        // 연성 슬롯 초기화
        currentElementCard = null;
        currentTransformCard = null;
    }

    public void RegisterQuickSlot(int slotIndex, SavedMagicRecipe recipe)
    {
        // 우선 연성판의 원소/변형 카드를 업데이트 (필요에 따라 퀵슬롯 배열 관리로 확장 가능)
        currentElementCard = recipe.elementCard;
        currentTransformCard = recipe.transformCard;
        
        Debug.Log($"[ MagicCaster ] 퀵슬롯 {slotIndex}번 설정 완료: {recipe.recipeName}");
    }

    private void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        isCasting = false;
        if (anim != null) anim.SetBool("isCasting", false);
    }

    private void OnDisable()
    {
        ResetTimeScale();
    }
}