using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TooltipWindow : MonoBehaviour
{

    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    public LayoutElement layoutElement;

    public RectTransform rectTransform;
    private Vector2 pivot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        layoutElement.enabled = layoutElement.enabled = Math.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElement.preferredWidth;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 position = Input.mousePosition;
        rectTransform.pivot = pivot;
        transform.position = position;
    }

    public void CalculatePivot()
    {
        Vector3 position = Input.mousePosition;
        Vector2 normalizedPosition = new(position.x / Screen.width, position.y / Screen.height);

        if (normalizedPosition.x < 0.5f && normalizedPosition.y >= 0.5f)
        {
            pivot = new Vector2(-0.05f, 1.05f); // pivotTopLeft;
        }
        else if (normalizedPosition.x > 0.5f && normalizedPosition.y >= 0.5f)
        {
            pivot = new Vector2(1.05f, 1.05f); // pivotTopRight;
        }
        else if (normalizedPosition.x <= 0.5f && normalizedPosition.y < 0.5f)
        {
            pivot = new Vector2(-0.05f, -0.05f); // pivotBottomLeft;
        }
        else
        {
            pivot = new Vector2(1.05f, -0.05f); // pivotBottomRight;
        }

        rectTransform.pivot = pivot;
        transform.position = position;
    }
}
