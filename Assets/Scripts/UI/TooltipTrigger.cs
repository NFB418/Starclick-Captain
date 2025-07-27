using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr delay;
    public float hoverDelay = 0.5f;
    public string header;

    [TextArea(3, 5)]
    public string content;

    public bool isTooltipActive = false; // Track if the tooltip is active
    private bool isTooltipEnabled = true;

    public void EnableTooltip()
    {
        isTooltipEnabled = true;
    }

    public void DisableTooltip()
    {
        CancelTooltip();
        isTooltipEnabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isTooltipEnabled) { return; }

        delay = LeanTween.delayedCall(hoverDelay, () =>
        {
            TooltipSystem.Show(content, header);
            isTooltipActive = true; // Mark tooltip as active
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelTooltip();
    }

    private void Update()
    {
        // Hide the tooltip if the player clicks the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (isTooltipActive) { CancelTooltip(); }
            else if (delay != null) { LeanTween.cancel(delay.uniqueId); }
        }
    }

    public void CancelTooltip()
    {
        // Cancel any delayed calls and hide the tooltip
        if (delay != null)
        {
            LeanTween.cancel(delay.uniqueId);
        }
        TooltipSystem.Hide();
        isTooltipActive = false; // Mark tooltip as inactive
    }
}
