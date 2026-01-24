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

    [AddTypeMenu("AfterLoopClosedEventWithItam")]
    [System.Serializable]
    public class AfterLoopClosedEventWithItam : BaseGameplayEvent
    {

    }
}
