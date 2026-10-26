# NEXUS

NEXUS es una aventura de exploración en C# sobre una cadete que investiga realidades conectadas y la misión «La anomalía perdida». La versión actual es una aplicación de consola desarrollada para H3. La documentación define una evolución hacia videojuego gráfico de escritorio en Windows Forms con progreso en SQL Server; esa versión gráfica y la persistencia todavía no están implementadas.

## Requisitos

- .NET SDK 10.0 o posterior, como indica `ConsoleApp3.csproj`.
- Windows, macOS o Linux con una terminal compatible para la versión de consola actual.

## Ejecutar

Desde esta carpeta:

```sh
dotnet run --project ConsoleApp3.csproj
```

El proyecto actual no requiere paquetes externos. El estado del juego vive en memoria durante la ejecución y se reinicia al volver a iniciar el programa.

## Funcionalidades implementadas (H3)

- Exploración de R-17 «Ciudad de los Ecos» y R-18 «Sector de las Sombras», con riesgo, estabilidad y costos.
- Cadete con salud, energía, estabilidad, escudo, XP e inventario.
- Objetos polimórficos: célula de energía, escudo, módulo de ataque, analizador y núcleo energético.
- Eventos, robots, enemigos, combate, análisis de anomalía, búsqueda del núcleo, extracción y misión con cuatro objetivos.
- Menú de consola para consultar estado, mapa, misión, inventario e información de realidad.

La lista describe el código actual; WinForms, guardado en SQL Server, perfiles y nuevas misiones son trabajo futuro.

## Estructura

- `Program.cs`: punto de entrada, menú, estado compartido, reglas y clases de dominio actuales.
- `ConsoleApp3.csproj`: proyecto .NET de consola (`net10.0`).
- `docs/manual-tecnico-h3.md`: manual técnico y guía de estudio para Hito 3.
- `docs/diagramas.md`: diagramas Mermaid de clases y de flujo de H3.
- `docs/diseno-del-juego.md`: visión, ciclo de juego, reglas, wireframes, UX y alcance.
- `docs/arquitectura-actual.md`: inventario y límites comprobados del código H3.
- `docs/arquitectura-propuesta.md`: separación recomendada y migración a WinForms.
- `docs/modelo-de-datos-propuesto.md`: entidades y estrategia transaccional para SQL Server.
- `docs/analisis-tecnico.md`: alternativas, decisiones y fuentes verificadas.
- `docs/evidencias/wireframes.md`: bocetos y mapa de navegación.

## Modelo principal

El código emplea abstracción, herencia, encapsulamiento y polimorfismo. `Personaje` y `Objeto` son clases abstractas; `Cadete`, `Robot`, `Enemigo` y los tipos concretos de objeto especializan esas bases. `Cadete` contiene un `Inventario`, que administra una `List<Objeto>`.

El programa no implementa autenticación de usuarios, persistencia, interfaces de C# ni consultas LINQ. El manual explica esos temas mediante ejemplos complementarios identificados como tales.

## Documentación

Empieza por el [diseño del juego](docs/diseno-del-juego.md), luego consulta la [arquitectura actual](docs/arquitectura-actual.md), la [arquitectura propuesta](docs/arquitectura-propuesta.md), el [modelo de datos](docs/modelo-de-datos-propuesto.md) y el [análisis técnico](docs/analisis-tecnico.md). También están disponibles el [manual técnico H3](docs/manual-tecnico-h3.md) y [diagramas existentes](docs/diagramas.md).

