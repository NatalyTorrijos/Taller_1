using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LectorProductos : MonoBehaviour
{
    public List<Producto> listaProductos = new List<Producto>();

    void Awake()
    {
        // ✅ Ajuste: ahora busca en la carpeta "StreamingAssets"
        string filePath = Path.Combine(Application.streamingAssetsPath, "productos.txt");

        if (File.Exists(filePath))
        {
            string[] lineas = File.ReadAllLines(filePath);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue; // evitar líneas vacías

                string[] datos = linea.Split('|');

                if (datos.Length == 6) // validar que tenga todos los campos
                {
                    Producto p = new Producto(
                        datos[0],                // id como string
                        datos[1],                // nombre
                        datos[2],                // tipo
                        float.Parse(datos[3]),   // peso
                        float.Parse(datos[4]),   // precio
                        float.Parse(datos[5])    // tiempo
                    );
                    listaProductos.Add(p);
                }
                else
                {
                    Debug.LogWarning("⚠ Línea inválida en productos.txt: " + linea);
                }
            }

            Debug.Log("✅ Productos cargados: " + listaProductos.Count);
        }
        else
        {
            Debug.LogError("❌ No se encontró el archivo en: " + filePath);
        }
    }

    public Producto GetProductoAleatorio()
    {
        if (listaProductos.Count == 0)
        {
            Debug.LogWarning("⚠ No hay productos cargados.");
            return null;
        }

        int index = Random.Range(0, listaProductos.Count);
        return listaProductos[index];
    }
}
