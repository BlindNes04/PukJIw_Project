using UnityEngine;
using UnityEngine.UI;

public class TopRightMenuView : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button eventButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button biomeButton;
    
    private void Awake()
    {
        if (eventButton != null) eventButton.onClick.AddListener(OnEventClicked);
        if (settingButton != null) settingButton.onClick.AddListener(OnSettingClicked);
        if (biomeButton != null) biomeButton.onClick.AddListener(onBiomeClicked);
    }

    public void OnEventClicked()
    {

    }

    public void OnSettingClicked()
    {

    }

    public void onBiomeClicked()
    {

    }

    private void OnDestroy()
    {
        if (eventButton != null) eventButton.onClick.RemoveListenr(OnEventClicked);
        if (settingButton != null) settingButton.onClick.RemoveListenr(OnSettingClicked);
        if (biomeButton != null) biomeButton.onClick.RemoveListenr(onBiomeClicked);
    }
}
