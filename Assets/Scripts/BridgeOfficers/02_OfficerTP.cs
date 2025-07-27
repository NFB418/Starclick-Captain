using BreakInfinity;
using UnityEngine;

public class OfficerTotalPowergamer: BridgeOfficer
{

    // OnDeck should be called when the Officer is selected for the Bridge
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public override void OnDeck(BridgeOfcSlot bridgeOfcSlot)
    {
        OnDeckBridgeOfcSlot = bridgeOfcSlot;
        OnDeckBridgeOfcSlot.SlottedBridgeOfficer = this;
        OnDeckBridgeOfcSlot.AbilityButton.interactable = true;
        OnDeckBridgeOfcSlot.AbilityButtonText.text = "Activate";
        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 1;
        OnDeckBridgeOfcSlot.AbilityButtonTooltip.content = ofcAbilityText;
        OnDeckBridgeOfcSlot.PortraitImage.sprite = Portrait;
        OnDeckBridgeOfcSlot.PortraitTooltip.header = ofcName;
        OnDeckBridgeOfcSlot.PortraitTooltip.content = $"<i>{ofcFlavorText}</i>\n(Click to switch officers.)";

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
        BigDouble unlockedAntimatter = BridgeOfcManager.instance.ReturnUnlockedAntimatter();
        BridgeOfcManager.instance.DecreaseAntimatterByAmount(unlockedAntimatter);
        Controller.instance.IncreasePowerDirect(unlockedAntimatter * Controller.instance.data.ClickLimit() * 10);
    }

    // OffDeck should be called when the Officer is removed from the Bridge
    // It should undo everything done by OnDeck
    public override void OffDeck()
    {
        // Does NOT change OnDeckBridgeOfcSlot UI, because that should simply be overwritten by is new officer's OnDeck method!
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
    }

    public override bool CheckUnlockable()
    {
        if (Controller.instance.data.crewScientists >= 1) { Controller.instance.data.Officer_02_TP_Unlockable = true; }
        return Controller.instance.data.Officer_02_TP_Unlockable;
    }
    public override bool CheckUnlocked()
    {
        return Controller.instance.data.Officer_02_TP_Unlocked;
    }
    public override void Unlock()
    {
        Controller.instance.data.Officer_02_TP_Unlockable = true;
        Controller.instance.data.Officer_02_TP_Unlocked = true;
    }
}
