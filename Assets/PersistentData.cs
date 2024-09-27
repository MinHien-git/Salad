using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance { get; private set; }
    public SaladScriptableObject previousSalad;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Đảm bảo không bị phá hủy khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
