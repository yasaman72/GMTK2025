using UnityEngine;

namespace Deviloop
{
    public class MagnetCard : MonoBehaviour
    {
        [SerializeField] private BaseCard magentCard;
        [SerializeField] private MaterialData material;
        [SerializeField] private float attractionForce = 15f;
        [SerializeField] private float attractionRadius = 5f;
        [SerializeField] private float holdDistance = 1.5f;
        [SerializeField] private float damping = 5f;

        private void FixedUpdate()
        {
            foreach (CardPrefab card in CardManager.Instance.thrownCards)
            {
                if (card.CardData == magentCard)
                    continue;

                if (!material.CompareMaterials(card.CardData.materialType))
                    continue;

                Rigidbody2D rb = card.GetComponent<Rigidbody2D>();
                if (rb == null)
                    continue;

                Vector2 direction = (Vector2)transform.position - rb.position;
                float distance = direction.magnitude;

                if (distance > attractionRadius)
                    continue;

                if (distance > holdDistance)
                {
                    Vector2 forceDir = direction.normalized;

                    float strength = Mathf.Lerp(
                        0,
                        attractionForce,
                        (distance - holdDistance) / (attractionRadius - holdDistance)
                    );

                    rb.AddForce(forceDir * strength, ForceMode2D.Force);
                }
                else
                {
                    rb.linearVelocity = Vector2.Lerp(
                        rb.linearVelocity,
                        Vector2.zero,
                        damping * Time.fixedDeltaTime
                    );
                }
            }
        }

        private void OnDrawGizmos()
        {
            // Attraction radius
            Gizmos.color = new Color(0f, 0.5f, 1f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, attractionRadius);

            // Hold distance
            Gizmos.color = new Color(0f, 1f, 0f, 0.35f);
            Gizmos.DrawWireSphere(transform.position, holdDistance);

            // Center marker
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}
