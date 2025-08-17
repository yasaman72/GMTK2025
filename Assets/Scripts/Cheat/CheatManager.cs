using UnityEngine;

public class CheatManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            AddCoin(200);
        }
    }

    private void AddCoin(int amount)
    {
        PlayerInventory.CoinCount += amount;
    }
}
