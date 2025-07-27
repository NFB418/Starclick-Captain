using UnityEngine;
using TMPro;

public class FadeAndMoveText : MonoBehaviour
{
    // Public variables to control the behavior
    public bool shouldMove = false;
    private readonly float moveSpeed = 20f; // Speed at which the text moves upward
    private readonly float fadeSpeed = 1f; // Speed at which the text fades

    private TextMeshProUGUI textMesh;
    private Color textColor;

    void Start()
    {
        // Get the TextMeshPro component
        textMesh = GetComponent<TextMeshProUGUI>();
        if (textMesh == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the object.");
            enabled = false;
            return;
        }

        // Initialize the text color
        textColor = textMesh.color;
    }

    void Update()
    {
        if (shouldMove)
        {
            // Increase the Pos Y value
            Vector3 newPosition = transform.localPosition;
            newPosition.y += moveSpeed * Time.deltaTime;
            transform.localPosition = newPosition;

            // Gradually reduce the alpha (transparency) of the text
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            // If the text is fully transparent, destroy the object
            if (textColor.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
