using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;
    private Tween idleTween;
    public IngredientPlate linkedPlate; // 🔥 Link tới plate gốc

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
        linkedPlate = plate;
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
