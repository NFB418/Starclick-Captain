using UnityEngine;

public class StarshipButton : MonoBehaviour
{

    public RectTransform starshipButtonRectTransform; // Assign the RectTransform of the button to rotate

    private float defaultAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // The default position for the button is facing East, which is at 0 degrees.
        defaultAngle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (starshipButtonRectTransform.eulerAngles.z != defaultAngle)
        { 
            if (!Controller.instance.starshipTargeting)
            {
                RotateBackToDefaultPosition(starshipButtonRectTransform);
            }
        }
    }

    private void RotateBackToDefaultPosition(RectTransform objectToRotate)
    {
        // Get the current angle of the object
        float currentAngle = objectToRotate.eulerAngles.z;

        if (defaultAngle != currentAngle)
        {

            // Calculate the absolute difference between the current angle and the target angle
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentAngle, defaultAngle));

            // If the angle difference is small enough, snap to the target angle and stop rotating
            if (angleDifference <= 0.2f)
            {
                currentAngle = defaultAngle; // Snap to the target angle to stop further rotation
            }
            else
            {
                // Smoothly interpolate between the current angle and the target angle
                currentAngle = Mathf.LerpAngle(currentAngle, defaultAngle, Time.deltaTime * Controller.instance.rotationSpeed * 0.2f);
            }

            // Apply the new angle to the object
            objectToRotate.rotation = Quaternion.Euler(0, 0, currentAngle);
            // Debug.Log($"Rotating towards: {defaultAngle}, Angle Difference: {angleDifference}, New Angle: {currentAngle}°");
        }
    }

}
