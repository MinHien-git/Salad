using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DropItemBezier : MonoBehaviour
{
    public Vector3 P0,
        P1,
        P2;
    public float duration = 0.5f;

    public Action OnReachDestination; // ➡️ Callback sau khi bay xong

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(MoveAlongBezier());
    }

    IEnumerator MoveAlongBezier()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Vector3 pos = CalculateQuadraticBezierPoint(t, P0, P1, P2);
            rectTransform.position = pos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.position = P2; // Ensure exact end position

        OnReachDestination?.Invoke(); // Gọi callback

        // KHÔNG destroy ở đây
    }

    // Công thức Bezier bậc 2
    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}
