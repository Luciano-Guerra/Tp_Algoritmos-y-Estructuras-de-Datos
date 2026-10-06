
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


        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            return  [];
        }

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
		{
            string[] pasos= rutaAlPadre.Split('/');
            ArbolGeneral<ItemCat> nodoActual = arbol;

            foreach (string paso in pasos)
            {
                ArbolGeneral<ItemCat> hijoEncontrado= null;
                
                foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos()){
                    if(hijo.getDatoRaiz().Nombre == pasos) {hijoEncontrado = hijo;}
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
            ArbolGeneral<ItemCat> hijoNuevo = new ArbolGeneral<ItemCat> (dato);
            nodoActual.agregarHijo(hijoNuevo);
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
			List<ItemCat> resultado = new List<ItemCat>();
			Queue<ArbolGeneral<ItemCat>> buscado = new Queue<ArbolGeneral<ItemCat>>();
			buscado.Enqueue(arbol);

			while(buscado.Count > 0)
			{
				ArbolGeneral<ItemCat> actual = buscado.Dequeue();
				if(actual.getDatoRaiz().Nombre.Contains(elementoABuscar))
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

		public todos(ArbolGeneral<ItemCat> arbol)
		{
			List<ItemCat> resultado = new List<ItemCat>();
			Queue<ArbolGeneral<ItemCat>> pendientes = new Queue<ArbolGeneral<ItemCat>>();
			pendientes.Enqueue(arbol);

			while (pendientes.Count > 0)
			{
				ArbolGeneral<ItemCat> actual = pendientes.Dequeue();
				if()
				{
				}

				foreach(ArbolGeneral<ItemCat> hijo in actual.getHijos())
				{
					pendientes.Enqueue(hijo);
				}

			}
		}
            
    }
}
