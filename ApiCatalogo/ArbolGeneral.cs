using System;
using System.Collections.Generic;

namespace tp1
{
	[Serializable]
	public class ArbolGeneral<T>
	{

		private T dato;
		private List<ArbolGeneral<T>> hijos = new List<ArbolGeneral<T>>();

		public ArbolGeneral(T dato)
		{
			this.dato = dato;
		}

		public T getDatoRaiz()
		{
			return this.dato;
		}

		public List<ArbolGeneral<T>> getHijos()
		{
			return hijos;
		}

		public void agregarHijo(ArbolGeneral<T> hijo)
		{
			this.getHijos().Add(hijo);
		}

		public void eliminarHijo(ArbolGeneral<T> hijo)
		{
			this.getHijos().Remove(hijo);
		}


		public bool esHoja()
		{
			return this.getHijos().Count == 0;
		}

		public int altura()
		{
			if (this.esHoja()){
				return 1;
			}
			
			int maxAlturaHijos = 0;
			
			foreach (var hijo in this.getHijos()){
				int alturaHijo = hijo.altura();
				
				if(alturaHijo > maxAlturaHijos){
					maxAlturaHijos = alturaHijo;
				}
			}
			
			return 1+ maxAlturaHijos;
		}


		public int nivel(T dato)
		{
			if (this.getDatoRaiz().Equals(dato)) {
				return 1;}
			foreach(var hijo in this.getHijos()){
				int nivelEnHijo = hijo.nivel(dato);
				if (nivelEnHijo != -1){
					return 1 + nivelEnHijo;
				}
			}
		}

	}
}
