using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardManager : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        OpenOnScreenKeyboard();
    }

    void OpenOnScreenKeyboard()
    {
        Process.Start("osk.exe");
    }
}
