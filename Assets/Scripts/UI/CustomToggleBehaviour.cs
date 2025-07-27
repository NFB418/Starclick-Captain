using UnityEngine;
using TMPro; // Ensure TextMeshPro is imported
using UnityEngine.UI; // Required for Toggle functionality

public class CustomToggleBehavior : MonoBehaviour
{
    [Header("References")]
    public Toggle toggle; // Reference to the Toggle component
    public TextMeshProUGUI toggleText; // The primary "Toggle Text" object
    public TextMeshProUGUI toggleGlow; // The "Toggle Glow" object

    private void Awake()
    {
        // Ensure all references are assigned
        if (toggle == null || toggleText == null || toggleGlow == null)
        {
            Debug.LogError("CustomToggleBehavior: All references must be assigned in the inspector.");
            return;
        }

        // Subscribe to the toggle's value change event
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        // Initialize the toggle state and sync text
        UpdateGlowState(toggle.isOn);
        SyncGlowText();
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool isOn)
    {
        // Update the glow visibility when the toggle state changes
        UpdateGlowState(isOn);
    }

    private void UpdateGlowState(bool isOn)
    {
        // Show or hide the "Toggle Glow" based on the toggle state
        toggleGlow.gameObject.SetActive(isOn);
    }

    private void SyncGlowText()
    {
        // Sync the text of "Toggle Glow" with "Toggle Text"
        toggleGlow.text = toggleText.text;
    }

}
