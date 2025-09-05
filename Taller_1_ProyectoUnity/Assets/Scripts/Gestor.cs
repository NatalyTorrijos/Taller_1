using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LectorProductos : MonoBehaviour
{
    public List<Producto> listaProductos = new List<Producto>();

    void Awake()
    {
     
        string filePath = Path.Combine(Application.streamingAssetsPath, "productos.txt");

        if (File.Exists(filePath))
        {
            string[] lineas = File.ReadAllLines(filePath);

            foreach (string linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue; 

                string[] datos = linea.Split('|');

                if (datos.Length == 6) 
                {
                    Producto p = new Producto(
                        datos[0],               
                        datos[1],                
                        datos[2],                
                        float.Parse(datos[3]),   
                        float.Parse(datos[4]),   
                        float.Parse(datos[5])    
                    );
                    listaProductos.Add(p);
                }
                else
                {
                    Debug.LogWarning(" Línea inválida en productos.txt: " + linea);
                }
            }

            Debug.Log(" Productos cargados: " + listaProductos.Count);
        }
        else
        {
            Debug.LogError("No se encontró el archivo en: " + filePath);
        }
    }

    public Producto GetProductoAleatorio()
    {
        if (listaProductos.Count == 0)
        {
            Debug.LogWarning(" No hay productos cargados.");
            return null;
        }

        int index = Random.Range(0, listaProductos.Count);
        return listaProductos[index];
    }
}
