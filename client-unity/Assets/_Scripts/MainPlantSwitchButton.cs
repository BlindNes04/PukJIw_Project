using UnityEngine;
using UnityEngine.UI;

public class MainPlantSwitchButton : MonoBehaviour
{
    [Header("Button Reference")]
    [SerializeField] private Button switchPlantBtn;

    [Header("UI Popups")]
    [SerializeField] private GameObject plantSelectorPopup;

    private void Awake()
    {
        if (switchPlantBtn == null)
            switchPlantBtn = GetComponent<Button>();

        if (switchPlantBtn != null)
            switchPlantBtn.onClick.AddListener(OnSwitchPlantClicked);
        
    }

    public void OnSwitchPlantClicked()
    {
        Debug.Log("Open Plant Selector Popup");

        if (plantSelectorPopup != null)
        {
            plantSelectorPopup.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (switchPlantBtn != null)
            switchPlantBtn.onClick.RemoveListener(OnSwitchPlantClicked);
    }
}
