using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorPila : MonoBehaviour
{
    public LectorProductos lector;
    private Stack<Producto> pila = new Stack<Producto>();

    private int totalGenerados = 0;
    private int totalDespachados = 0;
    private float tiempoTotalDespacho = 0f;

    private Coroutine generacionCoroutine;
    private Coroutine despachoCoroutine;
    private bool corriendo = false;

    public void Iniciar()
    {
        if (!corriendo)
        {
            corriendo = true;
            generacionCoroutine = StartCoroutine(GenerarProductos());
            despachoCoroutine = StartCoroutine(DespacharProductos());
            Debug.Log("▶ Simulación iniciada");
        }
    }

    public void Cerrar()
    {
        if (corriendo)
        {
            corriendo = false;
            StopAllCoroutines();
            Debug.Log("⏹ Simulación detenida");
            MostrarMetricas();
        }
    }

    IEnumerator GenerarProductos()
    {
        while (true)
        {
            int cantidad = Random.Range(1, 4);
            for (int i = 0; i < cantidad; i++)
            {
                Producto nuevo = lector.GetProductoAleatorio();
                if (nuevo != null)
                {
                    pila.Push(nuevo);
                    totalGenerados++;
                    Debug.Log($"➕ Generado y apilado: {nuevo.Nombre} | Pila: {pila.Count}");
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator DespacharProductos()
    {
        while (true)
        {
            if (pila.Count > 0)
            {
                Producto p = pila.Pop();
                totalDespachados++;
                tiempoTotalDespacho += p.Tiempo;

                Debug.Log($"📦 Despachado: {p.Nombre} | Tiempo: {p.Tiempo} | Pila restante: {pila.Count}");
                yield return new WaitForSeconds(p.Tiempo);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void MostrarMetricas()
    {
        float tiempoPromedio = (totalDespachados > 0) ? tiempoTotalDespacho / totalDespachados : 0f;

        Debug.Log("===== 📊 MÉTRICAS =====");
        Debug.Log("Total Generados: " + totalGenerados);
        Debug.Log("Total Despachados: " + totalDespachados);
        Debug.Log("Total en Pila: " + pila.Count);
        Debug.Log("Tiempo Promedio Despacho: " + tiempoPromedio);
        Debug.Log("=======================");
    }

    public int GetTamañoPila() { return pila.Count; }
    public int GetTotalGenerados() { return totalGenerados; }
    public int GetTotalDespachados() { return totalDespachados; }
    public float GetTiempoPromedioDespacho()
    {
        return (totalDespachados > 0) ? tiempoTotalDespacho / totalDespachados : 0f;
    }
}
