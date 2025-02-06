using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    public Animator animador; // Referência ao Animator
    public Transform jogador; // Referência ao jogador
    public float velocidadeRotacao = 5f; // Velocidade de rotação
    private bool interagindo = false;
    public AudioSource audioSource1;
    public AudioClip[] heySound;
    public AudioClip okSound;
   

    void Start()
    {

        audioSource1 = GetComponent<AudioSource>();

    }
    private void Update()
    {
        
            if (interagindo)
        {
            RotacionarParaJogador();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jogador"))
        
        {   
            interagindo = true;
            animador.SetBool("Interagindo", true); // Transição para o estado de interação     
            Debug.Log("Interagindo");
            animador.Play("Interagir State");  
            audioSource1.PlayOneShot(heySound[Random.Range(0, heySound.Length)]);
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Jogador"))
        {   
            
            audioSource1.PlayOneShot(okSound);           
            interagindo = false;
            animador.SetBool("Interagindo", false); 
            animador.Play("Idle State");
            Debug.Log("Deixou a area de interação"); 
            
        }
    }

    private void RotacionarParaJogador()
    {
        Vector3 direcao = (jogador.position - transform.position).normalized;
        Quaternion rotacaoOlhar = Quaternion.LookRotation(new Vector3(direcao.x, 0, direcao.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoOlhar, Time.deltaTime * velocidadeRotacao);
    }
}
