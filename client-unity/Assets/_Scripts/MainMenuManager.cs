using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loginPanel;

    [Header("UI Cards (Canvas Group)")]
    [SerializeField] private CanvasGroup loginMethodCard;
    [SerializeField] private CanvasGroup loginCard;
    [SerializeField] private CanvasGroup registerCard;

    [Header("Sequence Settings")]
    [Tooltip("ระยะเวลารอ Logo เล่นจบ (วินาที) ก่อนเปิดการ์ด")]
    [SerializeField] private float logoWaitTime = 0.8f; 
    [SerializeField] private float fadeDuration = 0.2f;

    private CanvasGroup currentActiveCard;
    private bool isSwitching = false;

    private void Start()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (loginPanel != null) loginPanel.SetActive(false);
    }

    // --- 1. ปุ่ม "เข้าสู่ระบบ" จากหน้าแรก (GrowGo) ---
    public void OnClickOpenLogin()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (loginPanel != null) loginPanel.SetActive(true);

        // ซ่อนการ์ดทุกใบไว้ก่อน ให้ผู้เล่นเห็นเฉพาะ Logo เล่นแอนิเมชัน
        SetupCard(loginMethodCard, false);
        SetupCard(loginCard, false);
        SetupCard(registerCard, false);

        // รอเวลาให้ Logo เล่นจบ แล้วค่อยปล่อยการ์ดแรกเด้งขึ้นมา
        StartCoroutine(ShowMethodCardAfterLogo());
    }

public void OnClickPlay()
{
    // เช็กชื่อคีย์ "HasCompletedMBTI" ให้ตรงกับที่เคยเซฟไว้
    bool hasCompletedMBTI = PlayerPrefs.GetInt("HasCompletedMBTI", 0) == 1;

    if (hasCompletedMBTI)
    {
        SceneManager.LoadScene("MainForest"); // เคยเล่นแล้ว -> เข้าฟาร์มหลัก
    }
    else
    {
        SceneManager.LoadScene("Quiz"); // เข้าครั้งแรก -> ไปทำ Quiz
    }
}

    private IEnumerator ShowMethodCardAfterLogo()
    {
        yield return new WaitForSeconds(logoWaitTime);

        // ครบเวลาแล้วค่อยเปิดการ์ด (แอนิเมชันเด้งของการ์ดจะเริ่มทำงานทันที ณ จังหวะนี้)
        SetupCard(loginMethodCard, true);
        currentActiveCard = loginMethodCard;
    }

    // --- 2. ฟังก์ชันสลับการ์ด ---
    public void OpenLogin() => SwitchToCard(loginCard);
    public void OpenRegister() => SwitchToCard(registerCard);
    public void BackToMethod() => SwitchToCard(loginMethodCard);

    // --- 3. ปุ่มย้อนกลับไปหน้าแรก (GrowGo) ---
    public void BackToMainMenu()
    {
        if (loginPanel != null) loginPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    private void SwitchToCard(CanvasGroup nextCard)
    {
        if (isSwitching || currentActiveCard == nextCard || nextCard == null) return;
        StartCoroutine(SwitchRoutine(currentActiveCard, nextCard));
    }

    private IEnumerator SwitchRoutine(CanvasGroup fromCard, CanvasGroup toCard)
    {
        isSwitching = true;

        if (fromCard != null)
        {
            fromCard.interactable = false;
            fromCard.blocksRaycasts = false;

            float timer = 0f;
            float startAlpha = fromCard.alpha;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fromCard.alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeDuration);
                yield return null;
            }

            fromCard.alpha = 0f;
            fromCard.gameObject.SetActive(false);
        }

        SetupCard(toCard, true);
        currentActiveCard = toCard;

        isSwitching = false;
    }

    private void SetupCard(CanvasGroup cg, bool show)
    {
        if (cg == null) return;

        cg.gameObject.SetActive(show);
        cg.alpha = show ? 1f : 0f;
        cg.interactable = show;
        cg.blocksRaycasts = show;
    }
}