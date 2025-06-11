using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField, Header("InputSystemをアタッチします")]
    InputAction inputAction;

    [HideInInspector]
    public float X { get; private set; }
    [HideInInspector]
    public bool JumpPressed { get; private set; }

    void Start() => inputAction.Enable();

    public void ReadInput()
    {
        X = inputAction.ReadValue<float>();
        JumpPressed = inputAction.ReadValue<bool>();
    }
}
