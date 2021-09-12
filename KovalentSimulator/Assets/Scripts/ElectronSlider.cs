using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElectronSlider : MonoBehaviour
{

    public Slider slider;
    public TMP_Text textMin;
    public TMP_Text textMax;
    public TMP_Text textDefault;
    public TMP_Text textCurrent;

    public Manager manager;

    void Start()
    {
        slider.wholeNumbers = true;
    }

    public void onSliderValueChange()
    {

        manager.onElectronSliderChange((int)slider.value);
        manager.updateSelectedText();

    }

    public void setSlider(int min, int max, int current, int def)
    {
        slider.minValue = min;
        slider.maxValue = max;

        slider.value = current;

        setCurrentText(current.ToString());
        setDefaultText(def.ToString());
        setMinText(min.ToString());
        setMaxText(max.ToString());

    }

    private void setCurrentText(string text)
    {
        textCurrent.text = text;
    }

    private void setDefaultText(string text)
    {
        textDefault.text = text;
    }

    private void setMinText(string text)
    {
        textMin.text = text;
    }

    private void setMaxText(string text)
    {
        textMax.text = text;
    }
}
