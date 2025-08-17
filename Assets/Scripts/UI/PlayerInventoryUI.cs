using System;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    // TODO: use scriptable events for UI updates
    public static Action Update;

    [SerializeField] private TMPro.TextMeshProUGUI _coinAmountText;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        Update += UpdateUI;
        UpdateUI();
    }

    private void OnDestroy()
    {
        Update -= UpdateUI;
    }

    private void UpdateUI()
    {
        int coinCount = PlayerInventory.CoinCount;
        _coinAmountText.text = coinCount.ToString();
    }
}
