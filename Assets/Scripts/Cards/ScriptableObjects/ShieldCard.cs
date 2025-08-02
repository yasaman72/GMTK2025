using System;
using System.Collections;
using UnityEngine;

namespace Cards.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShieldCard", menuName = "Cards/Shield Card")]
    public class ShieldCard : BaseCard
    {
        [Header("Shield Properties")]
        public int shieldAmount = 1;
        public float moveSpeed = 1f;

        public override void UseCard(MonoBehaviour runner, Action callBack, CardPrefab cardPrefab)
        {
            Debug.Log($"Shield provides {shieldAmount} protection");
            runner.StartCoroutine(ActivateCardEffect(callBack, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.3f);

            Transform playerHpPos = FindAnyObjectByType<PlayerManager>()._playerHpOrigin;

            while (Vector2.Distance(cardPrefab.transform.position, playerHpPos.position) > 1)
            {
                cardPrefab.transform.position = Vector2.MoveTowards(
                    cardPrefab.transform.position,
                    playerHpPos.position,
                    0.05f * moveSpeed);
                yield return null;
            }

            PlayerManager.PlayerDamageableInstance.AddShield(shieldAmount);
            AudioManager.OnPlaySoundEffct?.Invoke(onUseSound);


            callBack?.Invoke();
        }
    }
}