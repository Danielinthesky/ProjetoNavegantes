using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CarregadorDeCena : MonoBehaviour
{
    public string cenaParaCarregar; // Nome da cena a ser carregada
    public Slider barraDeProgresso; // Referência para a barra de progresso na UI

    void Start()
    {
        StartCoroutine(CarregarCenaAsync(cenaParaCarregar));
    }

    IEnumerator CarregarCenaAsync(string nomeCena)
    {
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeCena);
        operacao.allowSceneActivation = false;

        // Enquanto a cena carrega, atualiza a barra de progresso
        while (!operacao.isDone)
        {
            // O progresso varia entre 0 e 0.9 antes da ativação da cena
            float progresso = Mathf.Clamp01(operacao.progress / 0.9f);
            if (barraDeProgresso != null)
            {
                barraDeProgresso.value = progresso; // Atualiza a barra de progresso
            }

            // Quando o carregamento chega a 0.9, a cena está pronta para ser ativada
            if (operacao.progress >= 0.9f)
            {
                operacao.allowSceneActivation = true; // Ativa a cena carregada
            }

            yield return null;
        }
    }
}
