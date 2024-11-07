using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook cameraLivre;
    public float sensibilidade = 1.0f;

    private Vector2 inputDoStickDireito;

    // Método para capturar o input do stick direito
    public void AoMoverStickDireito(InputValue valor)
    {
        inputDoStickDireito = valor.Get<Vector2>();
    }

    private void Update()
    {
        if (cameraLivre != null)
        {
            // Aplica a sensibilidade à câmera livre do Cinemachine
            cameraLivre.m_XAxis.Value += inputDoStickDireito.x * sensibilidade;
            cameraLivre.m_YAxis.Value += inputDoStickDireito.y * sensibilidade;
        }
    }
}
