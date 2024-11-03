using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoConstante : MonoBehaviour
{

    private Vector3 passiveDirection;
    OceanManager oceanManager;
    Rigidbody rb;

    private float nextDirectionChangeTime;
    public float currentSpeed = 0.1f;         // Intensidade da correnteza (ajustada para ser mais leve)
    public float currentChangeInterval = 5.0f; // Intervalo de mudança de direção da correnteza

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                ApplyPassiveMovement();

    }

        void ApplyPassiveMovement()
    {
        if (Time.time >= nextDirectionChangeTime)
        {
            SetRandomPassiveDirection();
            nextDirectionChangeTime = Time.time + currentChangeInterval;
        }

        Vector3 currentForce = passiveDirection * currentSpeed;
        float waveOffset = Mathf.Sin(Time.time * oceanManager.wavesFrequency) * oceanManager.wavesHeight;
        Vector3 waveForce = new Vector3(0, waveOffset, 0);

        rb.AddForce(currentForce + waveForce, ForceMode.Acceleration);
    }

    void SetRandomPassiveDirection()
    {
        passiveDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }
}
