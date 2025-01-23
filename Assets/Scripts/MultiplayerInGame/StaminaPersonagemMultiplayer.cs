using UnityEngine;
using UnityEngine.UI;

public class StaminaPersonagemMultiplayer : MonoBehaviour
{
    [Header("Configurações de Stamina")]
    public float staminaMaxima = 100f;
    public float consumoStaminaPorToque = 10f;
    public float recuperacaoStaminaMovendo = 5f;
    public float recuperacaoStaminaParado = 10f;
    public float tempoEsperaRecuperacao = 5f;
    public Slider barraStamina;

    private float staminaAtual;
    private bool emEsperaRecuperacao;
    private float tempoRestanteEspera;

    private PlayerControllerMultiplayer1 playerMovement;

    private void Start()
    {
        staminaAtual = staminaMaxima;
        playerMovement = GetComponent<PlayerControllerMultiplayer1>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement não encontrado no GameObject!");
        }

        // Procura automaticamente o Slider pela tag "StaminaSlider" se não estiver atribuído
        if (barraStamina == null)
        {
            GameObject sliderObj = GameObject.FindWithTag("StaminaSlider");
            if (sliderObj != null)
            {
                barraStamina = sliderObj.GetComponent<Slider>();
                if (barraStamina == null)
                {
                    Debug.LogError("O objeto com a tag 'StaminaSlider' não possui um componente Slider.", this);
                }
            }
            else
            {
                Debug.LogError("Nenhum objeto com a tag 'StaminaSlider' foi encontrado na cena.", this);
            }
        }

        AtualizarBarraStamina();
    }

    private void Update()
    {
        if (emEsperaRecuperacao)
        {
            AtualizarEsperaRecuperacao();
        }
        else
        {
            AtualizarStamina();
        }
    }

    public bool ConsumirStamina()
    {
        // Verifica se o personagem está se movendo antes de consumir stamina
        if (playerMovement.EstaMovendo() && staminaAtual >= consumoStaminaPorToque)
        {
            staminaAtual -= consumoStaminaPorToque;
            staminaAtual = Mathf.Max(0, staminaAtual);
            AtualizarBarraStamina();

            if (staminaAtual <= 0)
            {
                IniciarEsperaRecuperacao();
            }
            return true;
        }
        return false;
    }

    private void AtualizarEsperaRecuperacao()
    {
        tempoRestanteEspera -= Time.deltaTime;

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
            // Use o PlayerMovement para verificar se está se movendo
            bool estaMovendo = playerMovement.EstaMovendo();

            float recuperacao = estaMovendo ? recuperacaoStaminaMovendo : recuperacaoStaminaParado;

            staminaAtual += recuperacao * Time.deltaTime;
            staminaAtual = Mathf.Min(staminaAtual, staminaMaxima);
            AtualizarBarraStamina();
        }
    }

    private void AtualizarBarraStamina()
    {
        if (barraStamina != null)
        {
            barraStamina.value = staminaAtual / staminaMaxima;
        }
    }

    private void IniciarEsperaRecuperacao()
    {
        emEsperaRecuperacao = true;
        tempoRestanteEspera = tempoEsperaRecuperacao;
    }
}
