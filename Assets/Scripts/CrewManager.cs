using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BreakInfinity;
using UnityEditor;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class CrewManager : MonoBehaviour
{
    public static CrewManager instance;
    private void Awake() => instance = this;

    public CrewUI crewUI;

    // Hire Fire Toggles
    private bool isHiring;
    private int HireFireAmount;

    // Ensign
    private BigDouble crewEnsignBaseCost;
    private BigDouble crewEnsignCostMult;
    [HideInInspector] public float crewEnsignBaseClickInterval;

    // Engineer
    private BigDouble crewEngineerBaseCost;
    private BigDouble crewEngineerCostMult;
    [HideInInspector] public BigDouble crewEngineerClickPowerBonus;

    // Doctor
    private BigDouble crewDoctorBaseCost;
    private BigDouble crewDoctorCostMult;
    [HideInInspector] public BigDouble crewDoctorReductionMult;

    // Scientist
    private BigDouble crewScientistBaseCost;
    private BigDouble crewScientistCostMult;
    [HideInInspector] public BigDouble crewScientistMaxAntimatterBonus;
    [HideInInspector] public BigDouble crewScientistAntimatterGeneration;

    public void Initialize()
    {

        crewEnsignBaseCost = 10;
        crewEnsignCostMult = 1.618;
        crewEnsignBaseClickInterval = 1;

        crewEngineerBaseCost = 100;
        crewEngineerCostMult = 1.618;
        crewEngineerClickPowerBonus = 1;

        crewDoctorBaseCost = 1000;
        crewDoctorCostMult = 1.618;
        crewDoctorReductionMult = 0.5;

        crewScientistBaseCost = 10000;
        crewScientistCostMult = 1.618;
        crewScientistMaxAntimatterBonus = 100;
        crewScientistAntimatterGeneration = 1; // 1 when not testing

        UpdateHireFire();
    }

    public void UpdateHireFire()
    {
        // Less efficient, but simpler to simply run this on toggle state change (will run this multiple times per click because of how toggles in a toggle group activate each other)
        if (crewUI.HireToggle.isOn) {isHiring = true;} else {isHiring = false;}
        if (crewUI.x100Toggle.isOn) { HireFireAmount = 100; }
        else if (crewUI.x10Toggle.isOn) { HireFireAmount = 10; }
        else { HireFireAmount = 1; }
    }

    public void Update()
    {
        Data data = Controller.instance.data; // for ease of reference
        BigDouble EnsignCost = CrewEnsignCost();
        BigDouble EngineerCost = CrewEngineerCost();
        BigDouble DoctorCost = CrewDoctorCost();
        BigDouble ScientistCost = CrewScientistCost();

        if (data.totalPower >= EnsignCost && (isHiring || data.crewEnsigns >= HireFireAmount))
        {
            crewUI.crewEnsignButton.interactable = true;
        }
        else
        {
            crewUI.crewEnsignButton.interactable = false;
        }
        if (data.hiredFirstEnsign)
        {
            crewUI.crewEnsignText.text = $"<size=10><u>{data.crewEnsigns} ENSIGN{(data.crewEnsigns != 1 ? "S" : "")}</u></size>\r\n<size=8>Each ensign <b>clicks</b> once every <b>{crewEnsignClickInterval()}</b> seconds.</size>\r\n" +
                $"<size=8>Cost: {EnsignCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewEnsignTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} Ensign{(HireFireAmount != 1 ? "s" : "")}";
            crewUI.crewEnsignTooltip.content = $"Each ensign <b>clicks</b> once every <b>{crewEnsignClickInterval()}</b> seconds.\r\n" +
                $"Cost: {EnsignCost} Power\r\n" +
                $"\n<i>The grease that keeps a starship running, everything you achieve from here on out will be paid for in the sweat and blood of your ensigns.</i>";
        }
        else 
        {
            crewUI.crewEnsignText.text = $"<size=10><u>?????????</u></size>\r\n" +
                $"<size=8>Cost: {EnsignCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewEnsignTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} ?????????";
            crewUI.crewEnsignTooltip.content = $"Cost: {EnsignCost} Power";
        }

        if (data.totalPower >= EngineerCost && (isHiring || data.crewEngineers >= HireFireAmount))
        {
            crewUI.crewEngineerButton.interactable = true;
        }
        else
        {
            crewUI.crewEngineerButton.interactable = false;
        }
        if (data.hiredFirstEngineer)
        {
            crewUI.crewEngineerText.text = $"<size=10><u>{data.crewEngineers} ENGINEER{(data.crewEngineers != 1 ? "S" : "")}</u></size>\r\n<size=8>Each engineer ups <b>click power</b> by <b>{crewEngineerClickPowerBonus}</b>.</size>\r\n" +
                $"<size=8>Cost: {EngineerCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewEngineerTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} Engineer{(HireFireAmount != 1 ? "s" : "")}";
            crewUI.crewEngineerTooltip.content = $"Each engineer ups <b>click power</b> by <b>{crewEngineerClickPowerBonus}</b>.\r\n" +
                $"Cost: {EngineerCost} Power\r\n" +
                $"\n<i>An engineer never fixes the ship too late, nor too early. Engineers fix the ship in the nick of time, precisely as they’re meant to.</i>";

        }
        else
        {
            crewUI.crewEngineerText.text = $"<size=10><u>?????????</u></size>\r\n" +
                $"<size=8>Cost: {EngineerCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewEngineerTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} ?????????";
            crewUI.crewEngineerTooltip.content = $"Cost: {EngineerCost} Power";
        }


        if (data.totalPower >= DoctorCost && (isHiring || data.crewDoctors >= HireFireAmount))
        {
            crewUI.crewDoctorButton.interactable = true;
        }
        else
        {
            crewUI.crewDoctorButton.interactable = false;
        }
        if (data.hiredFirstDoctor)
        {
            crewUI.crewDoctorText.text = $"<size=10><u>{data.crewDoctors} DOCTOR{(data.crewDoctors != 1 ? "S" : "")}</u></size>\r\n<size=8>Each doctor lowers <b>crew cost</b> as if you had <b>{crewDoctorReductionMult}</b> less crew of that type.</size>\r\n" +
                $"<size=8>Cost: {DoctorCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewDoctorTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} Doctor{(HireFireAmount != 1 ? "s" : "")}";
            crewUI.crewDoctorTooltip.content = $"Each doctor lowers <b>crew cost</b> as if you had <b>{crewDoctorReductionMult}</b> less crew of that type (to a minimum of 0).\r\n" +
                $"Cost: {DoctorCost} Power\r\n" +
                $"\n<i>You never need a doctor, until you do, and then you’re glad you hired a doctor and not another ensign or engineer!</i>";

        }
        else
        {
            crewUI.crewDoctorText.text = $"<size=10><u>?????????</u></size>\r\n" +
                $"<size=8>Cost: {DoctorCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewDoctorTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} ?????????";
            crewUI.crewDoctorTooltip.content = $"Cost: {DoctorCost} Power";
        }

        if (data.totalPower >= ScientistCost && (isHiring || data.crewScientists >= HireFireAmount))
        {
            crewUI.crewScientistButton.interactable = true;
        }
        else
        {
            crewUI.crewScientistButton.interactable = false;
        }
        if (data.hiredFirstScientist)
        {
            crewUI.crewScientistText.text = $"<size=10><u>{data.crewScientists} SCIENTIST{(data.crewScientists != 1 ? "S" : "")}</u></size>\r\n<size=8>Each scientist ups <b>antimatter generation</b> by <b>{crewScientistAntimatterGeneration}</b> (per second) and <b>storage</b> by <b>{crewScientistMaxAntimatterBonus}</b>.</size>\r\n" +
                $"<size=8>Cost: {ScientistCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewScientistTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} Scientist{(HireFireAmount != 1 ? "s" : "")}";
            crewUI.crewScientistTooltip.content = $"Each scientist ups <b>antimatter generation</b> by <b>{crewScientistAntimatterGeneration}</b> (per second) and <b>storage</b> by <b>{crewScientistMaxAntimatterBonus}</b>.\r\n" +
                $"Cost: {ScientistCost} Power\r\n" +
                $"\n<i>Scientists are always getting starships into trouble. Thankfully they usually techno-babble it out of trouble too, eventually...</i>";

        }
        else
        {
            crewUI.crewScientistText.text = $"<size=10><u>?????????</u></size>\r\n" +
                $"<size=8>Cost: {ScientistCost} Power to {(isHiring ? "hire" : "fire")} {HireFireAmount}</size>";
            crewUI.crewScientistTooltip.header = $"{(isHiring ? "Hire" : "Fire")} {HireFireAmount} ?????????";
            crewUI.crewScientistTooltip.content = $"Cost: {ScientistCost} Power";
        }

        if (data.firstCareer)
        {
            if (data.totalPower >= data.firstPromotionCost)
            {
                crewUI.tutorialPromotionButton.interactable = true;
            }
            else
            {
                crewUI.tutorialPromotionButton.interactable = false;
            }

            crewUI.tutorialPromotionText.text = $"<size=10><u>ACCEPT PROMOTION</u></size>\r\n<size=8>Accepting your first promotion will end the game's tutorial and this demo.</size>\r\n" +
                $"<size=8>Cost: {data.firstPromotionCost} Power</size>";
            crewUI.tutorialPromotionTooltip.content = $"Accepting your first promotion will end the game's tutorial and this demo. If you like what you've seen, please leave a comment!\r\n" +
                $"Cost: {data.firstPromotionCost} Power\r\n" +
                $"\n<i>You can't stay captain forever. A time comes when you must pass the chair to the next generation!</i>";

        }
        else
        {
            // Other features go here, as they're not supposed to run for the first game
        }
    }

    public float crewEnsignClickInterval() => crewEnsignBaseClickInterval * BridgeOfcManager.instance.crewEnsignClickIntervalMultiplier;

    public BigDouble CrewDoctorReduction() => crewDoctorReductionMult * Controller.instance.data.crewDoctors;
    public BigDouble MaxAntimatter() => crewScientistMaxAntimatterBonus * Controller.instance.data.crewScientists;
    public BigDouble AntimatterGeneration() => crewScientistAntimatterGeneration * Controller.instance.data.crewScientists;

    public BigDouble CrewEnsignCost()
    {
        BigDouble totalCost = 0;
        BigDouble currentIndex = BigDouble.Max(0, Controller.instance.data.crewEnsigns - CrewDoctorReduction());

        for (int i = 0; i < HireFireAmount; i++)
        {
            BigDouble cost = crewEnsignBaseCost * BigDouble.Pow(crewEnsignCostMult, currentIndex + i);
            totalCost += BigDouble.Floor(cost); // Ensuring each step follows the same rounding rules
        }

        // Apply hiring discount if not actively hiring
        if (!isHiring) { totalCost *= 0.1; }

        return BigDouble.Floor(totalCost);
    }
    public BigDouble CrewEngineerCost()
    {
        BigDouble totalCost = 0;
        BigDouble currentIndex = BigDouble.Max(0, Controller.instance.data.crewEngineers - CrewDoctorReduction());

        for (int i = 0; i < HireFireAmount; i++)
        {
            BigDouble cost = crewEngineerBaseCost * BigDouble.Pow(crewEngineerCostMult, currentIndex + i);
            totalCost += BigDouble.Floor(cost); // Ensuring each step follows the same rounding rules
        }

        // Apply hiring discount if not actively hiring
        if (!isHiring) { totalCost *= 0.1; }

        return BigDouble.Floor(totalCost);
    }
    public BigDouble CrewDoctorCost() 
    {
        BigDouble totalCost = 0;
        BigDouble currentIndex = BigDouble.Max(0, Controller.instance.data.crewDoctors - CrewDoctorReduction());

        for (int i = 0; i < HireFireAmount; i++)
        {
            BigDouble cost = crewDoctorBaseCost * BigDouble.Pow(crewDoctorCostMult, currentIndex + i);
            totalCost += BigDouble.Floor(cost); // Ensuring each step follows the same rounding rules
        }

        // Apply hiring discount if not actively hiring
        if (!isHiring) { totalCost *= 0.1; }

        return BigDouble.Floor(totalCost);
    }
    public BigDouble CrewScientistCost() 
    {
        BigDouble totalCost = 0;
        BigDouble currentIndex = BigDouble.Max(0, Controller.instance.data.crewScientists - CrewDoctorReduction());

        for (int i = 0; i < HireFireAmount; i++)
        {
            BigDouble cost = crewScientistBaseCost * BigDouble.Pow(crewScientistCostMult, currentIndex + i);
            totalCost += BigDouble.Floor(cost); // Ensuring each step follows the same rounding rules
        }

        // Apply hiring discount if not actively hiring
        if (!isHiring) { totalCost *= 0.1; }

        return BigDouble.Floor(totalCost);
    }

    IEnumerator UIFadeInAfterDelay(FadeInOutScript fadeInOutScript, float delay) 
    {
        // Wait for the specified amount of time
        float elapsed = 0f;

        while (elapsed < delay)
        {
            // Wait while the game is paused without progressing the timer
            while (Controller.instance.GamePaused)
            {
                yield return null; // Keep waiting until unpaused
            }

            // Progress the delay only when the game is not paused
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Perform the action after the wait
        GeneralUIController.instance.ActivateFadeInOutScript(fadeInOutScript, true, false); // bool Enable = true, bool IsInstant = false
    }
    public void HireEnsign()
    {
        if (Controller.instance.data.totalPower >= CrewEnsignCost())
        {
            Controller.instance.data.totalPower -= CrewEnsignCost();
            Controller.instance.data.crewEnsigns += HireFireAmount * (isHiring? 1: -1);
            Controller.instance.UpdateStatsCorner();

            if (!Controller.instance.data.hiredFirstEnsign)
            {
                Controller.instance.data.hiredFirstEnsign = true;
                TextEventSystem.instance.FirstEnsignEvent();
                StartCoroutine(UIFadeInAfterDelay(GeneralUIController.instance.FireHireTogglesFadeScript, 1f));
            }
        }
    }

    public void HireEngineer()
    {
        if (Controller.instance.data.totalPower >= CrewEngineerCost())
        {
            Controller.instance.data.totalPower -= CrewEngineerCost();
            Controller.instance.data.crewEngineers += HireFireAmount * (isHiring ? 1 : -1);
            Controller.instance.UpdateStatsCorner();

            if (!Controller.instance.data.hiredFirstEngineer)
            {
                Controller.instance.data.hiredFirstEngineer = true;
                TextEventSystem.instance.FirstEngineerEvent();
                StartCoroutine(StartEastAnomalyAfterDelay(10f));
            }
        }
    }

    public IEnumerator StartEastAnomalyAfterDelay(float delay)
    {
        // Wait for the specified amount of time
        float elapsed = 0f;

        while (elapsed < delay)
        {
            // Wait while the game is paused without progressing the timer
            while (Controller.instance.GamePaused)
            {
                yield return null; // Keep waiting until unpaused
            }

            // Progress the delay only when the game is not paused
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Perform the action after the wait
        Controller.instance.eastSpaceCollectible.StartAnomaly();
    }

    public void HireDoctor()
    {
        if (Controller.instance.data.totalPower >= CrewDoctorCost())
        {
            Controller.instance.data.totalPower -= CrewDoctorCost();
            Controller.instance.data.crewDoctors += HireFireAmount * (isHiring ? 1 : -1);
            Controller.instance.UpdateStatsCorner();

            if (!Controller.instance.data.hiredFirstDoctor)
            {
                Controller.instance.data.hiredFirstDoctor = true;
                TextEventSystem.instance.FirstDoctorEvent();
                StartCoroutine(UnlockCaptainsClickAfterDelay(5f));
            }
        }
    }

    public IEnumerator UnlockCaptainsClickAfterDelay(float delay)
    {
        // Wait for the specified amount of time
        float elapsed = 0f;

        while (elapsed < delay)
        {
            // Wait while the game is paused without progressing the timer
            while (Controller.instance.GamePaused)
            {
                yield return null; // Keep waiting until unpaused
            }

            // Progress the delay only when the game is not paused
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Perform the action after the wait
        Controller.instance.data.unlockCaptainsClick = true;
        GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.StoredClicksTextFadeScript, true, false);
        GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.TheCaptainFadeScript, true, false);
    }

    public void HireScientist()
    {
        if (Controller.instance.data.totalPower >= CrewScientistCost())
        {
            Controller.instance.data.totalPower -= CrewScientistCost();
            Controller.instance.data.crewScientists += HireFireAmount * (isHiring ? 1 : -1);
            Controller.instance.UpdateStatsCorner();

            if (!Controller.instance.data.hiredFirstScientist)
            {
                Controller.instance.data.hiredFirstScientist = true;
                TextEventSystem.instance.FirstScientistEvent();
                GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.BridgeOfcUnlockFadeScript, true, false);
            }
        }
    }

    public void AcceptPromotion()
    {
        if (Controller.instance.data.totalPower >= Controller.instance.data.firstPromotionCost)
        {
            Controller.instance.data.totalPower -= Controller.instance.data.firstPromotionCost;
            Controller.instance.data.achievedFirstPromotion = true;
            Controller.instance.data.firstPromotionCost = 0;
            Controller.instance.UpdateStatsCorner();

            crewUI.endOfTutorialWindow.OpenWindow();
        }
    }

}
