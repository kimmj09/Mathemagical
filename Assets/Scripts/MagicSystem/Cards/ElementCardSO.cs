using UnityEngine;

// 원소 속성 종류
public enum ElementType
{
    atmosphere,
    Light,
    Sphere
}

[CreateAssetMenu(fileName = "Card_Element_New", menuName = "MagicSystem/Element Card")]
public class ElementCardSO : ScriptableObject
{
    [Header("Card Info")]
    public string cardName;             // 카드 이름 (예 : 화염)
    public Sprite cardIcon;             // 카드 UI 아이콘
    public ElementType elementType;     // 원소 속성

    [Header("Base Stats")]
    public float baseDamage = 10f;       // 기본 데미지
    public GameObject baseParticlePrefab;// 원소 이펙트/투사체 프리팹
}