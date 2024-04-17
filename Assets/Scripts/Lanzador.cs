using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanzador : MonoBehaviour
{
    public GameObject dadoPrefab;
    public float varianza;
    public Vector3 direccion;
    public float fuerza;
    public int cuantos;
    public int queEs;


    // Start is called before the first frame update
    void Start()
    {
        varianza = 2;
        direccion = new Vector3(1, 1, 1);
        fuerza = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        print("soy un " + ObtenerNumeroDado());
        if(Input.GetKeyDown(KeyCode.K))
        {
            for(int i=0;i<cuantos;i++)
            {
                DadoNuevo();
            }
        }
        if (Input.GetKey(KeyCode.J))
        {
            DadoNuevo();
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                direccion = hit.point - transform.position;
                direccion = direccion.normalized;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            fuerza++;
            print("La fuerza es " + fuerza);
        }
        if (Input.GetKey(KeyCode.S))
        {
            fuerza--;
            print("La fuerza es " + fuerza);
        }
    }

    void DadoNuevo()
    {
        GameObject dado = Instantiate(dadoPrefab, transform.position + new Vector3(Random.Range(-varianza, varianza), Random.Range(-varianza, varianza), Random.Range(-varianza, varianza)), Quaternion.EulerAngles(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
        dado.GetComponent<Rigidbody>().velocity = (direccion*fuerza);
    }
}
