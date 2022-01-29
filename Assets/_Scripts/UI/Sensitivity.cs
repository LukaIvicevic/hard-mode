using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sensitivity : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField input;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private float minSens = 0f;
    [SerializeField]
    private float maxSens = 10f;

    private void Start()
    {
        input.text = GameManager.Instance.GetSensitivity().ToString();
        slider.value = GameManager.Instance.GetSensitivity();

        input.onValueChanged.AddListener(delegate { OnInputValueChanged(); });
        slider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });

        slider.minValue = minSens;
        slider.maxValue = maxSens;
    }

    private void OnInputValueChanged()
    {
        if (!float.TryParse(input.text, out var value))
        {
            return;
        }

        if (value < minSens)
        {
            value = minSens;
        }

        if (value > maxSens)
        {
            value = maxSens;
        }

        input.text = value.ToString();
        slider.value = value;

        GameManager.Instance.SetSensitivity(value);
    }

    private void OnSliderValueChanged()
    {
        var value = slider.value;

        if (value < minSens)
        {
            value = minSens;
        }

        if (value > maxSens)
        {
            value = maxSens;
        }

        input.text = value.ToString();
        slider.value = value;

        GameManager.Instance.SetSensitivity(value);
    }
}
