using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Cinemachine;

public class PlayerControllerMultiplayer1 : MonoBehaviourPun
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] public float playerSpeed = 3.0f;
    private float gravityValue = -9.81f;

    private Animator anim;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private Vector2 movimentoInput;

   private void Start()
{
    if (!photonView.IsMine)
    {
        GetComponent<PlayerInput>().enabled = false; // Desativa o PlayerInput para o jogador não local
        return;
    }

    controller = GetComponent<CharacterController>();
    anim = GetComponent<Animator>();
    playerInput = GetComponent<PlayerInput>();

    CinemachineFreeLook cinemachineCam = FindObjectOfType<CinemachineFreeLook>();
    if (cinemachineCam != null)
    {
        cinemachineCam.Follow = transform;
        cinemachineCam.LookAt = transform;
    }
    else
    {
        Debug.LogError("Cinemachine Free Look não encontrado na cena.");
    }

    if (Camera.main != null)
    {
        cameraTransform = Camera.main.transform;
    }
    else
    {
        Debug.LogError("Camera principal não encontrada.");
    }
}


    private void Update()
    {
        if (!photonView.IsMine) return; // Executa o movimento apenas para o primeiro jogador

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movimentoInput.x, 0, movimentoInput.y);
        move = move.x * cameraTransform.right + move.z * cameraTransform.forward;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        bool isWalking = movimentoInput.sqrMagnitude > 0.1f;
        anim.SetBool("isWalking", isWalking);

        if (isWalking && move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return; // Garante que apenas o primeiro jogador manipule o input

        movimentoInput = context.ReadValue<Vector2>();
    }
}
