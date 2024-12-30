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

    Slider slider;
    float index;

    // Start is called before the first frame update
    void Start()
    {
        sliderImg.GetComponent<Image>().sprite = spriteArray[0];
        slider = GetComponent<Slider>();
        slider.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        index += slider.value * Time.deltaTime * 10f;

        if (index >= spriteArray.Length - 1)
        {
            index = 0;
        }
        sliderImg.GetComponent<Image>().sprite = spriteArray[(int)index];
    }
}
