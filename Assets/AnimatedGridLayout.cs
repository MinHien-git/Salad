using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimatedGridLayout : MonoBehaviour
{
    public int columns = 5; // Bao nhiêu cột
    public Vector2 cellSize = new Vector2(150f, 150f);
    public Vector2 spacing = new Vector2(20f, 20f);
    public Vector2 startOffset = Vector2.zero; // Offset bắt đầu

    private List<RectTransform> children = new List<RectTransform>();

    private void Awake()
    {
        UpdateChildrenList();
        RepositionChildren();
    }

    public void UpdateChildrenList()
    {
        children.Clear();
        foreach (Transform child in transform)
        {
            RectTransform rect = child.GetComponent<RectTransform>();
            if (rect != null)
            {
                children.Add(rect);
            }
        }
    }

    public void RepositionChildren()
    {
        for (int i = 0; i < children.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            Vector2 targetPos = new Vector2(
                startOffset.x + col * (cellSize.x + spacing.x),
                startOffset.y - row * (cellSize.y + spacing.y)
            );

            RectTransform rect = children[i];
            rect.DOKill(); // stop previous tweens
            rect.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.OutQuad); // Animate di chuyển tới
        }
    }

    public void AddChild(RectTransform child)
    {
        child.SetParent(transform, false);
        children.Add(child);
        RepositionChildren();
    }

    public void RemoveChild(RectTransform child)
    {
        children.Remove(child);
        Destroy(child.gameObject);
        RepositionChildren();
    }
}
