using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SaladScriptableObject currentSalad;
    public SaladScriptableObject[] saladScriptableObjects;
    public TextMeshProUGUI currentAmountDisplayer;
    public GameObject background;

    public void Start()
    {
        currentSalad = saladScriptableObjects[Random.Range(0, saladScriptableObjects.Length)];
        GuideUI.Instance.Init(currentSalad);
        currentAmountDisplayer.text = 0 + "/" + currentSalad.ingredient.Length;
    }
}
