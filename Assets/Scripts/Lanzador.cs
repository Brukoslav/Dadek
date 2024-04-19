using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class Lanzador : MonoBehaviour
{
    public GameObject[] dadoPrefab;
    public float varianza;
    public Vector3 direccion;
    public float fuerza;
    public int cuantos;
    public int queEs;

    public TMP_Text puntos;
    // Start is called before the first frame update
    void Start()
    {
        varianza = 2;
        direccion = new Vector3(1, 1, 1);
        fuerza = 10;
        StartCoroutine(EvaluarDados());
    }

    private IEnumerator EvaluarDados()
    {
        // Bucle infinito
        while (true)
        {
            // Espera 10 segundos
            yield return new WaitForSeconds(0.3f);

            // Ejecuta la función
            List<float> dadoInfo = SumaDados();
            if (dadoInfo[1] == -1)
            {
                //puntos.text = "Dados: " + dadoInfo[0] + "\nSe están moviendo los dados qls";
            }
            else
            {
                puntos.text = "Dados: " + dadoInfo[0] + "\nSuma: " + dadoInfo[1];
            }

        }
    }
    // Update is called once per frame
    void Update()
    {



        if(Input.GetKeyDown(KeyCode.K))
        {
            for(int i=0;i<cuantos;i++)
            {
                DadoNuevo(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            DadoNuevo(2);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            DadoNuevo(1);
        }
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Crear una instancia de Stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Iniciar el cronómetro antes de la función a medir
            stopwatch.Start();




            print(SumaDados());


            //
            // Detener el cronómetro una vez finalizada la función
            stopwatch.Stop();

            // Obtener el tiempo transcurrido en milisegundos
            long milliseconds = stopwatch.ElapsedMilliseconds;

            // Imprimir el tiempo de ejecución en la consola de Unity
            print("Tiempo de ejecución en milisegundos: " + milliseconds + " ms");
        }
        */
        if (Input.GetKey(KeyCode.J))
        {
            DadoNuevo(0);
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
        }
        if (Input.GetKey(KeyCode.S))
        {
            fuerza--;
        }
    }

    public List<float> SumaDados()
    {


        // Mi función
        float sumaDados = 0;
        float nDados = 0;
        GameObject[] dados = GameObject.FindGameObjectsWithTag("dado");

        foreach (GameObject go in dados)
        {
            //Mata si se cayó
            if (go.transform.position.y < -3)
            {
                Destroy(go);
            }
            else
            {
                nDados++;
                sumaDados += go.GetComponent<QueNumeroEs>().GetUpwardFaceNumber();
                if(go.GetComponent<Rigidbody>().velocity.magnitude>0.5f)
                {
                    return new List<float> { nDados, -1 };
                }
            }
            
        }
        return new List<float> { nDados, sumaDados };
    }

    void DadoNuevo(int i)
    {
        GameObject dado = Instantiate(dadoPrefab[i], transform.position + new Vector3(Random.Range(-varianza, varianza), Random.Range(-varianza, varianza), Random.Range(-varianza, varianza)), Quaternion.EulerAngles(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
        dado.GetComponent<Rigidbody>().velocity = (direccion*fuerza);
        if(i == 1) // explosivo
        {
            dado.GetComponent<QueNumeroEs>().explota = true;
        }
    }
}
