using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePane : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI explaining;
    public ParticleSystem[] randomParticles;
    public ParticleSystem[] sadParticles;
    public ParticleSystem[] regretParticles;
    public ParticleSystem particle;
    public Image background;
    public TextMeshProUGUI ingredient_description;
    public TextMeshProUGUI ingredient_information;
    public Image[] buttonOutline;
    public TextMeshProUGUI[] buttonText;
    public VerticalLayoutGroup layoutGroup;

    private void OnEnable()
    {
        if (GameManager.Instance.currentSalad == null)
            return;
        SaladScriptableObject salad = GameManager.Instance.currentSalad;
        background.color = salad.backgroundColor;
        ingredient_description.text = salad.ingredient_description;
        ingredient_description.color = salad.textColor;
        ingredient_information.text = salad.ingredient_information;
        ingredient_information.color = salad.textColor;
        scoreText.color = salad.textColor;
        explaining.color = salad.textColor;
        for (int i = 0; i < buttonOutline.Length; ++i)
        {
            buttonOutline[i].color = salad.textColor;
            buttonText[i].color = salad.backgroundColor;
        }
        scoreText.text =
            GameManager.Instance.CheckScorev2()
            + "/"
            + GameManager.Instance.currentSalad.ingredient.Length;

        if (
            GameManager.Instance.currentSalad.ingredient.Length / 2
            <= GameManager.Instance.CheckScorev2()
        )
        {
            particle.Play();
            for (int i = 0; i < randomParticles.Length; ++i)
            {
                randomParticles[i].transform.position = GetRandomPositionInsideScreen();
                randomParticles[i].Play();
            }
            explaining.text = "Chúc mừng, bạn đã hoàn thành phần chơi của mình ";
        }
        else if (
            (int)GameManager.Instance.currentSalad.ingredient.Length / 2f
            > GameManager.Instance.CheckScorev2()
        )
        {
            explaining.text =
                "Tiếc quá, còn chút nữa thôi là hoàn thành món "
                + GameManager.Instance.currentSalad.salad_name;
            for (int i = 0; i < randomParticles.Length; ++i)
            {
                regretParticles[i].transform.position = GetRandomPositionInsideScreen();
                regretParticles[i].Play();
            }
        }
        else
        {
            explaining.text = "Bạn chưa đọc kĩ đề bài đúng không?";
            for (int i = 0; i < randomParticles.Length; ++i)
            {
                sadParticles[i].transform.position = GetRandomPositionInsideScreen();
                sadParticles[i].Play();
            }
        }
        StartCoroutine(RefreshLayoutNextFrame());
    }

    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null; // wait 1 frame
        layoutGroup.gameObject.SetActive(false);
        layoutGroup.gameObject.SetActive(true);
    }

    public Vector3 GetRandomPositionInsideScreen()
    {
        // Tạo ngẫu nhiên các tọa độ x và y bên trong kích thước màn hình
        float randomX = Random.Range(0, Screen.width);
        float randomY = Random.Range(0, Screen.height);

        // Chuyển đổi tọa độ màn hình sang tọa độ thế giới
        Vector3 screenPosition = new Vector3(randomX, randomY, Camera.main.nearClipPlane);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }
}
