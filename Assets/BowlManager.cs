using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BowlManager : MonoBehaviour
{
    [Header("Slice Settings")]
    public int numberOfSlices = 4;
    public float bowlSize = 500f;
    public float itemDistanceRatio = 0.5f; // item cách tâm bằng 50% bán kính
    private RectTransform[] slices;
    private int currentSlice = 0;

    [Header("Drop Item Settings")]
    public Vector2 itemSize = new Vector2(100f, 100f);

    private void Start()
    {
        GenerateSlices();
    }

    public bool IsFull()
    {
        return currentSlice >= numberOfSlices;
    }

    public void ShakeBowl()
    {
        RectTransform bowlRect = GetComponent<RectTransform>();

        if (bowlRect != null)
        {
            bowlRect
                .DOShakePosition(
                    0.3f,
                    strength: new Vector3(10f, 0f, 0f),
                    vibrato: 10,
                    randomness: 90
                )
                .SetEase(Ease.OutQuad);
        }
    }

    private void GenerateSlices()
    {
        slices = new RectTransform[numberOfSlices];

        for (int i = 0; i < numberOfSlices; i++)
        {
            GameObject sliceObj = new GameObject($"Slice_{i}");
            sliceObj.transform.SetParent(transform, false);

            RectTransform rect = sliceObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(bowlSize, bowlSize);
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.localPosition = Vector3.zero;
            rect.localRotation = Quaternion.Euler(0f, 0f, -360f / numberOfSlices * i);

            slices[i] = rect;
        }
    }

    public void AcceptDropItem(DropItem dropItem)
    {
        if (currentSlice >= numberOfSlices)
        {
            Debug.Log("Bowl đầy rồi!");
            Destroy(dropItem.gameObject);
            return;
        }

        Vector3 worldPos = dropItem.transform.position;

        dropItem.transform.SetParent(transform, true); // <-- true ở đây!!!

        float angle = (360f / numberOfSlices) * currentSlice + (180f / numberOfSlices);
        float rad = angle * Mathf.Deg2Rad;
        float radius = (bowlSize * 0.5f) * itemDistanceRatio;
        Vector2 localOffset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

        RectTransform dropRect = dropItem.GetComponent<RectTransform>();

        dropRect.anchorMin = new Vector2(0.5f, 0.5f);
        dropRect.anchorMax = new Vector2(0.5f, 0.5f);
        dropRect.pivot = new Vector2(0.5f, 0.5f);

        dropRect.localPosition = localOffset;
        dropRect.localRotation = Quaternion.identity;

        currentSlice++;
    }

    public void SpawnItemInNextSlice(Sprite itemSprite)
    {
        if (currentSlice >= slices.Length)
        {
            Debug.Log("Bowl đầy rồi!");
            return;
        }

        // Tính góc trung tâm của lát này
        float angle = (360f / numberOfSlices) * currentSlice + (180f / numberOfSlices);
        float rad = angle * Mathf.Deg2Rad;

        float radius = (bowlSize * 0.5f) * itemDistanceRatio; // khoảng cách ra từ tâm

        Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

        // Tạo item
        GameObject newItem = new GameObject($"Item_{currentSlice}");
        newItem.transform.SetParent(transform, false);

        RectTransform newItemRect = newItem.AddComponent<RectTransform>();
        newItemRect.sizeDelta = itemSize;
        newItemRect.anchorMin = new Vector2(0.5f, 0.5f);
        newItemRect.anchorMax = new Vector2(0.5f, 0.5f);
        newItemRect.pivot = new Vector2(0.5f, 0.5f);
        newItemRect.anchoredPosition = pos;
        newItemRect.localRotation = Quaternion.identity;

        Image img = newItem.AddComponent<Image>();
        img.sprite = itemSprite;
        img.preserveAspect = true;
        img.raycastTarget = false;

        currentSlice++;
    }
}
