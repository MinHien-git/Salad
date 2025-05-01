using System.Collections.Generic;
using UnityEngine;

public class SelectSaladManager : MonoBehaviour
{
    public SaladScriptableObject[] saladOptions; // assign in Inspector
    public SelectablePlate[] plates; // assign plates in Inspector

    void Start()
    {
        // Shuffle saladOptions
        ShuffleArray(saladOptions);

        // Assign each shuffled salad to a plate (up to plate count)
        for (int i = 0; i < plates.Length && i < saladOptions.Length; i++)
        {
            plates[i].AssignSalad(saladOptions[i]);
        }
    }

    // Fisher-Yates shuffle
    private void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (array[i], array[rand]) = (array[rand], array[i]);
        }
    }
}
