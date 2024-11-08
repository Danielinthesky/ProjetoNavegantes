using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 8.0f;
    private float gravityValue = -9.81f;

    private Animator anim;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private Vector2 movimentoInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        
        // Remove a força descendente quando o jogador está no chão
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Calcula a direção do movimento baseada na orientação da câmera e no input
        Vector3 move = new Vector3(movimentoInput.x, 0, movimentoInput.y);
        move = move.x * cameraTransform.right + move.z * cameraTransform.forward;
        move.y = 0f; // Mantém o movimento no plano horizontal

        // Move o jogador na direção calculada
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Aplica a gravidade
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Controla a animação entre Idle e Walk
        bool isWalking = movimentoInput.sqrMagnitude > 0.1f;
        anim.SetBool("isWalking", isWalking);

        // Rota o jogador para a direção do movimento
        if (isWalking && move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }

    // Método para capturar o movimento do PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        movimentoInput = context.ReadValue<Vector2>();
    }
}
