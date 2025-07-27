using System;
using UnityEngine;

public class OfficerRecklessInventor : BridgeOfficer
{

    // OnDeck should be called when the Officer is selected for the Bridge
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public override void OnDeck(BridgeOfcSlot bridgeOfcSlot)
    {
        OnDeckBridgeOfcSlot = bridgeOfcSlot;
        OnDeckBridgeOfcSlot.SlottedBridgeOfficer = this;
        OnDeckBridgeOfcSlot.AbilityButton.interactable = false;
        OnDeckBridgeOfcSlot.AbilityButtonText.text = "Passive";
        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 0;
        OnDeckBridgeOfcSlot.AbilityButtonTooltip.content = ofcAbilityText;
        OnDeckBridgeOfcSlot.PortraitImage.sprite = Portrait;
        OnDeckBridgeOfcSlot.PortraitTooltip.header = ofcName;
        OnDeckBridgeOfcSlot.PortraitTooltip.content = $"<i>{ofcFlavorText}</i>\n(Click to switch officers.)";

        // This officer has a lock percentage, so does not activate ability here.
    }

    // ActivateAbility should be called when the Officer's ability is activated
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public override void ActivateAbility()
    {
        // Designed to do nothing if ability already activated
        if (AbilityIsActive) { return; }
        AbilityIsActive = true;

        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 1;

        BridgeOfcManager.instance.AnomalyChanceMultiplier = Math.Round(BridgeOfcManager.instance.AnomalyChanceMultiplier * 0.5, 10); // rounding is to avoid precision errors
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
        DeactivateAbility();
        OnDeckBridgeOfcSlot = null;
    }

    // DeactivateAbility should be called when the Officer's ability is deactivated
    // It should undo anything permanent done by ActivateAbility
    public override void DeactivateAbility()
    {
        // Designed to do nothing if ability already deactivated
        if (!AbilityIsActive) { return; }
        AbilityIsActive = false;

        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 0;

        BridgeOfcManager.instance.AnomalyChanceMultiplier = Math.Round(BridgeOfcManager.instance.AnomalyChanceMultiplier * 2, 10); // rounding is to avoid precision errors
    }

    public override bool CheckUnlockable()
    {
        if (Controller.instance.data.crewScientists >= 2) { Controller.instance.data.Officer_01_RI_Unlockable = true; }
        return Controller.instance.data.Officer_01_RI_Unlockable;
    }
    public override bool CheckUnlocked()
    {
        return Controller.instance.data.Officer_01_RI_Unlocked;
    }
    public override void Unlock()
    {
        Controller.instance.data.Officer_01_RI_Unlockable = true;
        Controller.instance.data.Officer_01_RI_Unlocked = true;
    }
}
