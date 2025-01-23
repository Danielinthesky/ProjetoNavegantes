using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraLook : MonoBehaviour
{
    private CinemachineFreeLook cinemachine;
    private Player playerInput; // Classe gerada pelo Input System

    [Header("Configurações de Sensibilidade")]
    [SerializeField] private float lookSpeed = 200f; // Sensibilidade do eixo X
    [SerializeField] private float smoothTime = 0.1f; // Tempo de suavização

    private float currentAxisValue = 0f; // Valor atual do eixo X
    private float axisVelocity = 0f;    // Velocidade do Lerp

    private void Awake()
    {
        playerInput = new Player();
        cinemachine = GetComponent<CinemachineFreeLook>();
        
        // Ativa controle manual do InputAxis
        cinemachine.m_XAxis.m_InputAxisName = ""; // Desativa controles predefinidos
        cinemachine.m_XAxis.m_InputAxisValue = 0f; // Inicializa com zero
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        // Captura o input do Right Stick
        Vector2 cameraInput = playerInput.Wendell.Camera.ReadValue<Vector2>();

       
        // Suaviza a entrada usando Lerp
        float targetAxisValue = cameraInput.x * lookSpeed * Time.deltaTime;
        currentAxisValue = Mathf.SmoothDamp(currentAxisValue, targetAxisValue, ref axisVelocity, smoothTime);

        // Aplica suavemente no eixo X
        cinemachine.m_XAxis.m_InputAxisValue = currentAxisValue;
        cinemachine.m_YAxis.m_InputAxisValue = currentAxisValue;
    }
}
