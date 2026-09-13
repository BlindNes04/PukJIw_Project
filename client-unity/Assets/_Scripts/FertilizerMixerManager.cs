using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FertilizerMixerManager : MonoBehaviour
{
    [System.Serializable]
    public struct ItemData
    {
        public string itemName;
        public string description;
        public string location;
        public Sprite frameSprite;
        public Sprite itemSprite;
    }

    [Header("Storage & Detail UI")]
    public GameObject storageView;
    public GameObject itemDetailBox;
    public TMP_Text nameText;
    public TMP_Text descText;
    public TMP_Text locationText;

    [Header("Item Database")]
    public ItemData[] items;

    [Header("Item Selection Highlights")]
    public GameObject[] itemSelectHighlights;

    [Header("Bottom Slots (3 ช่องล่าง)")]
    public Image[] slotFrames;
    public Image[] slotIcons;
    public Sprite defaultSlotSprite; // ลากรูปสี่เหลี่ยมมนสีครีม (Rectangle 6) มาใส่

    [Header("Warning Toast")]
    public GameObject warningPopup;
    public Animator popupAnimator;
    public float showDuration = 2.1f;

    [Header("Boiling Minigame Transition")]
    public GameObject boilingMinigameView;
    public GameObject[] objectsToHide;

    private int currentSelectedItem = -1;
    private bool[] isSlotFilled = new bool[3];
    private int[] slotItemIndex = new int[3] { -1, -1, -1 }; // จำว่าช่องไหนใส่ไอเทมเบอร์อะไรไว้
    private bool[] isItemUsed;
    private Coroutine hideCoroutine;

    private void Awake()
    {
        isItemUsed = new bool[items.Length];
        
        // ถ้าไม่ได้ใส่ defaultSlotSprite ใน Inspector ให้จำรูปปัจจุบันของช่องแรกไว้
        if (defaultSlotSprite == null && slotFrames.Length > 0 && slotFrames[0] != null)
        {
            defaultSlotSprite = slotFrames[0].sprite;
        }
    }

    public void OpenStorage()
    {
        currentSelectedItem = -1;
        ClearAllHighlights();
        if (itemDetailBox != null) itemDetailBox.SetActive(false);
        storageView.SetActive(true);
    }

    public void CloseStorage()
    {
        ClearAllHighlights();
        storageView.SetActive(false);
    }

    // ฟังก์ชันใหม่: ผูกกับช่องสี่เหลี่ยมด้านล่างทั้ง 3 ช่อง
    public void OnClickBottomSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= isSlotFilled.Length) return;

        // กรณีที่ 1: ช่องนี้มีไอเทมอยู่แล้ว -> สั่งลบออก
        if (isSlotFilled[slotIndex])
        {
            int usedItem = slotItemIndex[slotIndex];
            if (usedItem >= 0 && usedItem < isItemUsed.Length)
            {
                isItemUsed[usedItem] = false; // คืนสิทธิ์ให้กลับไปเลือกชิ้นนี้ในคลังได้ใหม่
            }

            // ซ่อนรูปไอคอน และเปลี่ยนกรอบกลับเป็นช่องสีครีม
            slotIcons[slotIndex].gameObject.SetActive(false);
            if (defaultSlotSprite != null)
            {
                slotFrames[slotIndex].sprite = defaultSlotSprite;
            }

            // ล้างสถานะช่องว่าง
            isSlotFilled[slotIndex] = false;
            slotItemIndex[slotIndex] = -1;
        }
        // กรณีที่ 2: ช่องว่างอยู่ -> สั่งเปิดคลังเก็บของ
        else
        {
            OpenStorage();
        }
    }

    public void OnClickItemInGrid(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= items.Length) return;
        if (isItemUsed[itemIndex]) return;

        currentSelectedItem = itemIndex;
        nameText.text = items[itemIndex].itemName;
        descText.text = items[itemIndex].description;
        locationText.text = items[itemIndex].location;
        itemDetailBox.SetActive(true);

        UpdateSelectionHighlight(itemIndex);
    }

    public void OnConfirmSelect()
    {
        int targetSlot = -1;
        for (int i = 0; i < isSlotFilled.Length; i++)
        {
            if (!isSlotFilled[i])
            {
                targetSlot = i;
                break;
            }
        }

        if (targetSlot != -1 && currentSelectedItem != -1)
        {
            slotFrames[targetSlot].sprite = items[currentSelectedItem].frameSprite;
            slotIcons[targetSlot].sprite = items[currentSelectedItem].itemSprite;
            slotIcons[targetSlot].gameObject.SetActive(true);

            isSlotFilled[targetSlot] = true;
            slotItemIndex[targetSlot] = currentSelectedItem; // จำว่าช่องนี้ใส่ไอเทมชิ้นไหน
            isItemUsed[currentSelectedItem] = true;
        }

        ClearAllHighlights();
        itemDetailBox.SetActive(false);
        CloseStorage();
    }

    private void UpdateSelectionHighlight(int selectedIndex)
    {
        if (itemSelectHighlights == null) return;
        for (int i = 0; i < itemSelectHighlights.Length; i++)
        {
            if (itemSelectHighlights[i] != null)
                itemSelectHighlights[i].SetActive(i == selectedIndex);
        }
    }

    private void ClearAllHighlights()
    {
        if (itemSelectHighlights == null) return;
        for (int i = 0; i < itemSelectHighlights.Length; i++)
        {
            if (itemSelectHighlights[i] != null)
                itemSelectHighlights[i].SetActive(false);
        }
    }

    public void OnClickMix()
    {
        bool isComplete = isSlotFilled[0] && isSlotFilled[1] && isSlotFilled[2];

        if (!isComplete)
        {
            ShowWarning();
        }
        else
        {
            // 1. สั่งซ่อน UI ทุกอย่างที่เลือกไว้ทันที
            if (objectsToHide != null)
            {
                foreach (GameObject obj in objectsToHide)
                {
                    if (obj != null) obj.SetActive(false);
                }
            }

            // 2. เปิดหน้าต่างมินิเกมต้มปุ๋ย
            if (boilingMinigameView != null)
            {
                boilingMinigameView.SetActive(true);
            }
        }
    }

    public void ShowWarning()
    {
        if (warningPopup == null) return;
        warningPopup.SetActive(true);
        if (popupAnimator != null)
        {
            popupAnimator.Rebind();
            popupAnimator.Update(0f);
        }
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HidePopupRoutine());
    }

    private IEnumerator HidePopupRoutine()
    {
        yield return new WaitForSeconds(showDuration);
        warningPopup.SetActive(false);
    }
}