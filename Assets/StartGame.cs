using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    public void OnClick()
    {
        SceneManager.LoadScene("MainScene");
    }
}
