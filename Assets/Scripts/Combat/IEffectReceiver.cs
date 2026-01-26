using System.Collections.Generic;

namespace Deviloop
{
    public interface IEffectReceiver 
    {
        void AddEffect(CharacterEffectBase effect, int duration);
        void RemoveEffect(CharacterEffectBase effect);
        void ApplyEffect(CharacterEffectBase effect);
        void ApplyAllEffects(EnemyAction enemyAction);
        List<CharacterEffectBase> GetCurrentEffects { get; }
    }
}
