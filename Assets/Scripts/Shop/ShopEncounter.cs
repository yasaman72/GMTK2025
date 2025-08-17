using UnityEngine;

public class ShopEncounter : BaseEncounter
{
    [SerializeField] private ShopData _shopData;

    private void OnEnable()
    {
        // TODO: this is just for debugging
        StartEncounter();
        ShopUI.OnShopClosedEvent += FinishEncounter;
    }

    private void OnDisable()
    {
        ShopUI.OnShopClosedEvent -= FinishEncounter;
    }

    public override void StartEncounter()
    {
        ShopUI.OpenShopAction?.Invoke(_shopData);
    }
    public override void FinishEncounter()
    {
        gameObject.SetActive(false);
    }
}
