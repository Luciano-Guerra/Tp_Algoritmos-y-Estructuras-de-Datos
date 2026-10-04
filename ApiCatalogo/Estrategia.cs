
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
            string[] pasos= string rutaAlPadre.Split('/');
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
			return [];
		}
            
    }
}
