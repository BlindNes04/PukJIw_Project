using UnityEngine;
using UnityEngine.UI;

public class MoreMenuToggle : MonoBehaviour
{
    [Header("More Menu Panel")]
    public GameObject moreMenuPanel;

    // ฟังก์ชันสำหรับกดสลับ เปิด / ปิด
    public void ToggleMoreMenu()
    {
        if (moreMenuPanel != null)
        {
            // สลับสถานะ: ถ้าเปิดอยู่จะปิด ถ้าปิดอยู่จะเปิด
            moreMenuPanel.SetActive(!moreMenuPanel.activeSelf);
        }
    }
}
