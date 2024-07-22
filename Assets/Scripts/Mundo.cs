using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo : MonoBehaviour
{
    public List<Cartas> misCartas;
    // Start is called before the first frame update
    void Awake()
    {
        misCartas = new List<Cartas> {
            new Cartas("blanco", 3, "cacho", 1),
            new Cartas("blanco", 80, "flujo", 1),
            new Cartas("azul", 1, "cacho", 3),
            new Cartas("rojo", 1, "cacho", 1),
            new Cartas("morado", 1, "cacho", 1)
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
