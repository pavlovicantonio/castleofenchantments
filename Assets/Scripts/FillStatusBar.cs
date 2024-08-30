using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;
    private Slider slider;


    void Awake()
    {
        slider = GetComponent<Slider>();

    }

    void Update()
    {
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if(slider.value > slider.minValue && !fillImage.enabled) 
        {
            fillImage.enabled = true;
        }

        float fillValue = playerHealth.currentHealth / playerHealth.maxHealth;

        if (fillValue <= slider.maxValue / 3)
        {
            fillImage.color = Color.white; //critical condition
        }
        else if(fillValue > slider.minValue / 3)
        {
            fillImage.color = Color.red;
        }

        slider.value = fillValue;
    }
}
