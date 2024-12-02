using UnityEngine;
using UnityEngine.EventSystems;

public class DirectJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform fundo;
    public RectTransform alavanca;
    public JangadaVelejadora controladorJangada;

    private Vector2 posicaoInicial;
    public float sensibilidadeJoystick = 1.0f;
    private float fatorEscalaDpi;
    public bool estaArrastando { get; private set; }
    public GameObject joystickCanvas; // O joystick Canvas será ativado/desativado com proximidade

    public Transform player; // Referência ao jogador
    public Transform vela; // Referência à vela
    public float distanciaAtivacao = 2.0f; // Distância para ativar o joystick
    public float anguloVisaoAtivacao = 45.0f; // Ângulo de visão necessário para ativação

    void Start()
    {
        posicaoInicial = alavanca.anchoredPosition;

        fatorEscalaDpi = Screen.dpi / 160f;
        if (fatorEscalaDpi == 0)
        {
            fatorEscalaDpi = 1;
        }

        joystickCanvas.SetActive(false); // O joystick começa desativado
    }

    void Update()
    {
        VerificarProximidadeEVisao();
    }

    private void VerificarProximidadeEVisao()
    {
        Vector3 direcaoParaVela = vela.position - player.position;
        float distancia = direcaoParaVela.magnitude;

        // Verifica se o jogador está dentro da distância de ativação
        if (distancia <= distanciaAtivacao)
        {
            // Calcula o ângulo entre a frente do jogador e a direção da vela
            float angulo = Vector3.Angle(player.forward, direcaoParaVela);

            // Verifica se o jogador está olhando para a vela
            if (angulo <= anguloVisaoAtivacao)
            {
                joystickCanvas.SetActive(true); // Ativa o joystick
            }
            else
            {
                joystickCanvas.SetActive(false); // Desativa o joystick
            }
        }
        else
        {
            joystickCanvas.SetActive(false); // Desativa o joystick
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        estaArrastando = true;

        Vector2 posicao = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(fundo, eventData.position, eventData.pressEventCamera, out posicao);

        posicao *= sensibilidadeJoystick / fatorEscalaDpi;
        float clampedX = Mathf.Clamp(posicao.x, -fundo.rect.width / 2, fundo.rect.width / 2);
        alavanca.anchoredPosition = new Vector2(clampedX, posicaoInicial.y);

        float valorNormalizado = clampedX / (fundo.rect.width / 2);

        if (controladorJangada != null)
        {
            controladorJangada.DefinirEntradaJoystick(valorNormalizado); // Ajusta a rotação
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        estaArrastando = false;
        alavanca.anchoredPosition = posicaoInicial;

        if (controladorJangada != null)
        {
            controladorJangada.DefinirEntradaJoystick(0); // Reseta a rotação
        }
    }
}
