using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField, Header("InputSystemをアタッチします")]
    UnityEngine.InputSystem.PlayerInput playerInputSystem;

    [HideInInspector]
    public float X { get; private set; }
    [HideInInspector]
    public bool JumpTriggered { get; private set; }

    InputAction movex;
    InputAction jumpPressed;

    void Awake()
    {
        if (playerInputSystem == null)
        {
            Debug.LogError("playerInputSystemがアタッチされていません");
            return;
        }

        movex = playerInputSystem.actions["Move"];
        jumpPressed = playerInputSystem.actions["Jump"];
    }

    public void ReadInput()
    {
        X = movex.ReadValue<float>();
        JumpTriggered = jumpPressed.triggered;

        //Debug.Log($"JumpTriggerd : {JumpTriggered}");
    }
}
