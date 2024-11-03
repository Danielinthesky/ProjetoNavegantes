// using UnityEngine;
// using UnityEngine.EventSystems;

// public class DirectJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
// {
//     public RectTransform background;
//     public RectTransform handle;
//     public RaftController boatController;

//     private Vector2 initialPosition;
//     public float joystickSensitivity = 1.0f;
//     private float dpiScaleFactor;
//     public bool isDragging { get; private set; } // Propriedade pública para verificar o estado de arrasto

//     void Start()
//     {
//         initialPosition = handle.anchoredPosition;

//         dpiScaleFactor = Screen.dpi / 160f;
//         if (dpiScaleFactor == 0)
//         {
//             dpiScaleFactor = 1;
//         }
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         isDragging = true; // Define como arrastando quando começa a interação

//         Vector2 position = Vector2.zero;
//         RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position);

//         position *= joystickSensitivity / dpiScaleFactor;
//         float clampedX = Mathf.Clamp(position.x, -background.rect.width / 2, background.rect.width / 2);
//         handle.anchoredPosition = new Vector2(clampedX, initialPosition.y);

//         float normalizedValue = clampedX / (background.rect.width / 2);

//         if (boatController != null)
//         {
//             boatController.DefinirEntradaJoystick(normalizedValue);
//         }
//     }

//     public void OnEndDrag(PointerEventData eventData)
//     {
//         isDragging = false; // Define como não arrastando quando o toque termina
//         handle.anchoredPosition = initialPosition;

//         if (boatController != null)
//         {
//             boatController.DefinirEntradaJoystick(0);
//         }
//     }
// }
