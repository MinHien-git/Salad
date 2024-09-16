using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPlate : MonoBehaviour
{
    public Ingredient ingredient;
    public ObjectToolTip objectToolTip;
    public Image itemImage;

    public void Start()
    {
        itemImage.sprite = ingredient.sprite;
        objectToolTip.Init(ingredient.ingredient_name);
    }

    public void Init(Ingredient ingredient)
    {
        this.ingredient = ingredient;
        itemImage.sprite = ingredient.sprite;
        objectToolTip.Init(ingredient.ingredient_name);
    }
}
