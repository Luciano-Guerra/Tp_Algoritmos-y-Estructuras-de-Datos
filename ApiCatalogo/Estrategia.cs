using System;
using System.Collections.Generic;
using System.Text.RegularExpressions; // Herramienta para procesar textos avanzados usando expresiones regulares
using tp1; // ¡CLAVE! Importa el namespace donde viven ArbolGeneral y Cola
using static System.Runtime.InteropServices.JavaScript.JSType; // Interoperabilidad nativa de .NET (por defecto)

namespace tpfinal // Pertenece al espacio de nombres del proyecto del catálogo
{
    public class Estrategia
    {
        // --- MÉTODO 5: URL SEO POR ID (RESUELTO - RECORRIDO DFS VERTICAL) ---
        public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            // ESCUDO DE SEGURIDAD: Si no hay árbol, corta de inmediato y devuelve la cadena vacía
            if(arbol == null) {
                return ""; // Corta el método (string) para proteger la memoria de datos basura
            }
            // Llama al método auxiliar recursivo para buscar la URL SEO correspondiente al ID
            string urlResultado = GetUrlSeoPorIdAuxiliar(arbol, id, "");
            return urlResultado ?? ""; // Si no se encuentra el ID, devuelve cadena vacía para evitar que el programa se rompa(usando '??')
            
        }
        // Método auxiliar recursivo para construir la URL SEO a partir del ID
        private string GetUrlSeoPorIdAuxiliar(ArbolGeneral<ItemCat> nodo, int id, string urlAcumulado){
            // ESCUDO DE SEGURIDAD: Si el nodo es nulo, corta de inmediato y devuelve null
            if(nodo == null){
                return null;
            }
            // Construye la URL SEO acumulada hasta este nodo, reemplazando espacios por guiones y convirtiendo a minúsculas
            string nuevaUrl = urlAcumulado + "/" + nodo.getDatoRaiz().Nombre.ToLower().Replace(" ", "-");
            // Si es un nodo hoja y su ID coincide con el buscado, devuelve la URL construida
            if(nodo.esHoja() && nodo.getDatoRaiz().Id == id){
                return nuevaUrl;
            }
            // Recorre recursivamente los hijos del nodo actual para buscar el ID en niveles inferiores
            if(nodo.getHijos() != null){
                foreach(ArbolGeneral<ItemCat> nodoHijo in nodo.getHijos()){
                    string urlHijo = GetUrlSeoPorIdAuxiliar(nodoHijo, id, nuevaUrl); //Recursividad
                    // Si se encuentra la URL en algún hijo, la devuelve inmediatamente
                    if(!string.IsNullOrEmpty(urlHijo)){
                        return urlHijo;
                    }
                }
            }
            return null;// Si no se encuentra el ID en este nodo ni en sus hijos, devuelve null para indicar que no se halló la URL SEO
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

        //  --- MÉTODO 4: CONSULTA POR NIVELES (RESUELTO - RECORRIDO BFS CON FILTRO DE NOMBRES) ---
        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
        {
            // Instancia la lista final que contendrá sublistas de nombres por nivel
            List<<List<string>> listaFinal = new List<List<string>>();
            // ESCUDO DE SEGURIDAD: Si no hay árbol, corta de inmediato y devuelve la lista vacía
            if(arbol == null){
                return listaFinal;// Corta el método (List<List<string>>) para proteger la memoria de datos basura
            }
            // Instancia una cola para realizar el recorrido por niveles (BFS)
            List<ArbolGeneral<ItemCat>> nodosNivel = new List<ArbolGeneral<ItemCat>>();
            // Agrega la raíz del árbol a la cola para iniciar el recorrido
            Queue<ArbolGeneral<ItemCat>> cola = new Queue<ArbolGeneral<ItemCat>>();
            cola.Enqueue(arbol);
            // BUCLE MOTOR: Mientras queden nodos en la cola, procesa cada nivel del árbol
            while(cola.Count > 0){
                // Determina cuántos nodos hay en el nivel actual (la cantidad de elementos en la cola)
                int nodosEnEsteNivel = cola.Count;
                List<string> nombresNivel = new List<string>; // Lista temporal para almacenar los nombres de los nodos en el nivel actual
                // Recorre todos los nodos en el nivel actual
                for (int i = 0; i < nodosEnEsteNivel; i++){
                    ArbolGeneral<ItemCat> nodoActual = cola.Dequeue();// Saca el primer nodo de la cola para procesarlo
                    nombresNivel.Add(nodoActual.getDatoRaiz().Nombre);// Agrega el nombre del nodo actual a la lista de nombres del nivel
                    // Si el nodo actual tiene hijos, los agrega a la cola para procesarlos en el siguiente nivel
                    if(nodoActual.getHijos() != null){
                        // Recorre cada hijo del nodo actual y los agrega a la cola para el siguiente nivel
                        foreach(ArbolGeneral<ItemCat> hijo in nodoActual.getHijos()){
                            cola.Enqueue(hijo);// Agrega cada hijo a la cola para procesarlo en el siguiente nivel
                        }
                    }
                }
                listaFinal.Add(nombresNivel);// Agrega la lista de nombres del nivel actual a la lista final que contendrá todos los niveles
            }
            return listaFinal;// Devuelve la lista final con los nombres de los nodos organizados por niveles
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
            // Separa la ruta de texto en partes usando '/' como delimitador y eliminando entradas vacías
            string[] pasos = rutaAlPadre.Split('/', StringSplitOption.RemoveEmptyEntries);
            // Recorre cada paso de la ruta para navegar o crear nodos de categoría según corresponda
            foreach(string paso in pasos){
                ArbolGeneral<ItemCat> hijoEncontrado = null; // Inicializa la variable para buscar si el hijo ya existe
                // Recorre los hijos del nodo actual para ver si alguno coincide con el paso de la ruta
                foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos()){
                    // Compara el nombre del hijo con el paso de la ruta ignorando mayúsculas y minúsculas
                    if(hijo.getDatoRaiz().Nombre.Equals(paso, StringComparison.OrdinalIgnoreCase)){
                        // Si encuentra coincidencia, guarda el hijo y rompe el bucle
                        hijoEncontrado = hijo;
                        break;
                    }
                }
                // Si se encontró un hijo que coincide con el paso de la ruta, avanzamos a ese nodo; si no, creamos uno nuevo
                if(hijoEncontrado != null){
                    nodoActual = hijoEncontrado; // Avanza al hijo encontrado para continuar la navegación por la ruta
                }
                // Si no se encontró un hijo que coincida, creamos un nuevo nodo de categoría y lo agregamos al árbol
                else{
                ItemCat categoria = new ItemCat(paso, TipoElemento.Categoria); // Crea un nuevo objeto de categoría con el nombre del paso actual
                ArbolGeneral<ItemCat> arbolN = new ArbolGeneral<ItemCat> (categoria); // Crea un nuevo nodo de árbol con la categoría recién creada
                nodoActual.agregarHijo(arbolN); // Agrega el nuevo nodo como hijo del nodo actual
                nodoActual = arbolN; // Avanza al nuevo nodo creado para continuar la navegación por la ruta
                }
            }
            //evitar que se dupliquen los productos
            bool productoDuplicado = false;
            // Recorre los hijos del nodo actual para verificar si ya existe un producto con el mismo nombre
            foreach(ArbolGeneral<ItemCat> hijo in nodoActual.GetHijos()){
                // Compara si el hijo es una hoja (producto) y si su nombre coincide con el del producto a insertar, ignorando mayúsculas y minúsculas
                if(hijo.esHoja() && hijo.getDatoRaiz().Nombre.Equals(dato.Nombre, StringComparison.OrdinalIgnoreCase)){
                    // Si encuentra un producto duplicado, marca la bandera y rompe el bucle
                    productoDuplicado = true; 
                    break;
                }
            }
            // Si se detecta un producto duplicado, lanza una excepción para evitar la inserción y notificar al usuario
            if(productoDuplicado == true){
                throw new InvalidOperationException($"El producto '{dato.Nombre}' ya existe en esta categoria");
            }
            ArbolGeneral<ItemCat> hijoNuevo = new ArbolGeneral<ItemCat>(dato); // Crea un nuevo nodo de árbol para el producto a insertar
            nodoActual.agregarHijo(hijoNuevo);// Agrega el nuevo nodo de producto como hijo del nodo actual, completando la inserción en la ruta especificada
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
