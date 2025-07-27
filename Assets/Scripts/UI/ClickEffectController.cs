using UnityEngine;

public class ClickEffectController : MonoBehaviour
{
    public RectTransform effectObject;  // Assign the UI effect object in Inspector
    private Animator animator;

    private void Start()
    {
        animator = effectObject.GetComponent<Animator>();
    }

    public void PlayEffectAtMouse(float scale)
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            effectObject.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePos
        );

        effectObject.anchoredPosition = mousePos; // Move effect to mouse
        effectObject.localScale = new Vector3(scale, scale, scale); // Set scale dynamically

        animator.Play("SingleClick", 0, 0); // Play animation from start
    }
}
