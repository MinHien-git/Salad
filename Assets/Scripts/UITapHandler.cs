using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITapHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float doubleTapMaxTime = 0.3f;
    private float lastTapTime = -1f;
    private bool waitingSecondTap = false;

    private bool pointerDown = false;
    private float holdTimer = 0f;
    private bool cancelTapBecauseHold = false;

    public float holdThreshold = 0.5f; // <— thời gian vượt qua => coi là Hold, không Tap nữa

    // [Header("Single Tap Settings")]
    // public Transform horizontalLayoutGroupParent;
    private Transform bowlTarget;

    [Header("Target Offset Settings")]
    public float offsetY = 50f;
    public float offsetX = 20f;
    public float moveDuration = 0.5f;
    public float dropDuration = 0.5f;
    public float fadeDuration = 0.2f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Tween idleTween;

    [Header("Idle Animation Settings")]
    public RectTransform targetForIdleAnimation;
    public IngredientPlate ingredientPlate;

    private void Start()
    {
        ingredientPlate = GetComponent<IngredientPlate>();
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        GameObject bowlObj = GameObject.FindGameObjectWithTag("Bowl");
        if (bowlObj != null)
        {
            bowlTarget = bowlObj.transform;
        }

        if (targetForIdleAnimation == null)
        {
            targetForIdleAnimation = rectTransform;
        }

        PlayIdleAnimation();
    }

    private void Update()
    {
        if (pointerDown)
        {
            holdTimer += Time.unscaledDeltaTime;
            if (!cancelTapBecauseHold && holdTimer >= holdThreshold)
            {
                cancelTapBecauseHold = true;
            }
        }
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
        pointerDown = true;
        holdTimer = 0f;
        cancelTapBecauseHold = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;

        if (!cancelTapBecauseHold)
        {
            HandleTap();
        }
    }

    private void HandleTap()
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

    private void SingleTap()
    {
        Debug.Log("Single Tap Spawn!");
        if (GameManager.Instance.CanPlaceItem())
        {
            Debug.Log("Không thể đặt nguyên liệu nữa!");
            return;
        }
        // if (horizontalLayoutGroupParent != null)
        // {
        //     transform.SetParent(horizontalLayoutGroupParent, true);
        // }

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
        if (ingredientPlate.ingredient.isSaurce)
        {
            if (bowlManager != null && bowlManager.currentSauceSlot < bowlManager.sauceSlots.Count)
            {
                Vector2 localSlot = bowlManager.sauceSlots[bowlManager.currentSauceSlot];
                Vector3 targetWorldPos = bowlManager.container.TransformPoint(localSlot);

                rectTransform
                    .DOMove(targetWorldPos, moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        bowlManager.currentSauceSlot++;
                        SpawnDropItemBezier(
                            targetWorldPos,
                            bowlManager.container.TransformPoint(localSlot)
                        );
                        FadeAndDestroy();
                    });

                return; // 🔥 Xong sauce, không cần drop ngẫu nhiên
            }
        }
        // Vector3 targetLeft = new Vector3(bowlPos.x - offsetX, bowlPos.y + offsetY, 0f);
        // Vector3 targetRight = new Vector3(bowlPos.x + offsetX, bowlPos.y + offsetY, 0f);
        // Vector3 targetPos = (Random.value < 0.5f) ? targetLeft : targetRight;

        // rectTransform
        //     .DOMove(targetPos, moveDuration)
        //     .SetEase(Ease.OutQuad)
        //     .OnComplete(() =>
        //     {
        // BowlManager bowlManager = bowlTarget.GetComponent<BowlManager>();
        transform.SetParent(bowlManager.transform);
        // ★ Lấy world-pos lát tiếp theo
        Vector3 sliceWorldPos = bowlManager.GetNextSliceWorldPos();

        // ★ Bay thẳng đến vị trí lát (không ghé tâm)
        SpawnDropItemBezier(transform.position, sliceWorldPos);

        FadeAndDestroy();
        // });
    }

    private void FadeAndDestroy()
    {
        if (idleTween != null && idleTween.IsActive())
            idleTween.Kill();

        canvasGroup
            .DOFade(0f, fadeDuration)
            .OnComplete(() =>
            {
                transform.parent = null;
            });
    }

    private void SpawnDropItemBezier(Vector3 fromPos, Vector3 toPos)
    {
        if (ingredientPlate == null || ingredientPlate.itemImage == null)
        {
            Debug.LogWarning("Không tìm thấy IngredientPlate hoặc itemImage.");
            return;
        }

        GameObject dropItemObj = Instantiate(GameManager.Instance.dropItemPrefab, canvas.transform);
        RectTransform dropRect = dropItemObj.GetComponent<RectTransform>();
        dropRect.position = fromPos;

        DropItem dropItem = dropItemObj.GetComponent<DropItem>();

        dropItem.SetSprite(ingredientPlate.itemImage.sprite);
        dropItem.SetLinkedPlate(ingredientPlate);

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
            if (bowlManager != null)
            {
                if (dropItem.ingredient != null && dropItem.ingredient.isSaurce)
                {
                    Debug.Log("Đã thêm sauce!");
                    bowlManager.AcceptSauceItem(dropItem);
                }
                else
                {
                    Debug.Log("Đã thêm Nguyên liệu!");
                    bowlManager.AcceptDropItem(dropItem);
                }
            }
        };
    }

    private void ShakePlate()
    {
        Sequence shakeSeq = DOTween.Sequence();
        shakeSeq.Join(
            rectTransform
                .DOShakeRotation(0.4f, strength: new Vector3(0, 0, 20f), vibrato: 8, randomness: 90)
                .SetEase(Ease.OutQuad)
        );

        shakeSeq.Join(canvasGroup.DOFade(1f, 0.1f).OnComplete(() => canvasGroup.DOFade(1f, 0.2f)));
    }

    private void DoubleTap()
    {
        Debug.Log("Double Tap Move!");
        if (transform.parent != null)
        {
            transform.SetAsLastSibling();
        }
    }
}
