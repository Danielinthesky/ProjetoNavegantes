using UnityEngine;

public class NaveganteController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = 9.8f; // Intensidade da gravidade
    public DynamicJoystick joystick; // Componente de joystick no inspector.
    public Animator animator; // Componente Animator do personagem.
    public Transform cameraTransform; // Transform da câmera para ajustar o movimento conforme a orientação da câmera.
    private CharacterController characterController; // Componente CharacterController.

    private Vector3 moveDirection;
    private Vector3 verticalVelocity; // Velocidade vertical para aplicar a gravidade

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Movimento do personagem baseado no joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // Calcula a direção de movimento em relação ao input do joystick e orientação da câmera
            Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
            moveDirection = cameraTransform.TransformDirection(inputDirection);
            moveDirection.y = 0; // Mantém o movimento no plano horizontal

            // Rotaciona o personagem suavemente na direção do movimento
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            // Move o personagem na direção calculada
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            characterController.Move(movement);

            // Ativa a animação de caminhada e ajusta a direção
            animator.SetBool("isWalking", true);
            animator.SetFloat("MoveX", inputDirection.x);
            animator.SetFloat("MoveY", inputDirection.z);
        }
        else
        {
            // Ativa a animação de idle quando o personagem para de se mover
            animator.SetBool("isWalking", false);
        }

        // Aplica a gravidade sempre que o personagem estiver no ar ou em uma inclinação
        if (!characterController.isGrounded)
        {
            verticalVelocity.y -= gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity.y = 0; // Reseta a velocidade vertical ao tocar o chão
        }

        // Move o personagem com a gravidade aplicada
        characterController.Move(verticalVelocity * Time.deltaTime);
    }
}
