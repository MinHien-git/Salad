using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldToDisplayNote
    : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerExitHandler
{
    public GameObject note; // The note to display
    public Image background; // The note to display
    public float holdDuration = 2.0f; // Time in seconds before the note appears
    public float animationDuration = 0.3f; // Duration of the appear animation
    public Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f); // Initial scale for the animation
    public Color closeColor = new(1, 1, 1, 0);
    public Color openColor = new(1, 1, 1, 1);
    private bool isPointerDown = false;
    private bool hasDisplayedNote = false;
    private float pointerDownTimer;

    private void Start()
    {
        // Ensure the note is hidden and set its initial scale at the start
        note.SetActive(false);
        background.gameObject.SetActive(false);
        note.transform.localScale = initialScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Start the hold detection
        isPointerDown = true;
        hasDisplayedNote = false;
        pointerDownTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Reset the hold detection
        isPointerDown = false;
        pointerDownTimer = 0f;

        if (hasDisplayedNote)
        {
            HideNote();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Cancel the hold detection if the pointer exits the UI element
        isPointerDown = false;
        pointerDownTimer = 0f;

        if (hasDisplayedNote)
        {
            HideNote();
        }
    }

    private void Update()
    {
        // Track how long the pointer has been held down
        if (isPointerDown && !hasDisplayedNote)
        {
            pointerDownTimer += Time.deltaTime;

            if (pointerDownTimer >= holdDuration)
            {
                DisplayNote();
                hasDisplayedNote = true;
            }
        }
    }

    private void DisplayNote()
    {
        // Show the note and animate it
        note.SetActive(true);
        background.gameObject.SetActive(true);
        note.transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
        background.DOColor(openColor, animationDuration).SetEase(Ease.OutBack);
    }

    private void HideNote()
    {
        // Animate the note out and hide it
        note.transform.DOScale(initialScale, animationDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                note.SetActive(false);
            });
        background
            .DOColor(closeColor, animationDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                background.gameObject.SetActive(false);
            });
    }
}
