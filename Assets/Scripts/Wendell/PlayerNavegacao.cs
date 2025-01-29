using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerNavegacao : MonoBehaviour
{
    private RaftController raftController;

    [Header("Câmeras")]
    private CinemachineFreeLook freeLookPlayer;
    private CinemachineFreeLook freeLookRaft;

    [Header("UI")]
    public GameObject canvasMovimentacao;
    public GameObject canvasNavegacao;

    public bool Navegando { get; private set; }

    public void HandleTriggerEnter(Collider other)
    {
        if (other.CompareTag("Navegar"))
        {
            raftController = other.GetComponentInParent<RaftController>();
            if (raftController == null)
                Debug.LogError("RaftController não encontrado no objeto pai!");
        }
    }

    public void HandleTriggerExit(Collider other)
    {
        if (other.CompareTag("Navegar"))
            raftController = null;
    }

    public void HandleMover(InputAction.CallbackContext contexto)
    {
        if (raftController != null)
            raftController.Mover(contexto.ReadValue<Vector2>());
    }

    public void ToggleRaftMovement()
    {
        raftController?.ToggleMovement();
    }
}
