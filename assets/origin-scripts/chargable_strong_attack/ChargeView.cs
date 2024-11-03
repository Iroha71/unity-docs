using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeView : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image circleImage;
    [SerializeField]
    private Color filledColor;
    [SerializeField]
    private List<Image> chargeLevels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateCircle(float currentView)
    {
        if (currentView <= 0f)
        {
            canvasGroup.alpha = 0f;
            return;
        }
        else if (canvasGroup.alpha <= 0f)
        {
            canvasGroup.alpha = 1f;
        }

        if (currentView / 100f >= 1f && currentView % 100 == 0f)
        {
            circleImage.fillAmount = 1f;
            circleImage.color = filledColor;
        }
        else
        {
            float fillRate = (currentView % 100f) / 100f;
            circleImage.fillAmount = fillRate;
            circleImage.color = Color.white;
        }
    }

    public void UpdateChargeLevel(int level)
    {
        if (level <= 0)
        {
            foreach (Image image in chargeLevels)
            {
                image.enabled = false;
            }
            return;
        }

        chargeLevels[level - 1].enabled = true;
    }
}
