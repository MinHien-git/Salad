using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Salad", order = 1)]
public class SaladScriptableObject : ScriptableObject
{
    public Ingredient[] ingredient;
    public Sprite saladImage;
    public string salad_name;

    [TextAreaAttribute(10, 20)]
    public string description;

    [TextAreaAttribute(10, 20)]
    public string ingredient_description;

    [TextAreaAttribute(10, 20)]
    public string ingredient_information;
}
