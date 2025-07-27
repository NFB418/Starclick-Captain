using UnityEngine;

public class FadeInOutScript : MonoBehaviour
{
    // This script is primarily designed to fade in UI elements after unlock.
    public bool StartsDisabled;

    private CanvasGroup canvasGroup;
    private bool isFadingIn;
    private float fadeInSpeed = 0.25f;
    private bool isFadingOut;
    private float fadeOutSpeed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            // Add a CanvasGroup dynamically if it doesn't exist
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (StartsDisabled)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            gameObject.SetActive(true);
        }
        this.enabled = false;
    }

    public void AdjustFadeInSpeed(float newSpeed) => fadeInSpeed = newSpeed;

    public void AdjustFadeOutSpeed(float newSpeed) => fadeOutSpeed = newSpeed;

    public void InstantEnable()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        gameObject.SetActive(true);
        this.enabled = false;
    }

    public void InstantDisable()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        gameObject.SetActive(false);
        this.enabled = false;
    }

    public void StartFadeIn()
    {
        isFadingOut = false;
        isFadingIn = true;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        gameObject.SetActive(true);
        this.enabled = true;
    }

    public void StartFadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        gameObject.SetActive(true);
        this.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            if (canvasGroup.alpha < 1f) 
            { 
                canvasGroup.alpha += fadeInSpeed * Time.deltaTime; 
            } 
            else 
            {
                canvasGroup.alpha = 1f;
                isFadingIn = false;
                this.enabled = false;
                return;
            }
        }
        else if (isFadingOut)
        {
            if (canvasGroup.alpha > 0) 
            { 
                canvasGroup.alpha -= fadeOutSpeed * Time.deltaTime; 
            } 
            else 
            {
                canvasGroup.alpha = 0f;
                isFadingOut = false;
                this.enabled = false;
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
