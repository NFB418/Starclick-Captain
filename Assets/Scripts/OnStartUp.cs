using UnityEngine;

public class OnStartUp : MonoBehaviour
{
    // This was created as a kind of brief loading screen
    [SerializeField] FadeInOutScript StartUpScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartUpScreen.InstantEnable();
        StartUpScreen.AdjustFadeInSpeed(2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Starting...");
        GeneralUIController.instance.InitializeUI();
        StartUpScreen.StartFadeOut();
        gameObject.SetActive(false);
    }
}
