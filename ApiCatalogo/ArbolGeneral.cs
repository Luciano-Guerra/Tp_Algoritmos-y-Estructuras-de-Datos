using System;
using System.Collections.Generic;

namespace tp1 
{
	[Serializable] // Etiqueta que indica que esta estructura se puede convertir en bytes para viajar por red o guardarse
	public class ArbolGeneral<T> // Declaración de la clase del Árbol General usando un Tipo Genérico <T>
	{
		// --- ATRIBUTOS (VARIABLES INTERNAS) ---
		private T dato; // Variable privada que guarda el valor real del nodo (en tu caso, un objeto ItemCat)
		private List<ArbolGeneral<T>> hijos = new List<ArbolGeneral<T>>(); // Lista dinámica que almacena los subárboles hijos

		// --- CONSTRUCTOR ---
		public ArbolGeneral(T dato)
		{
			this.dato = dato; // Obliga a que al crear un nodo, sí o sí se le asigne su dato correspondiente
		}

		// --- MÉTODOS DE ACCESO Y CONFIGURACIÓN ---
		public T getDatoRaiz()
		{
			return this.dato; // Devuelve el dato guardado en la raíz de este nodo actual
		}

		public List<ArbolGeneral<T>> getHijos()
		{
			return hijos; // Devuelve la lista completa de referencias a sus subárboles hijos
		}

		public void agregarHijo(ArbolGeneral<T> hijo)
		{
			this.getHijos().Add(hijo); // Añade un nuevo subárbol como hijo directo en el nivel inferior
		}

		public void eliminarHijo(ArbolGeneral<T> hijo)
		{
			this.getHijos().Remove(hijo); // Quita de la lista al hijo que coincida con el pasado por parámetro
		}

		public bool esHoja()
		{
			return this.getHijos().Count == 0; // Devuelve true si el nodo no tiene descendientes; false si tiene al menos uno
		}

		// --- ALGORITMO 1: ALTURA (RECURSIVO - DFS) ---
		public int altura()
		{
			if (this.esHoja()){
				return 1; // CASO BASE: Si es un nodo hoja, su altura es 1 y frena la recursión para esta rama
			}
			
			int maxAlturaHijos = 0; // Variable "memoria" para registrar la rama más profunda entre todos los hijos
			
			foreach (var hijo in this.getHijos()){ // Bucle iterativo que visita cada subárbol hijo
				int alturaHijo = hijo.altura(); // LLAMADA RECURSIVA: El hijo calcula su propia altura
				
				if(alturaHijo > maxAlturaHijos){
					maxAlturaHijos = alturaHijo; // Si la altura de este hijo es mayor a la máxima conocida, la actualiza
				}
			}
			
			return 1 + maxAlturaHijos; // Devuelve el camino más largo de sus hijos incrementado en 1 (el nivel actual)
		}

		// --- ALGORITMO 2: NIVEL (RECURSIVO - DFS) ---
		public int nivel(T dato)
		{
			// CASO BASE DE ÉXITO: Si el dato de la raíz actual es igual al buscado, estamos en el inicio
			if (this.getDatoRaiz().Equals(dato)) {
				return 1; // Devuelve 1 (La raiz es contada como 1 en este caso)
			}
			
			// RECORREDOR RECURSIVO: Si no era la raíz, busca profundamente en cada uno de sus hijos
			foreach(var hijo in this.getHijos()){
				int nivelEnHijo = hijo.nivel(dato); // LLAMADA RECURSIVA: El hijo busca el dato en sus ramas
				
				if (nivelEnHijo != -1){
					return 1 + nivelEnHijo; // Si el hijo lo encontró (devolvió algo distinto de -1), sumamos 1 y subimos el resultado
				}
			}
			return -1; // CASO DE FALLO: Si terminó el foreach y ningún hijo encontró el dato, devolvemos -1
		}
	}
}
