using System;

public class PlayerController : Singleton<PlayerController>
{
    public Action AddEscafeAction(Action action) => OnEscafeAction += action;

    Action OnEscafeAction;

    protected override void Awake()
    {
        OnEscafeAction = null;
    }

    public void OnClickCancel()
    {
        OnEscafeAction?.Invoke();
    }
} 
