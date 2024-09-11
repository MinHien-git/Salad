using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePane : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI explaining;

    private void OnEnable()
    {
        explaining.text = "Score: " + GameManager.Instance.currentSalad.ingredient_information;
        scoreText.text = GameManager.Instance.currentAmountDisplayer.text;
    }
}
