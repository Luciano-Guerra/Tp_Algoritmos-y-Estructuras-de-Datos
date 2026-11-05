using System;
using System.Collections.Generic;


namespace tpfinal{
        public enum TipoElemento { Categoria, Producto }
        public class ItemCat{
                private static int NEXT_ID = 1;
                public int Id { get; set; }
                public string Nombre { get; set; }
                public TipoElemento Tipo { get; set; }
                public string CodigoSKU { get; set; } 
                public double Precio { get; set; }
                public ItemCat(){
                        Nombre = "nombre"; 
                        Tipo = TipoElemento.Categoria; 
                        CodigoSKU = ""; 
                        Precio = 0.0; 
                }
                public ItemCat(string nombre, TipoElemento tipo, string sku = "", double precio = 0.0){
                        Id = NEXT_ID++; 
                        Nombre = nombre; 
                        Tipo = tipo; 
                        CodigoSKU = sku; 
                        Precio = precio; 
                }
                public override string ToString(){
                        return Tipo == TipoElemento.Categoria
                                ? $"[CAT] {Nombre}"
                                : $"[PROD] {Nombre} (${Precio}) - SKU: {CodigoSKU}";
                }
                public override bool Equals(object obj){
                        if (obj is ItemCat otro){
                                return this.Nombre == otro.Nombre;
                        }
                        return false; 
                }
                public override int GetHashCode() => Nombre.GetHashCode();
        }
}
