using UnityEngine;

public class OfficerEmpty : BridgeOfficer
{

    // OnDeck should be called when the Officer is selected for the Bridge
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public override void OnDeck(BridgeOfcSlot bridgeOfcSlot)
    {
        OnDeckBridgeOfcSlot = bridgeOfcSlot;
        OnDeckBridgeOfcSlot.SlottedBridgeOfficer = this;
        OnDeckBridgeOfcSlot.AbilityButton.interactable = false;
        OnDeckBridgeOfcSlot.AbilityButtonText.text = string.Empty;
        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 0;
        OnDeckBridgeOfcSlot.AbilityButtonTooltip.content = "This bridge officer slot is vacant, click the portrait to assign an officer!";
        OnDeckBridgeOfcSlot.PortraitImage.sprite = Portrait;
        OnDeckBridgeOfcSlot.PortraitTooltip.header = "Select Bridge Officer";
        OnDeckBridgeOfcSlot.PortraitTooltip.content = "This bridge officer slot is vacant, click to assign an officer!";

        OnDeckBridgeOfcSlot = null; // Vacant seat does NOT want to keep this.

        ActivateAbility(); // Do NOT do this for officers with a lockPercentage, BridgeOfcManager.instance handles their activation when antimatter is available.
    }

    // ActivateAbility should be called when the Officer's ability is activated
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public override void ActivateAbility()
    {
        // Designed to do nothing if ability already activated
        if (AbilityIsActive) { return; }
        AbilityIsActive = true;
    }

    // Should be called every tenth second by BridgeOfcManager.instance for every officer currently on deck
    public override void OfficerPerTenthSecondUpdate()
    {
        
    }

    // ExecuteAction should be called when the Officer's action ability is used (usually when the ability button is clicked).
    public override void ExecuteAction()
    {
        
    }

    // OffDeck should be called when the Officer is removed from the Bridge
    // It should undo everything done by OnDeck
    public override void OffDeck()
    {
        // Does NOT change OnDeckBridgeOfcSlot UI, because that should simply be overwritten by is new officer's OnDeck method!
        
        // Vacant seat does not want to change anything here!
        // OnDeckBridgeOfcSlot = null;
        DeactivateAbility();
    }

    // DeactivateAbility should be called when the Officer's ability is deactivated
    // It should undo anything permanent done by ActivateAbility
    public override void DeactivateAbility()
    {
        // Designed to do nothing if ability already deactivated
        if (!AbilityIsActive) { return; }
        AbilityIsActive = false;
    }

    public override bool CheckUnlockable()
    {
        return Controller.instance.data.Officer_00_Empty_Unlockable;
    }
    public override bool CheckUnlocked()
    {
        return Controller.instance.data.Officer_00_Empty_Unlocked;
    }
    public override void Unlock()
    {
        Controller.instance.data.Officer_00_Empty_Unlockable = true;
        Controller.instance.data.Officer_00_Empty_Unlocked = true;
    }
}
