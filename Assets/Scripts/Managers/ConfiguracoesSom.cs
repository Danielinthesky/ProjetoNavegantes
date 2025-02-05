using UnityEngine;
using UnityEngine.UI;

public class ConfiguracoesSom : MonoBehaviour
{
    // Referência ao AudioSource da música de fundo
    public AudioSource fonteMusica;

    // Vetor de AudioSources para efeitos sonoros
    public AudioSource[] fontesEfeitos;

    // Referências aos Sliders
    public Slider sliderMusica;
    public Slider sliderEfeitos;
    public GameObject painelConfiguracoes;

    private void OnEnable()
    {
        // Atualiza os valores dos sliders sempre que a tela de configurações é aberta
        AtualizarSliders();
    }

    private void Start()
    {
        // Configura os listeners dos sliders
        ConfigurarSliders();
    }

    // Método para configurar os listeners dos sliders
    private void ConfigurarSliders()
    {
        if (sliderMusica != null)
        {
            sliderMusica.onValueChanged.AddListener(AtualizarVolumeMusica);
        }

        if (sliderEfeitos != null)
        {
            sliderEfeitos.onValueChanged.AddListener(AtualizarVolumeEfeitos);
        }
    }

    // Método para atualizar os valores dos sliders
    private void AtualizarSliders()
    {
        if (sliderMusica != null && fonteMusica != null)
        {
            sliderMusica.value = fonteMusica.volume;
        }

        if (sliderEfeitos != null && fontesEfeitos.Length > 0)
        {
            // Define o valor do slider de efeitos com base no volume do primeiro AudioSource
            sliderEfeitos.value = fontesEfeitos[0].volume;
        }
    }

    // Método para atualizar o volume da música de fundo
    private void AtualizarVolumeMusica(float volume)
    {
        if (fonteMusica != null)
        {
            fonteMusica.volume = volume;
        }
    }

    // Método para atualizar o volume dos efeitos sonoros
    private void AtualizarVolumeEfeitos(float volume)
    {
        foreach (AudioSource fonte in fontesEfeitos)
        {
            if (fonte != null)
            {
                fonte.volume = volume;
            }
        }
    }

    public void AlternarConfiguracoesSom()
    {
        if (painelConfiguracoes != null)
        {
            // Inverte o estado de ativação do painel (abre/fecha)
            painelConfiguracoes.SetActive(!painelConfiguracoes.activeSelf);

            // Pausa o jogo se o painel estiver aberto
            Time.timeScale = painelConfiguracoes.activeSelf ? 0f : 1f;
        }
    }
}