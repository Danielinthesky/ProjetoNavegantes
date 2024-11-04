using UnityEngine;

public class OceanFollow : MonoBehaviour
{
    public Transform jangada; 
    public float alturaDoOceano = 0f; 

    void Update()
    {
        if (jangada == null)
        {
            Debug.LogError("Transform da jangada não foi atribuído ao OceanFollow em " + gameObject.name);
            return;
        }

        // O oceano acompanha a Jangada se movendo
        transform.position = new Vector3(jangada.position.x, alturaDoOceano, jangada.position.z);
    }
}
