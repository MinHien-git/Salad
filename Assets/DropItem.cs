using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private Image shadow;
    private Tween idleTween;
    public Ingredient ingredient; // 🔥 Link tới plate gốc

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICKED " + name);
        ReturnToTable();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ReturnToTable();
    }

    private void ReturnToTable()
    {
        if (PrepareIngredientTable.Instance == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 start = transform.position;
        Vector3 end = PrepareIngredientTable.Instance.GetReturnWorldPos();
        float h = Vector3.Distance(start, end) * 0.1f;
        Vector3 mid = (start + end) * 0.5f + Vector3.up * h;

        DropItemBezier bez = GetComponent<DropItemBezier>();
        if (bez == null)
            bez = gameObject.AddComponent<DropItemBezier>();

        bez.Play(
            start,
            mid,
            end,
            0.4f,
            () =>
            {
                // 1. Sinh plate mới
                if (ingredient != null)
                {
                    IngredientPlate p = Instantiate(
                        PrepareIngredientTable.Instance.platePrefab,
                        end,
                        Quaternion.identity,
                        PrepareIngredientTable.Instance.container
                    );
                    p.Init(ingredient);
                }

                // 2. Báo bowl & huỷ chính mình
                BowlManager bowl = GetComponentInParent<BowlManager>();
                if (bowl != null)
                    bowl.RemoveDropItem(this);

                Destroy(gameObject);
            }
        );
    }

    private void Start()
    {
        // Không gọi PlayIdle ngay, chờ landing xong mới idle
    }

    public void SetSprite(Sprite sprite)
    {
        if (itemImage == null)
        {
            itemImage = GetComponent<Image>();
        }
        itemImage.sprite = sprite;
        itemImage.preserveAspect = true;
    }

    public void SetLinkedPlate(IngredientPlate plate)
    {
        ingredient = plate.ingredient;
        RectTransform rect = itemImage.GetComponent<RectTransform>();

        if (ingredient != null && ingredient.isSaurce)
        {
            rect.sizeDelta *= 1.25f; // 🔥 Gấp đôi kích thước!
            rect.pivot = new Vector2(0.5f, 0f); // Giữa giữa
            shadow.gameObject.SetActive(true); // Hiện shadow
        }
        else
        {
            rect.pivot = new Vector2(0.5f, 0.5f);
        }
    }

    public void PlayLandingAnimation()
    {
        RectTransform rect = GetComponent<RectTransform>();

        Sequence landingSeq = DOTween.Sequence();

        landingSeq
            .Append(rect.DOScale(new Vector3(1.2f, 0.8f, 1f), 0.1f).SetEase(Ease.OutQuad)) // Bẹp
            .Append(rect.DOScale(new Vector3(0.9f, 1.1f, 1f), 0.1f).SetEase(Ease.OutQuad)) // Nảy lên
            .Append(rect.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutQuad)) // Về bình thường
            .OnComplete(() => PlayIdleAnimation()); // Sau khi landing xong mới Idle
    }

    private void PlayIdleAnimation()
    {
        RectTransform rect = GetComponent<RectTransform>();

        idleTween = rect.DOScale(new Vector3(1.05f, 0.95f, 1f), 1f) // Xoay nhẹ theo kiểu "nảy" (hơi bẹt xuống rồi phồng)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        if (idleTween != null && idleTween.IsActive())
        {
            idleTween.Kill();
        }
    }
}
