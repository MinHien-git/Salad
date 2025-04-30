using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePane : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI explaining;
    public ParticleSystem[] randomParticles;
    public ParticleSystem[] sadParticles;
    public ParticleSystem[] regretParticles;
    public ParticleSystem particle;

    private void OnEnable()
    {
        scoreText.text =
            GameManager.Instance.CheckScorev2()
            + "/"
            + GameManager.Instance.currentSalad.ingredient.Length;

        if (
            GameManager.Instance.currentSalad.ingredient.Length
            == GameManager.Instance.CheckScorev2()
        )
        {
            particle.Play();
            for (int i = 0; i < randomParticles.Length; ++i)
            {
                randomParticles[i].transform.position = GetRandomPositionInsideScreen();
                randomParticles[i].Play();
            }
            explaining.text =
                "Chúc mừng, bạn đã hoàn thành phần chơi của mình với số điểm tuyệt đối";
        }
        else if (
            (int)GameManager.Instance.currentSalad.ingredient.Length / 2f
            < GameManager.Instance.CheckScorev2()
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
