using Microsoft.Extensions.Hosting; 
using tp1; 
using tpfinal; 

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5000");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
var estrategia = new Estrategia();
ArbolGeneral<ItemCat> arbol = new ArbolGeneral<ItemCat>(
    new ItemCat("Catalogo Global", TipoElemento.Categoria)
);
Util.init(arbol);
app.MapGet("/api/catalogo/todos", () =>
{
        var result = estrategia.Todos(arbol);
        return Results.Ok( result );
})
.WithName("TodoselCatalogo") 
.WithOpenApi(); 
app.MapPost("/api/catalogo/agregar", (ItemCat nuevoDato, string rutaAlPadre) =>
{
    try 
    {
        estrategia.Agregar(arbol, nuevoDato, rutaAlPadre);
        return Results.Created($"/api/catalogo/itemPorId/{nuevoDato.Id}", nuevoDato);
    }
    catch (Exception ex) 
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("AgregarProductoAlCatalogo")
.WithOpenApi();
app.MapGet("/api/catalogo/buscar", (string elemento) =>
{
    var collected = estrategia.Buscar(arbol, elemento);
    return Results.Ok( collected );
})
.WithName("BuscaProductoDelCatalogo")
.WithOpenApi();

app.MapGet("/api/catalogo/urls-seo", () =>
{
    return Results.Ok(estrategia.GetURLsSEO(arbol));
})
.WithName("GetURLsSEO")
.WithOpenApi();

app.MapGet("/api/catalogo/url-seoPorId", (int id) =>
{
    return Results.Ok(estrategia.GetUrlSeoPorId(arbol, id));
})
.WithName("url-seoPorId")
.WithOpenApi();

app.MapGet("/api/catalogo/niveles", () =>
{
    return Results.Ok(estrategia.ConsultaNiveles(arbol));
})
.WithName("ConsultaNiveles")
.WithOpenApi();

app.Run();
