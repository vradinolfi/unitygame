// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerControls : IInputActionCollection
{
    private InputActionAsset asset;
    public PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""81ce6025-7b6f-425c-8156-fb549cb994a4"",
            ""actions"": [
                {
                    ""name"": ""Horizontal"",
                    ""type"": ""Button"",
                    ""id"": ""a1812e3a-2ff9-4c99-b0ee-b6434e7b1b18"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Vertical"",
                    ""type"": ""Button"",
                    ""id"": ""a5d515ac-fa4e-4a49-95c0-6afc57afffa0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""da4987d3-5c11-4ac4-b811-d535db919933"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""1a69ac4b-2f87-4086-94b0-52a8b04c79c7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""0a0a801d-7059-4998-bfd7-3b9704099ba8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""655806a1-c959-44fa-811c-b776672effb6"",
                    ""path"": ""<Gamepad>/dpad/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""815da543-420a-4e24-8875-d061e60584f6"",
                    ""path"": ""<Gamepad>/dpad/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36f6585d-6bfe-42ba-b676-1ca6fd6521f3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bd33279-1441-4f54-a93d-8f3a2c05af71"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""548e523b-ce26-4ccb-8468-89f7847c090c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.GetActionMap("Gameplay");
        m_Gameplay_Horizontal = m_Gameplay.GetAction("Horizontal");
        m_Gameplay_Vertical = m_Gameplay.GetAction("Vertical");
        m_Gameplay_Interact = m_Gameplay.GetAction("Interact");
        m_Gameplay_Aim = m_Gameplay.GetAction("Aim");
        m_Gameplay_Run = m_Gameplay.GetAction("Run");
    }

    ~PlayerControls()
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Horizontal;
    private readonly InputAction m_Gameplay_Vertical;
    private readonly InputAction m_Gameplay_Interact;
    private readonly InputAction m_Gameplay_Aim;
    private readonly InputAction m_Gameplay_Run;
    public struct GameplayActions
    {
        private PlayerControls m_Wrapper;
        public GameplayActions(PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Horizontal => m_Wrapper.m_Gameplay_Horizontal;
        public InputAction @Vertical => m_Wrapper.m_Gameplay_Vertical;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputAction @Aim => m_Wrapper.m_Gameplay_Aim;
        public InputAction @Run => m_Wrapper.m_Gameplay_Run;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                Horizontal.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontal;
                Horizontal.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontal;
                Horizontal.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontal;
                Vertical.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVertical;
                Vertical.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVertical;
                Vertical.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVertical;
                Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                Aim.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                Aim.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                Aim.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAim;
                Run.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                Run.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                Run.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                Horizontal.started += instance.OnHorizontal;
                Horizontal.performed += instance.OnHorizontal;
                Horizontal.canceled += instance.OnHorizontal;
                Vertical.started += instance.OnVertical;
                Vertical.performed += instance.OnVertical;
                Vertical.canceled += instance.OnVertical;
                Interact.started += instance.OnInteract;
                Interact.performed += instance.OnInteract;
                Interact.canceled += instance.OnInteract;
                Aim.started += instance.OnAim;
                Aim.performed += instance.OnAim;
                Aim.canceled += instance.OnAim;
                Run.started += instance.OnRun;
                Run.performed += instance.OnRun;
                Run.canceled += instance.OnRun;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnHorizontal(InputAction.CallbackContext context);
        void OnVertical(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
    }
}
*/