using System;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloatMultiplayer : MonoBehaviourPun
{
    [Header("Configurações de Flutuação")]
    public Transform[] pontosFlutuantes;
    public float forcaDeFlutuacao = 15f;
    public float arrastoSubmerso = 3f;
    public float arrastoAngularSubmerso = 1f;
    public float arrastoNoAr = 0f;
    public float arrastoAngularNoAr = 0.05f;
    public float alturaDaAgua = 0f;

    private int pontosSubmersos;
    private bool estaSubmerso;
    private Rigidbody rb;

    public bool EstaNaAgua => pontosSubmersos > 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no GameObject!");
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        pontosSubmersos = 0;

        foreach (Transform ponto in pontosFlutuantes)
        {
            float diferenca = ponto.position.y - alturaDaAgua;

            if (diferenca < 0)
            {
                rb.AddForceAtPosition(Vector3.up * forcaDeFlutuacao * Mathf.Abs(diferenca), ponto.position, ForceMode.Force);
                pontosSubmersos++;
            }
        }

        if (pontosSubmersos > 0 && !estaSubmerso)
        {
            estaSubmerso = true;
            photonView.RPC("SincronizarEstadoSubmerso", RpcTarget.All, true);
        }
        else if (pontosSubmersos == 0 && estaSubmerso)
        {
            estaSubmerso = false;
            photonView.RPC("SincronizarEstadoSubmerso", RpcTarget.All, false);
        }
    }

    [PunRPC]
    private void SincronizarEstadoSubmerso(bool embaixoDAgua)
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
