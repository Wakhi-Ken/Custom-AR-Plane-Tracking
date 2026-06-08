using UnityEngine;
using UnityEngine.UI;

public class WelcomeManager : MonoBehaviour
{
    public GameObject welcomePanel;
    public GameObject instructionsPanel;

    [Header("UI Panel Images")]
    public Image welcomePanelImage;
    public Image instructionsPanelImage;

    [Header("Transparency Settings")]
    public float transparentAlpha = 0.4f;
    public float solidAlpha = 1f;

    private bool isTransparent = false;

    void Start()
    {
        welcomePanel.SetActive(true);
        instructionsPanel.SetActive(false);
    }

    public void StartApp()
    {
        welcomePanel.SetActive(false);
        instructionsPanel.SetActive(false);
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        welcomePanel.SetActive(false);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        welcomePanel.SetActive(true);
    }

    // FUNCTIONAL UI ELEMENT (TOGGLE)
    public void ToggleTransparency()
    {
        isTransparent = !isTransparent;

        ApplyAlpha(welcomePanelImage);
        ApplyAlpha(instructionsPanelImage);
    }

    void ApplyAlpha(Image img)
    {
        if (img == null) return;

        Color c = img.color;
        c.a = isTransparent ? transparentAlpha : solidAlpha;
        img.color = c;
    }

    
}