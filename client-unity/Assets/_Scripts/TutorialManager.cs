using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject tutorialViewRoot; // ตัวแผ่น TutorialView ทั้งก้อน
    [SerializeField] private Image cardImage;             // ตัว TutorialCard
    [SerializeField] private GameObject nextButton;       // ปุ่มลูกศรล่องหน Btn_Next
    [SerializeField] private GameObject previousButton;   // ปุ่มย้อนกลับ

    [Header("Tutorial Sprites")]
    [SerializeField] private Sprite[] tutorialPages;      // ลากรูป Tu1-Tu5 ใส่ตรงนี้

    private int currentIndex = 0;

    // เรียกตอนกดปุ่ม "?" ด้านบนขวาของจอหลัก
    public void OpenTutorial()
    {
        currentIndex = 0;
        UpdatePage();
        tutorialViewRoot.SetActive(true);
    }

    // เรียกตอนกดปุ่มลูกศร ->
    public void NextPage()
    {
        if (currentIndex < tutorialPages.Length - 1)
        {
            currentIndex++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePage();
        }
    }

    // อัปเดตรูปภาพ และซ่อนปุ่มลูกศรเมื่อถึงหน้าสุดท้าย
    private void UpdatePage()
    {
        if (tutorialPages.Length > 0 && cardImage != null)
        {
            cardImage.sprite = tutorialPages[currentIndex];
        }

        // ถ้าถึงหน้าสุดท้าย ให้ปิดปุ่มลูกศรล่องหนทิ้งไป
        if (nextButton != null)
        {
            nextButton.SetActive(currentIndex < tutorialPages.Length - 1);
        }

        if (previousButton != null)
        {
            previousButton.SetActive(currentIndex > 0);
        }

        previousButton.SetActive(currentIndex > 0 && currentIndex != 2);
    }

    // เรียกตอนกดปุ่ม "X" หรือกดพื้นที่สีเทารอบนอก
    public void CloseTutorial()
    {
        tutorialViewRoot.SetActive(false);
    }
}