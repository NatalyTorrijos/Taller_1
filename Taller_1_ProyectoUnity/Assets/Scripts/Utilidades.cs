using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GeneradorProductos : MonoBehaviour
{
    public LectorProductos lector;
    public GestorPila GestorPila;
    public float intervalo = 1f;
    private bool enEjecucion = false;
    private Coroutine rutinaGeneracion;

    private int totalGenerados = 0;
    private float tiempoInicio;

    // 🔹 Nueva función para botón "Cerrar Interacción"
    public void CerrarInteraccion()
    {
        // 1. Detener generación y despacho
        DetenerGeneracion();

        // 2. Mostrar métricas finales
       

        // 3. Guardar resultados en JSON
        GuardarResultadosJSON();
    }

    private void GuardarResultadosJSON()
    {
        try
        {
            // 🔹 Convertir el diccionario en una lista serializable
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
