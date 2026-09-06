using UnityEngine;
using UnityEngine.SceneManagement;

public class MBTITestController : MonoBehaviour
{
    // ฟังก์ชันสำหรับผูกกับปุ่มกด
    public void CompleteTestAndGoToMain()
    {
        // 1. บันทึกค่าว่าทำแบบทดสอบเสร็จแล้ว
        PlayerPrefs.SetInt("HasCompletedMBTI", 1);
        PlayerPrefs.Save();

        // 2. โหลดข้ามไปหน้าเกมหลัก (ใส่ชื่อ Scene หลักของคุณ เช่น "SampleScene")
        SceneManager.LoadScene("MainForest");
    }
}