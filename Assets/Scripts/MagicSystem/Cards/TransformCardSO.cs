using UnityEngine;

// 1. 변형 카드 베이스 (추상 클래스)
public abstract class TransformCardSO : ScriptableObject
{
    [Header("Card Info")]
    public string cardName;             // 카드 이름 (예: 직선)
    public Sprite cardIcon;             // 카드 UI 아이콘
    public float basePneumaCost = 10f;  // 가성비 시전 시 소모되는 프뉴마(MP)

    // 모든 변형 카드가 각자의 방식(수학적 궤적/기믹)으로 구현할 핵심 함수
    public abstract void ExecuteMagic(Transform casterTransform, ElementCardSO element);
}