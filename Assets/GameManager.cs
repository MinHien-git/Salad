using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public SaladScriptableObject currentSalad;
    public SaladScriptableObject[] saladScriptableObjects;
    public TextMeshProUGUI currentAmountDisplayer;
    public GameObject background;
    public List<IngredientPlate> plates = new List<IngredientPlate>();

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        currentSalad = saladScriptableObjects[Random.Range(0, saladScriptableObjects.Length)];
        GuideUI.Instance.Init(currentSalad);
        CompletePopup.Instance.Init(currentSalad);
        currentAmountDisplayer.text = 0 + "/" + currentSalad.ingredient.Length;
    }

    public void RemovePlate(IngredientPlate plate)
    {
        if (plates.Contains(plate))
        {
            plates.Remove(plate);
            ScoreText.Instance.AnimateScoreTextDecrease();
            currentAmountDisplayer.text = plates.Count + "/" + currentSalad.ingredient.Length;
        }
    }

    public void AddPlate(IngredientPlate plate)
    {
        if (!plates.Contains(plate))
        {
            plates.Add(plate);
            ScoreText.Instance.AnimateScoreTextIncrease();
            currentAmountDisplayer.text = plates.Count + "/" + currentSalad.ingredient.Length;
        }
    }

    public bool CanPlacePlate()
    {
        return plates.Count == currentSalad.ingredient.Length;
    }
}
