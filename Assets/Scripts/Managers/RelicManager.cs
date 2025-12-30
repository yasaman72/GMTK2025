using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deviloop
{
    public class RelicManager : MonoBehaviour
    {
        public static Action<Relic> OnRelicAdded;
        public static Action<Relic> OnRelicRemoved;

        [Header("the starting relic set could be left empty")]
        [SerializeField] private RelicSet _startingRelicSet;
        [ReadOnly, SerializeField] private static List<Relic> _ownedRelics = new List<Relic>();
        public static List<Relic> OwnedRelics => _ownedRelics;

        private void Awake()
        {
            if (_startingRelicSet == null)
            {
                Debug.Log("Starting Relic Set is not assigned in RelicManager.");
                return;
            }

            foreach (var relic in _startingRelicSet.Relics)
            {
                AddRelic(relic);
            }
        }

        public static void AddRelic(Relic relic)
        {
            if (relic == null)
            {
                Debug.LogWarning("Attempted to add a null relic.");
                return;
            }

            _ownedRelics.Add(relic);
            relic.relicEffectCompound.ForEach(compound =>
            {
                compound.relicEffect.ForEach(effect => effect.OnAdded());
            });
            OnRelicAdded?.Invoke(relic);
            Debug.Log($"Added relic: {relic.name}. Total relics now: {_ownedRelics.Count}");
        }

        public static void RemoveRelic(Relic relic)
        {
            if (_ownedRelics.Remove(relic))
            {
                OnRelicRemoved?.Invoke(relic);
                relic.relicEffectCompound.ForEach(compound =>
                {
                    compound.relicEffect.ForEach(effect => effect.OnRemoved());
                });
                Debug.Log($"Removed relic: {relic.name}. Total relics now: {_ownedRelics.Count}");
            }
            else
            {
                Debug.LogWarning($"Attempted to remove relic: {relic.name}, but it was not found in owned relics.");
            }
        }

        public static void ApplyEffectsForEvent<T>(MonoBehaviour caller) where T : BaseGameplayEvent
        {
            foreach (var relic in _ownedRelics)
            {
                foreach (var compound in relic.relicEffectCompound)
                {
                    if (compound.gameplayEvent is T)
                    {
                        // TODO: consider the priority of effects to apply
                        foreach (var effect in compound.relicEffect)
                            effect.Apply(caller);
                    }
                }
            }
        }
    }
}
