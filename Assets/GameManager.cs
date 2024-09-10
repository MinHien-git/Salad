using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SaladScriptableObject currentSalad;
    public SaladScriptableObject[] saladScriptableObjects;
    public GuideUI guideUI;
    public TextMeshProUGUI currentAmountDisplayer;

    public void Start()
    {
        currentSalad = saladScriptableObjects[Random.Range(0, saladScriptableObjects.Length)];
    }
}
