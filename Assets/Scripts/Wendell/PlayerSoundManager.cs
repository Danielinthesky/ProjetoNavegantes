using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    // Referência ao PlayerMovimento
    public PlayerMovimento playerMovimento;

    // AudioSources configurados manualmente no Inspector
    public AudioSource fonteMusica; // Para tocar músicas
    public AudioSource fonteInteracao; // Para efeitos de interação
    public AudioSource fontePassosChao; // Para o som contínuo do chão
    public AudioSource fonteMadeira; // Para o som contínuo da madeira

    // Listas de áudios
    public AudioClip[] musicasFundo; // Músicas de fundo
    public AudioClip somChao; // Som contínuo do chão
    public AudioClip somMadeira; // Som contínuo da madeira
    public AudioClip[] sonsInteracao; // Sons de interação

    // Variáveis de controle
    private float pitchNormal = 1.0f; // Pitch normal (velocidade padrão)
    private float pitchCorrendo = 1.5f; // Pitch acelerado (velocidade de corrida)

    private void Start()
    {
        // Toca a primeira música de fundo ao iniciar o jogo
        if (musicasFundo.Length > 0 && fonteMusica != null)
        {
            fonteMusica.clip = musicasFundo[0]; // Define a primeira música do vetor
            fonteMusica.Play(); // Toca a música
        }
    }

    private void Update()
    {
        // Controla o som contínuo do chão e da madeira
        ControlarSomChao();
        ControlarSomMadeira();
    }

    // Método para tocar música de fundo
    public void TocarMusicaFundo(int indice)
    {
        if (fonteMusica != null && musicasFundo.Length > indice)
        {
            fonteMusica.clip = musicasFundo[indice];
            fonteMusica.Play();
        }
    }

    // Método para parar a música de fundo
    public void PararMusicaFundo()
    {
        if (fonteMusica != null)
        {
            fonteMusica.Stop();
        }
    }

    // Método para tocar efeito de interação
    public void TocarSomInteracao(int indice)
    {
        if (fonteInteracao != null && sonsInteracao.Length > indice)
        {
            fonteInteracao.clip = sonsInteracao[indice];
            fonteInteracao.Play();
        }
    }

    // Método para controlar o som contínuo do chão
    private void ControlarSomChao()
    {
        if (!playerMovimento.EstaNoChao || !playerMovimento.EstaMovendo)
        {
            // Para o som do chão se o personagem não estiver se movendo ou não estiver no chão
            if (fontePassosChao != null && fontePassosChao.isPlaying)
            {
                fontePassosChao.Stop();
            }
            return;
        }

        // Obtém o objeto detectado pelo CheckSphere
        Collider[] colisores = Physics.OverlapSphere(playerMovimento.groundCheck.position, playerMovimento.groundCheckRadius, playerMovimento.groundLayer);
        if (colisores.Length > 0)
        {
            Collider colisor = colisores[0]; // Pega o primeiro colisor detectado
            string superficie = colisor.tag; // Obtém a tag do objeto colidido

            // Toca o som contínuo do chão
            if (superficie.ToLower() == "chao" && somChao != null && fontePassosChao != null)
            {
                if (!fontePassosChao.isPlaying)
                {
                    fontePassosChao.clip = somChao;
                    fontePassosChao.loop = true; // Reproduz em loop
                    fontePassosChao.Play();
                }

                // Ajusta o pitch com base na velocidade (correndo ou não)
                fontePassosChao.pitch = playerMovimento.EstaCorrendo ? pitchCorrendo : pitchNormal;
            }
            else
            {
                // Para o som do chão se não estiver no chão
                if (fontePassosChao != null && fontePassosChao.isPlaying)
                {
                    fontePassosChao.Stop();
                }
            }
        }
    }

    // Método para controlar o som contínuo da madeira
    private void ControlarSomMadeira()
    {
        if (!playerMovimento.EstaNoChao || !playerMovimento.EstaMovendo)
        {
            // Para o som da madeira se o personagem não estiver se movendo ou não estiver no chão
            if (fonteMadeira != null && fonteMadeira.isPlaying)
            {
                fonteMadeira.Stop();
            }
            return;
        }

        // Obtém o objeto detectado pelo CheckSphere
        Collider[] colisores = Physics.OverlapSphere(playerMovimento.groundCheck.position, playerMovimento.groundCheckRadius, playerMovimento.groundLayer);
        if (colisores.Length > 0)
        {
            Collider colisor = colisores[0]; // Pega o primeiro colisor detectado
            string superficie = colisor.tag; // Obtém a tag do objeto colidido

            // Toca o som contínuo da madeira
            if (superficie.ToLower() == "madeira" && somMadeira != null && fonteMadeira != null)
            {
                if (!fonteMadeira.isPlaying)
                {
                    fonteMadeira.clip = somMadeira;
                    fonteMadeira.loop = true; // Reproduz em loop
                    fonteMadeira.Play();
                }

                // Ajusta o pitch com base na velocidade (correndo ou não)
                fonteMadeira.pitch = playerMovimento.EstaCorrendo ? pitchCorrendo : pitchNormal;
            }
            else
            {
                // Para o som da madeira se não estiver na madeira
                if (fonteMadeira != null && fonteMadeira.isPlaying)
                {
                    fonteMadeira.Stop();
                }
            }
        }
    }
}