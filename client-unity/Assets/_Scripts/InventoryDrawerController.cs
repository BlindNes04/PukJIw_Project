using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDrawerController : MonoBehaviour
{
    [Header("Panel Reference")]
    [SerializeField] private RectTransform drawerRect;
    [SerializeField] private Button closeBtn;

    [Header("Animation Settings")]
    [SerializeField] private float targetOpenY = 0f;
    [SerializeField] private float targetClosedY = -700f;
    [SerializeField] private float slideSpeed = 12f;

    [Header("Debug Status")]
    [SerializeField] private bool isOpen = false;

    private Coroutine slideCoroutine;

    private void Awake()
    {
        if (drawerRect == null) drawerRect = GetComponent<RectTransform>();

        if (closeBtn != null)
        {
            closeBtn.onClick.AddListener(CloseDrawer);
            closeBtn.gameObject.SetActive(false);
        }

        drawerRect.anchoredPosition = new Vector2(drawerRect.anchoredPosition.x, targetClosedY);
        isOpen = false;
    }

    public void ToggleDrawer()
    {
        Debug.Log($"[Drawer] Toggle Called. Current isOpen = {isOpen}");
        if (isOpen)
            CloseDrawer();
        else
            OpenDrawer();
    }

    public void OpenDrawer()
    {
        Debug.Log("[Drawer] ---> Opening Drawer");
        isOpen = true;
        if (closeBtn != null) closeBtn.gameObject.SetActive(true);
        MoveTo(targetOpenY);
    }

    public void CloseDrawer()
    {
        Debug.Log("[Drawer] ---> Closing Drawer");
        isOpen = false;
        if (closeBtn != null) closeBtn.gameObject.SetActive(false);
        MoveTo(targetClosedY);
    }

    private void MoveTo(float targetY)
    {
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlideRoutine(targetY));
    }

    private IEnumerator SlideRoutine(float targetY)
    {
        Vector2 startPos = drawerRect.anchoredPosition;
        Vector2 targetPos = new Vector2(startPos.x, targetY);
        float t = 0f;

        Debug.Log($"[Drawer] Moving from Y:{startPos.y} to Y:{targetY}");

        while (t < 1f)
        {
            t += Time.deltaTime * slideSpeed;

            drawerRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        drawerRect.anchoredPosition = targetPos;
    }
}
