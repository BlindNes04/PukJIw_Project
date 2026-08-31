using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public void SetItem(Sprite icon, Sprite frame, int amount = 1)
    {
        if (iconImage != null)
        {
            if (icon != null)
            {
                iconImage.sprite = icon;
                iconImage.gameObject.SetActive(true);
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }
        }

        if (frameImage != null && frame != null)
        {
            frameImage.sprite = frame;
        }

        if (amountText != null)
        {
            amountText.text = amount > 1 ? $"x{amount}" : "";
        }
    }
}
