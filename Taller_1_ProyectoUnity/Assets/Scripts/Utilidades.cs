using UnityEngine;

public class GeneradorProductos : MonoBehaviour
{
    public LectorProductos lector;   // referencia al lector de productos
    public float intervalo = 1f;     // cada cuántos segundos se generan
    private float temporizador = 0f;
    private bool enEjecucion = false;

    // 👉 Método que asignas al botón "Iniciar"
    public void IniciarGeneracion()
    {
        enEjecucion = true;
        Debug.Log("🔵 Generación iniciada");
    }

    // 👉 Método que asignas a un botón "Detener"
    public void DetenerGeneracion()
    {
        enEjecucion = false;
        Debug.Log("🔴 Generación detenida");
    }

    void Update()
    {
        if (!enEjecucion) return;  // solo corre si está encendido

        temporizador += Time.deltaTime;

        if (temporizador >= intervalo)
        {
            temporizador = 0f; // reinicia
            GenerarProductos();
        }
    }

    void GenerarProductos()
    {
        int cantidad = Random.Range(1, 4); // entre 1 y 3 productos

        for (int i = 0; i < cantidad; i++)
        {
            Producto nuevo = lector.GetProductoAleatorio();
            if (nuevo != null)
            {
                Debug.Log("📦 Producto generado: " + nuevo.nombre 
                          + " | Tipo: " + nuevo.tipo 
                          + " | Precio: " + nuevo.precio);
            }
        }
    }
}


