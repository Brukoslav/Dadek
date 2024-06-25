public class Cartas {
    public string tipoDado;
    public int cantidad;
    public string forma;
    public float peso;

    public Cartas(string TipoDado, int Cantidad, string Forma, float Peso) {
        tipoDado = TipoDado;
        cantidad = Cantidad;
        forma = Forma;
        peso = Peso;
    }

    public string Muestra() {
        return cantidad + " dados " + tipoDado + ", forma " + forma + ", peso " + peso;
    }
}