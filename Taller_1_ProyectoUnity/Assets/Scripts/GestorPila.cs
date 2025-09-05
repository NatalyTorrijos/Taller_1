using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GestorPila : MonoBehaviour
{
    private Stack<Producto> pila = new Stack<Producto>();

    private int totalDespachados = 0;
    private float tiempoTotalDespacho = 0f;

    private Coroutine despachoCoroutine;
    private bool corriendo = false;

    // 🔹 Diccionario para contar productos por tipo
    private Dictionary<string, int> despachadosPorTipo = new Dictionary<string, int>();


    public TMP_Text pilaText;
    public TMP_Text TamañoPilaText;
    public TMP_Text despachosTText;
    public TMP_Text despachosText;

    public void ApilarProducto(Producto nuevo)
    {
        pila.Push(nuevo);
        Debug.Log($"Producto apilado: {nuevo.Nombre} | Pila actual: {pila.Count}");
        TamañoPilaText.text = pila.Count.ToString();
        ActualizarTexto();
    }

    public void IniciarDespacho()
    {
        if (!corriendo)
        {
            corriendo = true;
            despachoCoroutine = StartCoroutine(DespacharProductos());
            Debug.Log("Despacho iniciado");
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
            Debug.Log(" Despacho detenido");
           
        }
    }

    void Start()
    {
        // 🔹 Inicializar los tipos posibles en 0
        despachadosPorTipo["Basico"] = 0;
        despachadosPorTipo["Fragil"] = 0;
        despachadosPorTipo["Pesado"] = 0;
    }


    private IEnumerator DespacharProductos()
    {
        while (corriendo)
        {
            if (pila.Count > 0)
            {
                Producto p = pila.Pop();

                totalDespachados++;
                despachosText.text = totalDespachados.ToString();
                despachosTText.text = p.ToString();
                TamañoPilaText.text = pila.Count.ToString();
                tiempoTotalDespacho += p.Tiempo;

                // 🔹 Registrar despachos por tipo
                if (!despachadosPorTipo.ContainsKey(p.Tipo))
                    despachadosPorTipo[p.Tipo] = 0;
                despachadosPorTipo[p.Tipo]++;


                Debug.Log($"Despachado: {p.Nombre} | Tiempo: {p.Tiempo}s | Pila restante: {pila.Count}");

                yield return new WaitForSeconds(p.Tiempo);
            }
            else
            {
                yield return null;
            }
        }
    }

   
    private void ActualizarTexto()
    {
        string mostrar = "";
        foreach (var item in pila)
        {
            mostrar += item.ToString();
        }
        pilaText.text = mostrar;
    }

    public float GetTiempoTotalDespacho() { return tiempoTotalDespacho; }

    public Dictionary<string, int> GetDespachadosPorTipo() { return despachadosPorTipo; }

    public string GetTipoMasDespachado()
    {
        string maxTipo = "";
        int maxCantidad = 0;
        foreach (var kvp in despachadosPorTipo)
        {
            if (kvp.Value > maxCantidad)
            {
                maxCantidad = kvp.Value;
                maxTipo = kvp.Key;
            }
        }
        return maxTipo;
    }

    public int GetTamañoPila() { return pila.Count; }
    public int GetTotalDespachados() { return totalDespachados; }
    public float GetTiempoPromedioDespacho()
    {
        return (totalDespachados > 0) ? tiempoTotalDespacho / totalDespachados : 0f;
    }
}