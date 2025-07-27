using System;

[Serializable]
public class BridgeOfficerSaveData
{
    private string ofcName;
    private double secondsCharged;
    private bool boostIsActive;
    private double secondsBoosted;

    public BridgeOfficerSaveData(BridgeOfficer bridgeOfficer)
    {
        ofcName = bridgeOfficer.ofcName;
        secondsCharged = bridgeOfficer.secondsCharged;
        boostIsActive = bridgeOfficer.boostIsActive;
        secondsBoosted = bridgeOfficer.secondsBoosted;
    }

    public void LoadIntoOfcSlot(BridgeOfcSlot bridgeOfcSlot)
    {
        BridgeOfficer bridgeOfficer = BridgeOfcManager.instance.defaultBridgeOfficer;
        foreach (BridgeOfcSelectButton bridgeOfcSelectButton in BridgeOfcManager.instance.selectButtonsList)
        {
            if (bridgeOfcSelectButton.bridgeOfficer.ofcName == ofcName)
            {
                bridgeOfficer = bridgeOfcSelectButton.bridgeOfficer;
                break;
            }
        }
        // Below just does what BridgeOfcManager.instance.SelectButtonClick() does
        bridgeOfcSlot.SlottedBridgeOfficer.OffDeck();
        bridgeOfficer.OnDeck(bridgeOfcSlot);
        BridgeOfcManager.instance.ConstructOnDeckOfficerList();
        // Then set the associated values
        bridgeOfficer.secondsCharged = secondsCharged;
        bridgeOfficer.boostIsActive = false; // Make sure this value was reset
        if (boostIsActive) { bridgeOfficer.ExecuteAction(); } // If it was boosting, activate the boost (which sets boostIsActive = true)
        bridgeOfficer.secondsBoosted = secondsBoosted; // THEN set secondsBoosted
    }
}