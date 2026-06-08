using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    private GameObject currentObject;

    public Slider colorSlider;

    public void SetObject(GameObject obj)
    {
        currentObject = obj;
    }

    void Start()
    {
        if (colorSlider != null)
        {
            colorSlider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    public void OnSliderChanged(float value)
    {
        if (currentObject == null) return;

        Renderer rend = currentObject.GetComponent<Renderer>();

        if (rend != null)
        {
            // Convert slider value to HSV color
            Color newColor = Color.HSVToRGB(value, 1f, 1f);

            rend.material = new Material(rend.material);
            rend.material.color = newColor;
        }
    }
}