using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideUI : MonoBehaviour
{
    public static GuideUI Instance { get; set; }
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public Image saladImage;
    public Image container;
    public GameObject note; // The note to display
    public Image background; // The note to display
    public Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f); // Initial scale for the animation
    public Color closeColor = new(1, 1, 1, 0);
    public float animationDuration = 0.3f; // Duration of the appear animation
    public Color openColor = new(1, 1, 1, 1);
    public TextMeshProUGUI button;
    public Button realButton;
    public bool IsOpen;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(SaladScriptableObject saladScriptable)
    {
        title.text = saladScriptable.salad_name;
        if (saladScriptable.salad_script_name != "")
        {
            title.text = saladScriptable.salad_script_name;
        }
        container.color = saladScriptable.backgroundColor;
        content.color = saladScriptable.textColor;
        content.text = saladScriptable.description;
        saladImage.sprite = saladScriptable.saladImage;
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
        CloseOpen(true);
    }

    private void OnDisable()
    {
        IsOpen = false;
    }

    public void CloseOpen(bool active)
    {
        realButton.gameObject.SetActive(active);
    }

    public void Open(string closeLabel = "Sẵn Sàng")
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

    private void OnDestroy()
    {
        DOTween.Kill(this); // Kills any tween associated with this object
    }
}
