using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorPila : MonoBehaviour
{
    private Stack<Producto> pila = new Stack<Producto>();

    private int totalDespachados = 0;
    private float tiempoTotalDespacho = 0f;

    private Coroutine despachoCoroutine;
    private bool corriendo = false;

    public void ApilarProducto(Producto nuevo)
    {
        pila.Push(nuevo);
        Debug.Log($"➕ Producto apilado: {nuevo.Nombre} | Pila actual: {pila.Count}");
    }

    public void IniciarDespacho()
    {
        if (!corriendo)
        {
            corriendo = true;
            despachoCoroutine = StartCoroutine(DespacharProductos());
            Debug.Log("▶ Despacho iniciado");
        }
    }

    public void DetenerDespacho()
    {
        if (corriendo)
        {
            corriendo = false;
            if (despachoCoroutine != null)
            {
                StopCoroutine(despachoCoroutine);
                despachoCoroutine = null;
            }
            Debug.Log("⏹ Despacho detenido");
            MostrarMetricas();
        }
    }

    private IEnumerator DespacharProductos()
    {
        while (corriendo)
        {
            if (pila.Count > 0)
            {
                Producto p = pila.Pop();
                totalDespachados++;
                tiempoTotalDespacho += p.Tiempo;

                Debug.Log($"📦 Despachado: {p.Nombre} | Tiempo: {p.Tiempo}s | Pila restante: {pila.Count}");

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

        Debug.Log("===== 📊 MÉTRICAS DESPACHO =====");
        Debug.Log("Total Despachados: " + totalDespachados);
        Debug.Log("Total en Pila: " + pila.Count);
        Debug.Log("Tiempo Promedio Despacho: " + tiempoPromedio.ToString("F2") + "s");
        Debug.Log("================================");
    }

    public int GetTamañoPila() { return pila.Count; }
    public int GetTotalDespachados() { return totalDespachados; }
    public float GetTiempoPromedioDespacho()
    {
        return (totalDespachados > 0) ? tiempoTotalDespacho / totalDespachados : 0f;
    }
}