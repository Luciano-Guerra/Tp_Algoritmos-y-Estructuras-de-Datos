using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; 
using tp1; 
using static System.Runtime.InteropServices.JavaScript.JSType; 

namespace tpfinal{
    public class Estrategia{
        public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id){
            if(arbol == null) {
                return ""; 
            }
            string urlResultado = GetUrlSeoPorIdAuxiliar(arbol, id, "");
            return urlResultado ?? "";             
        }
        private string GetUrlSeoPorIdAuxiliar(ArbolGeneral<ItemCat> nodo, int id, string urlAcumulado){
            if(nodo == null){
                return null;
            }
            string nuevaUrl = urlAcumulado + "/" + nodo.getDatoRaiz().Nombre.ToLower().Replace(" ", "-");
            if(nodo.esHoja() && nodo.getDatoRaiz().Id == id){
                return nuevaUrl;
            }
            if(nodo.getHijos() != null){
                foreach(ArbolGeneral<ItemCat> nodoHijo in nodo.getHijos()){
                    string urlHijo = GetUrlSeoPorIdAuxiliar(nodoHijo, id, nuevaUrl); 
                    if(!string.IsNullOrEmpty(urlHijo)){
                        return urlHijo;
                    }
                }
            }
            return null;
        }
        
        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
        {
            List<string> urls = new List<string>();
            if(arbol == null) return null;
            GetURLsSEOAuxiliar(arbol, "", urls);
            return urls;

        }

        private void GetURLsSEOAuxiliar(ArbolGeneral<ItemCat> nodo, string urlAcumulado, List<string> urls)
        {
            if(nodo == null) return;
            string nuevaUrl = urlAcumulado + "/" + nodo.getDatoRaiz().Nombre.ToLower().Replace(" ", "-");
            if (nodo.esHoja())
            {
                urls.Add(nuevaUrl);
            }
            if(nodo.getHijos() != null){
                foreach(ArbolGeneral<ItemCat> hijo in nodo.getHijos())
                {
                    GetURLsSEOAuxiliar(hijo, nuevaUrl, urls);
                }
            }
        }
        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol){
            List<<List<string>> listaFinal = new List<List<string>>();
            if(arbol == null){
                return listaFinal;
            }
            List<ArbolGeneral<ItemCat>> nodosNivel = new List<ArbolGeneral<ItemCat>>();
            Queue<ArbolGeneral<ItemCat>> cola = new Queue<ArbolGeneral<ItemCat>>();
            cola.Enqueue(arbol);
            while(cola.Count > 0){
                int nodosEnEsteNivel = cola.Count;
                List<string> nombresNivel = new List<string>; 
                for (int i = 0; i < nodosEnEsteNivel; i++){
                    ArbolGeneral<ItemCat> nodoActual = cola.Dequeue();
                    nombresNivel.Add(nodoActual.getDatoRaiz().Nombre);
                    if(nodoActual.getHijos() != null){
                        foreach(ArbolGeneral<ItemCat> hijo in nodoActual.getHijos()){
                            cola.Enqueue(hijo);
                        }
                    }
                }
                listaFinal.Add(nombresNivel);
            }
            return listaFinal;
        }
        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre){
            if(arbol == null || dato == null){
                return; 
            }
            if(string.IsNullOrWhiteSpace(rutaAlPadre)){
                throw new ArgumentoException("Un producto debe tener una ruta de categorias validas");
            }
            ArbolGeneral<ItemCat> nodoActual = arbol;
            string[] pasos = rutaAlPadre.Split('/', StringSplitOption.RemoveEmptyEntries);
            foreach(string paso in pasos){
                ArbolGeneral<ItemCat> hijoEncontrado = null; 
                foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos()){
                    if(hijo.getDatoRaiz().Nombre.Equals(paso, StringComparison.OrdinalIgnoreCase)){
                        hijoEncontrado = hijo;
                        break;
                    }
                }
                if(hijoEncontrado != null){
                    nodoActual = hijoEncontrado;
                }
                else{
                ItemCat categoria = new ItemCat(paso, TipoElemento.Categoria); 
                ArbolGeneral<ItemCat> arbolN = new ArbolGeneral<ItemCat> (categoria); 
                nodoActual.agregarHijo(arbolN); 
                nodoActual = arbolN; 
                }
            }
            bool productoDuplicado = false;
            foreach(ArbolGeneral<ItemCat> hijo in nodoActual.GetHijos()){
                if(hijo.esHoja() && hijo.getDatoRaiz().Nombre.Equals(dato.Nombre, StringComparison.OrdinalIgnoreCase)){
                    productoDuplicado = true; 
                    break;
                }
            }
            if(productoDuplicado == true){
                throw new InvalidOperationException($"El producto '{dato.Nombre}' ya existe en esta categoria");
            }
            ArbolGeneral<ItemCat> hijoNuevo = new ArbolGeneral<ItemCat>(dato); 
            nodoActual.agregarHijo(hijoNuevo);
        }
        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
        {
            List<ItemCat> resultadoAcumulado = new List<ItemCat>();
            if(arbol == null || string.IsNullOrWhiteSpace(elementoABuscar)) return resultadoAcumulado;
            BuscarAuxiliar(arbol, elementoABuscar, resultadoAcumulado);
            return resultadoAcumulado;
        }
        private void BuscarAuxiliar(ArbolGeneral<ItemCat> nodo, string elementoABuscar, List<ItemCat> listaCompartida){
            if(nodo == null) return;
            if(nodo.getDatoRaiz().Nombre.Contains(elementoABuscar, StringComparison.OrdinalIgnoreCase)){
                listaCompartida.Add(nodo.getDatoRaiz());
            }
            foreach(ArbolGeneral<ItemCat> hijo in nodo.getHijos()){
                if(hijo != null){
                    BuscarAuxiliar(hijo, elementoABuscar, listaCompartida);
                }
            }
                
        }
        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol){
            List<ItemCat> resultado = new List<ItemCat>();
            Cola<ArbolGeneral<ItemCat>> pendientes = new Cola<ArbolGeneral<ItemCat>>();
            if(arbol == null){
                return resultado;
            }
            pendientes.encolar(arbol);
            while (!pendientes.esVacia()){
                ArbolGeneral<ItemCat> actual = pendientes.desencolar();
                if(actual.getDatoRaiz().Tipo == TipoElemento.Producto){
                    resultado.Add(actual.getDatoRaiz());
                }
                foreach(ArbolGeneral<ItemCat> hijo in actual.getHijos()){
                    pendientes.encolar(hijo);
                }
            }
            return resultado;
        }   
    }
}
