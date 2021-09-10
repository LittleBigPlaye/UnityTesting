using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledBarController : MonoBehaviour
{
    public Image background;
    public Image foreground;
    public Image delayedForeground;
    private float maxValue = 100f;
    public float MaxValue
    {
        get; set;
    }


    private float currentValue = 50f;
    public float CurrentValue
    {
        get => currentValue;
        set
        {
            if (currentValue > value)
            {
                currentDelay = delay;
            }
            currentValue = Mathf.Clamp(value, 0, maxValue);
        }
    }

    public float delayedSpeed = .5f;
    public float foregroundSpeed = .9f;
    public float backgroundSpeed = 1f;

    public float delay = .5f;
    private float foregroundVelocity;
    private float backgroundVelocity;
    private float currentDelay = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (delayedForeground != null && currentDelay <= 0)
        {
            UpdateBackgroundBar();

        }
        UpdateForegroundBar();
        currentDelay = Mathf.Max(0, currentDelay - Time.deltaTime);
    }

    public void InitializeBar(float maxValue)
    {
        this.maxValue = maxValue;
        this.currentValue = maxValue;
        foreground.fillAmount = 1;
        delayedForeground.fillAmount = 1;
    }

    private void UpdateForegroundBar()
    {
        float currentPercentage = GetCurrentPercentage();
        if (foreground.fillAmount > currentPercentage)
        {
            foreground.fillAmount = currentPercentage;
        }
        else if (foreground.fillAmount < currentPercentage)
        {
            foreground.fillAmount = Mathf.SmoothDamp(delayedForeground.fillAmount, currentPercentage, ref foregroundVelocity, foregroundSpeed);
        }
    }

    private void UpdateBackgroundBar()
    {
        float currentPercentage = GetCurrentPercentage();
        if (delayedForeground.fillAmount > currentPercentage)
        {
            delayedForeground.fillAmount = Mathf.SmoothDamp(delayedForeground.fillAmount, currentPercentage, ref backgroundVelocity, delayedSpeed);
        }
        else if (delayedForeground.fillAmount < currentValue)
        {
            delayedForeground.fillAmount = foreground.fillAmount;
        }
    }

    //converts a given value to a float between 0 and 1 based on the max value
    private float GetCurrentPercentage()
    {
        return currentValue / maxValue;
    }
}