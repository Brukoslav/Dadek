using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Lanzador : MonoBehaviour
{
    // Seriedad
    public int idCarta;
    public Cartas carta;
    public int dadosMax;

    // Sin seriedad
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
    public TMP_Text seleccionado;
    public GameObject barra;
    public float sizeBar;
    public float adelanteOrigen;
    public float alLadoOrigen;
    public int grillo;
    public Mundo mundo;
    // Start is called before the first frame update
    void Start()
    {

        mundo = GetComponent<Mundo>();

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
                puntos.text = "Dados: " + dadoInfo[0] + " (grillo: "+grillo+")\nSuma: " + dadoInfo[1] + "Suma: " + dadoInfo[2] + "% = " + Mathf.Round(dadoInfo[1]*(1+ dadoInfo[2]/100));
            }

        }
    }
    // Update is called once per frame
    void Update() {
        // Seriedad

        // Qué carta va a jugar
        bool mudo = false;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            idCarta = 0;
            mudo = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            idCarta = 1;
            mudo = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            idCarta = 2;
            mudo = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            idCarta = 3;
            mudo = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            idCarta = 4;
            mudo = true;
        }


        carta = mundo.misCartas[idCarta];
        if (mudo) {
            seleccionado.text = carta.Muestra();
            dadosMax = carta.cantidad;
        }

        if (carta.forma == "flujo" && Input.GetMouseButton(0) && dadosMax>0) {
            if (Random.value < 0.1f) {

                DadoNuevo(0);
                dadosMax--;
            }
        }




        // Poca seriedad

        //Lanzamiento dadal
        if (carta.forma == "cacho") {
            if (Input.GetMouseButtonDown(0)) {
                estaLanzando = true;
                lanzamientoPosicionInicial = Input.mousePosition;
                lanzamientoInicialTiempo = Time.time;
            }

            if (estaLanzando) {
                sizeBar = Mathf.Pow(Time.time - lanzamientoInicialTiempo, 1.8f) * 120;
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
                    //print(direccionPlana.y + ", " + direccionPlana.x);
                    for (int i = 0; i < carta.cantidad; i++) {
                        Vector3 origen = transform.position + (transform.parent.transform.up * Mathf.Max(direccionPlana.y, -0.1f) + transform.parent.transform.right * direccionPlana.x) * alLadoOrigen + transform.parent.transform.forward * adelanteOrigen;
                        direccion = hit.point - origen;
                        direccion = direccion.normalized;
                        LanzaDado(TipoDadoToPrefabId(carta.tipoDado), origen + Ruido(0.5f), direccion, fuerza * (0.3f + sizeBar / 300));
                    }
                    barra.GetComponent<RectTransform>().sizeDelta = new Vector3(0, barra.GetComponent<RectTransform>().sizeDelta.y);
                }
                estaLanzando = false;
            }
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
        /*
        if (Input.GetKey(KeyCode.A))
        {
            fuerza++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            fuerza--;
        }
        */
    }


    public int TipoDadoToPrefabId(string tipoDado) {
        if(tipoDado == "blanco") {
            return 0;
        } else if (tipoDado == "rojo") {
            return 1;
        } else if (tipoDado == "azul") {
            return 2;
        } else if (tipoDado == "negro") {
            return 3;
        } else if (tipoDado == "morado") {
            return 3;
        }
        return 0;
    }

    public List<float> SumaDados()
    {


        // Mi función
        float sumaDados = 0;
        float sumaDadosMulti = 0;
        float nDados = 0;
        grillo = 0;
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
                if (go.GetComponent<QueNumeroEs>().esMultiplicador) {
                    print("ajá");
                    sumaDadosMulti += go.GetComponent<QueNumeroEs>().GetUpwardFaceNumber();
                } else {
                    sumaDados += go.GetComponent<QueNumeroEs>().GetUpwardFaceNumber();
                }
                if(go.GetComponent<Rigidbody>().velocity.magnitude>0.5f)
                {
                    //return new List<float> { nDados, -1 };
                }
            }
            
        }
        return new List<float> { nDados, sumaDados, sumaDadosMulti };
    }

    void LanzaDado(int i, Vector3 origen,  Vector3 direccion, float fuerza) {
        GameObject dado = Instantiate(dadoPrefab[i], origen, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
        dado.GetComponent<Rigidbody>().velocity = (direccion * fuerza);
        dado.GetComponent<QueNumeroEs>().lanzadorSc = this;
        if (i == 1) // explosivo
        {
            dado.GetComponent<QueNumeroEs>().explota = true;
        }
        if (i == 3) // contacto
        {
            dado.GetComponent<QueNumeroEs>().colisiona = true;
            dado.GetComponent<QueNumeroEs>().esMultiplicador = true;
        }
    }

    public Vector3 Ruido(float varianza) {
        return new Vector3(Random.Range(-varianza, varianza), Random.Range(-varianza, varianza) - 2, Random.Range(-varianza, varianza));
    }

    void DadoNuevo(int i)
    {
        GameObject dado = Instantiate(dadoPrefab[i], transform.position + Ruido(1) + Vector3.down, Quaternion.EulerAngles(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) as GameObject;
        dado.GetComponent<Rigidbody>().velocity = (direccion*fuerza);
        dado.GetComponent<QueNumeroEs>().lanzadorSc = this;
        if (i == 1) // explosivo
        {
            dado.GetComponent<QueNumeroEs>().explota = true;
        }
        if (i == 3) // contacto
        {
            dado.GetComponent<QueNumeroEs>().colisiona = true;
        }
        if (i == 3) // contacto
        {
            print("yapu");
            dado.GetComponent<QueNumeroEs>().esMultiplicador = true;
        }
    }
}
