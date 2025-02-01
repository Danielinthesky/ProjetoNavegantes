using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerEscalada : MonoBehaviour
{
    private Rigidbody rb;
    public LayerMask climbableLayer; // Camada para superfícies escaláveis
    public Transform pontoRaio; // Objeto vazio para origem do raio
    private Animator animador;
    private bool bloqueandoTransicoes = false; // Bloqueia mudanças de animação enquanto a animação do topo está ativa

    public float velocidadeEscalada = 2f; // Velocidade de movimento vertical ao escalar
    private bool estaEscalando = false;
    private Vector2 entradaMovimento;

    // Variáveis para o som de escalada definidas via Inspetor
    public AudioClip somEscalada;         // AudioClip a ser reproduzido durante a escalada
    public AudioSource audioClimb;        // AudioSource que tocará o som

    private bool finalizandoEscalada = false; // Flag para evitar múltiplas chamadas da corrotina

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animador = GetComponent<Animator>();

        // Opcional: se desejar, já atribua o clip e configure o AudioSource para loop
        if (audioClimb != null && somEscalada != null)
        {
            audioClimb.clip = somEscalada;
            audioClimb.loop = true;
        }
    }

    void FixedUpdate()
    {
        if (bloqueandoTransicoes)
            return; // Impede qualquer atualização enquanto bloqueado

        if (estaEscalando)
        {
            ControlarEscalada();

            // Verifica se o raio não detecta mais a superfície escalável
            if (!DetectarSuperficieEscalavel() && !finalizandoEscalada)
            {
                StartCoroutine(AguardarAntesDeFinalizarEscalada());
            }
        }
        else
        {
            // Se não estiver escalando, garante que o som esteja parado
            if (audioClimb != null && audioClimb.isPlaying)
            {
                audioClimb.Stop();
            }
        }
    }

    public void IniciarEscalada()
    {
        estaEscalando = true;
        rb.velocity = Vector3.zero;        // Remove movimento residual
        rb.angularVelocity = Vector3.zero;   // Remove rotação residual
        rb.useGravity = false;               // Desativa gravidade

        animador.SetBool("EstaEscalando", true); // Ativa a escalada no Animator
        animador.speed = 1;                      // Garante que a animação esteja ativa inicialmente

        Debug.Log("Escalada iniciada");
        GetComponent<PlayerController>().AtivarEscalada();

        // Opcional: garanta que o AudioSource esteja preparado para tocar o clip
        if (audioClimb != null && somEscalada != null)
        {
            audioClimb.clip = somEscalada;
            audioClimb.loop = true;
        }
    }

    public void FinalizarEscalada()
    {
        estaEscalando = false;
        rb.useGravity = true; // Reativa a gravidade

        animador.SetBool("EstaEscalando", false); // Desativa a escalada no Animator
        animador.speed = 1;                      // Garante que a animação volte ao normal

        // Interrompe o som, se estiver tocando
        if (audioClimb != null && audioClimb.isPlaying)
        {
            audioClimb.Stop();
        }

        Debug.Log("Escalada finalizada");
        GetComponent<PlayerController>().AtivarMovimento();
    }

    public void OnMover(Vector2 movimento)
    {
        entradaMovimento = movimento;
    }

    private void ControlarEscalada()
    {
        ManterNaSuperficie();

        // Calcula o movimento vertical e horizontal com base na entrada do jogador
        float movimentoVertical = entradaMovimento.y * velocidadeEscalada * Time.fixedDeltaTime;
        float movimentoHorizontal = entradaMovimento.x * velocidadeEscalada * Time.fixedDeltaTime;

        // Cria a nova posição combinando movimento vertical e horizontal
        Vector3 novaPosicao = rb.position + new Vector3(movimentoHorizontal, movimentoVertical, 0);
        rb.MovePosition(novaPosicao);

        // Atualiza a animação e o som de acordo com a entrada do jogador
        if (Mathf.Abs(entradaMovimento.y) > 0.1f || Mathf.Abs(entradaMovimento.x) > 0.1f)
        {
            animador.SetFloat("VelocidadeEscalada", entradaMovimento.y); // Define a direção vertical da escalada
            animador.SetFloat("DirecaoEscalada", entradaMovimento.x);     // Define a direção horizontal da escalada
            animador.speed = 1;  // A animação avança

            // Se houver movimento, toca o som, caso ainda não esteja tocando
            if (audioClimb != null && somEscalada != null && !audioClimb.isPlaying)
            {
                audioClimb.Play();
            }
        }
        else
        {
            // Se não houver movimento, pausa a animação e interrompe o som
            animador.speed = 0;
            if (audioClimb != null && audioClimb.isPlaying)
            {
                audioClimb.Stop();
            }
        }
    }

    private bool DetectarSuperficieEscalavel()
    {
        if (pontoRaio == null)
        {
            Debug.LogError("Ponto de raio não atribuído!");
            return false;
        }

        Vector3 origem = pontoRaio.position;
        Vector3 direcao = (transform.forward + Vector3.down * 0.5f).normalized;

        // Raycast para detectar a superfície escalável
        if (Physics.Raycast(origem, direcao, out RaycastHit hit, 2.5f, climbableLayer))
        {
            // Verifica se a superfície atingida é plana (normal próxima de Vector3.up)
            if (Vector3.Angle(hit.normal, Vector3.up) < 45f)
            {
                Debug.Log("Superfície plana detectada. Movendo para o topo.");
                MoverParaSuperficiePlana(hit.point);
            }
            return true;
        }

        Debug.Log("Escalada: Nenhuma superfície escalável detectada.");
        return false;
    }

    private void ManterNaSuperficie()
    {
        Vector3 origemRaio = transform.position + transform.forward * 0.5f;
        Vector3 direcaoRaio = -transform.forward;

        if (Physics.Raycast(origemRaio, direcaoRaio, out RaycastHit hit, 2f, climbableLayer))
        {
            Vector3 posicaoAjustada = hit.point + hit.normal * 0.1f;
            rb.position = new Vector3(posicaoAjustada.x, rb.position.y, posicaoAjustada.z);
        }
        else
        {
            Debug.LogWarning("O personagem está se afastando da superfície escalável!");
        }
    }

    private void MoverParaSuperficiePlana(Vector3 pontoDestino)
    {
        Vector3 novaPosicao = new Vector3(pontoDestino.x, pontoDestino.y, pontoDestino.z);
        rb.MovePosition(novaPosicao);

        FinalizarEscalada();
    }

    private void OnDrawGizmos()
    {
        if (pontoRaio != null)
        {
            Vector3 direcao = (transform.forward + Vector3.down * 0.5f).normalized;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(pontoRaio.position, direcao * 2.5f);
        }
    }

    public bool EstaEscalando()
    {
        return estaEscalando;
    }

    private IEnumerator AguardarAntesDeFinalizarEscalada()
    {
        finalizandoEscalada = true;
        yield return new WaitForSeconds(1f);

        if (!DetectarSuperficieEscalavel())
        {
            Debug.Log("Finalizando escalada após 1s");
            FinalizarEscalada();
        }
        finalizandoEscalada = false;
    }
}
