using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AirTextController : MonoBehaviour
{
    private TextMeshProUGUI airText;
    private string startingAirText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        airText = GetComponent<TextMeshProUGUI>();
        startingAirText = airText.text;
    }

    public void SetInvincibilityText(float seconds)
    {
        StartCoroutine(InvincibilityText(seconds));
    }

    private IEnumerator InvincibilityText(float seconds)
    {
        airText.text = "Invincibility Active: " + (int)Math.Truncate(seconds) + "s";
        while (seconds > 0)
        {
            yield return new WaitForSeconds(1);
            seconds--;
            airText.text = "Invincibility Active: " + (int)Math.Truncate(seconds) + "s";
        }
        airText.text = startingAirText;
    }
}
