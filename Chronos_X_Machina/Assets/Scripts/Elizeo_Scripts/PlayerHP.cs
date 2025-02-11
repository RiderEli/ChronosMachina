using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public Slider hpSlider;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
