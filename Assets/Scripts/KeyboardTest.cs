using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardTest : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current != null)
        {
            Debug.Log("Teclado detectado!");
        }
        else
        {
            Debug.LogWarning("Teclado não detectado.");
        }
    }
}
