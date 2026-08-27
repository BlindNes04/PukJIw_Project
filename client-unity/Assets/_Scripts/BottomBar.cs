using UnityEngine;
using UnityEngine.UI;

public class BottomNavigationBar : MonoBehaviour
{
    [Header("Navigation Button Slots")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button addFriendButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Button plantDexButton;
    [SerializeField] private Button plantStoreButton;

    private void Awake()
    {
        if (homeButton != null) homeButton.onClick.AddListener(OnHomeClicked);
        if (addFriendButton != null) addFriendButton.onClick.AddListener(OnAddFriendClicked);
        if (inventoryButton != null) inventoryButton.onClick.AddListener(OnInventoryClicked);
        if (plantDexButton != null) plantDexButton.onClick.AddListener(OnPlantDexClicked);
        if (plantStoreButton != null) plantStoreButton.onClick.AddListener(OnPlantStoreClicked);
    }

    public void OnHomeClicked()
    {
        Debug.Log("Home Button Clicked");
    }

    public void OnAddFriendClicked()
    {
        Debug.Log("Add Friend Button Clicked");
    }

    public void OnInventoryClicked()
    {
        Debug.Log("Inventory Button Clicked");
    }

    public void OnPlantDexClicked()
    {
        Debug.Log("Plant Dex Button Clicked");
    }

    public void OnPlantStoreClicked()
    {
        Debug.Log("Gacha Shop Button Clicked");
    }

    private void OnDestroy()
    {
        if (homeButton != null) homeButton.onClick.RemoveListener(OnHomeClicked);
        if (addFriendButton != null) addFriendButton.onClick.RemoveListener(OnAddFriendClicked);
        if (inventoryButton != null) inventoryButton.onClick.RemoveListener(OnInventoryClicked);
        if (plantDexButton != null) plantDexButton.onClick.RemoveListener(OnPlantDexClicked);
        if (plantStoreButton != null) plantStoreButton.onClick.RemoveListener(OnPlantStoreClicked);
    }
}