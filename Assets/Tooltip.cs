using DG.Tweening;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public static Tooltip Instance;

    private RectTransform tooltipRectTransform;
    private Canvas canvas;
    public TextMeshProUGUI tooltipText;
    public RectTransform tooltipTail;

    private void Awake()
    {
        // Ensure this is a singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        tooltipRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        HideTooltip(true); // Ensure tooltip is hidden by default
    }

    public void ShowTooltip(
        RectTransform targetRect,
        float yOffsetAbove,
        float yOffsetBelow,
        string text,
        float xOffsetLeft = 30f,
        float xOffsetRight = 30f
    )
    {
        // Set the text and update the layout
        tooltipText.text = text;
        tooltipText.ForceMeshUpdate(); // Ensure the text is updated before measuring
        float textWidth = tooltipText.preferredWidth;

        // Adjust the tooltip's width to fit the text
        tooltipRectTransform.sizeDelta = new Vector2(
            textWidth - 60,
            tooltipRectTransform.sizeDelta.y
        );

        // Set the tooltip as a child of the target for initial positioning
        tooltipRectTransform.SetParent(targetRect, false);
        tooltipRectTransform.SetAsLastSibling(); // Ensure it's rendered on top of other elements

        // Get the position of the target in screen space
        Vector3[] worldCorners = new Vector3[4];
        targetRect.GetWorldCorners(worldCorners);

        // Use the camera to convert world positions to screen positions
        Camera camera = canvas.worldCamera != null ? canvas.worldCamera : Camera.main;
        Vector3 topLeftScreen = camera.WorldToScreenPoint(worldCorners[1]);
        Vector3 bottomLeftScreen = camera.WorldToScreenPoint(worldCorners[0]);
        Vector3 topRightScreen = camera.WorldToScreenPoint(worldCorners[2]);
        Vector3 bottomRightScreen = camera.WorldToScreenPoint(worldCorners[3]);

        // Determine available screen space
        float screenHeight = camera.pixelHeight;
        float screenWidth = camera.pixelWidth;

        // Default positions (centered on the target)
        float tooltipX = 0;
        float tooltipY = 0;
        float tooltipLeftEdge = topLeftScreen.x - (tooltipRectTransform.rect.width / 2);
        float tooltipRightEdge = topRightScreen.x + (tooltipRectTransform.rect.width / 2);
        // Attempt top/bottom placement first
        if (
            topLeftScreen.y + tooltipRectTransform.rect.height + yOffsetAbove <= screenHeight
            && tooltipLeftEdge >= 0
            && tooltipRightEdge <= screenWidth
        )
        {
            // Place tooltip above the target
            tooltipY = targetRect.rect.height / 2 + yOffsetAbove;
            tooltipTail.localPosition = new Vector2(0, -tooltipRectTransform.rect.height / 2 + 4); // Align tail with the center of the tooltip
        }
        else if (
            bottomLeftScreen.y - tooltipRectTransform.rect.height - yOffsetBelow >= 0
            && tooltipLeftEdge >= 0
            && tooltipRightEdge <= screenWidth
        )
        {
            // Place tooltip below the target
            tooltipY = -targetRect.rect.height / 2 - yOffsetBelow;
            tooltipTail.localPosition = new Vector2(0, tooltipRectTransform.rect.height / 2 - 4);
        }
        else if (topRightScreen.x + tooltipRectTransform.rect.width + xOffsetRight <= screenWidth)
        {
            // If no space above/below, place to the right of the target
            tooltipX =
                targetRect.rect.width / 2 + xOffsetRight + tooltipRectTransform.rect.width / 2;
            tooltipY = 0; // Align Y-axis with the center of the target
            tooltipTail.localPosition = new Vector2(-tooltipRectTransform.rect.width / 2 - 50, 0);
        }
        else if (topLeftScreen.x - tooltipRectTransform.rect.width - xOffsetLeft >= 0)
        {
            // If no space on the right, place to the left of the target
            tooltipX =
                -targetRect.rect.width / 2 - xOffsetLeft - tooltipRectTransform.rect.width / 2;
            tooltipY = 0; // Align Y-axis with the center of the target
            tooltipTail.localPosition = new Vector2(tooltipRectTransform.rect.width / 2 + 50, 0);
        }
        else
        {
            // Default to below if space is tight (worst-case scenario)
            tooltipY =
                -targetRect.rect.height / 2 - yOffsetBelow - tooltipRectTransform.rect.height;
            tooltipX = targetRect.rect.width / 2 + xOffsetRight;
            tooltipTail.localPosition = new Vector2(-tooltipRectTransform.rect.width / 2 - 50, 0);
        }

        // Set the tooltip's anchored position
        tooltipRectTransform.anchoredPosition = new Vector2(tooltipX, tooltipY);

        // Set tooltip as a child of the canvas for proper rendering
        tooltipRectTransform.SetParent(canvas.transform);
        tooltipRectTransform.localPosition = new Vector3(
            tooltipRectTransform.anchoredPosition.x,
            tooltipRectTransform.anchoredPosition.y,
            0
        );
        // Show the tooltip with an animation
        tooltipRectTransform.localScale = Vector3.zero; // Start from scale zero
        tooltipRectTransform.gameObject.SetActive(true);
        tooltipRectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack); // Animate to full size
    }

    public void HideTooltip(bool immediate = false)
    {
        tooltipRectTransform.SetParent(canvas.transform);
        if (immediate)
        {
            tooltipRectTransform.localScale = Vector3.zero;
            tooltipRectTransform.gameObject.SetActive(false);
        }
        else
        {
            tooltipRectTransform
                .DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    tooltipRectTransform.gameObject.SetActive(false);
                });
        }
        tooltipRectTransform.localPosition = new Vector3(
            tooltipRectTransform.anchoredPosition.x,
            tooltipRectTransform.anchoredPosition.y,
            0
        );
    }

    private void OnDestroy()
    {
        DOTween.Kill(this); // Kills any tween associated with this object
    }
}
