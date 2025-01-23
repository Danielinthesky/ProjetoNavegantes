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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animador = GetComponent<Animator>();
    }

    void FixedUpdate()
{
    if (bloqueandoTransicoes)
    {
        return; // Impede qualquer atualização enquanto bloqueado
    }

    if (estaEscalando)
    {
        ControlarEscalada();

        // Verifica se o raio não detecta mais a superfície escalável
        if (!DetectarSuperficieEscalavel() && !finalizandoEscalada)
        {
            StartCoroutine(AguardarAntesDeFinalizarEscalada());
        }
    }
}





    public void IniciarEscalada()
{
    estaEscalando = true;
    rb.velocity = Vector3.zero; // Remove movimento residual
    rb.angularVelocity = Vector3.zero; // Remove rotação residual
    rb.useGravity = false; // Desativa gravidade

    animador.SetBool("EstaEscalando", true); // Ativa a escalada no Animator
    animador.speed = 1; // Garante que a animação esteja ativa inicialmente

    Debug.Log("Escalada iniciada");
}


 public void FinalizarEscalada()
{
    estaEscalando = false;
    rb.useGravity = true; // Reativa gravidade

    animador.SetBool("EstaEscalando", false); // Desativa a escalada no Animator
    animador.speed = 1; // Garante que a animação volte ao normal

    Debug.Log("Escalada finalizada");
}





    public void OnMover(Vector2 movimento)
    {
        entradaMovimento = movimento;
    }

    private void ControlarEscalada()
{
    float movimentoVertical = entradaMovimento.y * velocidadeEscalada * Time.fixedDeltaTime;
    Vector3 novaPosicao = rb.position + new Vector3(0, movimentoVertical, 0);
    rb.MovePosition(novaPosicao);

    // Atualiza a animação apenas se houver movimento vertical
    if (Mathf.Abs(entradaMovimento.y) > 0.1f)
    {
        animador.SetFloat("VelocidadeEscalada", entradaMovimento.y); // Define a direção da escalada
        animador.speed = 1; // A animação avança
    }
    else
    {
        animador.speed = 0; // Pausa a animação no último frame
    }
}


    private bool DetectarSuperficieEscalavel()
{
    if (pontoRaio == null)
    {
        Debug.LogError("Ponto de raio não atribuído!");
        return false;
    }

    Vector3 origem = pontoRaio.position; // Origem do raio
    Vector3 direcao = (transform.forward + Vector3.down * 0.5f).normalized; // Direção inclinada para baixo

    // Raycast para detectar a superfície escalável
    if (Physics.Raycast(origem, direcao, out RaycastHit hit, 2.5f, climbableLayer))
    {
        Debug.Log($"Escalando: Raycast atingiu {hit.collider.name}");

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





private void MoverParaSuperficiePlana(Vector3 pontoDestino)
{
    
    // Ajusta a posição do personagem para o ponto atingido
    Vector3 novaPosicao = new Vector3(pontoDestino.x, pontoDestino.y, pontoDestino.z);
    rb.MovePosition(novaPosicao);

    // Finaliza a escalada após mover para a superfície plana
    FinalizarEscalada();
}



    private void OnDrawGizmos()
{
    if (pontoRaio != null)
    {
        Vector3 direcao = (transform.forward + Vector3.down * 0.5f).normalized; // Inclinação para baixo
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pontoRaio.position, direcao * 2.5f);
    }
}




    public bool EstaEscalando()
{
    return estaEscalando;
}

private bool finalizandoEscalada = false; // Flag para evitar múltiplas chamadas da corrotina

private IEnumerator AguardarAntesDeFinalizarEscalada()
{
    finalizandoEscalada = true; // Impede chamadas repetidas
    yield return new WaitForSeconds(1f); // Aguarda 0.5 segundos

    if (!DetectarSuperficieEscalavel()) // Verifica novamente se ainda não há superfície
    {
        Debug.Log("Finalizando escalada após 0.5s");
        FinalizarEscalada();
    }

    finalizandoEscalada = false; // Libera a flag
}



}
