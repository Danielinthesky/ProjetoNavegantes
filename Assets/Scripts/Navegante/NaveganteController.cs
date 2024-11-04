using UnityEngine;
using Cinemachine;

public class NaveganteController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float cameraRotationSensitivity = 0.1f; // Sensibilidade ajustada para a rotação da câmera
    public DynamicJoystick joystick; // Arraste o componente do joystick para este campo no inspector.
    public CinemachineVirtualCamera virtualCamera; // Arraste sua CinemachineVirtualCamera para este campo.
    public Animator animator; // Adicione o componente Animator do personagem aqui.

    private Camera mainCamera;
    private Vector3 moveDirection;
    private float currentCameraRotationX = 0f;
    public float maxCameraRotationX = 60f; // Limite para a rotação vertical da câmera

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Movimento do personagem baseado no joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // Calcula a direção de movimento em relação à orientação da câmera
            Vector3 forward = mainCamera.transform.forward;
            Vector3 right = mainCamera.transform.right;

            forward.y = 0f; // Ignora a inclinação vertical da câmera
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            moveDirection = (forward * vertical + right * horizontal).normalized;

            // Move o personagem na direção calculada
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            // Rotaciona o personagem na direção do movimento
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
            }

            // Ativa a animação de caminhada
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Ativa a animação de idle quando o personagem para de se mover
            animator.SetBool("isWalking", false);
        }

        // Controle de rotação da câmera por drag
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = touch.deltaPosition;

                // Rotaciona a câmera horizontalmente
                transform.Rotate(0, touchDeltaPosition.x * cameraRotationSensitivity, 0);

                // Controla a rotação vertical da câmera com restrição
                currentCameraRotationX -= touchDeltaPosition.y * cameraRotationSensitivity;
                currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -maxCameraRotationX, maxCameraRotationX);

                virtualCamera.transform.localRotation = Quaternion.Euler(currentCameraRotationX, 0, 0);
            }
        }
    }
}
