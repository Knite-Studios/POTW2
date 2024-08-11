using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public static InputAction Hype => Instance._inputs.Controls.Hype;
    
    private Inputs _inputs;
    
    protected override void Awake()
    {
        base.Awake();

        _inputs = new Inputs();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
}