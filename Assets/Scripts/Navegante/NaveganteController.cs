using UnityEngine;

public class NaveganteController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public DynamicJoystick joystick; // Componente de joystick no inspector.
    public Animator animator; // Componente Animator do personagem.

    private Vector3 moveDirection;

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Movimento do personagem baseado no joystick
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            // Calcula a direção de movimento em relação ao input do joystick
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;

            // Move o personagem na direção calculada
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            // Rotaciona o personagem na direção do movimento
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
            }

            // Ativa a animação de caminhada e ajusta a direção
            animator.SetBool("isWalking", true);
            animator.SetFloat("MoveX", moveDirection.x);
            animator.SetFloat("MoveY", moveDirection.z);
        }
        else
        {
            // Ativa a animação de idle quando o personagem para de se mover
            animator.SetBool("isWalking", false);
        }
    }
}
