using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    // Key to save and load currency data
    private const string currencyKey = "Currency";
    public TextMeshProUGUI currency;

    // Current currency amount
    public int currencyAmount;

    void Start()
    {
        // Load currency data from PlayerPrefs
        LoadCurrency();
    }

    private void Update()
    {
        currency.text = $"${GetCurrency()}";
    }

    void OnDestroy()
    {
        // Save currency data to PlayerPrefs when the game object is destroyed
        SaveCurrency();
    }

    public void AddCurrency(int amount)
    {
        // Add currency and save the updated amount
        currencyAmount += amount;
        SaveCurrency();
    }

    public bool SpendCurrency(int amount)
    {
        // Check if there is enough currency to spend
        if (currencyAmount >= amount)
        {
            // Subtract currency and save the updated amount
            currencyAmount -= amount;
            SaveCurrency();
            return true; // Currency spent successfully
        }
        else
        {
            return false; // Insufficient currency
        }
    }

    public int GetCurrency()
    {
        // Return the current currency amount
        return currencyAmount;
    }

    private void SaveCurrency()
    {
        // Save currency data to PlayerPrefs
        PlayerPrefs.SetInt(currencyKey, currencyAmount);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        // Load currency data from PlayerPrefs
        currencyAmount = PlayerPrefs.GetInt(currencyKey, 0);
    }
}
