using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterSlider : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public Animator anim;
    public bool lowEqualsBad;

    public void SetColor(Color color)
    {
        fillImage.color = color;
    }

    public void SetValue(int value)
    {
        slider.value = value;
        if (lowEqualsBad)
        {
            if (value >= 16)
                anim.SetFloat("Jitter", 0f);
            else
                anim.SetFloat("Jitter", (16 - value) / 16f);
        }
        else
        {
            if (value <= 16)
                anim.SetFloat("Jitter", 0f);
            else
                anim.SetFloat("Jitter", (value - 16) / 16f);
        }
    }
}
