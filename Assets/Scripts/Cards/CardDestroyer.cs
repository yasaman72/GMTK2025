using Cards;
using UnityEngine;

public class CardDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<CardPrefab>())
        {
            Destroy(collision.gameObject);
        }
    }
}
