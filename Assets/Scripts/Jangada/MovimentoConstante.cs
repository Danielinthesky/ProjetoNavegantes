using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoConstante : MonoBehaviour
{
    private Vector3 direcaoPassiva;
    OceanManager oceanManager;
    Rigidbody rb;

    private float proximoTempoDeMudancaDeDirecao;
    public float velocidadeAtual = 0.1f;         // Intensidade da correnteza 
    public float intervaloDeMudancaDeCorrenteza = 5.0f; 
    void Update()
    {
        AplicarMovimentoPassivo();
    }

    void AplicarMovimentoPassivo()
    {
        if (Time.time >= proximoTempoDeMudancaDeDirecao)
        {
            DefinirDirecaoPassivaAleatoria();
            proximoTempoDeMudancaDeDirecao = Time.time + intervaloDeMudancaDeCorrenteza;
        }

        Vector3 forcaAtual = direcaoPassiva * velocidadeAtual;
        float deslocamentoOnda = Mathf.Sin(Time.time * oceanManager.frequenciaDasOndas) * oceanManager.alturaDasOndas;
        Vector3 forcaDaOnda = new Vector3(0, deslocamentoOnda, 0);

        rb.AddForce(forcaAtual + forcaDaOnda, ForceMode.Acceleration);
    }

    void DefinirDirecaoPassivaAleatoria()
    {
        direcaoPassiva = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
}
