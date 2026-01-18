using System;
using System.Collections.Generic;
using System.Linq;
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

            var relicCopy = Instantiate(relic);

            _ownedRelics.Add(relicCopy);
            relicCopy.relicEffectCompound.relicEffect.ForEach(effect => effect.OnAdded());
            OnRelicAdded?.Invoke(relicCopy);
            Debug.Log($"Added relic: {relicCopy.name}. Total relics now: {_ownedRelics.Count}");
        }

        public static void RemoveRelic(Relic relic)
        {
            if (_ownedRelics.Remove(relic))
            {
                OnRelicRemoved?.Invoke(relic);
                relic.relicEffectCompound.relicEffect.ForEach(effect => effect.OnRemoved());
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
                var compound = relic.relicEffectCompound;

                if (compound.gameplayEvent is T)
                {
                    // TODO: consider the priority of effects to apply
                    foreach (var effect in compound.relicEffect)
                    {
                        // apply the effect is there's no predicates or all predicates are met
                        if ((compound.predicates.Count == 0 || compound.predicates == null)
                            || compound.predicates.All(p => p.Check()))
                        {
                            effect.Apply(caller);
                            if (!relic.relicEffectCompound.isPassive)
                                relic.AfterApply?.Invoke();
                        }
                    }
                }
            }
        }

        public static bool HasRelic(Relic relic)
        {
            return _ownedRelics.Where(r => r.GUID == relic.GUID).Any();
        }
    }
}
