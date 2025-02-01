using UnityEngine;
using UnityEngine.UI;

public class MenuComAbasMobile : MonoBehaviour
{
    // Referência ao painel principal do menu
    public GameObject menuPrincipal;

    // Referências aos painéis das abas
    public GameObject abaGeral;
    public GameObject abaInventario;
    public GameObject abaMapa;
    public GameObject abaConfiguracoes;

    // Referências às imagens das abas (aberta e fechada)
    public Image imagemGeralAberta;
    public Image imagemGeralFechada;
    public Image imagemInventarioAberta;
    public Image imagemInventarioFechada;
    public Image imagemMapaAberta;
    public Image imagemMapaFechada;
    public Image imagemConfiguracoesAberta;
    public Image imagemConfiguracoesFechada;

    // Referência ao botão de toggle do menu
    public Button botaoToggleMenu;

    // Referência à imagem do botão de toggle (opcional)
    
    private void Start()
    {
        // Configura o botão de toggle
        if (botaoToggleMenu != null)
        {
            botaoToggleMenu.onClick.AddListener(AlternarMenu);
        }

        // Abre a aba Geral por padrão ao iniciar o menu
        AbrirAbaGeral();

        // Atualiza a aparência do botão de toggle
       
    }

    // Método para abrir/fechar o menu
    public void AlternarMenu()
    {   Debug.Log("Botão de toggle clicado!");
        if (menuPrincipal != null)
        {   
            // Inverte o estado de ativação do menu (abre/fecha)
            menuPrincipal.SetActive(!menuPrincipal.activeSelf);

            // Pausa o jogo se o menu estiver aberto
            Time.timeScale = menuPrincipal.activeSelf ? 0f : 1f;

            // Abre a aba padrão (Geral) ao abrir o menu
            if (menuPrincipal.activeSelf)
            {
                AbrirAbaGeral();
            }

            
        }
    }

    // Método para atualizar a aparência do botão de toggle
   
    // Método para abrir a aba Geral
    public void AbrirAbaGeral()
    {
        FecharTodasAbas();
        if (abaGeral != null)
        {
            abaGeral.SetActive(true);
            AtualizarImagensAba("Geral");
        }
    }

    // Método para abrir a aba Inventário
    public void AbrirAbaInventario()
    {
        FecharTodasAbas();
        if (abaInventario != null)
        {
            abaInventario.SetActive(true);
            AtualizarImagensAba("Inventario");
        }
    }

    // Método para abrir a aba Mapa
    public void AbrirAbaMapa()
    {
        FecharTodasAbas();
        if (abaMapa != null)
        {
            abaMapa.SetActive(true);
            AtualizarImagensAba("Mapa");
        }
    }

    // Método para abrir a aba Configurações
    public void AbrirAbaConfiguracoes()
    {
        FecharTodasAbas();
        if (abaConfiguracoes != null)
        {
            abaConfiguracoes.SetActive(true);
            AtualizarImagensAba("Configuracoes");
        }
    }

    // Método para fechar todas as abas
    private void FecharTodasAbas()
    {
        if (abaGeral != null) abaGeral.SetActive(false);
        if (abaInventario != null) abaInventario.SetActive(false);
        if (abaMapa != null) abaMapa.SetActive(false);
        if (abaConfiguracoes != null) abaConfiguracoes.SetActive(false);
    }

    // Método para atualizar as imagens das abas
    private void AtualizarImagensAba(string abaAtual)
    {
        // Desativa todas as imagens abertas e ativa as fechadas
        imagemGeralAberta.gameObject.SetActive(abaAtual == "Geral");
        imagemGeralFechada.gameObject.SetActive(abaAtual != "Geral");

        imagemInventarioAberta.gameObject.SetActive(abaAtual == "Inventario");
        imagemInventarioFechada.gameObject.SetActive(abaAtual != "Inventario");

        imagemMapaAberta.gameObject.SetActive(abaAtual == "Mapa");
        imagemMapaFechada.gameObject.SetActive(abaAtual != "Mapa");

        imagemConfiguracoesAberta.gameObject.SetActive(abaAtual == "Configuracoes");
        imagemConfiguracoesFechada.gameObject.SetActive(abaAtual != "Configuracoes");
    }
}