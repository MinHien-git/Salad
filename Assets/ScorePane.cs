using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePane : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI explaining;
    public ParticleSystem[] randomParticles;
    public ParticleSystem particle;

    private void OnEnable()
    {
        explaining.text = GameManager.Instance.currentSalad.ingredient_information;
        scoreText.text =
            GameManager.Instance.CheckScore()
            + "/"
            + GameManager.Instance.currentSalad.ingredient.Length;

        if (
            GameManager.Instance.currentSalad.ingredient.Length == GameManager.Instance.CheckScore()
        )
        {
            particle.Play();
            for (int i = 0; i < randomParticles.Length; ++i)
            {
                randomParticles[i].transform.position = GetRandomPositionInsideScreen();
                randomParticles[i].Play();
            }
        }
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
