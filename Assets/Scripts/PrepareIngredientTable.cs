using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareIngredientTable : MonoBehaviour
{
    public static PrepareIngredientTable Instance { get; set; }
    public Transform container;
    public IngredientPlate platePrefab;

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetReturnWorldPos()
    {
        // Hạ cánh ngay chính giữa bàn (container) – muốn chỉnh thì bù thêm offset
        return container.position;
    }

    public void InitPlate(List<Ingredient> ingredients)
    {
        for (int i = 0; i < ingredients.Count; ++i)
        {
            IngredientPlate plate = Instantiate(platePrefab, container);
            plate.Init(ingredients[i]);
        }
        ShuffleChildObjects();
    }

    public void InitPlate(SaladScriptableObject saladScriptable)
    {
        for (int i = 0; i < saladScriptable.ingredient.Length; ++i)
        {
            IngredientPlate plate = Instantiate(platePrefab, container);
            plate.Init(saladScriptable.ingredient[i]);
        }
    }

    public void ShuffleChildObjects()
    {
        // Lấy danh sách các đối tượng con của GameObject hiện tại
        List<Transform> children = new List<Transform>();
        foreach (Transform child in container)
        {
            children.Add(child);
        }

        // Xáo trộn danh sách
        for (int i = 0; i < children.Count; i++)
        {
            Transform temp = children[i];
            int randomIndex = Random.Range(i, children.Count);
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        // Đặt lại thứ tự trong hierarchy
        foreach (Transform child in children)
        {
            child.SetSiblingIndex(children.IndexOf(child));
        }
    }
}
