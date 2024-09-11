using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class IngredientPlate : MonoBehaviour
{
    public Ingredient ingredient;
    public ObjectToolTip objectToolTip;
    public Image itemImage;

    public void Start()
    {
        objectToolTip.Init(ingredient.ingredient_name);
    }
}
