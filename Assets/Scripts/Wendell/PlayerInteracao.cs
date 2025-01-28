using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInteracao : MonoBehaviour
{
    private GameObject objetoInteragivel;
    public GameObject botaoInteracao;
    private bool debounce = false;

    public void HandleTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climb1") || other.CompareTag("Coletavel"))
            objetoInteragivel = other.gameObject;
            botaoInteracao.SetActive(true);

    }

    public void HandleTriggerExit(Collider other)
    {
        if (other.CompareTag("Climb1") || other.CompareTag("Coletavel"))
            objetoInteragivel = null;
            botaoInteracao.SetActive(false);
    }

    public void RealizarInteracao()
    {
        if (objetoInteragivel == null) return;

        if (objetoInteragivel.CompareTag("Climb1"))
        {
            Debug.Log("Iniciando escalada...");
            GetComponent<PlayerEscalada>().IniciarEscalada(); // Chama o script de escalada
        }
        else if (objetoInteragivel.CompareTag("Coletavel"))
        {
            Debug.Log($"Coletando objeto: {objetoInteragivel.name}");
            // Adicione a lógica de coleta aqui
        }
        else
        {
            Debug.LogWarning("Nenhuma ação definida para esta interação.");
        }
    }


    public void HandlePular(InputAction.CallbackContext contexto, Rigidbody rb, Animator animador, PlayerMovimento movimento, float forcaPulo)
    {
        if (contexto.performed && movimento.EstaNoChao)
        {
            rb.velocity = new Vector3(rb.velocity.x, forcaPulo, rb.velocity.z);
            animador.SetTrigger(movimento.EstaMovendo ? "PularEmMovimento" : "PularParado");
        }
    }

    private IEnumerator ResetDebounce()
    {
        yield return new WaitForSeconds(0.5f);
        debounce = false;
    }
}
