using UnityEngine;

[System.Serializable]
public class Producto
{
    public string id;
    public string nombre;
    public string tipo;
    public float peso;
    public float precio;
    public float tiempo;

    public Producto(string id, string nombre, string tipo, float peso, float precio, float tiempo)
    {
        this.id = id;
        this.nombre = nombre;
        this.tipo = tipo;
        this.peso = peso;
        this.precio = precio;
        this.tiempo = tiempo;
    }
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }

    public string Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }

    public float Peso
    {
        get { return peso; }
        set { peso = value; }
    }

    public float Precio
    {
        get { return precio; }
        set { precio = value; }
    }

    public float Tiempo
    {
        get { return tiempo; }
        set { tiempo = value; }
    }

    public override string ToString()
    {
        return $"{Nombre} ({Tipo}) | Peso: {Peso} | Precio: {Precio} | Tiempo: {Tiempo}\n";
    }



}



