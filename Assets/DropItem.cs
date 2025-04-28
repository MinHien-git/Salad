using UnityEngine;
using UnityEngine.UI;

public class DropItem : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    public void SetSprite(Sprite sprite)
    {
        if (itemImage == null)
        {
            itemImage = GetComponent<Image>();
        }
        itemImage.sprite = sprite;
        itemImage.preserveAspect = true;
    }
}
