using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;

[Serializable]
public class Data
{
    public BigDouble totalPower;
    public BigDouble gainedPower;
    public BigDouble baseClickLimit;

    public BigDouble clicksStored;
    public float captainMultiplier;

    public BigDouble antimatterStored;

    public bool unlockEnsigns;
    public BigDouble crewEnsigns;
    public bool hiredFirstEnsign;

    public bool unlockEngineers;
    public BigDouble crewEngineers;
    public bool hiredFirstEngineer;

    public bool unlockDoctors;
    public BigDouble crewDoctors;
    public bool hiredFirstDoctor;

    public bool unlockScientists;
    public BigDouble crewScientists;
    public bool hiredFirstScientist;

    public bool firstCareer;
    public bool unlockFirstPromotion;
    public BigDouble firstPromotionCost;
    public bool achievedFirstPromotion;

    public bool unlockSecurityOfc;
    public BigDouble crewSecurityOfc;
    public bool hiredFirstSecurityOfc;

    public bool unlockCommanders;
    public BigDouble crewCommanders;
    public bool hiredFirstCommander;

    public bool unlockMerchants;
    public BigDouble crewMerchants;
    public bool hiredFirstMerchant;

    public bool unlockDiplomats;
    public BigDouble crewDiplomats;
    public bool hiredFirstDiplomat;

    public bool Officer_00_Empty_Unlockable;
    public bool Officer_00_Empty_Unlocked;

    public bool Officer_01_RI_Unlockable;
    public bool Officer_01_RI_Unlocked;

    public bool Officer_02_TP_Unlockable;
    public bool Officer_02_TP_Unlocked;

    public bool Officer_03_PG_Unlockable;
    public bool Officer_03_PG_Unlocked;

    public bool Officer_04_DI_Unlockable;
    public bool Officer_04_DI_Unlocked;

    public bool unlockGameStart;
    public bool unlockAnomalies;
    public bool unlockCaptainsClick;
    public bool firstCaptainsClickEvent;

    public double baseAnomalyChance;

    public BigDouble baseClicksStoredMaxSeconds;
    public float captainMultiplierGeneration;
    public float captainMultiplierMax;

    public SaveStateData saveStateData; // A new version of this should be made before every save.
    private readonly bool debug_mode = false;

    public Data() 
    { 
        totalPower = 0;
        gainedPower = 0;
        baseClickLimit = 5;

        clicksStored = 0;
        captainMultiplier = 1f;

        antimatterStored = 0;

        unlockEnsigns = false;
        crewEnsigns = 0;
        hiredFirstEnsign = false;

        unlockEngineers = false;
        crewEngineers = 0;
        hiredFirstEngineer = false;

        unlockDoctors = false;
        crewDoctors = 0;
        hiredFirstDoctor = false;

        unlockScientists = false;
        crewScientists = 0;
        hiredFirstScientist = false;

        firstCareer = true;
        unlockFirstPromotion = false;
        firstPromotionCost = 100_000;
        achievedFirstPromotion = false;

        unlockGameStart = false;

        unlockAnomalies = false;

        unlockCaptainsClick = false;
        firstCaptainsClickEvent = false;
        baseClicksStoredMaxSeconds = 300;
        captainMultiplierGeneration = 0.01f;
        captainMultiplierMax = 1f + 1f;

        Officer_00_Empty_Unlockable = true; // Should NEVER be set to false
        Officer_00_Empty_Unlocked = true; // Should NEVER be set to false

        Officer_01_RI_Unlockable = false;
        Officer_01_RI_Unlocked = false;

        Officer_02_TP_Unlockable = false;
        Officer_02_TP_Unlocked = false;

        Officer_03_PG_Unlockable = false;
        Officer_03_PG_Unlocked = false;

        Officer_04_DI_Unlockable = false;
        Officer_04_DI_Unlocked = false;

        baseAnomalyChance = 450;  // Checked every 1 seconds, 900 equals 1 per 15 minutes (which spread across eight anomalies is one every two minutes)

        if (debug_mode)
        {
            // Here set stats meant to unlock and speed up progression for quick testing of new mechanics
            totalPower = 200_000;

            unlockEnsigns = true;
            unlockEngineers = true;
            unlockDoctors = true;
            unlockScientists = true;

            hiredFirstEnsign = true;
            hiredFirstEngineer = true;
            hiredFirstDoctor = true;
            hiredFirstScientist = true;

            crewScientists = 10;

            unlockAnomalies = true;
            unlockCaptainsClick = true;

            baseAnomalyChance = 8;
        }
    }

    public BigDouble ClickLimit() => baseClickLimit * BridgeOfcManager.instance.ClickLimitMultiplier;
    public BigDouble ClicksStoredMaxSeconds() => baseClicksStoredMaxSeconds;
    public BigDouble ClicksStoredMax() => ClicksStoredMaxSeconds() * ClickLimit();
    public int AnomalyChance() => (int)(baseAnomalyChance * BridgeOfcManager.instance.AnomalyChanceMultiplier);
}
