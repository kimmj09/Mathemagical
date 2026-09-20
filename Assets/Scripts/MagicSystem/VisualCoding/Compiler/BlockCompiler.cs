using UnityEngine;

public class BlockCompiler : MonoBehaviour
{
    [Header("Slots")]
    public UIBlockSlot elementSlot;
    public UIBlockSlot transformSlot;

    public SavedMagicRecipe CompileRecipe(string recipeName = "Compiled Magic")
    {
        if (elementSlot.currentBlock == null || transformSlot.currentBlock == null)
        {
            Debug.LogWarning("원소 블록과 변형 블록이 모두 결합되어야 연성할 수 있습니다!");
            return null;
        }

        ElementCardSO element = elementSlot.currentBlock.cardData as ElementCardSO;
        TransformCardSO transform = transformSlot.currentBlock.cardData as TransformCardSO;

        SavedMagicRecipe newRecipe = new SavedMagicRecipe
        {
            recipeName = recipeName,
            elementCard = element,
            transformCard = transform
        };

        Debug.Log($"[블록 컴파일 성공] 레시피 명: {recipeName} / 속성: {element?.cardName} / 형태: {transform?.cardName}");
        return newRecipe;
    }
}