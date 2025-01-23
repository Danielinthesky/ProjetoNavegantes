using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera; // Referência à câmera Cinemachine Free Look
    [SerializeField] private float rotationSpeed = 300f; // Velocidade da rotação

    private Vector2 cameraInput; // Entrada para a rotação da câmera

    public void OnDragScreen(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Captura o movimento de arrasto (delta do toque)
            cameraInput = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            // Reseta o input quando o toque para
            cameraInput = Vector2.zero;
        }
    }

    private void Update()
    {
        // Rotaciona a câmera no eixo X (horizontal) usando o input capturado
        if (cameraInput != Vector2.zero && freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.Value += cameraInput.x * rotationSpeed * Time.deltaTime;
        }
    }
}
