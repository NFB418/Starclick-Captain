using System;
using UnityEngine;

public class OfficerPonderingGenius: BridgeOfficer
{

    // OnDeck should be called when the Officer is selected for the Bridge
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public override void OnDeck(BridgeOfcSlot bridgeOfcSlot)
    {
        OnDeckBridgeOfcSlot = bridgeOfcSlot;
        OnDeckBridgeOfcSlot.SlottedBridgeOfficer = this;
        OnDeckBridgeOfcSlot.AbilityButton.interactable = false;
        OnDeckBridgeOfcSlot.AbilityButtonText.text = "Charging...";
        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 0;
        OnDeckBridgeOfcSlot.AbilityButtonTooltip.content = ofcAbilityText;
        OnDeckBridgeOfcSlot.PortraitImage.sprite = Portrait;
        OnDeckBridgeOfcSlot.PortraitTooltip.header = ofcName;
        OnDeckBridgeOfcSlot.PortraitTooltip.content = $"<i>{ofcFlavorText}</i>\n(Click to switch officers.)";

        // This officer has a lock percentage, so does not activate ability here.
        secondsCharged = 0;
        boostIsActive = false;
        secondsBoosted = 0;
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
        if (boostIsActive)
        {
            if (secondsBoosted < boostDuration)
            {
                secondsBoosted += 0.1;
                OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = (float)((boostDuration - secondsBoosted) / boostDuration);
            }
            else
            {
                boostIsActive = false;
                secondsBoosted = 0;
                BridgeOfcManager.instance.ClickLimitMultiplier *= 0.5;

                secondsCharged = 0;
                OnDeckBridgeOfcSlot.AbilityButton.interactable = false;
                OnDeckBridgeOfcSlot.AbilityButtonText.text = "Charging...";
                OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 0;
            }
            return; 
        }
        if (AbilityIsActive)
        {
            if (secondsCharged < secondsToCharge)
            {
                secondsCharged += 0.1;
                OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = (float)(secondsCharged / secondsToCharge);
            }
            if (secondsCharged >= secondsToCharge)
            {
                OnDeckBridgeOfcSlot.AbilityButton.interactable = true;
                OnDeckBridgeOfcSlot.AbilityButtonText.text = "Ready!";
                OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = 1;
            }
        }
    }

    // ExecuteAction should be called when the Officer's action ability is used (usually when the ability button is clicked).
    public override void ExecuteAction()
    {
        if (boostIsActive) { return; }

        boostIsActive = true;
        secondsBoosted = 0;
        BridgeOfcManager.instance.ClickLimitMultiplier *= 2;

        OnDeckBridgeOfcSlot.AbilityButton.interactable = false;
        OnDeckBridgeOfcSlot.AbilityButtonText.text = "Active!";
        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = (float)((boostDuration - secondsBoosted) / boostDuration);
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
        if (boostIsActive)
        {
            boostIsActive = false;
            BridgeOfcManager.instance.ClickLimitMultiplier *= 0.5;

            secondsCharged = Math.Round(((boostDuration - secondsBoosted) / boostDuration) * secondsToCharge, 1);

            secondsBoosted = 0;
        }
        OnDeckBridgeOfcSlot.AbilityButton.interactable = false;
        OnDeckBridgeOfcSlot.AbilityButtonText.text = "Charging...";
        OnDeckBridgeOfcSlot.AbilityButtonFill.fillAmount = (float)(secondsCharged / secondsToCharge);

        // Designed to do nothing if ability already deactivated
        if (!AbilityIsActive) { return; }
        AbilityIsActive = false;
    }

    public override bool CheckUnlockable()
    {
        if (Controller.instance.data.crewScientists >= 3) { Controller.instance.data.Officer_03_PG_Unlockable = true; }
        return Controller.instance.data.Officer_03_PG_Unlockable;
    }
    public override bool CheckUnlocked()
    {
        return Controller.instance.data.Officer_03_PG_Unlocked;
    }
    public override void Unlock()
    {
        Controller.instance.data.Officer_03_PG_Unlockable = true;
        Controller.instance.data.Officer_03_PG_Unlocked = true;
    }
}
