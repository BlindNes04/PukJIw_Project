using System.Collections;
using UnityEngine;

public class FertilizerMixer : MonoBehaviour
{
    [Header("Popup Settings")]
    public GameObject warningPopup;
    public Animator popupAnimator;        // ลาก WarningPopup มาใส่ช่องนี้
    public float showDuration = 2.1f;

    private Coroutine hideCoroutine;

    public void OnClickMix()
    {
        ShowWarning();
    }

    public void ShowWarning()
    {
        if (warningPopup == null) return;

        // 1. เปิด Object ขึ้นมา
        warningPopup.SetActive(true);

        // 2. สั่งดีด Animator กลับไปเล่นที่วินาที 0 ทันที แม้ของเก่ายังเล่นไม่จบ
        if (popupAnimator != null)
        {
            popupAnimator.Rebind();
            popupAnimator.Update(0f);
        }

        // 3. รีเซ็ตเวลานับถอยหลังใหม่ทุกครั้งที่กดซ้ำ
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HidePopupRoutine());
    }

    private IEnumerator HidePopupRoutine()
    {
        yield return new WaitForSeconds(showDuration);
        warningPopup.SetActive(false);
    }
}