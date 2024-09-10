using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaladScriptableObject : ScriptableObject
{
    public Ingredient[] ingredient;
    public string salad_name;

    [TextAreaAttribute]
    public string description;
}
