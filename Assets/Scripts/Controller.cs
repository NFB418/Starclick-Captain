using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BreakInfinity;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;

public class Controller : MonoBehaviour
{
    public static Controller instance;
    private void Awake() => instance = this;

    public Data data;

    private float oneTenthSecondTimer;
    private float oneSecondTimer;
    private float tenSecondTimer;
    private float crewEnsignTimer;
    private float SaveTime;

    private Queue<BigDouble> numberQueue;
    private BigDouble runningSum;
    private BigDouble timesClicked;

    [SerializeField] FadeInOutScript StartUpScreen;

    public CanvasGroup inGameUI;
    public AudioSource audioSource;  // Assign an AudioSource in the Inspector
    public AudioClip clickSoundEffect;   // Assign your sound effect in the Inspector
    public ClickEffectController clickEffectController;

    public TMP_Text totalPowerText;
    public TMP_Text powerPerSecondText;
    public TMP_Text clickPowerText;
    public TMP_Text clickLimitText;
    public TMP_Text storedClicksText;

    public Button starshipButton;
    [HideInInspector] public bool starshipTargeting;
    [HideInInspector] public float rotationSpeed;

    public CaptainBarManager captainBarManager;

    private SpaceCollectibleHandler[] arraySpaceCollectibles;

    public SpaceCollectibleHandler northSpaceCollectible;
    public SpaceCollectibleHandler northeastSpaceCollectible;
    public SpaceCollectibleHandler eastSpaceCollectible;
    public SpaceCollectibleHandler southeastSpaceCollectible;
    public SpaceCollectibleHandler southSpaceCollectible;
    public SpaceCollectibleHandler southwestSpaceCollectible;
    public SpaceCollectibleHandler westSpaceCollectible;
    public SpaceCollectibleHandler northwestSpaceCollectible;

    private bool SaveLoadSystemActive = true;
    private bool ResetSaveAtStart = false;
    private const string dataFileName = "PlayerData_Tutorial";
    public bool GamePaused = false;

    private void Start()
    {
        // First initialize all of Controller's own values

        crewEnsignTimer = 0f; // Tracks the elapsed time
        oneTenthSecondTimer = 0f; // Tracks the elapsed time
        oneSecondTimer = 0f; // Tracks the elapsed time
        tenSecondTimer = 10f; // Tracks the elapsed time

        numberQueue = new(); // Holds the numbers
        runningSum = 0; // Keeps the sum of all numbers in the queue
        timesClicked = 0; // Keeps track of times clicked per second

        starshipTargeting = false;
        rotationSpeed = 1.0f;

        arraySpaceCollectibles = new SpaceCollectibleHandler[]
        {
            northSpaceCollectible, northeastSpaceCollectible, eastSpaceCollectible, southeastSpaceCollectible, 
            southSpaceCollectible, southwestSpaceCollectible, westSpaceCollectible, northwestSpaceCollectible,
        };

        // Then initialize other systems
        foreach(SpaceCollectibleHandler spaceCollectibleHandler in arraySpaceCollectibles) { spaceCollectibleHandler.Initialize(); }
        CrewManager.instance.Initialize();
        BridgeOfcManager.instance.Initialize();

        // Reset save data (mostly for testing purposes)
        if (ResetSaveAtStart) { SaveLoadSystem.DeleteData(dataFileName); }

        // Finally, create or load data
        if (!SaveLoadSystemActive || !SaveLoadSystem.SaveExists(dataFileName)) // If the system isn't active OR there is no save.
        {
            data = new Data();
        }
        else
        {
            data = SaveLoadSystem.LoadData<Data>(dataFileName);
            data.saveStateData.LoadIntoUI(Controller.instance, BridgeOfcManager.instance);
        }

        // Finally + 1, do some UI updating
        captainBarManager.UpdateCaptainBar(); // Because otherwise it only updates after the first second.
        BridgeOfcManager.instance.UpdateAntimatterBar();
        UpdateStatsCorner();

        // Finally + 2, check for and if so open start screen.
        if (!data.unlockGameStart) 
        {
            data.unlockGameStart = true;
            TextEventSystem.instance.NewGameEvent();
        }

    }

    public void Restart()
    {
        // First restart all of Controller's own values

        crewEnsignTimer = 0f; // Tracks the elapsed time
        oneTenthSecondTimer = 0f; // Tracks the elapsed time
        oneSecondTimer = 0f; // Tracks the elapsed time
        tenSecondTimer = 10f; // Tracks the elapsed time

        numberQueue = new(); // Holds the numbers
        runningSum = 0; // Keeps the sum of all numbers in the queue
        timesClicked = 0; // Keeps track of times clicked per second

        starshipTargeting = false;
        rotationSpeed = 1.0f;

        // Then restart other systems
        foreach (SpaceCollectibleHandler SpaceCollectible in arraySpaceCollectibles) { SpaceCollectible.Restart(); }
        CrewManager.instance.Initialize();
        BridgeOfcManager.instance.Initialize();

        // Finally, create new data
        data = new Data();

        // Finally + 1, do some UI updating
        captainBarManager.UpdateCaptainBar(); // Because otherwise it only updates after the first second.
        BridgeOfcManager.instance.UpdateAntimatterBar();
        UpdateStatsCorner();

        // Finally + 2, check for and if so open start screen.
        if (!data.unlockGameStart)
        {
            data.unlockGameStart = true;
            TextEventSystem.instance.NewGameEvent();
        }

        // Finally + 2, restart the delayed UI updating
        StartUpScreen.StartFadeOut(); // (This serves as a brief loading screen)
        GeneralUIController.instance.InitializeUI();

    }

    private void Update()
    {
        if (GamePaused) return;  // Simple pause

        // Increment the timers by the time elapsed since the last frame
        crewEnsignTimer += Time.deltaTime;
        oneTenthSecondTimer += Time.deltaTime;
        oneSecondTimer += Time.deltaTime;
        tenSecondTimer += Time.deltaTime;

        // Check if the timer has exceeded the interval
        if (crewEnsignTimer >= CrewManager.instance.crewEnsignClickInterval())
        {
            // Reset the timer
            crewEnsignTimer -= CrewManager.instance.crewEnsignClickInterval(); // Subtract interval to account for potential overflows

            // Code to execute once every crewEnsignInterval
            PerEnsignIntervalUpdate();
        }

        // Check if the timer has exceeded the interval
        if (oneTenthSecondTimer >= 0.1f)
        {
            // Reset the timer
            oneTenthSecondTimer -= 0.1f; // Subtract interval to account for potential overflows

            // Code to execute once every tenth of a second
            PerTenthSecondUpdate();
        }

        // Check if the timer has exceeded the interval
        if (oneSecondTimer >= 1f)
        {
            // Reset the timer
            oneSecondTimer -= 1f; // Subtract interval to account for potential overflows

            // Code to execute once every second
            PerSecondUpdate();
        }

        // Check if the timer has exceeded the interval
        if (tenSecondTimer >= 10f)
        {
            // Reset the timer
            tenSecondTimer -= 10f; // Subtract interval to account for potential overflows

            // Code to execute once every second
            PerTenSecondsUpdate();
        }

        UpdateStatsCorner();

        if (SaveLoadSystemActive)
        {
            SaveTime += Time.deltaTime * (1 / Time.timeScale); // This way apparantly guards against variable game speed
            if (SaveTime >= 5)
            {
                data.saveStateData = new SaveStateData(Controller.instance, BridgeOfcManager.instance);
                SaveLoadSystem.SaveData(data, dataFileName);
                SaveTime = 0;
            }
        }
    }

    public void UpdateStatsCorner()
    {
        totalPowerText.text = $"POWER: {data.totalPower}";
        clickPowerText.text = $"Click Power: {ClickPower()}";
        clickLimitText.text = $"Click Limit: {data.ClickLimit()}/s";
        storedClicksText.text = $"Stored Clicks: {data.clicksStored}";
    }

    private void PerEnsignIntervalUpdate()
    {
        BigDouble powerGain = ClickPower() * data.crewEnsigns;
        data.totalPower += powerGain;
        data.gainedPower += powerGain;
    }

    private void PerTenthSecondUpdate()
    {
        if (!data.unlockEngineers && data.totalPower >= 30)
        {
            data.unlockEngineers = true;
            GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.EngineerButtonFadeScript, true, false);
        }
        else if (!data.unlockDoctors && data.totalPower >= 1000)
        {
            data.unlockDoctors = true;
            GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.DoctorButtonFadeScript, true, false);
        }
        else if (!data.unlockScientists && data.totalPower >= 2500)
        {
            data.unlockScientists = true;
            GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.ScientistButtonFadeScript, true, false);
        }

        if (data.firstCareer)
        {
            if (!data.unlockFirstPromotion && data.totalPower >= 15_000)
            {
                data.unlockFirstPromotion = true;
                GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.tutorialPromotionButtonFadeScript, true, false);
            }
        }
        else 
        { 
            // Other unlocks go here, as they're not supposed to run for the first game
        }

        BridgeOfcManager.instance.PerTenthSecondUpdate();
    }
    private void PerSecondUpdate()
    {
        if (data.unlockAnomalies)
        {
            int anomalyChance = data.AnomalyChance();
            foreach (SpaceCollectibleHandler SpaceCollectible in arraySpaceCollectibles)
            {
                if (!SpaceCollectible.isActive)
                {
                    if (RollChance(anomalyChance)) { SpaceCollectible.StartAnomaly(); }
                }
            }
        }

        if (numberQueue.Count >= 10) // If the queue has 10 numbers
        {
            BigDouble oldestNumber = numberQueue.Dequeue(); // Remove the oldest number
            runningSum -= oldestNumber; // Subtract it from the running sum
        }

        BigDouble newNumber = data.gainedPower;
        data.gainedPower -= newNumber;
        numberQueue.Enqueue(newNumber); // Add the new number
        runningSum += newNumber; // Add it to the running sum

        BigDouble avarageNumber = GetAverage();

        if (avarageNumber >= 10)
        {
            powerPerSecondText.text = $"P/s (last 10s): {avarageNumber:F0}";
        }
        else if (avarageNumber >= 1)
        {
            powerPerSecondText.text = $"P/s (last 10s): {avarageNumber:F1}";
        }
        else {
            powerPerSecondText.text = $"P/s (last 10s): {avarageNumber:F2}";
        }

        if (timesClicked == 0 && data.unlockCaptainsClick)
        {
            if (data.clicksStored < data.ClicksStoredMax()) { data.clicksStored = BigDouble.Min(data.ClicksStoredMax(), data.clicksStored + data.ClickLimit()); }
            if (data.captainMultiplier < data.captainMultiplierMax) { data.captainMultiplier = (float)Math.Round(data.captainMultiplier + data.captainMultiplierGeneration, 2); }
            captainBarManager.UpdateCaptainBar();
        }

        timesClicked = 0;
        starshipButton.interactable = true;
    }
    private void PerTenSecondsUpdate()
    {
        // Debug.Log($"isActive {northwestSpaceCollectible.isActive}; isFadingIn {northwestSpaceCollectible.isFadingIn}; isFadingOut {northwestSpaceCollectible.isFadingOut}");
    }

    private BigDouble GetAverage()
    {
        if (numberQueue.Count == 0) return 0; // Avoid division by zero
        return (BigDouble)BigDouble.Divide(runningSum, numberQueue.Count); // Calculate and return the average
    }

    public BigDouble ClickPower() => 1 + data.crewEngineers;

    public void IncreasePower(BigDouble powerGain) 
    {
        powerGain = BigDouble.Floor(powerGain); // Remove any decimals, this is easiest to do here so it doesn't need to be done anywhere else
        data.totalPower += powerGain;
        data.gainedPower += powerGain;
    }

    public void IncreasePowerDirect(BigDouble powerGain)
    {
        powerGain = BigDouble.Floor(powerGain); // Remove any decimals, this is easiest to do here so it doesn't need to be done anywhere else
        data.totalPower += powerGain;
    }

    public void StarshipClick()
    {
        if (timesClicked < data.ClickLimit())
        {
            timesClicked++;
            IncreasePower(ClickPower() * (1 + data.clicksStored) * data.captainMultiplier);
            if (data.clicksStored > 0 || data.captainMultiplier > 1f)
            {
                clickEffectController.PlayEffectAtMouse(data.captainMultiplier);
                audioSource.PlayOneShot(clickSoundEffect); // Plays the sound once
                data.clicksStored = 0;
                data.captainMultiplier = 1f;
                captainBarManager.UpdateCaptainBar();
                if (!data.firstCaptainsClickEvent)
                {
                    data.firstCaptainsClickEvent = true;
                    UpdateStatsCorner();
                    TextEventSystem.instance.FirstCaptainsClickEvent();
                }
            }

            if (!data.unlockEnsigns && data.totalPower >= 5)
            {
                data.unlockEnsigns = true;
                GeneralUIController.instance.ActivateFadeInOutScript(GeneralUIController.instance.EnsignButtonFadeScript, true, false);
            }
        }
        if (timesClicked >= data.ClickLimit())
        {
            starshipButton.interactable = false;
        }
    }

    // Math Equations
    // BigDouble
    public BigDouble LinearBigDoubleEquation(BigDouble number) => 5 * number;
    public BigDouble SquareRootBigDoubleEquation(BigDouble number) => BigDouble.Sqrt(number);
    public BigDouble SquareRootBigDoubleEquationAlter(BigDouble number) => BigDouble.Pow(number, power: 0.5);
    public BigDouble PolynomialBigDoubleEquation(BigDouble number) => BigDouble.Pow(number, power: 2);
    public BigDouble ExponentialBigDoubleEquation2(BigDouble number) => BigDouble.Pow(value: 2, power: number);
    public BigDouble LogBigDoubleEquation2(BigDouble number) => BigDouble.Log(number, @base: 2); // Inverse of above
    public BigDouble LogBigDoubleEquation2Alter(BigDouble number) => BigDouble.Log2(number);
    public BigDouble ExponentialBigDoubleEquation10(BigDouble number) => BigDouble.Pow(value: 10, power: number);
    public BigDouble ExponentialBigDoubleEquation10Alter(double number) => BigDouble.Pow10(number);
    public BigDouble LogBigDoubleEquation10(BigDouble number) => BigDouble.Log(number, @base: 10); // Inverse of above
    public BigDouble LogBigDoubleEquation10Alter(BigDouble number) => BigDouble.Log10(number);

    // double
    public double LinearDoubleEquation(double number) => 5 * number;
    public double SquareRootDoubleEquation(double number) => Math.Sqrt(number);
    public double SquareRootDoubleEquationAlter(double number) => Math.Pow(number, 0.5);
    public double PolynomialDoubleEquation(double number) => Math.Pow(number, 2);
    public double ExponentialDoubleEquation2(double number) => Math.Pow(2, number);
    public double LogDoubleEquation2(double number) => Math.Log(number, 2); // Inverse of above
    public double ExponentialDoubleEquation10(double number) => Math.Pow(10, number);
    public double LogDoubleEquation10(double number) => Math.Log(number, 10); // Inverse of above
    public double LogDoubleEquation10Alter(double number) => Math.Log10(number);

    // Custom BigDouble Tools
    public static float BigDoubleToFloat(BigDouble bigDouble)
    {
        try
        {
            double value = bigDouble.Mantissa * Math.Pow(10, bigDouble.Exponent);

            if (float.IsPositiveInfinity((float)value))
            {
                Debug.LogError($"BigDouble value {bigDouble} is too large to fit in a float. Returning float.MaxValue.");
                return float.MaxValue;
            }
            if (float.IsNegativeInfinity((float)value))
            {
                Debug.LogError($"BigDouble value {bigDouble} is too small to fit in a float. Returning float.MinValue.");
                return float.MinValue;
            }

            return (float)value;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception while converting BigDouble to float: {ex.Message}");
            return 0f; // Returning a neutral value to prevent crashes
        }
    }

    // dice methods
    public bool RollChance(int chanceOutOf) // chanceOutOf: Total chances (e.g., 10 for 1-in-10)
    {
        int roll = UnityEngine.Random.Range(0, chanceOutOf); // Generates a number between 0 (inclusive) and chanceOutOf (exclusive)
        return roll == 0; // True if the roll is 0 (1-in-chanceOutOf odds)
    }
}
