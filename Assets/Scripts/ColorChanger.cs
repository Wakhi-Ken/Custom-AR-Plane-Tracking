using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private GameObject currentObject;

    public Slider colorSlider;

    private const string COLOR_KEY = "SavedColorValue";

    public void SetObject(GameObject obj)
    {
        currentObject = obj;
    }

    void Start()
    {
        // Load saved value
        float savedValue = PlayerPrefs.GetFloat(COLOR_KEY, 0f);

        if (colorSlider != null)
        {
            colorSlider.value = savedValue;
            colorSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        // Apply saved color if object exists
        ApplyColor(savedValue);
    }

    public void OnSliderChanged(float value)
    {
        // Save value every change
        PlayerPrefs.SetFloat(COLOR_KEY, value);
        PlayerPrefs.Save();

        ApplyColor(value);
    }

    void ApplyColor(float value)
    {
        if (currentObject == null) return;

        Renderer rend = currentObject.GetComponent<Renderer>();

        if (rend != null)
        {
            Color newColor = Color.HSVToRGB(value, 1f, 1f);

            rend.material = new Material(rend.material);
            rend.material.color = newColor;
        }
    }
}