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
        Debug.Log("Event Button Clicked");
    }

    public void OnSettingClicked()
    {
        Debug.Log("Setting Button Clicked");
    }

    public void onBiomeClicked()
    {
        Debug.Log("Biome Button Clicked");
    }

    private void OnDestroy()
    {
        if (eventButton != null) eventButton.onClick.RemoveListener(OnEventClicked);
        if (settingButton != null) settingButton.onClick.RemoveListener(OnSettingClicked);
        if (biomeButton != null) biomeButton.onClick.RemoveListener(onBiomeClicked);
    }
}
