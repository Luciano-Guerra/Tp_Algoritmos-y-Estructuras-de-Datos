
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{

	public class Estrategia
	{
		
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            return "Implementar";
        }
        

        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
		{
			return ["Implementar"];
		}
        

              

        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
		{
            return [["Implementar"]];
        }

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
		{
			
			if(arbol  == null || dato == null)
			{
				return;
			}
			
            ArbolGeneral<ItemCat> nodoActual = arbol;

			if(!string.IsNullOrWhiteSpace(rutaAlPadre))
			{
				string[] pasos= rutaAlPadre.Split('/', StringSplitOptions.RemoveEmptyEntries);
            	foreach (string paso in pasos)
            	{
                	ArbolGeneral<ItemCat> hijoEncontrado= null;
                
                	foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos())
					{
                    	if(hijo.getDatoRaiz().Nombre == paso) {hijoEncontrado = hijo;}
                	}

                	if (hijoEncontrado != null){
                    	nodoActual = hijoEncontrado;
                	}else
                	{
                    	ItemCat categoria = new ItemCat (paso, TipoElemento.Categoria);
                    	ArbolGeneral<ItemCat> arbolN= new ArbolGeneral<ItemCat>(categoria);
                    	nodoActual.agregarHijo(arbolN);
                    	nodoActual = arbolN;
                	}
            	}
			}
            ArbolGeneral<ItemCat> hijoNuevo = new ArbolGeneral<ItemCat> (dato);
            nodoActual.agregarHijo(hijoNuevo);
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
			List<ItemCat> resultado = new List<ItemCat>();
			Queue<ArbolGeneral<ItemCat>> buscado = new Queue<ArbolGeneral<ItemCat>>();

			if(arbol  == null)
			{
				return resultado;
			}
			
			buscado.Enqueue(arbol);

			while(buscado.Count > 0)
			{
				ArbolGeneral<ItemCat> actual = buscado.Dequeue();
				if(actual.getDatoRaiz().Nombre.Contains(elementoABuscar, StringComparison.OrdinalIgnoreCase))
				{
					resultado.Add(actual.getDatoRaiz());
				}
				foreach(ArbolGeneral<ItemCat> hijo in actual.getHijos())
				{
					buscado.Enqueue(hijo);
				}
			}
			return resultado;
		}

		public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
		{
			List<ItemCat> resultado = new List<ItemCat>();
			Queue<ArbolGeneral<ItemCat>> pendientes = new Queue<ArbolGeneral<ItemCat>>();
			
			if(arbol  == null)
			{
				return resultado;
			}
			
			pendientes.Enqueue(arbol);

			while (pendientes.Count > 0)
			{
				ArbolGeneral<ItemCat> actual = pendientes.Dequeue();
				if(actual.getDatoRaiz().Tipo == TipoElemento.Producto)
				{
					resultado.Add(actual.getDatoRaiz());
				}

				foreach(ArbolGeneral<ItemCat> hijo in actual.getHijos())
				{
					pendientes.Enqueue(hijo);
				}

			}
			return resultado;
		}
            
    }
}
