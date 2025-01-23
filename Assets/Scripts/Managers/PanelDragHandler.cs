using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Cinemachine;

public class PanelDragHandler : MonoBehaviour
{
    public CinemachineFreeLook cameraAtiva; // Referência à Cinemachine FreeLook
    public float velocidadeRotacao = 150f; // Velocidade de rotação horizontal
    public float suavizacao = 10f; // Suavização do movimento

    private bool isArrastando = false; // Verifica se o painel está sendo arrastado
    private int idToquePainel = -1; // ID do toque ativo no painel
    private float velocidadeRotacaoSuave = 0f; // Velocidade de rotação suavizada

    public void OnCameraDrag(InputAction.CallbackContext context)
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count <= 0) return;

        foreach (var toque in Touchscreen.current.touches)
        {
            Vector2 posicaoToque = toque.position.ReadValue();

            // Detecta início do toque no painel
            if (context.started && PointerSobrePainel(posicaoToque) && idToquePainel == -1)
            {
                idToquePainel = toque.touchId.ReadValue();
                isArrastando = true;
            }

            // Atualiza o valor da rotação com base no movimento do dedo
            if (isArrastando && idToquePainel == toque.touchId.ReadValue() && context.performed)
            {
                Vector2 movimentoDelta = toque.delta.ReadValue();
                velocidadeRotacaoSuave = movimentoDelta.x * velocidadeRotacao * Time.deltaTime;
            }

            // Cancela o arrasto ao final do toque
            if (idToquePainel == toque.touchId.ReadValue() && context.canceled)
            {
                idToquePainel = -1;
                isArrastando = false;
                velocidadeRotacaoSuave = 0f; // Zera a velocidade ao finalizar
            }
        }
    }

    private void Update()
    {
        if (cameraAtiva == null) return;

        // Suaviza a rotação no eixo X usando Lerp
        cameraAtiva.m_XAxis.Value = Mathf.Lerp(cameraAtiva.m_XAxis.Value, cameraAtiva.m_XAxis.Value + velocidadeRotacaoSuave, Time.deltaTime * suavizacao);
    }

    private bool PointerSobrePainel(Vector2 posicaoToque)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = posicaoToque
        };

        var resultados = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, resultados);

        foreach (var resultado in resultados)
        {
            if (resultado.gameObject == gameObject) // Detecta se o toque está no painel
            {
                return true;
            }
        }

        return false;
    }
}
