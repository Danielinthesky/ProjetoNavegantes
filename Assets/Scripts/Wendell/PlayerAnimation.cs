// AnimationManager.cs
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animador;

    public enum EstadoJogador
    {
        Parado,
        Andando,
        Correndo,
        Caindo,
        Nadando,
        ParadoNadando,
        CairDeAltura
    }

    public EstadoJogador EstadoAtual { get; private set; }

    void Awake()
    {
        animador = GetComponent<Animator>();
    }

    public void AlterarEstado(EstadoJogador novoEstado)
    {
        EstadoAtual = novoEstado;
        AtualizarAnimator();
    }

    private void AtualizarAnimator()
    {
        // Resetar todos os parâmetros
        animador.SetBool("Parado", false);
        animador.SetBool("Andando", false);
        animador.SetBool("Correndo", false);
        animador.SetBool("Caindo", false);
        animador.SetBool("Nadando", false);
        animador.SetBool("ParadoNadando", false);

        // Ativar o estado atual
        switch (EstadoAtual)
        {
            case EstadoJogador.Parado:
                animador.SetBool("Parado", true);
                break;
            case EstadoJogador.Andando:
                animador.SetBool("Andando", true);
                break;
            case EstadoJogador.Correndo:
                animador.SetBool("Correndo", true);
                break;
            case EstadoJogador.Caindo:
                animador.SetBool("Caindo", true);
                break;
            case EstadoJogador.CairDeAltura:
                animador.SetBool("CairDeAltura", true);
                break;

            case EstadoJogador.Nadando:
                animador.SetBool("Nadando", true);
                break;
            case EstadoJogador.ParadoNadando:
                animador.SetBool("ParadoNadando", true);
                break;
        }

        Debug.Log($"Estado atualizado para: {EstadoAtual}");
    }



}
