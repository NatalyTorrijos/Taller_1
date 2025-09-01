using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LectorProductos : MonoBehaviour
{
    public List<Producto> listaProductos = new List<Producto>();

    void Awake()
    {
        string path = Application.dataPath + "/productos.txt";
        if (File.Exists(path))
        {
            string[] lineas = File.ReadAllLines(path);
            foreach (string linea in lineas)
            {
                string[] datos = linea.Split('|');
                Producto p = new Producto(
                    datos[0], datos[1], datos[2],
                    float.Parse(datos[3]),
                    float.Parse(datos[4]),
                    float.Parse(datos[5])
                );
                listaProductos.Add(p);
            }
        }
    }

    public Producto GetProductoAleatorio()
    {
        int index = Random.Range(0, listaProductos.Count);
        return listaProductos[index];
    }
}
