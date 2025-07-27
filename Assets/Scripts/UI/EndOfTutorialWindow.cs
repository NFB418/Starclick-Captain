using UnityEngine;

public class EndOfTutorialWindow : MonoBehaviour
{
    public AudioSource audioSource;  // Assign an AudioSource in the Inspector
    public AudioClip victorySoundEffect;   // Assign your sound effect in the Inspector

    private bool isWindowOpen = false;

    void Update()
    {
        // Check if the ESC key is pressed
        if (isWindowOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow();
        }
    }

    public void OpenWindow()
    {
        Controller.instance.inGameUI.interactable = false;
        this.gameObject.SetActive(true);
        isWindowOpen = true;
        audioSource.PlayOneShot(victorySoundEffect); // Plays the sound once
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        Controller.instance.inGameUI.interactable = true;
        isWindowOpen = false;
    }

    public void ContinueButtonClick()
    {
        CloseWindow();
    }
    public void RestartButtonClick()
    {
        CloseWindow();
        Controller.instance.Restart();
    }
}
