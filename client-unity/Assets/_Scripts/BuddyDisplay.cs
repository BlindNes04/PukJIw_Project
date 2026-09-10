using UnityEngine;
using UnityEngine.UI;

public class BuddyDisplay : MonoBehaviour
{
    private void Start()
    {
        UpdateBuddyAppearance();
    }

    public void UpdateBuddyAppearance()
    {
        // 1. ดึงชื่อ MBTI ที่เซฟไว้จากหน้า Quiz (ถ้าไม่มี ให้ใช้ entp เป็นค่าเริ่มต้น)
        string mbti = PlayerPrefs.GetString("PlayerMBTI", "entp").Trim().ToLower();

        // 2. โหลดรูปภาพจาก Assets/Resources/buddy/ ตามชื่อ MBTI
        Sprite newBuddySprite = Resources.Load<Sprite>("buddy/" + mbti);

        // 3. เอามาใส่ในช่อง Image ของตัวมันเอง
        if (newBuddySprite != null)
        {
            Image image = GetComponent<Image>();
            if (image != null)
            {
                image.sprite = newBuddySprite;
            }
        }
        else
        {
            Debug.LogError("หารูปบัดดี้ไม่เจอใน Assets/Resources/buddy/" + mbti);
        }
    }
}