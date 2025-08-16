using Cards;
using UnityEngine;

public class CardDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var CardPrefab = collision.gameObject.GetComponent<CardPrefab>();
        if (CardPrefab)
        {
            CardPrefab.OnCardDroppedOut();
        }
    }
}
