using UnityEngine;
using Cinemachine;

public class CinemachinePanelInput : MonoBehaviour
{
    [Header("References")]
    public FixedTouchField fixedTouchField; // Referência ao FixedTouchField
    public CinemachineFreeLook freeLookCamera; // Referência ao Cinemachine Free Look Camera

    [Header("Settings")]
    public float xSensitivity = 0.1f; // Sensibilidade para o eixo X (horizontal)
    public float ySensitivity = 0.05f; // Sensibilidade para o eixo Y (vertical)
    public float yMinLimit = 10f; // Limite mínimo para o eixo Y
    public float yMaxLimit = 70f; // Limite máximo para o eixo Y

    private bool isPlayerMoving = false; // Indica se o personagem está se movendo

    void Update()
    {
        if (fixedTouchField != null && fixedTouchField.Pressed)
        {
            HandleCameraRotation();
        }
    }

    public void SetPlayerMovementState(bool isMoving)
    {
        // Atualiza o estado de movimento do personagem
        isPlayerMoving = isMoving;
    }

    private void HandleCameraRotation()
    {
        if (freeLookCamera != null)
        {
            // Sempre rotaciona o eixo X (horizontal)
            freeLookCamera.m_XAxis.Value += fixedTouchField.TouchDist.x * xSensitivity;

            // Apenas rotaciona o eixo Y (vertical) quando o personagem está parado
            if (!isPlayerMoving)
            {
                float newYValue = freeLookCamera.m_YAxis.Value - (fixedTouchField.TouchDist.y * ySensitivity);

                // Limita a rotação no eixo Y dentro dos limites
                freeLookCamera.m_YAxis.Value = Mathf.Clamp(newYValue, yMinLimit, yMaxLimit);
            }
        }
    }
}
