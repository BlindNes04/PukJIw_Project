using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Slider loadingSlider;

    [Header("Target Scene Names")]
    public string mbtiSceneName = "Quiz";
    public string mainGameSceneName = "MainForest";

    void Start()
    {
        StartCoroutine(LoadTargetSceneAsync());
    }

    IEnumerator LoadTargetSceneAsync()
    {
        // 1. ตรวจสอบว่าเคยทำแบบทดสอบ MBTI หรือยัง (0 = ยังไม่เคย, 1 = ทำแล้ว)
        bool hasCompletedMBTI = PlayerPrefs.GetInt("HasCompletedMBTI", 0) == 1;
        string sceneToLoad = hasCompletedMBTI ? mainGameSceneName : mbtiSceneName;

        // 2. เริ่มโหลด Scene ข้อมูลและ Asset เข้า RAM จริงๆ เบื้องหลัง
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        // ปิดไม่ให้มันตัดข้ามซีนทันทีจนกว่าหลอดจะเต็ม
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // ค่า progress ของ Unity async จะวิ่งจาก 0.0 ถึง 0.9 เมื่อโหลดข้อมูลเสร็จ
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (loadingSlider != null)
            {
                loadingSlider.value = progress;
            }

            // เมื่อโหลดข้อมูลเสร็จสมบูรณ์ (progress >= 0.9f)
            if (operation.progress >= 0.9f)
            {
                // หน่วงเวลาสั้นๆ 0.5 วินาที เพื่อให้ผู้เล่นเห็นโลโก้และหลอดเต็ม 100% ชัดเจน
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}