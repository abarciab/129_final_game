//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/PlayerControls.inputactions
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

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""7b6a199f-111c-427e-948c-3487e19dfc0e"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ed919ddf-9fa4-4ee8-9ecc-74c115044e33"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""032488fd-c31c-422b-8384-bcb58c96064e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StabAttack"",
                    ""type"": ""Button"",
                    ""id"": ""39e7a4eb-a3a8-4bc3-8fee-2adfa0be9be9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SlashAttack"",
                    ""type"": ""Button"",
                    ""id"": ""8b33c38f-71a6-4f79-b016-eff883a52822"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StabDefend"",
                    ""type"": ""Button"",
                    ""id"": ""f725b5c0-e802-4ef7-9e4a-fb5aa4c43295"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SlashDefend"",
                    ""type"": ""Button"",
                    ""id"": ""ac451db9-bd18-4316-ad95-f02983c0f2f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""9099e910-03ba-40b2-b785-8667016932fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""b56f8672-52b9-47cc-a2d6-a5459ce22d48"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d64e7f81-f9ba-43a7-ab2b-aa1d357483f2"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a7a5895-30f3-4243-a6c1-d1a3a66efd81"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35e74cb3-3f10-4ede-8fcc-a23ac4994aad"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StabAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""414704cc-16dd-4215-9dde-d98647eca1d2"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SlashAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11c6f8e2-f38f-4f16-bf2c-fab638b901af"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StabDefend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59055c7e-15bd-4c35-9eb9-600e1e2205eb"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SlashDefend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f45f6c3-a129-42b7-8dd6-d19bcb944b43"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c6abbe76-3b08-482a-ac14-6b7fcb3b84b3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_StabAttack = m_Player.FindAction("StabAttack", throwIfNotFound: true);
        m_Player_SlashAttack = m_Player.FindAction("SlashAttack", throwIfNotFound: true);
        m_Player_StabDefend = m_Player.FindAction("StabDefend", throwIfNotFound: true);
        m_Player_SlashDefend = m_Player.FindAction("SlashDefend", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_StabAttack;
    private readonly InputAction m_Player_SlashAttack;
    private readonly InputAction m_Player_StabDefend;
    private readonly InputAction m_Player_SlashDefend;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Dash;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @StabAttack => m_Wrapper.m_Player_StabAttack;
        public InputAction @SlashAttack => m_Wrapper.m_Player_SlashAttack;
        public InputAction @StabDefend => m_Wrapper.m_Player_StabDefend;
        public InputAction @SlashDefend => m_Wrapper.m_Player_SlashDefend;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @StabAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStabAttack;
                @StabAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStabAttack;
                @StabAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStabAttack;
                @SlashAttack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlashAttack;
                @SlashAttack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlashAttack;
                @SlashAttack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlashAttack;
                @StabDefend.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStabDefend;
                @StabDefend.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStabDefend;
                @StabDefend.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnStabDefend;
                @SlashDefend.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlashDefend;
                @SlashDefend.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlashDefend;
                @SlashDefend.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSlashDefend;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @StabAttack.started += instance.OnStabAttack;
                @StabAttack.performed += instance.OnStabAttack;
                @StabAttack.canceled += instance.OnStabAttack;
                @SlashAttack.started += instance.OnSlashAttack;
                @SlashAttack.performed += instance.OnSlashAttack;
                @SlashAttack.canceled += instance.OnSlashAttack;
                @StabDefend.started += instance.OnStabDefend;
                @StabDefend.performed += instance.OnStabDefend;
                @StabDefend.canceled += instance.OnStabDefend;
                @SlashDefend.started += instance.OnSlashDefend;
                @SlashDefend.performed += instance.OnSlashDefend;
                @SlashDefend.canceled += instance.OnSlashDefend;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnStabAttack(InputAction.CallbackContext context);
        void OnSlashAttack(InputAction.CallbackContext context);
        void OnStabDefend(InputAction.CallbackContext context);
        void OnSlashDefend(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
}
