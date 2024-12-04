using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MovimentoPersonagem : MonoBehaviour
{
    [Header("Configurações de Velocidade")]
    public float velocidadeBase = 5f; // Velocidade normal de andar
    public float incrementoVelocidade = 2f; // Valor que aumenta ao tocar no painel
    public float velocidadeMaxima = 10f; // Velocidade máxima
    public float taxaDecremento = 1f; // Velocidade com que a velocidade diminui por segundo

    [Header("Configurações de Stamina")]
    public float staminaMaxima = 100f; // Valor máximo de stamina
    public float consumoStaminaPorToque = 10f; // Stamina consumida por toque
    public float recuperacaoStaminaMovendo = 5f; // Recuperação lenta ao se mover
    public float recuperacaoStaminaParado = 10f; // Recuperação rápida quando parado
    public float tempoEsperaRecuperacao = 5f; // Tempo de espera para começar a recuperar stamina
    public Slider barraStamina; // Referência ao Slider da UI

    [Header("Referências")]
    public CharacterController controladorPersonagem; // Referência ao CharacterController
    public Animator animador; // Referência ao Animator
    private Camera mainCamera; // Referência à câmera principal

    private Vector2 inputMovimento; // Entrada do joystick
    public float velocidadeAtual; // Velocidade atual do personagem
    private Vector3 direcaoMovimento; // Direção do movimento no espaço mundial
    private float staminaAtual; // Valor atual de stamina
    private bool emEsperaRecuperacao; // Indica se está no período de espera antes da recuperação
    private float tempoRestanteEspera; // Tempo restante de espera
    private bool podeAumentarVelocidade = true;


    void Awake()
    {
        // Obtém automaticamente o CharacterController e a câmera
        controladorPersonagem = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        animador = GetComponent<Animator>();

        if (controladorPersonagem == null)
            Debug.LogError("CharacterController não encontrado!");

        if (mainCamera == null)
            Debug.LogError("Câmera principal não encontrada!");

        if (animador == null)
            Debug.LogError("Animator não configurado!");

        // Inicializa a velocidade e stamina
        velocidadeAtual = velocidadeBase;
        staminaAtual = staminaMaxima;

        // Atualiza a barra de stamina
        AtualizarBarraStamina();
    }

    void Update()
    {
        if (emEsperaRecuperacao)
        {
            AtualizarEsperaRecuperacao();
        }
        else
        {
            AtualizarStamina();
        }

        AplicarDecrementoVelocidade();
        MoverPersonagem();
        AtualizarAnimacao();
    }

    public void OnMover(InputAction.CallbackContext context)
    {
        inputMovimento = context.ReadValue<Vector2>();
    }

    public void OnToqueNoPainel()
{
    // Verifica se o personagem pode correr (tem stamina, está em movimento e não atingiu a velocidade máxima)
    if (!emEsperaRecuperacao && staminaAtual > 0 && inputMovimento.magnitude > 0.1f && podeAumentarVelocidade)
    {
        // Incrementa a velocidade somente se ainda não atingiu a máxima
        if (velocidadeAtual < velocidadeMaxima)
        {
            velocidadeAtual = Mathf.Min(velocidadeAtual + incrementoVelocidade, velocidadeMaxima);

            // Consome stamina
            staminaAtual -= consumoStaminaPorToque;
            staminaAtual = Mathf.Max(0, staminaAtual); // Garante que não fique negativa

            // Inicia período de espera se a stamina acabar
            if (staminaAtual <= 0)
            {
                IniciarEsperaRecuperacao();
            }
        }

        // Bloqueia o incremento se a velocidade máxima foi atingida
        if (velocidadeAtual >= velocidadeMaxima)
        {
            podeAumentarVelocidade = false;
        }
    }
}




    private void AtualizarEsperaRecuperacao()
    {
        // Reduz o tempo de espera
        tempoRestanteEspera -= Time.deltaTime;

        // Se o tempo de espera terminar, inicia a recuperação
        if (tempoRestanteEspera <= 0)
        {
            emEsperaRecuperacao = false;
            tempoRestanteEspera = 0;
        }
    }

    private void AtualizarStamina()
    {
        if (staminaAtual < staminaMaxima)
        {
            if (direcaoMovimento.magnitude > 0.1f)
            {
                // Recuperação lenta enquanto se move
                staminaAtual += recuperacaoStaminaMovendo * Time.deltaTime;
            }
            else
            {
                // Recuperação rápida enquanto parado
                staminaAtual += recuperacaoStaminaParado * Time.deltaTime;
            }

            staminaAtual = Mathf.Min(staminaAtual, staminaMaxima); // Limita ao máximo
        }

        // Atualiza a barra de stamina
        AtualizarBarraStamina();
    }

    private void AtualizarBarraStamina()
    {
        if (barraStamina != null)
        {
            barraStamina.value = staminaAtual / staminaMaxima;
        }
    }

    private void AplicarDecrementoVelocidade()
{
    if (velocidadeAtual > velocidadeBase)
    {
        velocidadeAtual -= taxaDecremento * Time.deltaTime;
        velocidadeAtual = Mathf.Max(velocidadeAtual, velocidadeBase); // Não deixa passar abaixo da base
    }

    // Permite que o jogador aumente a velocidade novamente
    if (velocidadeAtual == velocidadeBase)
    {
        podeAumentarVelocidade = true;
    }
}




    private void MoverPersonagem()
    {
        if (mainCamera != null)
        {
            // Calcula a direção baseada na câmera
            Vector3 frente = mainCamera.transform.forward;
            Vector3 direita = mainCamera.transform.right;

            // Ignora a componente Y
            frente.y = 0f;
            direita.y = 0f;
            frente.Normalize();
            direita.Normalize();

            // Direção do movimento com base no joystick
            direcaoMovimento = frente * inputMovimento.y + direita * inputMovimento.x;

            if (direcaoMovimento.magnitude > 0.1f)
            {
                // Gira o personagem na direção do movimento
                Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoMovimento);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 10f);
            }

            // Move o personagem usando a velocidade atual
            controladorPersonagem.Move(direcaoMovimento * velocidadeAtual * Time.deltaTime);
        }
    }

    private void AtualizarAnimacao()
{
    if (animador != null)
    {
        // Verifica se o joystick está parado
        if (inputMovimento.magnitude <= 0.1f)
        {
            // Estado parado (Idle)
            animador.SetBool("isWalking", false);
            animador.SetBool("isRunning", false);
            animador.SetBool("isIdle", true);
            return;
        }

        // Define se o personagem está andando (velocidade igual à base)
        if (velocidadeAtual == velocidadeBase)
        {
            animador.SetBool("isWalking", true);
            animador.SetBool("isIdle", false);
            animador.SetBool("isRunning", false);
        }
        // Define se o personagem está correndo (velocidade acima da base)
        else if (velocidadeAtual > velocidadeBase)
        {
            animador.SetBool("isWalking", false);
            animador.SetBool("isIdle", false);
            animador.SetBool("isRunning", true);
        }
    }
}



    private void IniciarEsperaRecuperacao()
    {
        emEsperaRecuperacao = true;
        tempoRestanteEspera = tempoEsperaRecuperacao;
    }
}
