

using Cards;

public class Player : CombatCharacter
{
    public static CombatCharacter PlayerCombatCharacter { get; private set; }

    private void Awake()
    {
        PlayerCombatCharacter = this;
    }

    private void OnEnable()
    {
        CombatManager.OnCombatFinishedEvent += OnCombatFinished;
    }

    private void OnDisable()
    {
        CombatManager.OnCombatFinishedEvent -= OnCombatFinished;
    }

    private void OnCombatFinished()
    {
        RemoveAllShields();
        CardManager.ReturnAllCardsToHand?.Invoke();
    }
}
