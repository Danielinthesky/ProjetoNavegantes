using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 TouchDist; // Delta do movimento do toque
    private Vector2 pointerOld; // Posição anterior do toque
    private bool pressed; // Status do toque
    private bool isTouchInsidePanel; // Verifica se o toque começou dentro do Panel

    public bool Pressed => pressed && isTouchInsidePanel; // Propriedade para verificar se o toque é válido

    void Update()
    {
        if (Pressed)
        {
            Vector2 pointerCurrent = Input.mousePosition; // Captura a posição atual do toque
            TouchDist = pointerCurrent - pointerOld; // Calcula o delta
            pointerOld = pointerCurrent; // Atualiza a posição anterior
        }
        else
        {
            TouchDist = Vector2.zero; // Zera o delta se o toque não for válido
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouchInsidePanel = RectTransformUtility.RectangleContainsScreenPoint(
            GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera
        );

        if (isTouchInsidePanel)
        {
            pressed = true;
            pointerOld = Input.mousePosition; // Captura a posição inicial do toque
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        isTouchInsidePanel = false;
    }
}
