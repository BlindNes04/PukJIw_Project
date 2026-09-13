using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // เพิ่มบรรทัดนี้

public class BoilingMinigame : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform indicator;
    [SerializeField] private GameObject resultRewardView; // ลาก RewardView มาใส่

    [Header("Bar Settings")]
    [SerializeField] private float moveRange = 250f;
    [SerializeField] private float speed = 500f;

    private bool isPlaying = false;

    private void OnEnable()
    {
        isPlaying = true;
    }

    private void Update()
    {
        if (!isPlaying || indicator == null) return;

        float x = Mathf.PingPong(Time.time * speed, moveRange * 2) - moveRange;
        indicator.anchoredPosition = new Vector2(x, indicator.anchoredPosition.y);
    }

    // ผูกกับปุ่ม "หยุด"
    public void OnClickStop()
    {
        if (!isPlaying) return;

        isPlaying = false;
        StartCoroutine(TransitionToRewardRoutine());
    }

    private IEnumerator TransitionToRewardRoutine()
    {
        yield return new WaitForSeconds(0.4f); // เข็มหยุดนิ่งชั่วครู่
        gameObject.SetActive(false);            // ซ่อนหน้ามินิเกม

        if (resultRewardView != null)
        {
            resultRewardView.SetActive(true);   // เปิดหน้ารับปุ๋ย
        }
    }

    // ฟังก์ชันผูกกับปุ่ม "กลับสู่หน้าหลัก"
    public void OnClickBackToHome()
    {
        // โหลดฉากปัจจุบันใหม่ทั้งหมดเพื่อรีเซ็ตของทุกอย่างพร้อมเริ่มรอบใหม่
        SceneManager.LoadScene("MainForest");
    }
}