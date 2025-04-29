using System.Collections.Generic;
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
    CanvasGroup cg;
    public RectTransform container;
    public int currentSauce = 0;
    public List<Vector2> sauceSlots = new List<Vector2>();
    public int maxSauceSlots = 4; // Số slot cố định
    public int currentSauceSlot = 0;
    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = gameObject.AddComponent<CanvasGroup>();
        }
        GenerateSlices();
        GenerateSauceSlots();
    }
   private void GenerateSauceSlots()
    {
        sauceSlots.Clear();

        float slotSpacing = 100f; // 🔥 Khoảng cách mỗi slot (bạn có thể chỉnh)

        int half = maxSauceSlots / 2;

        for (int i = 0; i < maxSauceSlots; i++)
        {
            float xOffset = (i - half) * slotSpacing;
            Vector2 localPos = new Vector2(xOffset, bowlSize * 0.3f); // 🔥 0.3f: vị trí sauce hơi phía trên Bowl 1 tí

            sauceSlots.Add(localPos);
        }

        Debug.Log($"Generated {sauceSlots.Count} sauce slots (horizontal)!");
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
            // 🔥 Dùng DOTween Sequence để vừa Scale vừa Shake Rotation cùng lúc
            Sequence bowlShakeSeq = DOTween.Sequence();

            bowlShakeSeq.Append(
                bowlRect.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad) // Scale to lên
            );
            bowlShakeSeq.Append(
                bowlRect.DOScale(1f, 0.1f).SetEase(Ease.OutQuad) // Scale nhỏ lại
            );

            bowlShakeSeq.Join(
                bowlRect
                    .DOShakeRotation(
                        0.4f,
                        strength: new Vector3(0f, 0f, 20f),
                        vibrato: 8,
                        randomness: 90
                    )
                    .SetEase(Ease.OutQuad)
            );
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

    public void AcceptSauceItem(DropItem dropItem)
    {
        if (dropItem == null || dropItem.linkedPlate == null)
        {
            Debug.LogWarning("DropItem hoặc linkedPlate null!");
            return;
        }

        if (currentSauce >= 2)
        {
            return;
        }
        Debug.Log("Bỏ vào ô Sauce Slots !");
        // Lấy rect của dropItem
        RectTransform dropRect = dropItem.GetComponent<RectTransform>();

        // Set parent về Bowl
        dropItem.transform.SetParent(container.transform, true);
        dropRect.anchorMin = new Vector2(0.5f, 0.5f);
        dropRect.anchorMax = new Vector2(0.5f, 0.5f);
        dropRect.pivot = new Vector2(0.5f, 0.5f);


        // Nếu cần resize sauce cho nhỏ hơn nguyên liệu 1 xíu, có thể thêm:
        dropRect.sizeDelta *= 2f;

        currentSauce++;

        dropItem.PlayLandingAnimation(); // Cho sauce cũng có hiệu ứng hạ cánh
    }

    public void AcceptDropItem(DropItem dropItem)
    {
        if (currentSlice >= numberOfSlices)
        {
            Debug.Log("Bowl đầy rồi!");
            Destroy(dropItem.gameObject);
            return;
        }

        float angle = (360f / numberOfSlices) * currentSlice + (180f / numberOfSlices);
        float rad = angle * Mathf.Deg2Rad;
        float radius = (bowlSize * 0.5f) * itemDistanceRatio;
        Vector2 localOffset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

        RectTransform dropRect = dropItem.GetComponent<RectTransform>();

        dropItem.transform.SetParent(transform, true);

        dropRect.anchorMin = new Vector2(0.5f, 0.5f);
        dropRect.anchorMax = new Vector2(0.5f, 0.5f);
        dropRect.pivot = new Vector2(0.5f, 0.5f);

        dropRect.localPosition = localOffset;

        currentSlice++;
        dropItem.PlayLandingAnimation();
        BounceBowl();
    }

    private void BounceBowl()
    {
        RectTransform bowlRect = GetComponent<RectTransform>();

        if (bowlRect != null)
        {
            bowlRect
                .DOScale(new Vector3(1.05f, 1.05f, 1f), 0.15f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    bowlRect.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutQuad);
                });
        }
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
