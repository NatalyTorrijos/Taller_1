using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class GeneradorProductos : MonoBehaviour
{
    public LectorProductos lector;
    public GestorPila GestorPila;
    public float intervalo = 1f;
    private bool enEjecucion = false;
    private Coroutine rutinaGeneracion;

    private int totalGenerados = 0;
    private float tiempoInicio;

    // 🔹 Referencias UI
    public GameObject panelResultados;
    public TMP_Text totalGeneradosText;
    public TMP_Text totalDespachadosText;
    public TMP_Text totalEnPilaText;
    public TMP_Text tiempoPromedioText;
    public TMP_Text tiempoTotalGeneracionText;
    public TMP_Text tiempoTotalDespachoText;
    public TMP_Text tipoMasDespachadoText;
    public TMP_Text despachoPorTipoText; // ✅ Nuevo campo para mostrar despachos por tipo

    // 🔹 Botón "Cerrar Interacción"
    public void CerrarInteraccion()
    {
        // 1. Detener generación y despacho
        DetenerGeneracion();

        // 2. Calcular métricas
        int generados = totalGenerados;
        int despachados = GestorPila.GetTotalDespachados();
        int enPila = GestorPila.GetTamañoPila();
        float promedio = GestorPila.GetTiempoPromedioDespacho();
        float totalGen = Time.time - tiempoInicio;
        float totalDesp = GestorPila.GetTiempoTotalDespacho();
        string tipoMas = GestorPila.GetTipoMasDespachado();

        // 3. Mostrar en UI
        if (panelResultados != null)
        {
            panelResultados.SetActive(true);
            totalGeneradosText.text = "Total Generados: " + generados;
            totalDespachadosText.text = "Total Despachados: " + despachados;
            totalEnPilaText.text = "Total en Pila: " + enPila;
            tiempoPromedioText.text = "Promedio Despacho: " + promedio.ToString("F2") + "s";
            tiempoTotalGeneracionText.text = "Tiempo Total Generación: " + totalGen.ToString("F2") + "s";
            tiempoTotalDespachoText.text = "Tiempo Total Despacho: " + totalDesp.ToString("F2") + "s";
            tipoMasDespachadoText.text = "Tipo más despachado: " + tipoMas;

            // 🔹 Mostrar detalle de despachos por tipo
            var dict = GestorPila.GetDespachadosPorTipo();
            string detalleTipos = "Despachos por tipo:\n";
            foreach (var kvp in dict)
            {
                detalleTipos += kvp.Key + ": " + kvp.Value + "\n";
            }
            despachoPorTipoText.text = detalleTipos;
        }

        // 4. Guardar resultados en JSON
        GuardarResultadosJSON();
    }

    private void GuardarResultadosJSON()
    {
        try
        {
            // 🔹 Convertir diccionario en lista serializable
            var dict = GestorPila.GetDespachadosPorTipo();
            List<TipoConteo> listaTipos = new List<TipoConteo>();
            foreach (var kvp in dict)
            {
                listaTipos.Add(new TipoConteo(kvp.Key, kvp.Value));
            }

            var resultados = new MetricasSimulacion
            {
                totalGenerados = totalGenerados,
                totalDespachados = GestorPila.GetTotalDespachados(),
                totalEnPila = GestorPila.GetTamañoPila(),
                tiempoPromedioDespacho = GestorPila.GetTiempoPromedioDespacho(),
                tiempoTotalGeneracion = Time.time - tiempoInicio,
                tiempoTotalDespacho = GestorPila.GetTiempoTotalDespacho(),
                despachadosPorTipo = listaTipos,
                tipoMasDespachado = GestorPila.GetTipoMasDespachado()
            };

            string jsonString = JsonUtility.ToJson(resultados, true);
            string directoryPath = Application.streamingAssetsPath;
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, "resultadosSimulacion.json");
            File.WriteAllText(filePath, jsonString);
            Debug.Log("Resultados guardados en: " + filePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar archivo JSON: " + ex.Message);
        }
    }

    public void IniciarGeneracion()
    {
        if (!enEjecucion)
        {
            tiempoInicio = Time.time; // 🔹 Guardar tiempo de inicio
            enEjecucion = true;
            rutinaGeneracion = StartCoroutine(GenerarProductosCoroutine());
            GestorPila.IniciarDespacho();
        }
    }

    public void DetenerGeneracion()
    {
        if (enEjecucion)
        {
            enEjecucion = false;
            if (rutinaGeneracion != null)
            {
                StopCoroutine(rutinaGeneracion);
                rutinaGeneracion = null;
            }
            GestorPila.DetenerDespacho();
        }
    }

    private IEnumerator GenerarProductosCoroutine()
    {
        while (enEjecucion)
        {
            int cantidad = Random.Range(1, 4);
            Debug.Log("Generando " + cantidad + " productos...");
            totalGenerados += cantidad;

            for (int i = 0; i < cantidad; i++)
            {
                Producto nuevo = lector.GetProductoAleatorio();
                if (nuevo != null)
                {
                    GestorPila.ApilarProducto(nuevo);
                }
            }

            yield return new WaitForSeconds(intervalo);
        }
    }
}

// 🔹 Clase auxiliar para serializar el conteo por tipo
[System.Serializable]
public class TipoConteo
{
    public string tipo;
    public int cantidad;

    public TipoConteo(string tipo, int cantidad)
    {
        this.tipo = tipo;
        this.cantidad = cantidad;
    }
}

// 🔹 Clase de métricas finales para exportar a JSON
[System.Serializable]
public class MetricasSimulacion
{
    public int totalGenerados;
    public int totalDespachados;
    public int totalEnPila;
    public float tiempoPromedioDespacho;
    public float tiempoTotalGeneracion;
    public float tiempoTotalDespacho;
    public List<TipoConteo> despachadosPorTipo; // ✅ ahora lista serializable
    public string tipoMasDespachado;
}
