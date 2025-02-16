using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Slider hpSlider;


    public void SetMaxHP(int hp)
    {
        hpSlider.maxValue = hp;
        hpSlider.value = hp;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
