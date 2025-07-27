using BreakInfinity;
using UnityEngine;

public abstract class BridgeOfficer : MonoBehaviour
{
    public Sprite Portrait;
    public string ofcName; // Note that the save system uses these as UNIQUE ID's!

    [TextArea(3, 5)]
    public string ofcFlavorText;

    [TextArea(3, 5)]
    public string ofcUnlockReqText;

    [TextArea(3, 5)]
    public string ofcAbilityText;

    public bool allowMultiple = false;
    public bool isPassive = false;
    public float lockPercentage = 0f;
    public BigDouble minimumScientists = 0;
    public double secondsToCharge = 0;
    public double boostDuration = 0;

    // Below values should be saved and loaded.
    [HideInInspector] public double secondsCharged;
    [HideInInspector] public bool boostIsActive;
    [HideInInspector] public double secondsBoosted;

    // get; -> This means the property is publicly readable (any class can access its value).
    // protected set; -> This means the property can only be modified within the class itself or its derived classes.
    // We use this to create a semi-private value that won't show up in the unity editor
    public bool AbilityIsActive { get; protected set; } = false;

    public BridgeOfcSlot OnDeckBridgeOfcSlot { get; protected set; }

    // OnDeck should be called when the Officer is selected for the Bridge
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public abstract void OnDeck(BridgeOfcSlot bridgeOfcSlot); // This is an abstract method that should be overridden by the officer-specific subclass!

    // ActivateAbility should be called when the Officer's ability is activated
    // It should modify appropriate values in BridgeOfcManager.instance so that THE LATTER can handle Bridge Officers interaction with other systems
    public virtual void ActivateAbility()
    {
        // Designed to do nothing if ability already activated
        if (AbilityIsActive) { return; }
        AbilityIsActive = true;
        // This is a dummy method that should be overridden by the officer-specific subclass!
    }

    // Should be called every tenth second by BridgeOfcManager.instance for every officer currently on deck
    public virtual void OfficerPerTenthSecondUpdate()
    {
        // This is a dummy method that should be overridden by the officer-specific subclass!
    }

    // ExecuteAction should be called when the Officer's action ability is used (usually when the ability button is clicked).
    public virtual void ExecuteAction()
    {
        // This is a dummy method that should be overridden by the officer-specific subclass!
    }

    // OffDeck should be called when the Officer is removed from the Bridge
    // It should undo everything done by OnDeck
    public virtual void OffDeck()
    {
        DeactivateAbility();
        OnDeckBridgeOfcSlot = null;
        // This is a dummy method that should be overridden by the officer-specific subclass!
    }

    // DeactivateAbility should be called when the Officer's ability is deactivated
    // It should undo anything permanent done by ActivateAbility
    public virtual void DeactivateAbility()
    {
        // Designed to do nothing if ability already deactivated
        if (!AbilityIsActive) { return; }
        AbilityIsActive = false;
        // This is a dummy method that should be overridden by the officer-specific subclass!
    }

    public virtual bool CheckUnlockable()
    {
        // This is a dummy method that should be overridden by the officer-specific subclass!
        return false;
    }
    public virtual bool CheckUnlocked()
    {
        // This is a dummy method that should be overridden by the officer-specific subclass!
        return false;
    }
    public virtual void Unlock()
    {
        // This is a dummy method that should be overridden by the officer-specific subclass!
    }
}
