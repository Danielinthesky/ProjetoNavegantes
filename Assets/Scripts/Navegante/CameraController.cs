using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Alvo que a câmera seguirá
    public DynamicJoystick joystick; // Referência ao joystick para ignorar a área de drag
    public CinemachineFreeLook cinemachineCamera; // Referência ao Cinemachine FreeLook Camera

    public float rotationSpeed = 100f; // Velocidade de rotação
    public float minVerticalAngle = 10f; // Ângulo mínimo da câmera para evitar visão abaixo do terreno
    public float maxVerticalAngle = 80f; // Ângulo máximo da câmera para evitar visão muito acima

    private Vector2 lastTouchPosition;
    private bool isDraggingCamera = false;

    void Update()
    {
        HandleCameraRotation();
    }

    private void HandleCameraRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Verifica se o toque está fora da área do joystick
            if (!IsTouchOnJoystick(touch))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPosition = touch.position;
                    isDraggingCamera = true;
                }
                else if (touch.phase == TouchPhase.Moved && isDraggingCamera)
                {
                    Vector2 delta = touch.position - lastTouchPosition;

                    // Ajuste a rotação da câmera usando o Cinemachine FreeLook Camera
                    cinemachineCamera.m_XAxis.Value += delta.x * rotationSpeed * Time.deltaTime;
                    cinemachineCamera.m_YAxis.Value -= delta.y * rotationSpeed * Time.deltaTime;

                    // Limita o ângulo vertical da câmera
                    cinemachineCamera.m_YAxis.Value = Mathf.Clamp(cinemachineCamera.m_YAxis.Value, minVerticalAngle / 100f, maxVerticalAngle / 100f);

                    lastTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isDraggingCamera = false;
                }
            }
        }
    }

    private bool IsTouchOnJoystick(Touch touch)
    {
        // Verifica se o toque está na área do joystick
        RectTransform joystickRect = joystick.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(joystickRect, touch.position);
    }
}
