
using System;
using System.Collections.Generic;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal{
    public class Util{
        public static void init(ArbolGeneral<ItemCat> catalogo){
            Console.WriteLine("Generando catálogo masivo...");

            string[] sectores = { "Electrónica", "Moda", "Hogar", "Deportes", "Belleza", "Juguetes" };
            
            Random rnd = new Random();

            foreach (var sectorNombre in sectores){
                ArbolGeneral<ItemCat> sector = new ArbolGeneral<ItemCat>(
                    new ItemCat(sectorNombre, TipoElemento.Categoria)
                );
                catalogo.agregarHijo(sector);
                int numSubCategorias = rnd.Next(2, 7);
                
                for (int i = 1; i <= numSubCategorias; i++){
                    ArbolGeneral<ItemCat> subCat = new ArbolGeneral<ItemCat>(
                        new ItemCat($"{sectorNombre} - Grupo {i}", TipoElemento.Categoria)
                    );
                    
                    sector.agregarHijo(subCat);

                    int numProductos = rnd.Next(5, 16);
                
                    for (int j = 1; j <= numProductos; j++){
                        ItemCat producto = new ItemCat(
                            $"{subCat.getDatoRaiz().Nombre} Prod {j}", 
                            TipoElemento.Producto, 
                            sku: $"SKU-{rnd.Next(1000, 9999)}", 
                            precio: Math.Round(rnd.NextDouble() * 500, 2) 
                        );
                        
                        subCat.agregarHijo(new ArbolGeneral<ItemCat>(producto));
                    }
                }
            }
        }
    }
}
