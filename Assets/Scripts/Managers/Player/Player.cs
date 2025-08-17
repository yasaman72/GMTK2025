
public class Player : CombatCharacter
{
    public static CombatCharacter PlayerCombatCharacter { get; private set; }

    private void Awake()
    {
        PlayerCombatCharacter = this;
    }
}
