using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteLevel : MonoBehaviour
{
    public void OnClick()
    {
        BowlManager.Instance.StartMixingSequence();
    }
}
