using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_CombatCharacter
{
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    [UnityTest]
    public IEnumerator Test_CombatCharacterWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
