using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Herramienta para procesar textos avanzados usando expresiones regulares
using tp1; // ¡CLAVE! Importa el namespace donde viven ArbolGeneral y Cola
using static System.Runtime.InteropServices.JavaScript.JSType; // Interoperabilidad nativa de .NET (por defecto)

namespace tpfinal // Pertenece al espacio de nombres del proyecto del catálogo
{
    public class Estrategia
    {
        // --- MÉTODOS PENDIENTES DE IMPLEMENTAR (Esqueletos originales de la cátedra) ---
        
        public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            return "Implementar"; // Retorno provisorio
        }

        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
        {
            return ["Implementar"]; // Retorno provisorio usando sintaxis de colección moderna
        }

        //  --- MÉTODO 4: CONSULTA POR NIVELES (RESUELTO - RECORRIDO BFS CON FILTRO DE NOMBRES) ---
        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
        {
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

        // --- MÉTODO 1: AGREGAR (RESUELTO - INSERCIÓN DINÁMICA DE RUTAS E IDENTIFICACION DE DUPLICADOS) ---
        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
        {
            // ESCUDO PROTECTOR: Si no hay árbol o el producto a insertar es nulo, frena de inmediato
            if(arbol == null || dato == null)
            {
                return; // Corta el método (void) para proteger la memoria de datos basura
            }
            // Validamos que la ruta de texto no sea nula, ni esté vacía o llena de espacios
            if(string.IsNullOrWhiteSpace(rutaAlPadre)){
                throw new ArgumentoException("Un producto debe tener una ruta de categorias validas");
            }
            // Declaramos el puntero del viaje arrancando parados desde la raíz del árbol
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
            //evitar que se dupliquen los productos
            bool productoDuplicado = false;
            foreach(ArbolGeneral<ItemCat> hijo in nodoActual.GetHijos()){
                if(hijo.getDatoRaiz().esHoja && hijo.getDatoRaiz().Nombre.Equals(dato.Nombre, StringComparison.OrdinalIgnoreCase)){
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

        // --- MÉTODO 2: BUSCAR (RESUELTO - RECORRIDO DFS VERTICAL) ---
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

        // --- MÉTODO 3: TODOS (RESUELTO - RECORRIDO BFS CON FILTRO DE PRODUCTOS) ---
        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            // Instancia la lista plana donde guardaremos únicamente los productos finales
            List<ItemCat> resultado = new List<ItemCat>();
            
            // Instancia la clase Cola para coordinar el recorrido iterativo por niveles
            Cola<ArbolGeneral<ItemCat>> pendientes = new Cola<ArbolGeneral<ItemCat>>();
            
            // ESCUDO DE SEGURIDAD: Si no hay árbol, corta de inmediato y devuelve la lista vacía
            if(arbol == null)
            {
                return resultado;
            }
            
            // Metemos la raíz en la fila para arrancar la exploración piso por piso
            pendientes.encolar(arbol);

            // BUCLE MOTOR: Mientras queden nodos anotados en la lista de espera...
            while (!pendientes.esVacia())
            {
                // Sacamos al nodo que estaba primero en la fila
                ArbolGeneral<ItemCat> actual = pendientes.desencolar();
                
                // FILTRO DE TIPO: Pregunta si el elemento actual es estrictamente un Producto (y descarta las Categorías)
                if(actual.getDatoRaiz().Tipo == TipoElemento.Producto)
                {
                    // Si es un producto real con precio y SKU, lo mete en la bolsa acumuladora
                    resultado.Add(actual.getDatoRaiz());
                }

                // RAMIFICACIÓN: Toma a todos los hijos directos del nivel inferior de este nodo
                foreach(ArbolGeneral<ItemCat> hijo in actual.getHijos())
                {
                    // Los anota ordenadamente al final de la fila de espera
                    pendientes.encolar(hijo);
                }
            }
            // Cuando la fila se vacía por completo, devuelve la bolsa con todos los productos del catálogo
            return resultado;
        }
            
    }
}
