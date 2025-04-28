using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITapHandler : MonoBehaviour, IPointerDownHandler
{
    public float doubleTapMaxTime = 0.3f;
    private float lastTapTime = -1f;
    private bool waitingSecondTap = false;

    [Header("Single Tap Settings")]
    public Transform horizontalLayoutGroupParent;
    private Transform bowlTarget;

    [Header("Target Offset Settings")]
    public float offsetY = 50f; // Bay cao hơn Bowl 50
    public float offsetX = 20f; // Lệch ngang 20

    public float moveDuration = 0.5f;
    public float dropDuration = 0.5f;
    public float fadeDuration = 0.2f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Tween idleTween;

    [Header("Idle Animation Settings")]
    public RectTransform targetForIdleAnimation;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        GameObject bowlObj = GameObject.FindGameObjectWithTag("Bowl");
        if (bowlObj != null)
        {
            bowlTarget = bowlObj.transform;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Bowl (Tag = 'Bowl') trong scene.");
        }

        if (targetForIdleAnimation == null)
        {
            targetForIdleAnimation = rectTransform; // Nếu chưa gán thì tự dùng chính object
        }

        PlayIdleAnimation();
    }

    private void PlayIdleAnimation()
    {
        if (targetForIdleAnimation == null)
            return;

        idleTween = targetForIdleAnimation
            .DOScale(new Vector3(1.05f, 1.05f, 1f), 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (waitingSecondTap && (Time.unscaledTime - lastTapTime) <= doubleTapMaxTime)
        {
            DoubleTap();
            waitingSecondTap = false;
        }
        else
        {
            lastTapTime = Time.unscaledTime;
            waitingSecondTap = true;
            Invoke(nameof(CheckSingleTap), doubleTapMaxTime);
        }
    }

    private void CheckSingleTap()
    {
        if (waitingSecondTap)
        {
            waitingSecondTap = false;
            SingleTap();
        }
    }

    private void ShakePlate()
    {
        Debug.Log("Bowl đầy rồi, lắc cái plate!");

        rectTransform
            .DOShakePosition(0.3f, strength: new Vector3(10f, 0f, 0f), vibrato: 10, randomness: 90)
            .SetEase(Ease.OutQuad);
    }

    private void SingleTap()
    {
        Debug.Log("Single Tap on: " + gameObject.name);

        if (horizontalLayoutGroupParent != null)
        {
            transform.SetParent(horizontalLayoutGroupParent, true);
        }

        if (bowlTarget == null)
        {
            Debug.LogError("Không tìm thấy Bowl!");
            return;
        }
        BowlManager bowlManager = bowlTarget.GetComponent<BowlManager>();
        if (bowlManager != null && bowlManager.IsFull())
        {
            ShakePlate();
            bowlManager.ShakeBowl();
            return;
        }

        Vector3 bowlPos = bowlTarget.position;

        // Tính target vị trí
        Vector3 targetLeft = new Vector3(bowlPos.x - offsetX, bowlPos.y + offsetY, 0f);
        Vector3 targetRight = new Vector3(bowlPos.x + offsetX, bowlPos.y + offsetY, 0f);

        // Random chọn bên trái hoặc phải
        Vector3 targetPos = (Random.value < 0.5f) ? targetLeft : targetRight;

        rectTransform
            .DOMove(targetPos, moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                SpawnDropItemBezier(targetPos, bowlTarget.position);
                FadeAndDestroy();
            });
    }

    private void FadeAndDestroy()
    {
        if (idleTween != null && idleTween.IsActive())
        {
            idleTween.Kill();
        }

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup
            .DOFade(0f, fadeDuration)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    private void SpawnDropItemBezier(Vector3 fromPos, Vector3 toPos)
    {
        IngredientPlate ingredientPlate = GetComponent<IngredientPlate>();
        if (ingredientPlate == null || ingredientPlate.itemImage == null)
        {
            Debug.LogWarning("Không tìm thấy IngredientPlate hoặc itemImage.");
            return;
        }

        // 🔥 Tạo dropItem từ prefab
        GameObject dropItemObj = Instantiate(GameManager.Instance.dropItemPrefab, canvas.transform);
        RectTransform dropRect = dropItemObj.GetComponent<RectTransform>();
        dropRect.position = fromPos;

        // 🧠 Gán ảnh vào DropItem script
        DropItem dropItem = dropItemObj.GetComponent<DropItem>();
        if (dropItem != null)
        {
            dropItem.SetSprite(ingredientPlate.itemImage.sprite);
        }

        // Move theo Bezier
        DropItemBezier bezierMove = dropItemObj.GetComponent<DropItemBezier>();
        if (bezierMove == null)
            bezierMove = dropItemObj.AddComponent<DropItemBezier>();

        bezierMove.P0 = fromPos;
        bezierMove.P2 = toPos;
        bezierMove.P1 = (fromPos + toPos) / 2f + new Vector3(0, 5f, 0f);
        bezierMove.duration = 0.5f;

        bezierMove.OnReachDestination = () =>
        {
            BowlManager bowlManager = bowlTarget.GetComponent<BowlManager>();
            DropItem dropItem = dropItemObj.GetComponent<DropItem>();

            if (bowlManager != null && dropItem != null)
            {
                bowlManager.AcceptDropItem(dropItem);
            }
        };
    }

    private void DoubleTap()
    {
        Debug.Log("Double Tap: Move to Last Sibling - " + gameObject.name);
        if (transform.parent != null)
        {
            transform.SetAsLastSibling();
        }
    }
}
