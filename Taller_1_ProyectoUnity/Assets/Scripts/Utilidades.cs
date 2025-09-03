using UnityEngine;
using System.Collections;
public class GeneradorProductos : MonoBehaviour
{
    public LectorProductos lector;   
    public float intervalo = 1f;    
    private bool enEjecucion = false;
    private Coroutine rutinaGeneracion;
    public void IniciarGeneracion()
    {
        if (!enEjecucion)
        {
            enEjecucion = true;
            rutinaGeneracion = StartCoroutine(GenerarProductosCoroutine());
           
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
            }

            yield return new WaitForSeconds(intervalo);
        }
    }
}



