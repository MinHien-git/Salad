using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideButton : MonoBehaviour
{
    public GuideUI node;

    public void OnClick()
    {
        node.Open("Đóng");
    }
}
