 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueNumeroEs : MonoBehaviour
{
    // Start is called before the first frame update
    public float margenExactitud = 0.95f; // Margen de exactitud requerido
    public bool explota = false;
    public bool colisiona = false;
    public float explosionForce;
    public float explosionRadius;

    private void Start()
    {
        explosionForce = 1000f;
        explosionRadius = 5f;
    }

    private void Update()
    {
        if(explota)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Explota();
                Destroy(this.gameObject);
            }
        }
    }

    public void Explota()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null) // && rb.gameObject.transform.tag == "dado")
            {
                // Aplica una fuerza de explosión
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (colisiona == true)
        {

            Debug.Log("Entered collision with " + collision.gameObject.name);
        }
    }

    private Vector3[] diceFaces = new Vector3[]
    {
        new Vector3(0, 1, 0),  // Cara 1 
        new Vector3(0, -1, 0), // Cara 2
        new Vector3(1, 0, 0),  // Cara 3
        new Vector3(-1, 0, 0), // Cara 4
        new Vector3(0, 0, 1),  // Cara 5
        new Vector3(0, 0, -1)  // Cara 6
    };

    public Vector3[] Caras()
    {
        return new Vector3[]
        {
            -transform.right,transform.forward,transform.up, -transform.up,-transform.forward,transform.right
        };
    }


    public int GetUpwardFaceNumber()
    {
        Vector3 upwardVector = Vector3.up; // Vector hacia arriba del dado
        int closestFace = 1; // Comenzar asumiendo que la cara 1 está hacia arriba
        float maxDotProduct = -Mathf.Infinity;

        for (int i = 0; i < diceFaces.Length; i++)
        {
            float dotProduct = Vector3.Dot(upwardVector, Caras()[i]);
            if (dotProduct > maxDotProduct)
            {
                maxDotProduct = dotProduct;
                closestFace = i + 1; // Las caras están indexadas desde 1
                //print(dotProduct);

            }
        }

        if (maxDotProduct > 0.7*0) // 0.7 es el margen; ajuste según la precisión deseada
        {
            return closestFace;
        } else
        {
            //print("no cuenta" + maxDotProduct);
        }

        return 0; // Retorna -1 si ninguna cara está suficientemente hacia arriba
    }
}

