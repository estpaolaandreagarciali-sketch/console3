# Arquitectura actual de NEXUS (H3)

## Base observada

Inspección del código versionado en `Program.cs` (namespace `NEXUS`) y `ConsoleApp3.csproj`. El proyecto actual es una aplicación de consola C# que apunta a `net10.0`, sin paquetes externos declarados. El punto de entrada es `Program.Main`; el estado se reinicia en cada ejecución. No hay formularios, base SQL, persistencia, capas separadas, pruebas incluidas ni interfaz C# implementada.

## Tipos y responsabilidades reales

| Tipo | Papel y comportamiento observados |
|---|---|
| `TipoObjeto` | Enumera `CelulaEnergia`, `Escudo`, `ModuloAtaque`, `Analizador` y `NucleoEnergetico`. |
| `Objeto` | Abstracta; nombre, tipo, descripción y contrato `Usar(Cadete)`. |
| `CelulaEnergia` | Aumenta energía en 25 hasta 100; no altera si ya está al máximo. |
| `Escudo` | Aumenta escudo en 30 hasta 100. |
| `ModuloAtaque` | Añade 15 a `DanoExtra`. |
| `Analizador` | Muestra que está listo para analizar entidades. |
| `NucleoEnergetico` | Comunica que debe llevarse a extracción; es el objetivo de misión. |
| `Inventario` | Encapsula `List<Objeto>`; agregar, mostrar, obtener, eliminar, contar y consultar tipo. No gestiona cantidades agregadas por tipo. |
| `Personaje` | Abstracta; nombre, salud, método abstracto de información, daño genérico y `EstaVivo`. |
| `Cadete` | Salud inicial 100, energía/estabilidad 100, escudo 50, XP y daño extra 0; contiene inventario. Administra XP y absorción de daño mediante escudo. |
| `Robot` | Salud 80 y defensa 25; informa/activa defensa con salida textual. |
| `Enemigo` | Salud y daño parametrizados; puede atacar al cadete. |
| `Realidad` | Código, nombre, estabilidad, riesgo y estado (estable/inestable/crítica); aplica costos y límites al explorar. |
| `Mision` | Registra cuatro banderas: anomalía, núcleo, enemigo y extracción; verifica/muestra progreso. |
| `Program` | Orquesta instancia global de cadete, R-17/R-18, misión, enemigo actual, núcleo, jefe, menú, exploración, eventos aleatorios, combate, análisis, extracción, mapa y derrota. |

## Flujo real

`Main` elige R-17 como realidad inicial, muestra `Inicio`, agrega célula, escudo y analizador, y repite mientras `sesionActiva`. En cada vuelta comprueba si el cadete está vivo y llama a `ComprobarIRIS`, presenta recursos y menú numerado, lee la consola y despacha acciones con `switch`. El código almacena estado compartido como campos `static` de `Program`; el menú de trece opciones gobierna exploración, inventario, estado, mapa, cambio de realidad, misión, análisis, búsqueda del núcleo, extracción, información y salida.

Explorar descuenta recursos mediante `Realidad.Explorar` y luego `GenerarEvento`; el flujo no recibe un resultado de aceptación de exploración y puede lanzar un evento aunque la realidad haya rechazado el intento. Las acciones escriben directamente en `Console`; `Thread.Sleep` añade una pausa. La aleatoriedad usa `Random`. Combate admite acciones por consola, ataques enemigos y retirada. La misión se resuelve con las cuatro condiciones definidas en `Mision`.

## Límites para la evolución

- La UI, el ciclo de juego y la construcción inicial del mundo están mezclados en `Program.cs`; controles de formulario no deberían llamar métodos que escriben en consola.
- El estado se guarda en variables estáticas y referencias mutables; no hay objeto serializable de sesión ni interfaz de almacenamiento.
- `Inventario` contiene instancias polimórficas, pero no tiene ID, cantidad ni representación persistible estable; mapear objetos por su texto/nombre sería frágil.
- Los métodos de dominio suelen imprimir resultados y varios setters son privados/protegidos; hay que sustituir mensajes por resultados de acción explícitos antes de conectar la UI.
- `Realidad.Explorar` muta estado sin resultado tipado, lo que dificulta que UI y persistencia sepan cuándo hubo acción válida.
- No hay inicio de sesión, perfil, partidas guardadas, política de migración, configuración segura de conexión ni manejo de base de datos.
- `ConsoleApp3.csproj` no es proyecto Windows Forms; pasar a WinForms requiere cambiar el SDK/configuración de Windows y el punto de entrada.

## Relación con el diseño propuesto

La interfaz conserva los casos de uso que actualmente ofrece el menú; `Cadete`, `Realidad`, `Mision`, `Inventario`, `Objeto` y sus derivados son los conceptos iniciales del dominio. La propuesta crea servicios de aplicación para reemplazar el `switch` y extrae UI/aleatoriedad/persistencia. Sus mejoras se identifican como refactor y no como capacidades ya presentes.
