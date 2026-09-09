using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene(string sceneName)
    {
        // 1. บันทึกข้อมูลลงเครื่องว่าผู้เล่นผ่าน MBTI แล้ว (ชื่อคีย์ต้องตรงกับ LoadingManager)
        PlayerPrefs.SetInt("HasCompletedMBTI", 1);
        PlayerPrefs.Save();

        // 2. โหลดไปหน้าถัดไปตามปกติ
        SceneManager.LoadScene(sceneName);
    }
}