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

    private const string TRANSPARENCY_KEY = "UI_Transparency";

    void Start()
    {
        welcomePanel.SetActive(true);
        instructionsPanel.SetActive(false);

        // LOAD saved setting
        isTransparent = PlayerPrefs.GetInt(TRANSPARENCY_KEY, 0) == 1;

        ApplyAlpha(welcomePanelImage);
        ApplyAlpha(instructionsPanelImage);
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

    // FUNCTIONAL UI ELEMENT (TOGGLE + SAVE)
    public void ToggleTransparency()
    {
        isTransparent = !isTransparent;

        // SAVE setting
        PlayerPrefs.SetInt(TRANSPARENCY_KEY, isTransparent ? 1 : 0);
        PlayerPrefs.Save();

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