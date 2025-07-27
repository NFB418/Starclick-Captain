using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BridgeOfcSlot : MonoBehaviour
{
    [HideInInspector] public BridgeOfficer SlottedBridgeOfficer; // Should only be set within code

    public Button AbilityButton;
    public TMP_Text AbilityButtonText;
    public Image AbilityButtonFill;
    public TooltipTrigger AbilityButtonTooltip;
    public Image PortraitImage;
    public TooltipTrigger PortraitTooltip;
    public Animator portraitAnimator;

    public void Initialize()
    {
        SlottedBridgeOfficer = BridgeOfcManager.instance.defaultBridgeOfficer;
        AbilityButton.interactable = false;
        AbilityButtonText.text = string.Empty;
        AbilityButtonFill.fillAmount = 0;
        AbilityButtonTooltip.content = "This bridge officer slot is vacant, click the portrait to assign an officer!";
        PortraitImage.sprite = SlottedBridgeOfficer.Portrait;
        PortraitTooltip.content = "This bridge officer slot is vacant, click to assign an officer!";

        portraitAnimator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PortraitClick()
    {
        BridgeOfcManager.instance.OpenSelectWindow(this);
    }
    public void AbilityButtonClick()
    {
        SlottedBridgeOfficer.ExecuteAction();
    }
}
