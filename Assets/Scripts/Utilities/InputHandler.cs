using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manage the input of the new Input System.
/// </summary>

[AddComponentMenu("Scripts/ESI/Input Handler")]
public class InputHandler : MonoBehaviour {

    //[field: Header("AutoAttach on Editor properties")]

    [field: Header("Other properties")]
    PlayerInputActions inputActions { get; set; }
    public PlayerInputActions.PlayerActions playerActions { get; private set; }
    public PlayerInputActions.UIActions UIActions { get; private set; }

    [field: SerializeField] public bool MoveCorpseIsPressed { get; private set; } = false;

    /// <summary> True if the pause button was pressed this frame. </summary>
    public bool PauseWasPressedThisFrame { get; private set; } = false;

    
    void OnEnable() {
        inputActions = new PlayerInputActions();
        playerActions = inputActions.Player;
        UIActions = inputActions.UI;
        inputActions.Enable();

        playerActions.MoveCorpse.started += MoveCorpse;
        playerActions.MoveCorpse.canceled += MoveCorpse;
    }

    // Update is called once per frame
    void Update() {
        PauseWasPressedThisFrame = UIActions.Pause.WasPressedThisFrame();
    }

    // Called when the MoveCorpseButton is or is being pressed.
    private void MoveCorpse(InputAction.CallbackContext ctx) {
        MoveCorpseIsPressed = ctx.started;
    }

    private void OnDisable() {
        playerActions.MoveCorpse.started -= MoveCorpse;
        playerActions.MoveCorpse.canceled -= MoveCorpse;

        inputActions.Disable();
    }
}
