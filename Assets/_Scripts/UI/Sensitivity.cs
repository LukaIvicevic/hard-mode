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
        input.text = SettingsManager.Instance.Sensitivity.ToString();
        slider.value = SettingsManager.Instance.Sensitivity;

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

        value = Mathf.Floor(value * 100) / 100;

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

        SettingsManager.Instance.ChangeSensitivity(value);
    }

    private void OnSliderValueChanged()
    {
        var value = slider.value;

        value = Mathf.Floor(value * 100) / 100;

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

        SettingsManager.Instance.ChangeSensitivity(value);
    }
}
