using UnityEngine;
using UnityEngine.UI;

public class BridgeOfcSelectButton : MonoBehaviour
{
    [Header("Button Functionality")]
    [SerializeField] Button SelectButton;
    [SerializeField] Image SelectButtonPortrait;
    [SerializeField] Animator portraitAnimator;
    [SerializeField] Sprite LockedPortrait;

    [Header("Bridge Officer")]
    public BridgeOfficer bridgeOfficer;

    private bool unlockable;
    private bool unlocked;

    // A version of start callable whenever new Data has been loaded
    public void Initialize()
    {
        unlockable = false;
        unlocked = false;
        SelectButton.interactable = false;
        portraitAnimator.enabled = false;
        SelectButtonPortrait.color = Color.white;  // resets color changes from animator
        SelectButtonPortrait.sprite = LockedPortrait;
    }

    // Update is called once per frame
    private void Update()
    {
        if (unlocked) {return;}
        if (bridgeOfficer.CheckUnlocked()) 
        {
            UnlockOfficer();
            return; 
        }
        if (!unlockable)
        {
            if (bridgeOfficer.CheckUnlockable())
            {
                unlockable = true;
                SelectButton.interactable = true;
                portraitAnimator.enabled = true;
            }
        }
    }

    public void RunUpdate() { Update(); } // Use this to force an update before opening select window

    private void UnlockOfficer()
    {
        bridgeOfficer.Unlock();
        unlockable = true;
        unlocked = true;
        SelectButton.interactable = true;
        portraitAnimator.enabled = false;
        SelectButtonPortrait.color = Color.white;  // resets color changes from animator
        SelectButtonPortrait.sprite = bridgeOfficer.Portrait;
    }

    public void SelectOfficer()
    {
        if (!unlockable) {return;}
        if (!unlocked) { UnlockOfficer();}
        BridgeOfcManager.instance.SelectOfficer(bridgeOfficer);
    }
}
