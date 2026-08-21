using UnityEngine;
using UnityEngine.UI;

public class PlantStatusView : MonoBehaviour
{
    [Header("Plant Status Sliders")]
    [SerializeField] private Slider needProgressSlider;
    [SerializeField] private Slider plantExpSlider;
    
    private void UpdateNeedProgress(float normalizedValue)
    {
        if (needProgressSlider != null) needProgressSlider.value = normalizedValue;
    }

    public void UpdatePlantExp(float currentExp, float maxExp)
    {
        if (plantExpSlider != null && maxExp > 0)
        {
            plantExpSlider.value = currentExp / maxExp;
        }
    }
}
