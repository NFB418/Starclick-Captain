using System.Collections.Generic;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BridgeOfcManager : MonoBehaviour
{
    public static BridgeOfcManager instance;
    private void Awake() => instance = this;

    public BridgeOfcSelectWindow bridgeOfcSelectWindow;
    public List<BridgeOfcSelectButton> selectButtonsList; // Do not forget to update this in the editor, it is used by the save system!

    public Image selectionPortrait;
    public TMP_Text selectionTextHeader;
    public TMP_Text selectionTextFlavor;
    public TMP_Text selectionTextAbility;
    public Button selectButton;
    public TooltipTrigger selectButtonTooltip;
    public Button cancelButton;

    public BridgeOfficer defaultBridgeOfficer;
    private BridgeOfficer currentSelectedOfficer;
    private BridgeOfcSlot currentSelectedOfcSlot;

    [HideInInspector] public List<BridgeOfcSlot> BridgeOfficerSlotList = new(); //  list of all BridgeOfcSlot, to be used to generate onDeckBridgeOfficerList
    private List<BridgeOfficer> onDeckBridgeOfficerList = new(); //  list of all onDeckBridgeOfficers, to be used for sorting

    public Image antimatterFill;
    public RectTransform antimatterSlider;
    public int antimatterSliderMinX;
    public int antimatterSliderMaxX;
    public TooltipTrigger antimatterBarTooltip;

    private float lockedPercentage;

    [Header("Save State References")]
    public CaptainPortraitSelectWindow captainPortraitSelectWindow; // relevant attribute is captainPortraitSelectWindow.selectedCaptainPortrait
    public TMP_InputField CaptainNameInput; // relevant attribute is CaptainNameInput.text
    public BridgeOfcSlot bridgeOfcSlot_0;
    public BridgeOfcSlot bridgeOfcSlot_1;
    public BridgeOfcSlot bridgeOfcSlot_2;
    public BridgeOfcSlot bridgeOfcSlot_3;

    // [Header("Modifiers")]
    // Suggestion: use MULTIPLIER for multiplication and MODIFIER for addition/substraction
    [HideInInspector] public double AnomalyChanceMultiplier;
    [HideInInspector] public BigDouble ClickLimitMultiplier;
    [HideInInspector] public float crewEnsignClickIntervalMultiplier;

    // A version of start callable whenever new Data has been loaded
    public void Initialize()
    {
        BridgeOfficerSlotList = new();
        BridgeOfficerSlotList.Add(bridgeOfcSlot_0);
        BridgeOfficerSlotList.Add(bridgeOfcSlot_1);
        BridgeOfficerSlotList.Add(bridgeOfcSlot_2);
        BridgeOfficerSlotList.Add(bridgeOfcSlot_3);

        foreach (BridgeOfcSlot bridgeOfcSlot in BridgeOfficerSlotList) { bridgeOfcSlot.Initialize(); }
        foreach (BridgeOfcSelectButton bridgeOfcSelectButton in selectButtonsList) { bridgeOfcSelectButton.Initialize(); }
        lockedPercentage = 0f;
        ConstructOnDeckOfficerList();

        AnomalyChanceMultiplier = 1f;
        ClickLimitMultiplier = 1;
        crewEnsignClickIntervalMultiplier = 1f;

        captainPortraitSelectWindow.SelectPortraitNull();
        CaptainNameInput.text = null;
    }

    public void ConstructOnDeckOfficerList()
    {
        onDeckBridgeOfficerList = new();
        foreach (BridgeOfcSlot bridgeOfcSlot in BridgeOfficerSlotList)
        {
            onDeckBridgeOfficerList.Add(bridgeOfcSlot.SlottedBridgeOfficer);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenSelectWindow(BridgeOfcSlot bridgeOfcSlot)
    {
        currentSelectedOfcSlot = bridgeOfcSlot;
        SelectOfficer(bridgeOfcSlot.SlottedBridgeOfficer);
        bridgeOfcSelectWindow.OpenWindow();
    }

    // Is called by BridgeOfcSelectButton.SelectOfficer()
    public void SelectOfficer(BridgeOfficer bridgeOfficer)
    {
        currentSelectedOfficer = bridgeOfficer;

        selectionPortrait.sprite = bridgeOfficer.Portrait;
        selectionTextHeader.text = bridgeOfficer.ofcName;
        selectionTextFlavor.text = bridgeOfficer.ofcFlavorText;
        selectionTextAbility.text = bridgeOfficer.ofcAbilityText;

        string invalid_tooltip = "";
        if (onDeckBridgeOfficerList.Contains(currentSelectedOfficer) && !currentSelectedOfficer.allowMultiple)
        {
            invalid_tooltip += " - Officer is already on the bridge!";
        }
        if (PotentialLockedPercentage() + currentSelectedOfficer.lockPercentage - currentSelectedOfcSlot.SlottedBridgeOfficer.lockPercentage > 1)
        {
            if (!string.IsNullOrEmpty(invalid_tooltip)) { invalid_tooltip += "\n"; }
            invalid_tooltip += " - Not enough unlocked antimatter!";
        }

        if (!string.IsNullOrEmpty(invalid_tooltip))
        {
            selectButton.interactable = false;
            selectButtonTooltip.content = invalid_tooltip;
            selectButtonTooltip.enabled = true;
        }
        else
        {
            selectButton.interactable = true;
            selectButtonTooltip.enabled = false;
        }
    }

    private float PotentialLockedPercentage()
    {
        // Because actual locked percentage ignores officers for whom the lock requirement isn't fulfilled!

        float potentialLockedPercentage = 0f;
        foreach (BridgeOfficer bridgeOfficer in onDeckBridgeOfficerList)
        {
            potentialLockedPercentage += bridgeOfficer.lockPercentage;
        }
        return potentialLockedPercentage;
    }

    public void SelectButtonClick()
    {
        // Debug.Log($"{currentSelectedOfcSlot.SlottedBridgeOfficer}");
        currentSelectedOfcSlot.SlottedBridgeOfficer.OffDeck();
        currentSelectedOfficer.OnDeck(currentSelectedOfcSlot);
        ConstructOnDeckOfficerList();
        CloseSelectWindow();
    }

    public void CancelButtonClick()
    {
        CloseSelectWindow();
    }

    public void CloseSelectWindow()
    {
        currentSelectedOfcSlot = null;
        bridgeOfcSelectWindow.CloseWindow();
    }

    public void PerTenthSecondUpdate()
    {
        bool unlockableBridgeOfc = false;
        foreach (BridgeOfcSelectButton bridgeOfcSelectButton in selectButtonsList)
        {
            if (!bridgeOfcSelectButton.bridgeOfficer.CheckUnlocked() && bridgeOfcSelectButton.bridgeOfficer.CheckUnlockable())
            {
                unlockableBridgeOfc = true;
                break;
            }
        }
        foreach (BridgeOfcSlot bridgeOfcSlot in BridgeOfficerSlotList)
        {
            bridgeOfcSlot.portraitAnimator.enabled = unlockableBridgeOfc;
        }

        BigDouble max_antimatter = CrewManager.instance.MaxAntimatter();
        if (max_antimatter <= 0) 
        {
            Controller.instance.data.antimatterStored = 0; // Lowering max_antimatter always clamps antimatterStored to the cap!
            lockedPercentage = 0f;
            foreach (BridgeOfficer bridgeOfficer in onDeckBridgeOfficerList)
            {
                if (bridgeOfficer.lockPercentage > 0f) // If this officer uses the lock mechanic
                {
                    bridgeOfficer.DeactivateAbility();
                }
            }
            UpdateAntimatterBar();
            return; 
        }

        if (Controller.instance.data.antimatterStored < max_antimatter)
        {
            Controller.instance.data.antimatterStored += CrewManager.instance.AntimatterGeneration() * 0.1;
            if (Controller.instance.data.antimatterStored > max_antimatter) { Controller.instance.data.antimatterStored = max_antimatter; }
        }
        else 
        {
            Controller.instance.data.antimatterStored = max_antimatter; // Lowering max_antimatter always clamps antimatterStored to the cap!
        }

        BigDouble fill_amount_raw = BigDouble.Divide(Controller.instance.data.antimatterStored, max_antimatter);
        float fill_amount = Controller.BigDoubleToFloat(fill_amount_raw);
        antimatterFill.fillAmount = fill_amount;

        onDeckBridgeOfficerList.Sort((x, y) => x.lockPercentage.CompareTo(y.lockPercentage)); // Sort so lowest percentage goes first
        lockedPercentage = 0f;
        foreach (BridgeOfficer bridgeOfficer in onDeckBridgeOfficerList)
        {
            if (bridgeOfficer.lockPercentage > 0f) // If this officer uses the lock mechanic
            {
                // Must meet the minimum scientists requirement
                if (Controller.instance.data.crewScientists >= bridgeOfficer.minimumScientists)
                { 
                    // If there is enough locked antimatter, activate ability, else deactivate it (duplicate activation gets ignored)
                    lockedPercentage += bridgeOfficer.lockPercentage;
                    if (fill_amount >= lockedPercentage)
                    {
                        bridgeOfficer.ActivateAbility();
                    }
                    else
                    {
                        bridgeOfficer.DeactivateAbility();
                    }
                }
                else
                {
                    bridgeOfficer.DeactivateAbility();
                }
            }
            bridgeOfficer.OfficerPerTenthSecondUpdate(); // Run officer's own update
        }
        SetSliderPosition(lockedPercentage);

        // UpdateAntimatterBar(); Now integrated in above code
        UpdateAntimatterBarTooltip();
    }

    public void UpdateAntimatterBar() // This same code is integrated into PerTenthSecondUpdate(), this method is for changes from elsewhere.
    {
        BigDouble max_antimatter = CrewManager.instance.MaxAntimatter();
        if (max_antimatter > 0)
        {
            BigDouble fill_amount_raw = BigDouble.Divide(Controller.instance.data.antimatterStored, max_antimatter);
            // Must convert this BigDouble to a float. THIS WILL NOT WORK PROPERLY IF fill_amount_raw IS TOO BIG/SMALL TO BE PROPERLY CONVERTED
            float fill_amount = Controller.BigDoubleToFloat(fill_amount_raw);
            antimatterFill.fillAmount = fill_amount;

            SetSliderPosition(lockedPercentage);
        }
        else
        {
            antimatterFill.fillAmount = 0f;
            SetSliderPosition(0f);
        }

        UpdateAntimatterBarTooltip();
    }

    private void UpdateAntimatterBarTooltip()
    {
        antimatterBarTooltip.content = $"<b>Antimatter</b> is used by your <b>bridge officers</b> to activate their <b>abilities</b>. Some <b>abilities</b> may <b>lock</B> a percentage of your <b>maximum antimatter storage</b>. (<b>Locked antimatter</b> is unavailable to other <b>officers</b>. Moreover, <b>abilities</b> which require an <b>antimatter lock</b> <u>do not activate</u> until you have stored <b>antimatter</b> equal to the <b>lock percentage</b>.)" +
            $"\n\nAntimatter generation: {CrewManager.instance.AntimatterGeneration()} per second." +
            $"\nAntimatter stored: {Controller.instance.data.antimatterStored} / {CrewManager.instance.MaxAntimatter()}" +
            $"\nLock percentage: {Mathf.Round(lockedPercentage * 100)}%";

        if (antimatterBarTooltip.isTooltipActive)
        {
            TooltipSystem.instance.tooltip.SetText(antimatterBarTooltip.content, antimatterBarTooltip.header);
        }
    }

    private void SetSliderPosition(float normalizedValue) // normalizedValue should be between 0 and 1
    {
        float newX = Mathf.Lerp(antimatterSliderMinX, antimatterSliderMaxX, normalizedValue);
        antimatterSlider.anchoredPosition = new Vector2(newX, antimatterSlider.anchoredPosition.y);
    }

    public BigDouble ReturnUnlockedAntimatter()
    {
        BigDouble locked_amount = BigDouble.Multiply(CrewManager.instance.MaxAntimatter(), lockedPercentage);
        BigDouble unlocked_amount = Controller.instance.data.antimatterStored - locked_amount;
        if (unlocked_amount > 0) { return unlocked_amount; } else { return 0; }
    }

    public void DecreaseAntimatterByAmount(BigDouble amount) 
    {
        Controller.instance.data.antimatterStored -= amount;
        if (Controller.instance.data.antimatterStored < 0) { Controller.instance.data.antimatterStored = 0; }
        UpdateAntimatterBar();
    }
}
