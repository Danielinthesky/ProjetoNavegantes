using UnityEngine;
using Cinemachine;

public class NaveganteController : MonoBehaviour
{
    public float speed = 5f;                      // Velocidade de movimento do personagem
    public float rotationSpeed = 10f;             // Velocidade de rotação do personagem
    public DynamicJoystick movementJoystick;      // Joystick para movimento do personagem (esquerdo)
    public DynamicJoystick cameraJoystick;        // Joystick para rotação da câmera (direito)
    public CinemachineFreeLook freeLookCamera;    // Referência à Cinemachine FreeLook Camera

    public float cameraSensitivity = 1f;          // Sensibilidade de rotação da câmera
    public float maxHorizontalAngle = 90f;        // Ângulo máximo para a rotação horizontal da câmera

    private Animator animator;
    private Vector3 moveDirection;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Certifique-se de que a câmera não tenha rotação inicial ao redor do personagem
        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.Value = 0;
            freeLookCamera.m_YAxis.Value = 0.5f; // Posição inicial no eixo Y (opcional)
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCameraRotation();
    }

    void HandleMovement()
    {
        // Captura os inputs do joystick de movimento
        float horizontal = movementJoystick.Horizontal;
        float vertical = movementJoystick.Vertical;

        if (horizontal != 0 || vertical != 0)
        {
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;

            // Aplica rotação do personagem na direção do movimento
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // Move o personagem
            transform.position += moveDirection * speed * Time.deltaTime;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void HandleCameraRotation()
    {
        // Controla a rotação da câmera usando o joystick da câmera
        float cameraHorizontal = cameraJoystick.Horizontal;
        float cameraVertical = cameraJoystick.Vertical;

        if (freeLookCamera != null)
        {
            // Ajusta o valor de rotação em X com a sensibilidade e limita o ângulo horizontal
            float newXAxisValue = freeLookCamera.m_XAxis.Value + cameraHorizontal * cameraSensitivity * Time.deltaTime;

            // Limita o ângulo horizontal entre -maxHorizontalAngle e +maxHorizontalAngle
            newXAxisValue = Mathf.Clamp(newXAxisValue, -maxHorizontalAngle, maxHorizontalAngle);
            freeLookCamera.m_XAxis.Value = newXAxisValue;

            // Ajusta o valor de rotação em Y com a sensibilidade
            freeLookCamera.m_YAxis.Value += cameraVertical * cameraSensitivity * Time.deltaTime;
        }
    }
}
