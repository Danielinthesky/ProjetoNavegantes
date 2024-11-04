using UnityEngine;
using UnityEngine.EventSystems;

public class DirectJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform fundo;
    public RectTransform alavanca;
    public RaftController controladorJangada;

    private Vector2 posicaoInicial;
    public float sensibilidadeJoystick = 1.0f;
    private float fatorEscalaDpi;
    public bool estaArrastando { get; private set; }

    void Start()
    {
        posicaoInicial = alavanca.anchoredPosition;

        fatorEscalaDpi = Screen.dpi / 160f;
        if (fatorEscalaDpi == 0)
        {
            fatorEscalaDpi = 1;
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
            // controladorJangada.DefinirEntradaJoystick(valorNormalizado);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        estaArrastando = false;
        alavanca.anchoredPosition = posicaoInicial;

        if (controladorJangada != null)
        {
            // controladorJangada.DefinirEntradaJoystick(0);
        }
    }
}
