using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Lanzador : MonoBehaviour
{
    public GameObject[] dadoPrefab;
    public float varianza;
    public Vector3 direccion;
    public float fuerza;
    public int cuantos;
    public int queEs;
    public bool estaLanzando;
    public Vector2 lanzamientoPosicionInicial;
    public float lanzamientoInicialTiempo;
    public Vector3 lanzamientoPosicionUltima;
    public TMP_Text puntos;
    public GameObject barra;
    public float sizeBar;
    public float adelanteOrigen;
    // Start is called before the first frame update
    void Start()
    {
        //varianza = 2;
        direccion = new Vector3(1, 1, 1);
        //fuerza = 10;
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
        //Lanzamiento dadal
        if(Input.GetMouseButtonDown(0)) {
            estaLanzando = true;
            lanzamientoPosicionInicial = Input.mousePosition;
            lanzamientoInicialTiempo = Time.time;
        }

        if(estaLanzando) {
            sizeBar = Mathf.Pow( Time.time - lanzamientoInicialTiempo,1.8f)*120;
            sizeBar = Mathf.Min(sizeBar, 300);
            barra.GetComponent<RectTransform>().sizeDelta = new Vector3(sizeBar, barra.GetComponent<RectTransform>().sizeDelta.y);
        }

        if (Input.GetMouseButtonUp(0)) {
            Vector2 lanzamientoPosicionFinal = Input.mousePosition;
            Vector2 diferencia = lanzamientoPosicionFinal - lanzamientoPosicionInicial;
            diferencia = new Vector2(diferencia.x / Screen.width, diferencia.y / Screen.height);
            //float fuerzaExtra = 0;// diferencia.magnitude;
            //print("Lanzó a " + (Time.time - lanzamientoInicialTiempo));
            //Objetivo
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)) {


                Vector3 direccionPlana = -diferencia.normalized;

                Vector3 origen = transform.position + (transform.parent.transform.up*direccionPlana.y + transform.parent.transform.right*direccionPlana.x)*5+ transform.parent.transform.forward*adelanteOrigen;
                direccion = hit.point - origen;
                direccion = direccion.normalized;
                LanzaDado(0, origen, direccion, fuerza* (0.3f+sizeBar/300));
                barra.GetComponent<RectTransform>().sizeDelta = new Vector3(0, barra.GetComponent<RectTransform>().sizeDelta.y);
            }
            estaLanzando = false;
        }


        if (Input.GetKeyDown(KeyCode.K))
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
        if (Input.GetKeyDown(KeyCode.U)) {
            DadoNuevo(3);
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

    void LanzaDado(int i, Vector3 origen,  Vector3 direccion, float fuerza) {
        GameObject dado = Instantiate(dadoPrefab[i], origen, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
        dado.GetComponent<Rigidbody>().velocity = (direccion * fuerza);
        if (i == 1) // explosivo
        {
            dado.GetComponent<QueNumeroEs>().explota = true;
        }
        if (i == 3) // contacto
        {
            dado.GetComponent<QueNumeroEs>().colisiona = true;
        }
    }

    void DadoNuevo(int i)
    {
        GameObject dado = Instantiate(dadoPrefab[i], transform.position + new Vector3(Random.Range(-varianza, varianza), Random.Range(-varianza, varianza), Random.Range(-varianza, varianza)), Quaternion.EulerAngles(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
        dado.GetComponent<Rigidbody>().velocity = (direccion*fuerza);
        if(i == 1) // explosivo
        {
            dado.GetComponent<QueNumeroEs>().explota = true;
        }
        if (i == 3) // contacto
        {
            dado.GetComponent<QueNumeroEs>().colisiona = true;
        }
    }
}
