using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public void OnClick()
    {
        CompletePopup.Instance.Open();
    }
}
