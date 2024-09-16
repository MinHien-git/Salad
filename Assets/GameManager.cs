using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public SaladScriptableObject currentSalad;
    public SaladScriptableObject[] saladScriptableObjects;
    public List<Ingredient> ingredients = new();
    public TextMeshProUGUI currentAmountDisplayer;
    public GameObject background;
    public List<IngredientPlate> plates = new List<IngredientPlate>();

    void Awake()
    {
        Instance = this;
    }

    public List<Ingredient> GetRandomUniqueItems(int amount)
    {
        // Kiểm tra nếu số lượng cần lấy lớn hơn danh sách gốc
        if (amount > ingredients.Count)
        {
            Debug.LogError("Số lượng cần lấy lớn hơn danh sách nguồn.");
            return null;
        }

        // Tạo danh sách kết quả và sao chép danh sách nguồn
        List<Ingredient> result = new();
        List<Ingredient> tempList = new(ingredients);

        // Xáo trộn danh sách tạm thời
        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            result.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex); // Đảm bảo tính duy nhất bằng cách loại bỏ phần tử đã chọn
        }

        return result;
    }

    public void Start()
    {
        currentSalad = saladScriptableObjects[Random.Range(0, saladScriptableObjects.Length)];
        GuideUI.Instance.Init(currentSalad);
        CompletePopup.Instance.Init(currentSalad);
        currentAmountDisplayer.text = 0 + "/" + currentSalad.ingredient.Length;
        PrepareIngredientTable.Instance.InitPlate(GetRandomUniqueItems(5));
        PrepareIngredientTable.Instance.InitPlate(currentSalad);
        PrepareIngredientTable.Instance.ShuffleChildObjects();
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
