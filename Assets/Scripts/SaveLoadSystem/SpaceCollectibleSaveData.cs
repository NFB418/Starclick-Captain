using System;

[Serializable]
public class SpaceCollectibleSaveData
{
    private bool isActive;
    private bool isFadingOut;
    private bool isFadingIn;
    private float canvasGroupAlpha;
    private float healthAmount;

    public SpaceCollectibleSaveData(SpaceCollectibleHandler spaceCollectibleHandler)
    {
        if (!spaceCollectibleHandler.isActive) { isActive = false; return; } // Because currently, other values aren't used for inactive anomalies

        isActive = spaceCollectibleHandler.isActive;
        isFadingIn = spaceCollectibleHandler.isFadingIn;
        isFadingOut = spaceCollectibleHandler.isFadingOut;
        canvasGroupAlpha = spaceCollectibleHandler.canvasGroup.alpha;
        healthAmount = spaceCollectibleHandler.healthBarManager.healthAmount;
    }

    public void LoadIntoCollectible(SpaceCollectibleHandler spaceCollectibleHandler)
    {
        if (spaceCollectibleHandler.beamActivated) { spaceCollectibleHandler.DeactivateBeam(); } // Just always deactivate beam.
        if (!isActive) { spaceCollectibleHandler.Restart(); return; } // Inactive ones can just be restarted

        if (isActive)
        {
            spaceCollectibleHandler.isActive = true;
            spaceCollectibleHandler.damageTimer = 0f; // No need to preserve this

            spaceCollectibleHandler.isFadingOut = isFadingOut;
            spaceCollectibleHandler.canvasGroup.alpha = canvasGroupAlpha;

            if (!isFadingOut)
            {
                spaceCollectibleHandler.isFadingIn = isFadingIn;
                spaceCollectibleHandler.collectibleButton.interactable = true;
                spaceCollectibleHandler.buttonTooltip.EnableTooltip();
                spaceCollectibleHandler.buttonHold.EnableHold();
            }
            else
            {
                spaceCollectibleHandler.isFadingIn = false;
                spaceCollectibleHandler.collectibleButton.interactable = false;
                spaceCollectibleHandler.buttonTooltip.DisableTooltip();
                spaceCollectibleHandler.buttonHold.DisableHold();
            }

            spaceCollectibleHandler.healthBarManager.healthAmount = healthAmount;
            spaceCollectibleHandler.healthBarManager.TakeDamage(0f); // Doing 0 damage to make it set its internal values appropriately
        }
    }
}