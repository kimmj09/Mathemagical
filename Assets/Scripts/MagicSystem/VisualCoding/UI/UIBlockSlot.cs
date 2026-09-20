using UnityEngine;

public class UIBlockSlot : MonoBehaviour
{
    public BlockType targetBlockType;
    public UIBlock currentBlock;

    public bool CanAcceptBlock(UIBlock block)
    {
        return currentBlock == null && block.blockType == targetBlockType;
    }

    public void AttachBlock(UIBlock block)
    {
        currentBlock = block;
        block.transform.SetParent(transform, false);
        
        RectTransform blockRect = block.GetComponent<RectTransform>();
        blockRect.anchoredPosition = Vector2.zero; // 소켓 정중앙에 마그네틱 정렬
    }

    public void DetachBlock()
    {
        currentBlock = null;
    }
}