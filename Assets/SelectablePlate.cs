using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectablePlate : MonoBehaviour, IPointerClickHandler
{
    public GameObject lid; // Drag lid (UI object) here
    public Image plateImage; // Drag plate image here

    // public SaladPopupUI popupUI; // Drag reference to shared popup UI

    private SaladScriptableObject assignedSalad;
    private bool isClicked = false;

    void Start()
    {
        // Scale pulse idle animation (subtle breathing effect)
        transform
            .DOScale(Vector3.one * 1.05f, 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void AssignSalad(SaladScriptableObject salad)
    {
        assignedSalad = salad;
        plateImage.sprite = salad.saladImage; // Assuming SaladScriptableObject has a sprite for the plate
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked || assignedSalad == null)
            return;

        isClicked = true;
        PersistentData.Instance.selectedSalad = assignedSalad;
        PersistentData.Instance.previousSalad = assignedSalad;
        // Animate the lid up (assumes vertical lift)
        lid.transform.DOLocalMoveY(lid.transform.localPosition.y + 100f, 0.4f) // Adjust value for UI
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                SelectSaladShowcase.Instance.Init(assignedSalad);
                SelectSaladShowcase.Instance.Open();
                // popupUI.Show(assignedSalad);
            });
    }
}
