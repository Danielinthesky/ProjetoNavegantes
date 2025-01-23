using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float horizontalSpeed = 5f;
    public float climbSpeed = 3f;
    public float rayDistance = 1.5f;
    public LayerMask climbableLayer;

    private Rigidbody rb;
    private Vector3 inputDirection;
    private MovementState currentState = MovementState.Horizontal;

    private enum MovementState
    {
        Horizontal,
        Vertical
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Gravidade sempre ativa, desativada automaticamente na escalada
    }

    void FixedUpdate()
    {
        DetectSurface();
        MoveCharacter();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDirection = new Vector3(input.x, 0, input.y).normalized;
    }

    private void DetectSurface()
{
    RaycastHit hit;

    // Inclinação para baixo e para trás
    Vector3 rayDirection = Quaternion.Euler(120f, 180f, 0f) * transform.forward;

    // Detecta superfície inclinada ou vertical
    if (Physics.Raycast(transform.position, rayDirection, out hit, rayDistance, climbableLayer))
    {
        // Log do objeto atingido pelo raio inclinado
        Debug.Log($"Raio Inclinado Vermelho Tocou: {hit.collider.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

        Vector3 surfaceNormal = hit.normal;
        float angle = Vector3.Angle(surfaceNormal, Vector3.up);

        if (angle > 45 && angle < 135) // Superfície vertical
        {
            currentState = MovementState.Vertical;
            rb.useGravity = false;
            return;
        }
    }
    else
    {
        // Se o raio inclinado não detectar nada escalável, volta ao movimento horizontal
        if (currentState == MovementState.Vertical)
        {
            Debug.Log("Fim da superfície escalável detectado. Transição para movimento horizontal.");
            currentState = MovementState.Horizontal;
            rb.useGravity = true;
        }
    }

    // Detecta chão ou fim de superfície escalável
    if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
    {
        // Log do objeto atingido pelo raio para baixo
        Debug.Log($"Raio Azul para Baixo Tocou: {hit.collider.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");

        if (Vector3.Angle(hit.normal, Vector3.up) < 45) // Superfície horizontal
        {
            currentState = MovementState.Horizontal;
            rb.useGravity = true;
        }
    }
}




    private void MoveCharacter()
    {
        switch (currentState)
        {
            case MovementState.Horizontal:
                MoveHorizontal();
                break;

            case MovementState.Vertical:
                ClimbVertical();
                break;
        }
    }

    private void MoveHorizontal()
{
    Vector3 targetVelocity = new Vector3(inputDirection.x, 0, inputDirection.z) * horizontalSpeed;
    Vector3 velocityChange = targetVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z);
    rb.AddForce(velocityChange, ForceMode.VelocityChange);
}



    private void ClimbVertical()
    {
        Vector3 climb = new Vector3(0, inputDirection.z, 0) * climbSpeed;
        rb.velocity = new Vector3(0, climb.y, 0);
    }

  private void OnDrawGizmos()
{
    // Inclinação para baixo e para trás (oposto ao forward)
    Vector3 rayDirection = Quaternion.Euler(120f, 180f, 0f) * transform.forward;

    // Desenha o raio inclinado (reflete o Raycast inclinado)
    Gizmos.color = Color.red;
    Gizmos.DrawRay(transform.position, rayDirection * rayDistance);

    // Desenha o raio para baixo (opcional, para detectar chão)
    Gizmos.color = Color.blue;
    Gizmos.DrawRay(transform.position, Vector3.down * rayDistance);
}



}
