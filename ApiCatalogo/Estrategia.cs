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

        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
        {
            return [["Implementar"]]; // Retorno provisorio de lista de listas
        }

        // --- MÉTODO 1: AGREGAR (RESUELTO - INSERCIÓN DINÁMICA DE RUTAS) ---
        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
        {
            // ESCUDO PROTECTOR: Si no hay árbol o el producto a insertar es nulo, frena de inmediato
            if(arbol == null || dato == null)
            {
                return; // Corta el método (void) para proteger la memoria de datos basura
            }
            
            // Declaramos el puntero del viaje arrancando parados desde la raíz del árbol
            ArbolGeneral<ItemCat> nodoActual = arbol;

            // Validamos que la ruta de texto no sea nula, ni esté vacía o llena de espacios
            if(!string.IsNullOrWhiteSpace(rutaAlPadre))
            {
                // Rompe el texto por las barras y elimina entradas vacías accidentales (ej: "Moda//Running")
                string[] pasos = rutaAlPadre.Split('/', StringSplitOptions.RemoveEmptyEntries);
                
                // BUCLE PRINCIPAL: Procesa palabra por palabra la ruta (ej: primero "Moda", luego "Ropa")
                foreach (string paso in pasos)
                {
                    ArbolGeneral<ItemCat> hijoEncontrado = null; // Asumimos que la carpeta no existe en este nivel
                
                    // BUCLE INTERNO: Revisa uno por uno los hijos directos del nodo donde estamos parados
                    foreach (ArbolGeneral<ItemCat> hijo in nodoActual.getHijos())
                    {
                        // Si el nombre del hijo coincide exactamente con el escalón de la ruta que buscamos...
                        if(hijo.getDatoRaiz().Nombre == paso) 
                        {
                            hijoEncontrado = hijo; // ¡Lo encontramos! Guardamos su referencia
                        }
                    }

                    // TOMAMOS UNA DECISIÓN BASADA EN LA BÚSQUEDA:
                    if (hijoEncontrado != null)
                    {
                        // Opción A: Si ya existía, avanzamos nuestro viaje y nos paramos sobre ese hijo
                        nodoActual = hijoEncontrado;
                    }
                    else
                    {
                        // Opción B: Si NO existía, creamos la categoría intermedia obligatoriamente
                        ItemCat categoria = new ItemCat(paso, TipoElemento.Categoria); // Crea el dato
                        ArbolGeneral<ItemCat> arbolN = new ArbolGeneral<ItemCat>(categoria); // Lo envuelve en un nodo
                        
                        nodoActual.agregarHijo(arbolN); // Cuelga la nueva categoría del padre actual
                        nodoActual = arbolN; // Movemos nuestra posición adentro de la categoría que acabamos de crear
                    }
                }
            }
            
            // CIERRE DE AGREGAR: Ya sea que la ruta estaba vacía o viajamos 10 niveles, 
            // envolvemos el producto real final en un nodo y lo colgamos en la ubicación exacta
            ArbolGeneral<ItemCat> hijoNuevo = new ArbolGeneral<ItemCat>(dato);
            nodoActual.agregarHijo(hijoNuevo);
        }

        // --- MÉTODO 2: BUSCAR (RESUELTO - RECORRIDO BFS HORIZONTAL) ---
        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
        {
            // Instancia la lista plana donde acumularemos las coincidencias de texto
            List<ItemCat> resultado = new List<ItemCat>();
            
            // Instancia la clase Cola para administrar el orden horizontal (Piso por piso)
            Cola<ArbolGeneral<ItemCat>> buscado = new Cola<ArbolGeneral<ItemCat>>();

            // ESCUDO DE SEGURIDAD: Si el árbol es nulo o no pasaron un texto para buscar, devolvemos la lista vacía
            if(arbol == null || string.IsNullOrWhiteSpace(elementoABuscar))
            {
                return resultado;
            }
            
            // Encolamos el nodo raíz para encender el motor del bucle
            buscado.encolar(arbol);

            // Mientras la cola tenga elementos pendientes por visitar...
            while(!buscado.esVacia())
            {
                // Desencolamos el nodo al que le toca el turno y lo guardamos en 'actual'
                ArbolGeneral<ItemCat> actual = buscado.desencolar();
                
                // CONDICIONAL FILTRO: Compara si el nombre del ítem contiene el texto buscado (ignorando mayúsculas/minúsculas)
                if(actual.getDatoRaiz().Nombre.Contains(elementoABuscar, StringComparison.OrdinalIgnoreCase))
                {
                    // Si coincide total o parcialmente, guarda el ItemCat adentro de nuestra bolsa de resultados
                    resultado.Add(actual.getDatoRaiz());
                }
                
                // RECORRER HIJOS: Explora los descendientes directos del nodo procesado
                foreach(ArbolGeneral<ItemCat> hijo in actual.getHijos())
                {
                    // Los manda al final de la fila de espera para revisarlos cuando toque su nivel
                    buscado.encolar(hijo);
                }
            }
            // Retorna la lista con todas las coincidencias recolectadas
            return resultado;
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
