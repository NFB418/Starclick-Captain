using System;
using UnityEngine;
using UnityEngine.UI;

public class SimpleHealthBarManager : MonoBehaviour
{
    public Image healthBar;
    public float healthMax;
    public float healthAmount;

    public bool isDepleted;

    private CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        healthMax = 100f;
        healthAmount = 0f;
        isDepleted = true;

        // Optionally use CanvasGroup for simpler fading if the entire object uses UI
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            // Add a CanvasGroup dynamically if it doesn't exist
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void ResetHealthBar()
    {
        healthAmount = healthMax;
        isDepleted = false;

        canvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        Math.Clamp(healthAmount, 0, healthMax);
        healthBar.fillAmount = healthAmount / healthMax;

        if (healthAmount == healthMax) { canvasGroup.alpha = 0f; } else { canvasGroup.alpha = 1f; }
        if (healthAmount <= 0) { isDepleted = true; canvasGroup.alpha = 0f; } else { isDepleted = false; }
    }

    public void GetHealed(float healing)
    {
        healthAmount += healing;
        Math.Clamp(healthAmount, 0, healthMax);
        healthBar.fillAmount = healthAmount / healthMax;

        if (healthAmount == healthMax) { canvasGroup.alpha = 0f; } else { canvasGroup.alpha = 1f; }
        if (healthAmount <= 0) { isDepleted = true; canvasGroup.alpha = 0f; } else { isDepleted = false; }
    }
}
