using UnityEngine;

public class SistemaFlutuante : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ControladorDaAgua controladorDaAgua;

    public float waveFrequency = 0.5f; // Frequência da onda
    public float waveAmplitude = 1.0f; // Amplitude da onda
    public float waveSpeed = 1.0f;     // Velocidade da onda

    public float depthBeforeSubmerged = 1.5f;
    public float cubeVolume = 2f;
    public float waterDragBase = 0.9f;
    public float waterAngularDragBase = 0.4f;
    public Transform[] floaters;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no objeto " + gameObject.name + ". Por favor, adicione um Rigidbody.");
            enabled = false;
            return;
        }

        if (controladorDaAgua == null)
        {
            controladorDaAgua = ControladorDaAgua.instancia;
            if (controladorDaAgua == null)
            {
                Debug.LogError("ControladorDaAgua não encontrado. Certifique-se de que ele esteja presente na cena.");
                enabled = false;
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb == null || controladorDaAgua == null) return;

        float waveStrength = controladorDaAgua.GetWaveStrength();
        float foamWidth = controladorDaAgua.GetFoamWidth();
        float reflectionSmoothness = controladorDaAgua.GetReflectionSmoothness();
        Vector2 waveDirection = controladorDaAgua.GetWaveDirection();

        Vector3 totalForce = Vector3.zero;
        Vector3 totalTorque = Vector3.zero;

        foreach (Transform floater in floaters)
        {
            Vector3 worldPoint = floater.position;

            // Calcula a altura da onda usando uma função seno para imitar o shader
            float waveHeight = Mathf.Sin((worldPoint.x + Time.time * waveSpeed) * waveFrequency) * waveAmplitude;

            if (worldPoint.y < waveHeight)
            {
                float displacementMultiplier = Mathf.Clamp01((waveHeight - worldPoint.y) / depthBeforeSubmerged) * cubeVolume * waveStrength;
                Vector3 upForce = new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier * (1 + waveStrength * 0.5f), 0f);

                totalForce += upForce;

                Vector3 torqueDirection = Vector3.Cross(transform.up, worldPoint - transform.position).normalized;
                totalTorque += Vector3.Cross(torqueDirection, upForce) * displacementMultiplier * reflectionSmoothness;

                Vector3 wavePush = new Vector3(waveDirection.x, 0f, waveDirection.y) * waveStrength * displacementMultiplier;
                totalForce += wavePush;
            }
        }

        float waterDrag = waterDragBase * (1 + foamWidth * 0.5f);
        float waterAngularDrag = waterAngularDragBase * (1 + reflectionSmoothness * 0.3f);

        rb.AddForce(totalForce, ForceMode.Acceleration);
        rb.AddTorque(totalTorque * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);

        rb.AddForce(-rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(-rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}
