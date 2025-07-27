using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;
    private void Awake() => instance = this;

    public TooltipWindow tooltip;

    public static void Show(string content, string header = "")
    {
        instance.tooltip.SetText(content, header);
        instance.tooltip.CalculatePivot();
        instance.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        if (instance.tooltip != null)
        { 
            instance.tooltip.gameObject.SetActive(false); 
        }
    }
}
