using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusView : MonoBehaviour
{
    [Header("Level UI")]
    [SerializeField] private TextMeshProGUI levelText;
    [SerializeField] private image levelExpBar;

    [Header("Points UI")]
    [SerializeField] private TextMeshProGUI flowerPointsText;
    [SerializeField] private Slider flowerProgressBar;
    [SerializeField] private TextMeshProGUI relationshipPointsText;
    [SerializeField] private Slider relationshipProgressBar;
    
    public void UpdateLevel(int currentLevel, float progressPercent)
    {

    }

    public void UpdateFlowerPoints(int amount)
    {

    }

    public void UpdateRelationship(float value)
    {
        
    }
}
