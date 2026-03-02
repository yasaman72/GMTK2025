namespace Deviloop
{
    public class InputSettings 
    {
        public static bool IsGameplayInputBlocked { get; set; }
        public static bool AreAllInputBlocked { get; set; }

        public static bool IsPointrerOverUI
        {
            get
            {
                if (UnityEngine.EventSystems.EventSystem.current == null)
                    return false;
                return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            }
        }
    }
}
