using System;
using System.Collections.Generic;


namespace tpfinal
{
        // Define un tipo especial llamado Enumerador para restringir las opciones válidas a solo dos: Categoria o Producto
        // Evita errores de tipeo en el sistema.
        public enum TipoElemento { Categoria, Producto }
        
        // Define la clase principal "ItemCat" (Ítem de Catálogo) que moldeará a cada objeto guardado en el árbol.
        public class ItemCat{
                // Variable estática y privada. Funciona como un contador global en la memoria RAM compartido por todos los ítems.
                private static int NEXT_ID = 1;
                
                // Propiedad para el identificador numérico único de cada objeto. get permite leerlo, set modificarlo.
                public int Id { get; set; }
                
                // Propiedad de tipo texto (string) para almacenar el nombre de la categoría o producto.
                public string Nombre { get; set; }
                // Propiedad que guarda el tipo del ítem, restringido únicamente a las opciones del Enumerador de arriba.
                public TipoElemento Tipo { get; set; }
                // Propiedad de tipo texto para el código de barra/identificación del producto (Stock Keeping Unit).
                public string CodigoSKU { get; set; } // Solo para productos
                // Propiedad decimal de doble precisión para almacenar el valor comercial del producto.
                public double Precio { get; set; }    // Solo para productos
                
                // --- CONSTRUCTOR 1: Por Defecto o Vacío (Sin parámetros) ---
                // Se ejecuta automáticamente al escribir: new ItemCat()
                public ItemCat(){
                        Nombre = "nombre"; // Asigna un texto genérico inicial
                        Tipo = TipoElemento.Categoria; // Por defecto asume que el elemento nuevo es una Categoría
                        CodigoSKU = ""; // El SKU arranca vacío
                        Precio = 0.0; // El precio inicial es cero
                }
                
                // --- CONSTRUCTOR 2: Con Parámetros (Para datos reales) ---
                // Se ejecuta al escribir: new ItemCat("Celular", TipoElemento.Producto, "SKU-123", 450.0)
                // Nota: 'sku' y 'precio' tienen valores por defecto por si no se especifican al invocarlo.
                public ItemCat(string nombre, TipoElemento tipo, string sku = "", double precio = 0.0){
                        Id = NEXT_ID++; // Asigna el valor actual de NEXT_ID al Id de este objeto específico, y LUEGO incrementa NEXT_ID en 1
                        Nombre = nombre; // Almacena el texto recibido en la propiedad Nombre
                        Tipo = tipo; // Almacena el tipo recibido en la propiedad Tipo
                        CodigoSKU = sku; // Almacena el sku recibido en la propiedad CodigoSKU
                        Precio = precio; // Almacena el precio recibido en la propiedad Precio
                }
                
                // --- SOBRESCRITURA DE TOSTRING ---
                // Redefine cómo se representará este objeto textualmente si se imprime en consola o en pantalla.
                public override string ToString(){
                        // Operador ternario (Un IF en una sola línea). 
                        // Si el Tipo es Categoria, devuelve el string formateado para categoría.
                        // Si no (es Producto), devuelve la cadena detallada con el Precio y su SKU.
                        return Tipo == TipoElemento.Categoria
                                ? $"[CAT] {Nombre}"
                                : $"[PROD] {Nombre} (${Precio}) - SKU: {CodigoSKU}";
                }
                
                // --- SOBRESCRITURA DE EQUALS ---    
                // Define la regla de negocio exacta para determinar si dos objetos ItemCat son considerados "iguales"
                // Clave para que tus métodos .Contains() o .Equals() del Árbol General funcionen con precisión.
                public override bool Equals(object obj){
                        // Pattern Matching: Pregunta si el objeto genérico 'obj' que le pasaron es de tipo ItemCat.
                        // Si da verdadero, crea de forma automática una variable temporal llamada 'otro' ya casteada.
                        if (obj is ItemCat otro){
                                // Regla lógica: Dos ítems son exactamente iguales si comparten el mismo Nombre (Ignora ID, precio, etc.)
                                return this.Nombre == otro.Nombre;
                        }
                        return false; // Si el objeto a comparar ni siquiera es un ItemCat, determina que no son iguales.
                }
                
                // --- SOBRESCRITURA DE GETHASHCODE ---
                // Devuelve un identificador numérico único (hash) basado en el Nombre. 
                // Es una obligación de C# sobreescribirlo si decidiste redefinir el método 'Equals'.
                public override int GetHashCode() => Nombre.GetHashCode();
        }
}
