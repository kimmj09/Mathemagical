using System.Collections.Generic;
using UnityEngine;

// UI 및 마법 연성판에서 조립된 변수 데이터
[System.Serializable]
public class SavedMagicRecipe
{
    public string recipeName;           // 마법 조합 이름 (예: "대기 직선 파동")
    public ElementCardSO elementCard;   // 선택된 원소
    public TransformCardSO transformCard; // 선택된 변형
    
    // 유저가 슬라이더나 텍스트로 입력한 매개변수 값들
    public float mass = 1f;             // 질량
    public float speed = 15f;           // 속도
    public Vector2 direction = Vector2.right; // 방향
}