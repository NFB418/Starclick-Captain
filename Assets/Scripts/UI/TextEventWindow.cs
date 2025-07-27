using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextEventWindow : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    private bool isWindowOpen = false;

    void Update()
    {
        // Check if the ESC key is pressed
        if (isWindowOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow();
        }
    }

    public void SetText(string content, string header = "")
    {
        headerField.text = header.ToUpper();
        contentField.text = content;
    }

    public void OpenWindow()
    {
        Controller.instance.inGameUI.interactable = false;
        Controller.instance.GamePaused = true;
        this.gameObject.SetActive(true);
        isWindowOpen = true;
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        Controller.instance.inGameUI.interactable = true;
        Controller.instance.GamePaused = false;
        isWindowOpen = false;
    }

}
