﻿using System;
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

        protected override void UseCard(MonoBehaviour runner, Action callback, CardPrefab cardPrefab)
        {
            Logger.Log($"Shield provides {shieldAmount} protection", shouldLog);
            runner.StartCoroutine(ActivateCardEffect(callback, cardPrefab));
        }

        private IEnumerator ActivateCardEffect(Action callBack, CardPrefab cardPrefab)
        {
            yield return new WaitForSeconds(.3f);

            Transform playerHpPos = FindAnyObjectByType<Player>().HPOrigin;

            while (Vector2.Distance(cardPrefab.transform.position, playerHpPos.position) > 1)
            {
                cardPrefab.transform.position = Vector2.MoveTowards(
                    cardPrefab.transform.position,
                    playerHpPos.position,
                    0.05f * moveSpeed);
                yield return null;
            }

            Player.PlayerCombatCharacter.AddShield(shieldAmount);
            AudioManager.PlayAudioOneShot?.Invoke(OnUseSound);


            callBack?.Invoke();
        }
    }
}