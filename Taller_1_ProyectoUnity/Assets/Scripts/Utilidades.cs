using UnityEngine;
using System.Collections;

public class GeneradorProductos : MonoBehaviour

{
    public LectorProductos lector; 
    public GestorPila GestorPila;
    public float intervalo = 1f;    
    private bool enEjecucion = false;
    
  

    private Coroutine rutinaGeneracion;
    public void IniciarGeneracion()
    {
        if (!enEjecucion)
        {
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



