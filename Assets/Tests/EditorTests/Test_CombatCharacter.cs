using Deviloop;
using NUnit.Framework;
using UnityEngine;

public class Test_CombatCharacter
{
    // A Test behaves as an ordinary method
    [Test]
    public void Test_CombatCharacter_Reset()
    {
        // Use the Assert class to test conditions
        CombatCharacter combatCharacter = new CombatCharacter();
        combatCharacter.Stats = ScriptableObject.CreateInstance<PlayerCombatStats>();
        combatCharacter.ResetStats();
        Assert.AreEqual(combatCharacter.MaxHealth, combatCharacter.GetCurrentHealth);
    }
}
