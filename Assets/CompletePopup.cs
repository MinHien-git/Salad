using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletePopup : MonoBehaviour
{
    public static CompletePopup Instance { get; set; }
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public Image saladImage;
    public GameObject note; // The note to display
    public Image background; // The note to display
    public Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f); // Initial scale for the animation
    public Color closeColor = new(1, 1, 1, 0);
    public float animationDuration = 0.3f; // Duration of the appear animation
    public Color openColor = new(1, 1, 1, 1);
    public TextMeshProUGUI button;
    public bool IsOpen;

    private void Awake()
    {
        Instance = this;
        note.SetActive(false);
    }

    public void Init(SaladScriptableObject saladScriptable)
    {
        title.text = saladScriptable.salad_name;
        content.text = saladScriptable.ingredient_description;
    }

    public void CloseGuide()
    {
        // note.SetActive(false);
        // background.gameObject.SetActive(false);
        // note.transform.localScale = initialScale;

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
        IsOpen = false;
    }

    private void OnEnable()
    {
        Open();
    }

    private void OnDisable()
    {
        IsOpen = false;
    }

    public void Open(string closeLabel = "Next")
    {
        if (!IsOpen)
        {
            IsOpen = true;
            button.text = closeLabel;
            note.transform.localScale = initialScale;
            background.color = closeColor;
            // Show the note and animate it
            note.SetActive(true);
            background.gameObject.SetActive(true);
            note.transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
            background.DOColor(openColor, animationDuration).SetEase(Ease.OutBack);
        }
    }
}
