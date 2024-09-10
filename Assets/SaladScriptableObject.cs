using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SaladScriptableObject : ScriptableObject
{
    public Ingredient[] ingredient;
    public Image saladImage;
    public string salad_name;

    [TextAreaAttribute]
    public string description;
}
