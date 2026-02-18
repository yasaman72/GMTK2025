
using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Threading.Tasks;
using UnityEngine;

namespace Deviloop
{
    [System.Serializable]
    public abstract class CardAnimationType
    {
        public bool playWithNext;
        [SerializeField] protected ModifiableFloat DelayBeforeMove = new ModifiableFloat(.3f);
        [SerializeField] protected ModifiableFloat DelayAfterMove = new ModifiableFloat(.3f);
        
        public abstract Task Play(GameObject card, GameObject target = null);
    }

    [AddTypeMenu("MoveToTarget")]
    [System.Serializable]
    public class MoveToTarget : CardAnimationType
    {
        [SerializeField] private ModifiableFloat MoveDuration = new ModifiableFloat(.2f);

        public override async Task Play(GameObject card, GameObject target)
        {
            await Awaitable.WaitForSecondsAsync(DelayBeforeMove.Value);
            if (card == null || target == null) return;

            Tween tween = card.transform
                .DOMove(target != null ? target.transform.position : card.transform.position, MoveDuration.Value)
                .SetEase(Ease.Linear)
                .SetLink(target); // auto-kill if card dies
            
            await Awaitable.WaitForSecondsAsync(DelayAfterMove.Value);

            await tween.AsyncWaitForCompletion();
        }
    }

    [AddTypeMenu("MoveToPlayerHP")]
    [System.Serializable]
    public class MoveToPlayer : CardAnimationType
    {
        [SerializeField] private ModifiableFloat MoveDuration = new ModifiableFloat(.2f);

        public override async Task Play(GameObject card, GameObject target)
        {
            Transform playerHpPos = Player.PlayerCombatCharacter.HPOrigin;

            await Awaitable.WaitForSecondsAsync(DelayBeforeMove.Value);
            if (card == null || target == null) return;

            Tween tween = card.transform
                .DOMove(playerHpPos.gameObject != null ? playerHpPos.position : card.transform.position, MoveDuration.Value)
                .SetEase(Ease.Linear)
                .SetLink(target); // auto-kill if target dies

            await Awaitable.WaitForSecondsAsync(DelayAfterMove.Value);

            await tween.AsyncWaitForCompletion();
        }
    }


    [AddTypeMenu("MoveToPlayerShield")]
    [System.Serializable]
    public class MoveToPlayerShield : CardAnimationType
    {
        [SerializeField] private ModifiableFloat MoveDuration = new ModifiableFloat(.2f);

        public override async Task Play(GameObject card, GameObject target)
        {
            Transform playerShieldPos = Player.PlayerCombatCharacter.ShieldOrigin;

            await Awaitable.WaitForSecondsAsync(DelayBeforeMove.Value);
            if (card == null || target == null) return;

            Tween tween = card.transform
                .DOMove(playerShieldPos.gameObject != null ? playerShieldPos.position : card.transform.position, MoveDuration.Value)
                .SetEase(Ease.Linear)
                .SetLink(target); // auto-kill if target dies

            await Awaitable.WaitForSecondsAsync(DelayAfterMove.Value);

            await tween.AsyncWaitForCompletion();
        }
    }


    [AddTypeMenu("ScaleAnimation")]
    [System.Serializable]
    public class ScaleAnimation : CardAnimationType
    {
        [SerializeField] private ModifiableFloat _scaleDuration = new ModifiableFloat(0.5f);
        [SerializeField] private float _targetScaleMult = 2;
        [SerializeField] private Ease _ease = Ease.OutBack;

        public override async Task Play(GameObject card, GameObject target = null)
        {
            await Awaitable.WaitForSecondsAsync(DelayBeforeMove.Value);
            if (card == null || target == null) return;

            Vector3 targetScale = card.transform.localScale * _targetScaleMult;

            Tween tween = card.transform
                .DOScale(targetScale, _scaleDuration.Value)
                .SetEase(_ease)
                .SetLink(target);

            if (card == null || target == null) return;

            await Awaitable.WaitForSecondsAsync(DelayAfterMove.Value);

            await tween.AsyncWaitForCompletion();
        }
    }

    [AddTypeMenu("RotateAnimation")]
    [System.Serializable]
    public class RotateAnimation : CardAnimationType
    {
        [SerializeField] private ModifiableFloat _rotateDuration = new ModifiableFloat(.5f);

        [SerializeField] private int rounds = 5;

        public override async Task Play(GameObject card, GameObject target = null)
        {
            await Awaitable.WaitForSecondsAsync(DelayBeforeMove.Value);

            Tween tween = card.transform
                .DORotate(new Vector3(0, 0, 360 * rounds), _rotateDuration.Value, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLink(target);

            await Awaitable.WaitForSecondsAsync(DelayAfterMove.Value);

            await tween.AsyncWaitForCompletion();
        }
    }
}
