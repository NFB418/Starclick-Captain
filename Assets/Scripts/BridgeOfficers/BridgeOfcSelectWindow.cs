using UnityEngine;
using UnityEngine.UI;

public class BridgeOfcSelectWindow : MonoBehaviour
{

    private bool isWindowOpen = false;
    [SerializeField] Scrollbar ScrollbarScript;

    void Update()
    {
        // Check if the ESC key is pressed
        if (isWindowOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            BridgeOfcManager.instance.CloseSelectWindow();
        }
    }

    public void OpenWindow()
    {
        foreach (BridgeOfcSelectButton selectButton in BridgeOfcManager.instance.selectButtonsList) { selectButton.RunUpdate();}
        Controller.instance.inGameUI.interactable = false;
        this.gameObject.SetActive(true);
        ScrollbarScript.value = 1f;
        isWindowOpen = true;
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        Controller.instance.inGameUI.interactable = true;
        isWindowOpen = false;
    }
}
