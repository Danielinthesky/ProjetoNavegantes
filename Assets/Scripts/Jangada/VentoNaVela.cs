using UnityEngine;

public class VentoNaVela : MonoBehaviour
{
    [SerializeField]public Cloth velaCloth;   // Referência ao componente Cloth da vela
    public float intensidadeVento = 5.0f;  // Intensidade do vento

    void Update()
    {
        // Define a direção do vento como a frente da jangada (-transform.right)
        Vector3 direcaoVento = -transform.right * intensidadeVento;

        // Aplica a direção do vento no Cloth
        velaCloth.externalAcceleration = direcaoVento;
    }
}
