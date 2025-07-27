using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

public class CaptainPortraitSelectWindow : MonoBehaviour
{

    [SerializeField] Image captainPortraitImage;
    [SerializeField] TMP_Text captainPortraitText;

    [SerializeField] Image buttonPortraitA;
    [SerializeField] Image buttonPortraitB;
    [SerializeField] Image buttonPortraitC;
    [SerializeField] Image buttonPortraitD;
    [SerializeField] Image buttonPortraitE;
    [SerializeField] Image buttonPortraitF;
    [SerializeField] Image buttonPortraitG;
    [SerializeField] Image buttonPortraitH;

    public char selectedCaptainPortrait = '?';

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
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        Controller.instance.inGameUI.interactable = true;
        isWindowOpen = false;
    }

    // For the record, I am aware this is an inelegant solution. But it works and this feature is not worth the time I'd need right now to figure out how to do it elegantly in C# :P
    public void SetPortrait(char Char)
    {
        switch (char.ToUpper(Char))
        {
            case 'A':
                SelectPortraitA();
                break;
            case 'B':
                SelectPortraitB();
                break;
            case 'C':
                SelectPortraitC();
                break;
            case 'D':
                SelectPortraitD();
                break;
            case 'E':
                SelectPortraitE();
                break;
            case 'F':
                SelectPortraitF();
                break;
            case 'G':
                SelectPortraitG();
                break;
            case 'H':
                SelectPortraitH();
                break;
            default:
                SelectPortraitNull();
                break;
        }
    }

    // Function to reset the CaptainPortraitImage
    public void SelectPortraitNull()
    {
        selectedCaptainPortrait = '?';
        captainPortraitImage.sprite = null;
        captainPortraitText.gameObject.SetActive(true);
        captainPortraitImage.gameObject.SetActive(false);
        CloseWindow();
    }

    // Functions to update the CaptainPortraitImage based on the button pressed
    public void SelectPortraitA()
    {
        selectedCaptainPortrait = 'A';
        captainPortraitImage.sprite = buttonPortraitA.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }

    public void SelectPortraitB()
    {
        selectedCaptainPortrait = 'B';
        captainPortraitImage.sprite = buttonPortraitB.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }

    public void SelectPortraitC()
    {
        selectedCaptainPortrait = 'C';
        captainPortraitImage.sprite = buttonPortraitC.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }

    public void SelectPortraitD()
    {
        selectedCaptainPortrait = 'D';
        captainPortraitImage.sprite = buttonPortraitD.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }

    public void SelectPortraitE()
    {
        selectedCaptainPortrait = 'E';
        captainPortraitImage.sprite = buttonPortraitE.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }

    public void SelectPortraitF()
    {
        selectedCaptainPortrait = 'F';
        captainPortraitImage.sprite = buttonPortraitF.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }

    public void SelectPortraitG()
    {
        selectedCaptainPortrait = 'G';
        captainPortraitImage.sprite = buttonPortraitG.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }

    public void SelectPortraitH()
    {
        selectedCaptainPortrait = 'H';
        captainPortraitImage.sprite = buttonPortraitH.sprite;
        captainPortraitText.gameObject.SetActive(false);
        captainPortraitImage.gameObject.SetActive(true);
        CloseWindow();
    }
}
