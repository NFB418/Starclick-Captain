using UnityEngine;
using UnityEngine.EventSystems; // Required for Pointer events

public class ButtonHoldHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    
    public bool isHeld; // To track the button hold state
    private bool holdIsEnabled; // To disable this feature

    private void Start()
    {
        isHeld = false;
        holdIsEnabled = false;
    }

    public void EnableHold() { holdIsEnabled = true; }
    public void DisableHold() { holdIsEnabled = false; isHeld = false; Controller.instance.starshipTargeting = false; }

    // IPointerDownHandler implementation
    public void OnPointerDown(PointerEventData eventData)
    {
        if (holdIsEnabled && Controller.instance.inGameUI.interactable)
        {
            Controller.instance.starshipTargeting = true;
            isHeld = true;
        }
    }

    // IPointerUpHandler implementation
    public void OnPointerUp(PointerEventData eventData)
    {
        if (holdIsEnabled)
        {
            isHeld = false;
            Controller.instance.starshipTargeting = false;
        }
    }
}
