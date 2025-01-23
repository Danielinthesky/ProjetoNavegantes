using UnityEngine;
using Photon.Pun;

public class PuloPersonagemMultiplayer : MonoBehaviourPun
{
    [Header("Configurações de Pulo")]
    public float alturaPulo = 2f;
    public float gravidade = -9.81f;
    public float distanciaChecarChao = 0.2f;
    public LayerMask camadaChao;

    private CharacterController controladorPersonagem;
    private Animator animador;
    private Vector3 velocidade;
    private bool estaNoChao;

    void Awake()
    {
        controladorPersonagem = GetComponent<CharacterController>();
        if (controladorPersonagem == null)
        {
            Debug.LogError("CharacterController não encontrado neste GameObject!");
        }

        animador = GetComponent<Animator>();
        if (animador == null)
        {
            Debug.LogError("Animator não encontrado neste GameObject!");
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        VerificarChao();
        AplicarGravidade();
        AtualizarAnimacao();
    }

    private void VerificarChao()
    {
        estaNoChao = Physics.CheckSphere(transform.position, distanciaChecarChao, camadaChao);

        if (estaNoChao && velocidade.y < 0)
        {
            velocidade.y = -2f;
        }
    }

    private void AplicarGravidade()
    {
        velocidade.y += gravidade * Time.deltaTime;
        controladorPersonagem.Move(velocidade * Time.deltaTime);
    }

    public void Pular()
    {
        if (estaNoChao)
        {
            photonView.RPC("SincronizarPulo", RpcTarget.All);

            velocidade.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
        }
    }

    [PunRPC]
    private void SincronizarPulo()
    {
        if (animador != null)
        {
            animador.SetTrigger("Jump");
        }
    }

    private void AtualizarAnimacao()
    {
        if (animador != null)
        {
            animador.SetBool("NoChao", estaNoChao);
        }
    }
}
