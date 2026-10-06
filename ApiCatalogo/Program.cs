using Microsoft.Extensions.Hosting; // Importa herramientas avanzadas de hospedaje de .NET para controlar el ciclo de vida de la app
using tp1; // Importa el0 namespace base donde están definidos ArbolGeneral y Cola
using tpfinal; // Importa el namespace del proyecto final donde residen ItemCat y Estrategia

// Crea el constructor de la aplicación web encargándose de leer archivos de configuración (como appsettings.json)
var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURACIÓN DE SERVICIOS EN EL CONTENEDOR (Inyección de Dependencias) ---

// Le ordena al servidor web interno (Kestrel) exponer la API en el puerto 5000 para cualquier IP de la red (0.0.0.0)
builder.WebHost.UseUrls("http://0.0.0.0:5000");

// Agrega las herramientas necesarias para rastrear, mapear y explorar los puntos de acceso (endpoints) HTTP de la API
builder.Services.AddEndpointsApiExplorer();

// Añade el generador de Swagger encargado de crear la documentación técnica y la interfaz interactiva de pruebas
builder.Services.AddSwaggerGen();

// --- CONSTRUCCIÓN DE LA APLICACIÓN ---
// Finaliza la etapa de configuración de servicios y construye físicamente el servidor web listo para operar
var app = builder.Build();

// --- CONFIGURACIÓN DEL PIPELINE DE PETICIONES HTTP (Middlewares) ---

// Activa el motor interno que procesa y estructura la especificación OpenAPI de Swagger
app.UseSwagger();

// Levanta la interfaz de usuario interactiva visual de Swagger en el navegador (por defecto en /swagger/index.html)
app.UseSwaggerUI();

// Instancia la clase Estrategia en la memoria RAM para que todos los endpoints puedan usar sus algoritmos
var estrategia = new Estrategia();

// --- 1. CREACIÓN E INICIALIZACIÓN DEL ÁRBOL CENTRAL ---

// Instancia el nodo raíz principal del catálogo en memoria asignándole el nombre de "Catalogo Global"
ArbolGeneral<ItemCat> arbol = new ArbolGeneral<ItemCat>(
    new ItemCat("Catalogo Global", TipoElemento.Categoria)
);

// Llama al método estático 'init' de la clase Util para poblar inmediatamente el árbol con cientos de datos falsos de prueba
Util.init(arbol);

// --- MAPEOS DE ENDPOINTS (RUTAS WEB DE INTERNET) ---

// 1. Ruta TODOS (Operación HTTP GET: Se usa para solicitar información)
// Cuando alguien entre a /api/catalogo/todos, el sistema ejecutará este bloque
app.MapGet("/api/catalogo/todos", () =>
{
        // Llama a tu método iterativo BFS de la estrategia para recolectar todos los productos del árbol
        var result = estrategia.Todos(arbol);
        
        // Devuelve un estado HTTP 200 OK y transforma automáticamente tu lista de C# al formato universal JSON
        return Results.Ok( result );
})
.WithName("TodoselCatalogo") // Le asigna un nombre de identificación interna a la ruta en .NET
.WithOpenApi(); // Registra y expone este endpoint de forma automática en el panel visual de Swagger

// 2. Ruta AGREGAR (Operación HTTP POST: Se usa para enviar y crear recursos nuevos)
// Recibe un objeto 'ItemCat' en el cuerpo de la petición y un string 'rutaAlPadre' por parámetro web
app.MapPost("/api/catalogo/agregar", (ItemCat nuevoDato, string rutaAlPadre) =>
{
    try // Bloque de seguridad: Intenta realizar la inserción dinámica
    {
        // Invoca a el algoritmo de inserción por ruta
        estrategia.Agregar(arbol, nuevoDato, rutaAlPadre);
        
        // Devuelve un estado HTTP 201 Created confirmando el éxito e indica una ruta teórica de consulta
        return Results.Created($"/api/catalogo/itemPorId/{nuevoDato.Id}", nuevoDato);
    }
    catch (Exception ex) // Si la estrategia llega a lanzar una excepción (error controlado)...
    {
        // Atrapa el error y devuelve un HTTP 400 Bad Request empaquetando el mensaje para el usuario
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("AgregarProductoAlCatalogo")
.WithOpenApi();

// 3. Ruta BUSCAR (Operación HTTP GET)
// Recibe una cadena de texto llamada 'elemento' desde la barra de direcciones de la petición
app.MapGet("/api/catalogo/buscar", (string elemento) =>
{
    // Ejecuta tu algoritmo iterativo BFS de búsqueda pasándole el texto recibido
    var collected = estrategia.Buscar(arbol, elemento);
    
    // Retorna HTTP 200 OK enviando las coincidencias encontradas filtradas por texto
    return Results.Ok( collected );
})
.WithName("BuscaProductoDelCatalogo")
.WithOpenApi();

// 5. Ruta URLs SEO COMPLETAS (Operación HTTP GET)
app.MapGet("/api/catalogo/urls-seo", () =>
{
    // Invoca al método que resolverá la lista de todas las URLs amigables del catálogo
    return Results.Ok(estrategia.GetURLsSEO(arbol));
})
.WithName("GetURLsSEO")
.WithOpenApi();

// 6. Ruta URL SEO POR ID (Operación HTTP GET)
// Recibe el identificador numérico único 'id' de un ítem en particular
app.MapGet("/api/catalogo/url-seoPorId", (int id) =>
{
    // Invoca al método que calculará la URL amigable del ítem correspondiente a ese ID específico
    return Results.Ok(estrategia.GetUrlSeoPorId(arbol, id));
})
.WithName("url-seoPorId")
.WithOpenApi();

// 7. Ruta CONSULTA POR NIVELES (Operación HTTP GET)
app.MapGet("/api/catalogo/niveles", () =>
{
    // Invoca al método encargado de formatear y devolver el árbol agrupado piso por piso
    return Results.Ok(estrategia.ConsultaNiveles(arbol));
})
.WithName("ConsultaNiveles")
.WithOpenApi();

// --- INICIO FORMAL DEL SERVIDOR ---
// Enciende de forma definitiva los motores de la aplicación web y los deja "escuchando" peticiones de internet indefinidamente
app.Run();
