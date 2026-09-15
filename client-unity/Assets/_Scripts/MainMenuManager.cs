using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels (Canvas Groups)")]
    [SerializeField] private CanvasGroup mainMenuCG;
    [SerializeField] private CanvasGroup loginCG;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 0.3f;

    private Coroutine activeTransition;

    private void Start()
    {
        // เริ่มเกม: หน้า Main ชัดสุด / หน้า Login ปิดไว้
        SetCanvasGroupState(mainMenuCG, true);
        
        loginCG.alpha = 0f;
        loginCG.gameObject.SetActive(false);
    }

    // ผูกกับปุ่ม "เข้าสู่ระบบ" (จากหน้า Main ไป Login)
    public void OnClickLoginFromMain()
    {
        if (activeTransition != null) StopCoroutine(activeTransition);
        activeTransition = StartCoroutine(TransitionToLoginRoutine());
    }

    // ผูกกับปุ่ม "ย้อนกลับ" (จากหน้า Login กลับมา Main)
    public void OnClickBackToMain()
    {
        if (activeTransition != null) StopCoroutine(activeTransition);
        activeTransition = StartCoroutine(TransitionToMainRoutine());
    }

    private IEnumerator TransitionToLoginRoutine()
    {
        // 1. ปิดคลิกหน้า Main แล้ว Fade Out ให้หายไป
        mainMenuCG.interactable = false;
        mainMenuCG.blocksRaycasts = false;

        yield return StartCoroutine(FadeRoutine(mainMenuCG, 1f, 0f));
        mainMenuCG.gameObject.SetActive(false);

        // 2. จอว่างเสี้ยววินาที
        yield return new WaitForSeconds(0.08f);

        // 3. เปิดหน้า Login (รีเซ็ต alpha = 1 เพื่อให้อนิเมชันเด้งเล่นได้ชัดเจน)
        loginCG.gameObject.SetActive(true);
        loginCG.alpha = 1f;
        loginCG.interactable = true;
        loginCG.blocksRaycasts = true;
    }

    private IEnumerator TransitionToMainRoutine()
    {
        // 1. ปิดคลิกหน้า Login แล้วค่อยๆ Fade Out ออกไป
        loginCG.interactable = false;
        loginCG.blocksRaycasts = false;

        yield return StartCoroutine(FadeRoutine(loginCG, 1f, 0f));
        loginCG.gameObject.SetActive(false);

        // 2. จอว่างเสี้ยววินาที
        yield return new WaitForSeconds(0.08f);

        // 3. เปิดหน้า Main แล้วค่อยๆ Fade In กลับขึ้นมา
        mainMenuCG.gameObject.SetActive(true);
        mainMenuCG.alpha = 0f;

        yield return StartCoroutine(FadeRoutine(mainMenuCG, 0f, 1f));
        SetCanvasGroupState(mainMenuCG, true);
    }

    // ฟังก์ชันช่วยเกลี่ย Fade Alpha นุ่มๆ
    private IEnumerator FadeRoutine(CanvasGroup cg, float startAlpha, float targetAlpha)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }
        cg.alpha = targetAlpha;
    }

    private void SetCanvasGroupState(CanvasGroup cg, bool isActive)
    {
        cg.alpha = isActive ? 1f : 0f;
        cg.interactable = isActive;
        cg.blocksRaycasts = isActive;
    }
}