using System;

public static class GameStateManager
{
    // After items are thrown, these values change to allow player start drawing the lasso
    public static Action OnPlayerDrawTurnStart;
    private static bool _canPlayerDrawLasso = false;
    public static bool CanPlayerDrawLasso
    {
        get
        {
            return _canPlayerDrawLasso;
        }
        set
        {
            if (value)
            {
                OnPlayerDrawTurnStart?.Invoke();
            }
            _canPlayerDrawLasso = value;
        }
    }
}
