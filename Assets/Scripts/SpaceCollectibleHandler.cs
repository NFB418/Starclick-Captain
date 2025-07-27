using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpaceCollectibleHandler : MonoBehaviour
{
    public Button collectibleButton; // Assign your button in the Inspector
    public TooltipTrigger buttonTooltip;
    public ButtonHoldHandler buttonHold; // Handles the button's held state and click events
    public RectTransform starshipRectTransform; // Assign the RectTransform of the button to rotate
    public BeamScript beamScript; // BeamSprite Object
    public RectTransform beamTarget; // target for BeamSprite to fire at
    public SimpleHealthBarManager healthBarManager; // Collectible's healthBarManager
    public TextMeshProUGUI fadingTextPrefab;

    private float beamActivationThreshold = 10f; // Degrees within which the beam will activate
    [HideInInspector] public bool beamActivated;

    [HideInInspector] public CanvasGroup canvasGroup;
    private readonly float fadeInSpeed = 0.1f; // Speed at which the collectible fades in
    private readonly float fadeOutSpeed = 1f; // Speed at which the collectible fades out
    [HideInInspector] public float damageTimer;  // to track intervals between damage
    [HideInInspector] public bool isActive; // should be on from the start of FadeIn to the end of FadeOut
    [HideInInspector] public bool isFadingOut;
    [HideInInspector] public bool isFadingIn;

    public void Initialize()
    {
        beamActivated = false;

        damageTimer = 0f;
        isActive = false;
        isFadingIn = false;
        isFadingOut = false;
        buttonTooltip.DisableTooltip();

        // Optionally use CanvasGroup for simpler fading if the entire object uses UI
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            // Add a CanvasGroup dynamically if it doesn't exist
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
    }

    public void Restart()
    {
        if (beamActivated) { DeactivateBeam(); } // Also sets beamActivated = false;

        damageTimer = 0f;
        isActive = false;
        isFadingIn = false;
        isFadingOut = false;
        collectibleButton.interactable = false;
        buttonTooltip.DisableTooltip();
        buttonHold.DisableHold();
        healthBarManager.ResetHealthBar();

        canvasGroup.alpha = 0f;
    }

    public void StartAnomaly()
    {
        isActive = true;
        isFadingIn = true;
        isFadingOut = false;
        collectibleButton.interactable = true;
        buttonTooltip.EnableTooltip();
        buttonHold.EnableHold();
        healthBarManager.ResetHealthBar();
    }

    public void EndAnomaly()
    {
        collectibleButton.interactable = false;
        buttonTooltip.DisableTooltip();
        buttonHold.DisableHold();
        if (beamActivated) { DeactivateBeam(); } // Deactivate the beam
        damageTimer = 0f;
        isFadingIn = false;
        isFadingOut = true;

        if (!Controller.instance.data.unlockAnomalies)
        {
            Controller.instance.data.unlockAnomalies = true;
            TextEventSystem.instance.FirstAnomalyEvent();
        }
        
    }

    private void Update()
    {
        if (!isFadingOut)
        {
            if (isFadingIn)
            {
                if (canvasGroup.alpha < 1f) { canvasGroup.alpha += fadeInSpeed * Time.deltaTime; } else { isFadingIn = false; }
            }
            if (buttonHold.isHeld)
            {
                RunWhenHeld(); // Continuously call this function while the button is held
            }
            else if (beamActivated) { DeactivateBeam(); } // Else if no longer being held, deactivate the beam if it was activated.

        }
        else if (isFadingOut)
        {
            if (canvasGroup.alpha > 0) { canvasGroup.alpha -= fadeOutSpeed * Time.deltaTime; } else { isFadingOut = false; isActive = false; }
        }
    }

    // This is the function to be called repeatedly while the button is held
    private void RunWhenHeld()
    {
        if (buttonTooltip.isTooltipActive) { buttonTooltip.CancelTooltip(); }
        RotateTowards(collectibleButton.transform.position, starshipRectTransform);
        // Replace this with your desired functionality
        // Debug.Log("Button is being held!");
    }

    // Rotate the rotatingButton to face the position of targetPosition
    private void RotateTowards(Vector3 targetPosition, RectTransform objectToRotate)
    {
        // Get the direction to the target in 2D (X, Y plane only)
        Vector3 direction = targetPosition - objectToRotate.position;
        direction.z = 0; // Ignore Z since it's irrelevant here

        // Calculate the target angle in degrees
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Get the current angle of the object
        float currentAngle = objectToRotate.eulerAngles.z;

        float angleDifference = 0f;

        if (targetAngle != currentAngle)
        {

            // Calculate the absolute difference between the current angle and the target angle
            angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

            // If the angle difference is small enough, snap to the target angle and stop rotating
            if (angleDifference <= 0.1f)
            {
                currentAngle = targetAngle; // Snap to the target angle to stop further rotation
            }
            else
            {
                // Smoothly interpolate between the current angle and the target angle
                currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * Controller.instance.rotationSpeed);
            }

            // Apply the new angle to the object
            objectToRotate.rotation = Quaternion.Euler(0, 0, currentAngle);
            // Debug.Log($"Rotating towards: {targetPosition}, Angle Difference: {angleDifference}, New Angle: {currentAngle}°");
        }

        // Check if the angle is within the beam activation threshold
        // Debug.Log($"Angle Difference: {angleDifference}, beamActivationThreshold: {beamActivationThreshold}°");
        if (angleDifference <= beamActivationThreshold)
        {
            ActivateBeam(objectToRotate.position, targetPosition); // Activate the beam

            // Check if the timer has exceeded the interval
            damageTimer += Time.deltaTime;
            if (damageTimer >= 0.1f)
            {
                // Reset the timer
                damageTimer -= 0.1f; // Subtract interval to account for potential overflows
                if (beamScript.IsLooping())
                {

                    healthBarManager.TakeDamage(4f);
                    BigDouble powerGain = Controller.instance.ClickPower() * Controller.instance.data.ClickLimit() * 4; // x10 faster than just clicking
                    Controller.instance.IncreasePower(powerGain);

                    // Instantiate the prefab
                    TextMeshProUGUI fadingText = Instantiate(fadingTextPrefab) as TextMeshProUGUI;

                    fadingText.text = $"+{powerGain}";

                    // Get the RectTransform of the parent to determine its bounds
                    if (this.TryGetComponent<RectTransform>(out var parentRect))
                    {
                        // Calculate random X and Y within the bounds of the parent
                        float randomX = Random.Range(-parentRect.rect.width / 2, parentRect.rect.width / 2);
                        float randomY = Random.Range(-parentRect.rect.height / 2, parentRect.rect.height / 2);

                        // Set the position of the new object
                        fadingText.transform.SetParent(this.transform, false);
                        fadingText.transform.localPosition = new Vector2(randomX, randomY);
                    }
                    else
                    {
                        Debug.LogWarning("Parent object does not have a RectTransform. Defaulting to center position.");
                        fadingText.transform.SetParent(this.transform, false);
                        fadingText.transform.localPosition = Vector2.zero;
                    }

                    // Get the FadeAndMoveText component from the new instance and set shouldMove to true
                    if (fadingText.TryGetComponent<FadeAndMoveText>(out var fadeAndMoveScript))
                    {
                        fadeAndMoveScript.shouldMove = true;
                    }

                    if (healthBarManager.isDepleted)
                    {
                        EndAnomaly();
                    }
                }
                else
                {
                    damageTimer = 0f;
                }
            }
        }
        else
        {
            DeactivateBeam(); // Deactivate the beam if angle is too far
            damageTimer = 0f;
        }

    }

    // Activate the beam effect
    public void ActivateBeam(Vector3 startPosition, Vector3 endPosition)
    {
        beamActivated = true;
        beamScript.ActivateBeam(beamTarget.position);
    }

    // Deactivate the beam effect
    public void DeactivateBeam()
    {
        beamActivated = false;
        beamScript.DeactivateBeam();
    }

}