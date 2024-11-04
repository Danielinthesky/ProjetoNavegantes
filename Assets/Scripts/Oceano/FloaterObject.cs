using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class FloaterObject : MonoBehaviour
{
    
    public Transform[] pontosFlutuantes;
    public float arrastoSubmerso = 3f; 
    public float arrastoAngularSubmerso = 1f; 
    public float arrastoNoAr = 0f; 
    public float arrastoAngularNoAr = 0.05f; 
    public float forcaDeFlutuacao = 15f; 
    public float alturaDaAgua = 0f; 
    public int pontosSubmersos; 
    bool estaSubmerso; 

    OceanManager gerenciadorDoOceano; 
    Rigidbody rb; 
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gerenciadorDoOceano = GetComponent<OceanManager>();
    }

    
    void FixedUpdate()
    {
        pontosSubmersos = 0; 

        
        for (int i = 0; i < pontosFlutuantes.Length; i++)
        {
            // Calcula a diferença entre a posição do ponto de flutuação e a altura da agua
            float diferenca = pontosFlutuantes[i].position.y - gerenciadorDoOceano.WaterHeightAtPosition(pontosFlutuantes[i].position);

            // Se o ponto está abaixo da superfície da água.
            if (diferenca < 0)
            {
                // Aplica uma força de flutuação no ponto específico.
                rb.AddForceAtPosition(Vector3.up * forcaDeFlutuacao * Math.Abs(diferenca), pontosFlutuantes[i].position, ForceMode.Force);
                pontosSubmersos += 1;

                
                if (!estaSubmerso)
                {
                    estaSubmerso = true;
                    AlterarEstado(true);
                }
            }

            // Se o objeto estava submerso e todos os pontos saíram da agua
            if (estaSubmerso && pontosSubmersos == 0)
            {
                estaSubmerso = false;
                AlterarEstado(false);
            }
        }
    }

    // Altera o estado do arrasto do Rigidbody dependendo se esta submerso ou nao
    void AlterarEstado(bool embaixoDAgua)
    {
        if (embaixoDAgua)
        {
            rb.drag = arrastoSubmerso;
            rb.angularDrag = arrastoAngularSubmerso;
        }
        else
        {
            rb.drag = arrastoNoAr;
            rb.angularDrag = arrastoAngularNoAr;
        }
    }
}
