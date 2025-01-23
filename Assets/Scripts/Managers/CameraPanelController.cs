// using UnityEngine;
// using UnityEngine.EventSystems;

// public class CameraPanelController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
// {
//     public CameraLook cameraLook; // Referência ao script CameraLook
//     private bool isDragging = false;

//     public void OnPointerDown(PointerEventData eventData)
//     {
//         isDragging = true;
//         cameraLook.SetDragging(true);
//     }

//     public void OnPointerUp(PointerEventData eventData)
//     {
//         isDragging = false;
//         cameraLook.SetDragging(false);
//     }

//     public void OnDrag(PointerEventData eventData)
//     {
//         if (isDragging)
//         {
//             cameraLook.OnCameraDrag(eventData.delta);
//         }
//     }
// }
