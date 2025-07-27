using UnityEngine;

public class GeneralUIController : MonoBehaviour
{
    public static GeneralUIController instance;
    private void Awake() => instance = this;

    public FadeInOutScript FireHireTogglesFadeScript;
    public FadeInOutScript EnsignButtonFadeScript;
    public FadeInOutScript EngineerButtonFadeScript;
    public FadeInOutScript DoctorButtonFadeScript;
    public FadeInOutScript ScientistButtonFadeScript;
    public FadeInOutScript tutorialPromotionButtonFadeScript;
    public FadeInOutScript SecurityOfcButtonFadeScript;
    public FadeInOutScript CommanderButtonFadeScript;
    public FadeInOutScript MerchantButtonFadeScript;
    public FadeInOutScript DiplomatButtonFadeScript;

    public FadeInOutScript StoredClicksTextFadeScript;
    public FadeInOutScript TheCaptainFadeScript;
    public FadeInOutScript BridgeOfcUnlockFadeScript;
    public FadeInOutScript FirstOfcUnlockFadeScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void InitializeUI()
    {
        // This should be run whenever Data has been created or loaded, it sets UI elements according to their unlock status.

        Data data = Controller.instance.data; // for ease of reference

        if (data.unlockEnsigns) { EnsignButtonFadeScript.InstantEnable(); } else { EnsignButtonFadeScript.InstantDisable(); }
        if (data.unlockEngineers) { EngineerButtonFadeScript.InstantEnable(); } else { EngineerButtonFadeScript.InstantDisable(); }
        if (data.unlockDoctors) { DoctorButtonFadeScript.InstantEnable(); } else { DoctorButtonFadeScript.InstantDisable(); }
        if (data.unlockScientists) { ScientistButtonFadeScript.InstantEnable(); } else { ScientistButtonFadeScript.InstantDisable(); }
        if (data.unlockSecurityOfc) { SecurityOfcButtonFadeScript.InstantEnable(); } else { SecurityOfcButtonFadeScript.InstantDisable(); }
        if (data.unlockCommanders) { CommanderButtonFadeScript.InstantEnable(); } else { CommanderButtonFadeScript.InstantDisable(); }
        if (data.unlockMerchants) { MerchantButtonFadeScript.InstantEnable(); } else { MerchantButtonFadeScript.InstantDisable(); }
        if (data.unlockDiplomats) { DiplomatButtonFadeScript.InstantEnable(); } else { DiplomatButtonFadeScript.InstantDisable(); }

        if (data.hiredFirstEnsign) { FireHireTogglesFadeScript.InstantEnable();} else { FireHireTogglesFadeScript.InstantDisable(); }

        // Below are meant to also check if the delayed unlock effect got interrupted by a save/reload, and if so restart the delayed effect.
        if (data.hiredFirstEngineer)
        {
            if (!data.unlockAnomalies && !Controller.instance.eastSpaceCollectible.isActive)
            {
                CrewManager.instance.StartCoroutine(CrewManager.instance.StartEastAnomalyAfterDelay(10f));
            }
        }

        if (!data.unlockCaptainsClick)
        {
            StoredClicksTextFadeScript.InstantDisable();
            TheCaptainFadeScript.InstantDisable();

            if (data.hiredFirstDoctor)
            {
                CrewManager.instance.StartCoroutine(CrewManager.instance.UnlockCaptainsClickAfterDelay(5f));
            }
        }
        else 
        {
            StoredClicksTextFadeScript.InstantEnable();
            TheCaptainFadeScript.InstantEnable();
        }

        if (data.hiredFirstScientist) { BridgeOfcUnlockFadeScript.InstantEnable(); } else { BridgeOfcUnlockFadeScript.InstantDisable(); }
        if (data.firstCareer && data.unlockFirstPromotion) { tutorialPromotionButtonFadeScript.InstantEnable(); } else { tutorialPromotionButtonFadeScript.InstantDisable(); }
    }

    // I am using below method to centralise UI references here. Use GeneralUIController.instance.[FadeInOutScript] as the first argument.
    public void ActivateFadeInOutScript(FadeInOutScript fadeInOutScript, bool Enable, bool IsInstant)
    {
        if (Enable) { if (IsInstant) { fadeInOutScript.InstantEnable(); } else { fadeInOutScript.StartFadeIn(); } }
        else { if (IsInstant) { fadeInOutScript.InstantDisable(); } else { fadeInOutScript.StartFadeOut(); } }
    }
}
