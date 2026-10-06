
using System;
using System.Collections.Generic;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{
    // Clase pública Util. Contiene herramientas de asistencia y soporte para el desarrollo del proyecto.
    public class Util
    {
        // --- MÉTODO ESTÁTICO: INIT (void: realiza modificaciones pero no retorna ningún valor) ---
        // Al ser 'static', se puede invocar directamente escribiendo 'Util.init(arbol)' sin hacer un 'new Util()'
        // Recibe por parámetro el árbol original (vacio) creado en Program.cs para poblarlo en la memoria RAM
        public static void init(ArbolGeneral<ItemCat> catalogo)
        {
            // Imprime un mensaje informativo directo en la terminal de Visual Studio Code para avisar que arrancó el proceso
            Console.WriteLine("Generando catálogo masivo...");

            // Crea un vector (array) de textos fijos con los nombres de los sectores o categorías principales
            string[] sectores = { "Electrónica", "Moda", "Hogar", "Deportes", "Belleza", "Juguetes" };
            
            // Instancia el generador de números aleatorios (el "dado electrónico") que simulará la asimetría del mundo real
            Random rnd = new Random();

            // =========================================================================
            // BUCLE DE NIVEL 1 (Sectores Principales): Recorre cada string del array de sectores
            // =========================================================================
            foreach (var sectorNombre in sectores)
            {
                // Crea un subárbol para el sector. Adentro le mete un objeto ItemCat configurado como Categoría.
                ArbolGeneral<ItemCat> sector = new ArbolGeneral<ItemCat>(
                    new ItemCat(sectorNombre, TipoElemento.Categoria)
                );
                
                // Cuelga este nuevo sector como hijo directo de la raíz del catálogo principal que llegó por parámetro
                catalogo.agregarHijo(sector);

                // Determina al azar cuántas subcategorías tendrá este sector (un número entero entre 2 y 6 inclusive)
                // Nota teórica: En .NET, el límite superior del rnd.Next(min, max) es EXCLUSIVO. Por eso se pone 7 para que tome hasta el 6.
                int numSubCategorias = rnd.Next(2, 7);
                
                // =========================================================================
                // BUCLE DE NIVEL 2 (Subcategorías): Se repite según el número aleatorio obtenido arriba
                // =========================================================================
                for (int i = 1; i <= numSubCategorias; i++)
                {
                    // Crea un subárbol para la subcategoría usando interpolación de texto (ej: "Electrónica - Grupo 1")
                    ArbolGeneral<ItemCat> subCat = new ArbolGeneral<ItemCat>(
                        new ItemCat($"{sectorNombre} - Grupo {i}", TipoElemento.Categoria)
                    );
                    
                    // Cuelga esta subcategoría como hija del sector actual (va un nivel más abajo en la jerarquía)
                    sector.agregarHijo(subCat);

                    // Determina al azar cuántos productos individuales tendrá esta subcategoría (un entero entre 5 y 15 inclusive)
                    // Nuevamente se coloca 16 como límite exclusivo para obligar al dado a tomar hasta el 15.
                    int numProductos = rnd.Next(5, 16);
                    
                    // =========================================================================
                    // BUCLE DE NIVEL 3 (Productos/Nodos Hoja): Se repite para crear los productos de la subcategoría
                    // =========================================================================
                    for (int j = 1; j <= numProductos; j++)
                    {
                        // Instancia el objeto de datos del producto
                        ItemCat producto = new ItemCat(
                            $"{subCat.getDatoRaiz().Nombre} Prod {j}", // Nombre dinámico basado en el nombre de su subcategoría padre
                            TipoElemento.Producto, // Establece que el enumerador sea de tipo Producto (nodo terminal)
                            sku: $"SKU-{rnd.Next(1000, 9999)}", // Genera un código SKU al azar de 4 dígitos entre 1000 y 9999
                            precio: Math.Round(rnd.NextDouble() * 500, 2) // rnd.NextDouble() da un decimal entre 0.0 y 1.0. Lo multiplica por 500 y lo redondea a 2 decimales.
                        );
                        
                        // ¡CLAVE JERÁRQUICA!: Envuelve el producto recién creado en un nuevo nodo ArbolGeneral
                        // y lo cuelga como un nodo hijo (una hoja) de la subcategoría actual
                        subCat.agregarHijo(new ArbolGeneral<ItemCat>(producto));
                    }
                }
            }
        }
    }    }
}
