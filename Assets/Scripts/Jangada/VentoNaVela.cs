using UnityEngine;

public class VentoNaVela : MonoBehaviour
{
    [SerializeField]public Cloth velaCloth;   
    public float intensidadeVento = 5.0f;  

    void Update()
    {
        // Define a direção do vento como a frente da jangada, o vento sempre vai soprar na direção que a jangada considera como frente
        //No gizmo do editor a jangada esta posicionada como frente a posição -X, no codigo considerado como '-transform.right'
        Vector3 direcaoVento = -transform.right * intensidadeVento;

        // Aplica a direção do vento no Cloth
        velaCloth.externalAcceleration = direcaoVento;
    }
}
