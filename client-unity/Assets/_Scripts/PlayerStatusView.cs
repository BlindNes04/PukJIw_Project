using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusView : MonoBehaviour
{
    [Header("Level UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image levelExpBar;

    [Header("Points UI")]
    [SerializeField] private TextMeshProUGUI flowerPointsText;
    [SerializeField] private Slider flowerProgressBar;
    [SerializeField] private TextMeshProUGUI relationshipPointsText;
    [SerializeField] private Slider relationshipProgressBar;
    
    public void UpdateLevel(int currentLevel, float progressPercent)
    {
        if (levelText != null) levelText.text = currentLevel.ToString();
        if (levelExpBar != null) levelExpBar.fillAmount = progressPercent;
    }

    public void UpdateFlowerPoints(int amount)
    {
        if (flowerPointsText != null) flowerPointsText.text = amount.ToString();
    }

    public void UpdateRelationship(float value)
    {
        if (relationshipProgressBar != null) relationshipProgressBar.value = value;
    }
}