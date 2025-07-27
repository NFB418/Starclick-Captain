using System;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;

public class CaptainBarManager : MonoBehaviour
{
    public Image CapClickBarFill;
    public Animator EnergyPointAnimator;
    public TooltipTrigger captainBarTooltip;

    public void UpdateCaptainBar()
    {
        BigDouble fill_amount_raw = BigDouble.Divide(Controller.instance.data.clicksStored, Controller.instance.data.ClicksStoredMax());
        // Must convert this BigDouble to a float. THIS WILL NOT WORK PROPERLY IF fill_amount_raw IS TOO BIG/SMALL TO BE PROPERLY CONVERTED
        float fill_amount = Controller.BigDoubleToFloat(fill_amount_raw);
        // Debug.Log($"{Controller.instance.data.clicksStored} / {Controller.instance.data.ClicksStoredMax()} = {fill_amount}");
        
        CapClickBarFill.fillAmount = fill_amount;

        if (Controller.instance.data.captainMultiplier >= Controller.instance.data.captainMultiplierMax)
        {
            EnergyPointAnimator.SetBool("MaxMultiplier", true);
        }
        else
        {
            EnergyPointAnimator.SetBool("MaxMultiplier", false);
        }

        captainBarTooltip.content = $"Each second where you have NOT <b>clicked</b> the <b>ship</b>, your <b>captain's click</b> stores a number of <b>clicks</b> equal to your <b>click limit</b> AND increases your <b>captain's click charge</b> (up to both values' maximum storage). " +
            $"The next time you <b>click</b> the <b>ship</b>, you will automatically spend everything you've built up to gain your stored <b>clicks'</b> worth of <b>click power</b> multiplied by your <b>captain's click charge</b>.\r\n\r\n" +
            $"Stored Clicks: {Controller.instance.data.clicksStored} / {Controller.instance.data.ClicksStoredMax()} \r\n" +
            $"Captain's Click Charge: {Controller.instance.data.captainMultiplier} / {Controller.instance.data.captainMultiplierMax}";

        if (captainBarTooltip.isTooltipActive)
        {
            TooltipSystem.instance.tooltip.SetText(captainBarTooltip.content, captainBarTooltip.header);
        }

    }
}
