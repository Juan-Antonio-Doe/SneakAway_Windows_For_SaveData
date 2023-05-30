//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Resources/Inputs/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""fc1188f6-9a6f-4aac-aafe-5d884f79b7ee"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""fa39f7e9-c98a-4ca0-b3d4-ae8710cca19d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""UsePowerUp_0"",
                    ""type"": ""Button"",
                    ""id"": ""be3f6978-39c0-48af-a1bf-146e82a6f9cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsePowerUp_1"",
                    ""type"": ""Button"",
                    ""id"": ""093fef35-1765-41f6-8b1f-28352ffff9dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsePowerUp_2"",
                    ""type"": ""Button"",
                    ""id"": ""e4e8901a-b297-4b04-9a64-d61717e5e706"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsePowerUp_3"",
                    ""type"": ""Button"",
                    ""id"": ""5f8d964f-b3ad-442e-8da6-f57b3ef10657"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsePowerUp_4"",
                    ""type"": ""Button"",
                    ""id"": ""98faae2f-0186-469c-8cb3-c893d29727df"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UsePowerUp_5"",
                    ""type"": ""Button"",
                    ""id"": ""32d23bb1-c060-42f5-96e4-714e4e7eacd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveCorpse"",
                    ""type"": ""Value"",
                    ""id"": ""25e4abfb-e03c-43d3-b71d-f28b8c090edc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4f78509e-dfb0-496f-a3f2-c5568c3ec339"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""88c76317-f9bf-49bb-a207-cd7f174a7c00"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bdf23c8a-76b2-44ca-9866-b945524e2029"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""50198efc-135c-4f84-8a27-69f04aa0c8ec"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6243bdd3-e024-4bd9-9963-dab1cda8b364"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""74b2722b-09fc-44ff-a7c1-9872c0852621"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""983d90e3-54ae-4a37-92a2-02cdc8b2707b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c78ec11c-7448-48c3-82d3-f9f2581864bb"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93dc2ae4-872e-4f86-aec5-3ca123ec4fcd"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2048c9ed-bd16-48d7-9857-7c6ced243b43"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a689824-c57c-4eda-94fe-93a84711737e"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""657d66d1-0de3-4bbf-aa07-bf495b233481"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCorpse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9bfb525-3934-4dc3-b071-610cac7f8ed0"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a6d63ea-b8ae-41ee-9dec-a9418e0bde5e"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10142721-828e-4614-91d3-b5ff6894781e"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b288025c-5e42-46b1-b430-3934542d2dcf"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2e7875b-8451-4c86-b6cb-584c2c2d907d"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08d31b29-a3a3-43bd-928a-4c0c15b7dc0d"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9aa7b2fc-ea8f-4f0d-a6af-e5de77446cd8"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UsePowerUp_5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a937df81-c442-4a89-a063-6a466ea2ed98"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCorpse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""acda6b6c-8ae9-4677-ac11-aa691f8ac599"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""a03e2aa1-f62a-429b-9ea8-05094041e8b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6f961755-8522-48c1-8ab3-1c4b987f0c40"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77a01d91-70be-456e-b968-9693121a9f15"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mobile"",
            ""bindingGroup"": ""Mobile"",
            ""devices"": []
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_UsePowerUp_0 = m_Player.FindAction("UsePowerUp_0", throwIfNotFound: true);
        m_Player_UsePowerUp_1 = m_Player.FindAction("UsePowerUp_1", throwIfNotFound: true);
        m_Player_UsePowerUp_2 = m_Player.FindAction("UsePowerUp_2", throwIfNotFound: true);
        m_Player_UsePowerUp_3 = m_Player.FindAction("UsePowerUp_3", throwIfNotFound: true);
        m_Player_UsePowerUp_4 = m_Player.FindAction("UsePowerUp_4", throwIfNotFound: true);
        m_Player_UsePowerUp_5 = m_Player.FindAction("UsePowerUp_5", throwIfNotFound: true);
        m_Player_MoveCorpse = m_Player.FindAction("MoveCorpse", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_UsePowerUp_0;
    private readonly InputAction m_Player_UsePowerUp_1;
    private readonly InputAction m_Player_UsePowerUp_2;
    private readonly InputAction m_Player_UsePowerUp_3;
    private readonly InputAction m_Player_UsePowerUp_4;
    private readonly InputAction m_Player_UsePowerUp_5;
    private readonly InputAction m_Player_MoveCorpse;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @UsePowerUp_0 => m_Wrapper.m_Player_UsePowerUp_0;
        public InputAction @UsePowerUp_1 => m_Wrapper.m_Player_UsePowerUp_1;
        public InputAction @UsePowerUp_2 => m_Wrapper.m_Player_UsePowerUp_2;
        public InputAction @UsePowerUp_3 => m_Wrapper.m_Player_UsePowerUp_3;
        public InputAction @UsePowerUp_4 => m_Wrapper.m_Player_UsePowerUp_4;
        public InputAction @UsePowerUp_5 => m_Wrapper.m_Player_UsePowerUp_5;
        public InputAction @MoveCorpse => m_Wrapper.m_Player_MoveCorpse;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @UsePowerUp_0.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_0;
                @UsePowerUp_0.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_0;
                @UsePowerUp_0.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_0;
                @UsePowerUp_1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_1;
                @UsePowerUp_1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_1;
                @UsePowerUp_1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_1;
                @UsePowerUp_2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_2;
                @UsePowerUp_2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_2;
                @UsePowerUp_2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_2;
                @UsePowerUp_3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_3;
                @UsePowerUp_3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_3;
                @UsePowerUp_3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_3;
                @UsePowerUp_4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_4;
                @UsePowerUp_4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_4;
                @UsePowerUp_4.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_4;
                @UsePowerUp_5.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_5;
                @UsePowerUp_5.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_5;
                @UsePowerUp_5.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUsePowerUp_5;
                @MoveCorpse.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCorpse;
                @MoveCorpse.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCorpse;
                @MoveCorpse.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCorpse;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @UsePowerUp_0.started += instance.OnUsePowerUp_0;
                @UsePowerUp_0.performed += instance.OnUsePowerUp_0;
                @UsePowerUp_0.canceled += instance.OnUsePowerUp_0;
                @UsePowerUp_1.started += instance.OnUsePowerUp_1;
                @UsePowerUp_1.performed += instance.OnUsePowerUp_1;
                @UsePowerUp_1.canceled += instance.OnUsePowerUp_1;
                @UsePowerUp_2.started += instance.OnUsePowerUp_2;
                @UsePowerUp_2.performed += instance.OnUsePowerUp_2;
                @UsePowerUp_2.canceled += instance.OnUsePowerUp_2;
                @UsePowerUp_3.started += instance.OnUsePowerUp_3;
                @UsePowerUp_3.performed += instance.OnUsePowerUp_3;
                @UsePowerUp_3.canceled += instance.OnUsePowerUp_3;
                @UsePowerUp_4.started += instance.OnUsePowerUp_4;
                @UsePowerUp_4.performed += instance.OnUsePowerUp_4;
                @UsePowerUp_4.canceled += instance.OnUsePowerUp_4;
                @UsePowerUp_5.started += instance.OnUsePowerUp_5;
                @UsePowerUp_5.performed += instance.OnUsePowerUp_5;
                @UsePowerUp_5.canceled += instance.OnUsePowerUp_5;
                @MoveCorpse.started += instance.OnMoveCorpse;
                @MoveCorpse.performed += instance.OnMoveCorpse;
                @MoveCorpse.canceled += instance.OnMoveCorpse;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Pause;
    public struct UIActions
    {
        private @PlayerInputActions m_Wrapper;
        public UIActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_MobileSchemeIndex = -1;
    public InputControlScheme MobileScheme
    {
        get
        {
            if (m_MobileSchemeIndex == -1) m_MobileSchemeIndex = asset.FindControlSchemeIndex("Mobile");
            return asset.controlSchemes[m_MobileSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnUsePowerUp_0(InputAction.CallbackContext context);
        void OnUsePowerUp_1(InputAction.CallbackContext context);
        void OnUsePowerUp_2(InputAction.CallbackContext context);
        void OnUsePowerUp_3(InputAction.CallbackContext context);
        void OnUsePowerUp_4(InputAction.CallbackContext context);
        void OnUsePowerUp_5(InputAction.CallbackContext context);
        void OnMoveCorpse(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
