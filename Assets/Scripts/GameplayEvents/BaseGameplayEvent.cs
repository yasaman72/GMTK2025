namespace Deviloop
{
    [System.Serializable]
    public abstract class BaseGameplayEvent
    {

    }

    [AddTypeMenu("OnCombatEndEvent")]
    [System.Serializable]
    public class OnCombatEndEvent : BaseGameplayEvent
    {

    }

    [AddTypeMenu("AfterLoopClosedEvent")]
    [System.Serializable]
    public class AfterLoopClosedEvent : BaseGameplayEvent
    {

    }
}
