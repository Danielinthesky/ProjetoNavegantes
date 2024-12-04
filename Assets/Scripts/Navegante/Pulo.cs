using UnityEngine;

public class PuloPersonagem : MonoBehaviour
{
    [Header("Configurações de Pulo")]
    public float alturaPulo = 2f; // Altura do pulo
    public float gravidade = -9.81f; // Força da gravidade
    public float distanciaChecarChao = 0.2f; // Distância para verificar o chão
    public LayerMask camadaChao; // Camada para detectar o chão

    private CharacterController controladorPersonagem; // Referência ao CharacterController
    private Animator animador; // Referência ao Animator
    private Vector3 velocidade; // Controle da gravidade e movimento vertical
    private bool estaNoChao; // Verifica se o personagem está no chão

    void Awake()
    {
        // Obtém automaticamente o CharacterController do GameObject
        controladorPersonagem = GetComponent<CharacterController>();
        if (controladorPersonagem == null)
        {
            Debug.LogError("CharacterController não encontrado neste GameObject!");
        }

        // Obtém automaticamente o Animator do GameObject
        animador = GetComponent<Animator>();
        if (animador == null)
        {
            Debug.LogError("Animator não encontrado neste GameObject!");
        }
    }

    void Update()
    {
        VerificarChao();
        AplicarGravidade();
        AtualizarAnimacao();
    }

    private void VerificarChao()
    {
        // Verifica se o personagem está no chão
        estaNoChao = Physics.CheckSphere(transform.position, distanciaChecarChao, camadaChao);

        // Se o personagem estiver no chão, zera a velocidade vertical
        if (estaNoChao && velocidade.y < 0)
        {
            velocidade.y = -2f; // Mantém o personagem colado ao chão
        }
    }

    private void AplicarGravidade()
    {
        // Aplica a gravidade continuamente
        velocidade.y += gravidade * Time.deltaTime;

        // Move o personagem no eixo Y usando o CharacterController
        controladorPersonagem.Move(velocidade * Time.deltaTime);
    }

    public void Pular()
    {
        if (estaNoChao)
        {
            // Aciona a animação de pulo antes do movimento
            if (animador != null)
            {
                animador.SetTrigger("Jump");
            }

            // Calcula a velocidade vertical necessária para o pulo
            velocidade.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }
    }

    private void AtualizarAnimacao()
    {
        if (animador != null)
        {
            // Define se o personagem está no chão para a transição de animações
            animador.SetBool("NoChao", estaNoChao);
        }
    }
}
