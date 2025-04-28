using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class Ingredient : ScriptableObject
{
    public Sprite sprite;

    [TextAreaAttribute(1, 3)]
    public string ingredient_name;
    public bool isSaurce;
}
