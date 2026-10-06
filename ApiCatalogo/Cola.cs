using System;
using System.Collections.Generic;

namespace tp1
{
    // --- ATRIBUTOS (VARIABLES INTERNAS) ---
    private List<T> datos = new List<T>();
    
    // --- MÉTODO: ENCOLAR (void: realiza la acción pero no devuelve nada) ---
    // Inserta un elemento al final de la fila (Operación Push / Enqueue)
    public void encolar(T elem)
    {
        // Utiliza el método .Add() nativo de C# para meter el elemento al final de la lista interna
        this.datos.Add(elem);
    }
    
    // --- MÉTODO: DESENCOLAR (Devuelve un elemento de tipo T) ---
    // Atiende, remueve y retorna al primero de la fila (Operación Pop / Dequeue)
    public T desencolar()
    {
        // 1. Copia y guarda temporalmente el elemento que está en la posición 0 (el primero de la fila)
        T temp = this.datos[0];
        
        // 2. Elimina físicamente el elemento en el índice 0. 
        // C# reordena la memoria automáticamente haciendo que el que estaba en el índice 1 pase al 0, y así con todos.
        this.datos.RemoveAt(0);
        
        // 3. Devuelve el elemento que guardamos en la variable temporal
        return temp;
    }
    
    // --- MÉTODO: TOPE (Devuelve un elemento de tipo T) ---
    // Permite "espiar" quién está primero en la fila sin sacarlo de la estructura (Operación Peek)
    public T tope()
    {
        // Retorna el elemento en la posición 0, pero NO lo elimina de la lista interna
        return this.datos[0];
    }
    
    // --- MÉTODO: ES VACÍA (Devuelve un booleano true/false) ---
    // Controla si la estructura se quedó sin elementos para evitar errores antes de desencolar
    public bool esVacia()
    {
        // Compara si el conteo de la lista es igual a 0. Devuelve true si está vacía, false si tiene datos.
        return this.datos.Count == 0;
    }
    
    // --- MÉTODO: CANTIDAD ELEMENTOS (Devuelve un entero) ---
    // Indica cuántos turnos o elementos tiene registrados la cola en este momento
    public int cantidadElementos()
    {
        // Devuelve el número total de ítems guardados en la lista interna
        return this.datos.Count;
    }
}
