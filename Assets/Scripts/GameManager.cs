using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public SaladScriptableObject currentSalad;
    public SaladScriptableObject[] saladScriptableObjects;
    public List<Ingredient> ingredients = new();
    public TextMeshProUGUI currentAmountDisplayer;
    public GameObject background;
    public List<IngredientPlate> plates = new List<IngredientPlate>();
    public GameObject dropItemPrefab;
    public List<DropItem> dropItems = new List<DropItem>();
    public Button completeButton;

    public bool CanPlaceItem()
    {
        return dropItems.Count == currentSalad.ingredient.Length;
    }

    public int CheckScorev2()
    {
        HashSet<Ingredient> unique = new(); // tránh trùng nguyên liệu
        foreach (DropItem d in dropItems)
            if (d != null)
                unique.Add(d.ingredient);

        int point = 0;
        foreach (Ingredient ing in currentSalad.ingredient)
            if (unique.Contains(ing))
                point++;

        return point; // 0 → Max
    }

    public void AddDropItem(DropItem item)
    {
        if (!dropItems.Contains(item))
        {
            dropItems.Add(item);
            ScoreText.Instance.AnimateScoreTextIncrease();
            currentAmountDisplayer.text = dropItems.Count + "/" + currentSalad.ingredient.Length;
            if (CanPlaceItem())
            {
                completeButton.interactable = true;
            }
            else
            {
                completeButton.interactable = false;
            }
        }
    }

    public void RemoveDropItem(DropItem item)
    {
        if (dropItems.Remove(item)) // true nếu xoá được
        {
            ScoreText.Instance.AnimateScoreTextDecrease();
            currentAmountDisplayer.text = dropItems.Count + "/" + currentSalad.ingredient.Length;
            if (CanPlaceItem())
            {
                completeButton.interactable = true;
            }
            else
            {
                completeButton.interactable = false;
            }
        }
    }

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
        currentSalad = GetRandomSalad();
        GuideUI.Instance.Init(currentSalad);
        int saurceAmount = currentSalad.ingredient.Count(i => i.isSaurce);
        BowlManager.Instance.Init(
            currentSalad.ingredient.Length,
            currentSalad.ingredient.Length - saurceAmount
        );
        CompletePopup.Instance.Init(currentSalad);
        currentAmountDisplayer.text = 0 + "/" + currentSalad.ingredient.Length;
        PrepareIngredientTable.Instance.InitPlate(GetRandomUniqueItems(10));
        PrepareIngredientTable.Instance.InitPlate(currentSalad);
        PrepareIngredientTable.Instance.ShuffleChildObjects();
    }

    SaladScriptableObject GetRandomSalad()
    {
        List<SaladScriptableObject> validSalads = saladScriptableObjects
            .Where(s => s != PersistentData.Instance.previousSalad)
            .ToList();

        // Nếu không còn salad hợp lệ, sử dụng danh sách đầy đủ
        if (validSalads.Count == 0)
        {
            validSalads = saladScriptableObjects.ToList();
        }

        SaladScriptableObject newSalad = validSalads[Random.Range(0, validSalads.Count)];
        PersistentData.Instance.previousSalad = newSalad;
        return newSalad;
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

    public int CheckScore()
    {
        List<Ingredient> temp = new();
        int point = 0;
        for (int i = 0; i < plates.Count; ++i)
        {
            if (
                currentSalad.ingredient.Contains(plates[i].ingredient)
                && !temp.Contains(plates[i].ingredient)
            )
            {
                temp.Add(plates[i].ingredient);
                ++point;
            }
        }
        return point;
    }
}
