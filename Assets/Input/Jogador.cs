//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/Jogador.inputactions
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

public partial class @Jogador: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Jogador()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Jogador"",
    ""maps"": [
        {
            ""name"": ""Wendell"",
            ""id"": ""1cf77c17-c496-415c-b333-dfd23d3aad09"",
            ""actions"": [
                {
                    ""name"": ""Andar"",
                    ""type"": ""Value"",
                    ""id"": ""5d6333c4-fcb7-4480-bde3-4ead0d8fb5b5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Correr"",
                    ""type"": ""Button"",
                    ""id"": ""d3e118c6-e0cf-4d03-956d-6bfdae73cd0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pular"",
                    ""type"": ""Button"",
                    ""id"": ""9eefa296-1b67-48fc-8ae8-88ae665fb82e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interagir"",
                    ""type"": ""Button"",
                    ""id"": ""7c6f6c71-2575-4c12-9149-bec2ab99fa8a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""36483f6c-a023-4d9a-bde2-23f8abb4b958"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e21586ba-bfb9-4af7-a845-47d6f18fc791"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Andar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""0909a853-c897-4629-bec5-c365d2cef08f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Andar"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""74631c6a-8886-42f6-a474-bfdd3f82b053"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Andar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d8f44a1a-b2f9-4d9d-8c02-3db8d2d0583a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Andar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""319ff7b2-2793-405f-94e2-83b6faa6f422"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Andar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3bd7a175-3bf3-493b-9c06-89d23a94917c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Andar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d8b3d0af-7e50-41c0-9732-f3255d612b65"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Correr"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1a218f17-4565-46c3-80f6-465985d6a7da"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pular"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18b3910e-8645-4e02-bb33-e7f73216da26"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interagir"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e434187-484d-43c6-b723-7a6ad03f557b"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Jangada"",
            ""id"": ""bc6796de-4578-4440-927b-34f016a40819"",
            ""actions"": [
                {
                    ""name"": ""MoverJangada"",
                    ""type"": ""Value"",
                    ""id"": ""3704f72f-ffa8-4112-b397-13b59cb8a066"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""IniciarNavegacao"",
                    ""type"": ""Button"",
                    ""id"": ""86339982-e26e-4eb4-be3c-17f9bf6df4bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1b14bcb7-9235-4437-acc0-fa6abfd7a9ef"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoverJangada"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""3b8e9e14-414f-40ae-ada4-45bc594bc61e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoverJangada"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""38d493c6-dba2-428a-a3db-2d9cb023e462"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoverJangada"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2e0b718b-df03-488c-ae76-5dbcda468796"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoverJangada"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a38fc464-efc9-4ea4-8be4-3db9dabe1a19"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoverJangada"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5b7edb18-7a11-4d82-b441-92440b169588"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoverJangada"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e95c9982-b2b7-4013-a857-d04d6c566477"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""IniciarNavegacao"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Wendell
        m_Wendell = asset.FindActionMap("Wendell", throwIfNotFound: true);
        m_Wendell_Andar = m_Wendell.FindAction("Andar", throwIfNotFound: true);
        m_Wendell_Correr = m_Wendell.FindAction("Correr", throwIfNotFound: true);
        m_Wendell_Pular = m_Wendell.FindAction("Pular", throwIfNotFound: true);
        m_Wendell_Interagir = m_Wendell.FindAction("Interagir", throwIfNotFound: true);
        m_Wendell_Camera = m_Wendell.FindAction("Camera", throwIfNotFound: true);
        // Jangada
        m_Jangada = asset.FindActionMap("Jangada", throwIfNotFound: true);
        m_Jangada_MoverJangada = m_Jangada.FindAction("MoverJangada", throwIfNotFound: true);
        m_Jangada_IniciarNavegacao = m_Jangada.FindAction("IniciarNavegacao", throwIfNotFound: true);
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

    // Wendell
    private readonly InputActionMap m_Wendell;
    private List<IWendellActions> m_WendellActionsCallbackInterfaces = new List<IWendellActions>();
    private readonly InputAction m_Wendell_Andar;
    private readonly InputAction m_Wendell_Correr;
    private readonly InputAction m_Wendell_Pular;
    private readonly InputAction m_Wendell_Interagir;
    private readonly InputAction m_Wendell_Camera;
    public struct WendellActions
    {
        private @Jogador m_Wrapper;
        public WendellActions(@Jogador wrapper) { m_Wrapper = wrapper; }
        public InputAction @Andar => m_Wrapper.m_Wendell_Andar;
        public InputAction @Correr => m_Wrapper.m_Wendell_Correr;
        public InputAction @Pular => m_Wrapper.m_Wendell_Pular;
        public InputAction @Interagir => m_Wrapper.m_Wendell_Interagir;
        public InputAction @Camera => m_Wrapper.m_Wendell_Camera;
        public InputActionMap Get() { return m_Wrapper.m_Wendell; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WendellActions set) { return set.Get(); }
        public void AddCallbacks(IWendellActions instance)
        {
            if (instance == null || m_Wrapper.m_WendellActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WendellActionsCallbackInterfaces.Add(instance);
            @Andar.started += instance.OnAndar;
            @Andar.performed += instance.OnAndar;
            @Andar.canceled += instance.OnAndar;
            @Correr.started += instance.OnCorrer;
            @Correr.performed += instance.OnCorrer;
            @Correr.canceled += instance.OnCorrer;
            @Pular.started += instance.OnPular;
            @Pular.performed += instance.OnPular;
            @Pular.canceled += instance.OnPular;
            @Interagir.started += instance.OnInteragir;
            @Interagir.performed += instance.OnInteragir;
            @Interagir.canceled += instance.OnInteragir;
            @Camera.started += instance.OnCamera;
            @Camera.performed += instance.OnCamera;
            @Camera.canceled += instance.OnCamera;
        }

        private void UnregisterCallbacks(IWendellActions instance)
        {
            @Andar.started -= instance.OnAndar;
            @Andar.performed -= instance.OnAndar;
            @Andar.canceled -= instance.OnAndar;
            @Correr.started -= instance.OnCorrer;
            @Correr.performed -= instance.OnCorrer;
            @Correr.canceled -= instance.OnCorrer;
            @Pular.started -= instance.OnPular;
            @Pular.performed -= instance.OnPular;
            @Pular.canceled -= instance.OnPular;
            @Interagir.started -= instance.OnInteragir;
            @Interagir.performed -= instance.OnInteragir;
            @Interagir.canceled -= instance.OnInteragir;
            @Camera.started -= instance.OnCamera;
            @Camera.performed -= instance.OnCamera;
            @Camera.canceled -= instance.OnCamera;
        }

        public void RemoveCallbacks(IWendellActions instance)
        {
            if (m_Wrapper.m_WendellActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWendellActions instance)
        {
            foreach (var item in m_Wrapper.m_WendellActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WendellActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WendellActions @Wendell => new WendellActions(this);

    // Jangada
    private readonly InputActionMap m_Jangada;
    private List<IJangadaActions> m_JangadaActionsCallbackInterfaces = new List<IJangadaActions>();
    private readonly InputAction m_Jangada_MoverJangada;
    private readonly InputAction m_Jangada_IniciarNavegacao;
    public struct JangadaActions
    {
        private @Jogador m_Wrapper;
        public JangadaActions(@Jogador wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoverJangada => m_Wrapper.m_Jangada_MoverJangada;
        public InputAction @IniciarNavegacao => m_Wrapper.m_Jangada_IniciarNavegacao;
        public InputActionMap Get() { return m_Wrapper.m_Jangada; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(JangadaActions set) { return set.Get(); }
        public void AddCallbacks(IJangadaActions instance)
        {
            if (instance == null || m_Wrapper.m_JangadaActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_JangadaActionsCallbackInterfaces.Add(instance);
            @MoverJangada.started += instance.OnMoverJangada;
            @MoverJangada.performed += instance.OnMoverJangada;
            @MoverJangada.canceled += instance.OnMoverJangada;
            @IniciarNavegacao.started += instance.OnIniciarNavegacao;
            @IniciarNavegacao.performed += instance.OnIniciarNavegacao;
            @IniciarNavegacao.canceled += instance.OnIniciarNavegacao;
        }

        private void UnregisterCallbacks(IJangadaActions instance)
        {
            @MoverJangada.started -= instance.OnMoverJangada;
            @MoverJangada.performed -= instance.OnMoverJangada;
            @MoverJangada.canceled -= instance.OnMoverJangada;
            @IniciarNavegacao.started -= instance.OnIniciarNavegacao;
            @IniciarNavegacao.performed -= instance.OnIniciarNavegacao;
            @IniciarNavegacao.canceled -= instance.OnIniciarNavegacao;
        }

        public void RemoveCallbacks(IJangadaActions instance)
        {
            if (m_Wrapper.m_JangadaActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IJangadaActions instance)
        {
            foreach (var item in m_Wrapper.m_JangadaActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_JangadaActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public JangadaActions @Jangada => new JangadaActions(this);
    public interface IWendellActions
    {
        void OnAndar(InputAction.CallbackContext context);
        void OnCorrer(InputAction.CallbackContext context);
        void OnPular(InputAction.CallbackContext context);
        void OnInteragir(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
    }
    public interface IJangadaActions
    {
        void OnMoverJangada(InputAction.CallbackContext context);
        void OnIniciarNavegacao(InputAction.CallbackContext context);
    }
}