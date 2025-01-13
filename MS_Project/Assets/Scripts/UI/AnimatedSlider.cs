using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedSlider : MonoBehaviour
{
    [SerializeField]
    Sprite[] spriteArray;

    [SerializeField]
    Image sliderImg;

    public Slider slider;
    float index;

    void Start()
    {
        sliderImg.GetComponent<Image>().sprite = spriteArray[0];
        slider = GetComponent<Slider>();
        slider.value = 0f;
    }

    void Update()
    {
        index += slider.value * Time.deltaTime * 10f;

        if (index >= spriteArray.Length - 1)
        {
            index = 0;
        }
        sliderImg.GetComponent<Image>().sprite = spriteArray[(int)index];
    }

    // スライダー値を更新するメソッド
    public void UpdateSlider(float progress)
    {
        if (slider != null)
        {
            slider.value = progress;
        }
    }
}
