using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapHandler : MonoBehaviour
{
    public float doubleTapTime = 0.3f;
    private float lastTapTime = 0f;
    private bool isWaitingForSecondTap = false;
    private GameObject selectedObject;

    public LayerMask clickableLayer; // Only clickable objects

    void Update()
    {
        // Handle Mouse Input
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleTap(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        // Handle Touch Input
        if (
            Input.touchCount > 0
            && Input.touches[0].phase == TouchPhase.Began
            && !IsTouchOverUI(Input.touches[0])
        )
        {
            HandleTap(Camera.main.ScreenToWorldPoint(Input.touches[0].position));
        }

        if (isWaitingForSecondTap && (Time.time - lastTapTime) > doubleTapTime)
        {
            isWaitingForSecondTap = false;
            SingleTapAction(selectedObject);
        }
    }

    void HandleTap(Vector3 tapPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            tapPosition,
            Vector2.zero,
            Mathf.Infinity,
            clickableLayer
        );
        if (hit.collider != null)
        {
            GameObject tappedObject = hit.collider.gameObject;

            if (isWaitingForSecondTap && tappedObject == selectedObject)
            {
                DoubleTapAction(tappedObject);
                isWaitingForSecondTap = false;
            }
            else
            {
                selectedObject = tappedObject;
                isWaitingForSecondTap = true;
                lastTapTime = Time.time;
            }
        }
    }

    bool IsTouchOverUI(Touch touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    void SingleTapAction(GameObject obj)
    {
        if (obj == null)
            return;
        Debug.Log("Selected: " + obj.name);
    }

    void DoubleTapAction(GameObject obj)
    {
        if (obj == null)
            return;
        Debug.Log("Double Tapped: " + obj.name);
        Transform parent = obj.transform.parent;
        if (parent != null)
        {
            obj.transform.SetAsLastSibling();
        }
    }
}
