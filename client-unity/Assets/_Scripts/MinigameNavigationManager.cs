using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameNavigationManager : MonoBehaviour
{
    [Header("Scene Names (ตั้งค่าหรือเปลี่ยนชื่อได้ใน Inspector)")]
    [SerializeField] private string mainSceneName = "MainForest";
    [SerializeField] private string mixFertilizerSceneName = "MixFertilizer";
    [SerializeField] private string mixFruitSceneName = "MixFruit";

    // 1. ฟังก์ชันกลับหน้าหลัก (MainForest)
    public void GoToMainForest()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    // 2. ฟังก์ชันไปมินิเกมผสมปุ๋ย (MixFertilizer)
    public void GoToMixFertilizer()
    {
        SceneManager.LoadScene(mixFertilizerSceneName);
    }

    // 3. ฟังก์ชันไปมินิเกมผสมผลไม้ (MixFruit)
    public void GoToMixFruit()
    {
        SceneManager.LoadScene(mixFruitSceneName);
    }

    // ฟังก์ชันเสริม: เผื่ออยากโหลดซีนอื่นโดยพิมพ์ชื่อผ่าน Inspector
    public void LoadSceneByName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
