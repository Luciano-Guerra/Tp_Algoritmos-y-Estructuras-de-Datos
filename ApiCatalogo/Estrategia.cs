
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
            //implementar
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
			return [];
		}
            
    }
}