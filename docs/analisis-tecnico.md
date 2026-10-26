# Análisis técnico y decisiones

## Decisiones recomendadas

| Tema | Alternativas | Recomendación | Motivo y costo |
|---|---|---|---|
| Interfaz | Consola; Windows Forms; WPF; motor 2D/3D | Windows Forms sobre .NET moderno | Cumple el destino de escritorio Windows, es coherente con el alcance y ofrece formularios, controles, gráficos y binding. No incluye renderizado/motor de videojuego 3D; la experiencia diseñada es una aventura 2D basada en paneles y mapa esquemático. |
| Persistencia | Memoria; archivo JSON; SQL Server directo desde formulario; SQL Server tras repositorio | SQL Server mediante `Microsoft.Data.SqlClient`, aislado tras repositorio | El requisito pide SQL Server; ADO.NET ofrece conexiones, comandos parametrizados, lectores y transacciones. Introduce configuración/instancia y trabajo de esquema. JSON sería útil como prototipo/offline, pero no satisface persistencia final pedida. |
| Organización | Todo en `Program.cs`; MVC/MVVM; capas simples | Dominio + aplicación + infraestructura + WinForms, sin framework de mediación inicial | Extraer reglas del código actual reduce acoplamiento y habilita UI/DB independientes. Más archivos/configuración iniciales; evitar arquitectura excesiva para un juego pequeño. |
| Estado de misión | Flags codificados; objetivos de catálogo y progreso | Definiciones/objetivos con códigos estables y progreso guardado | Se mapea directamente a las cuatro condiciones de `Mision`, facilita UI, consulta y extensión a nuevas misiones. |
| Respuesta del dominio | Escritura a consola/void; resultado tipado | `ExplorationResult`, `ActionResult`, `CombatResult` | La interfaz debe distinguir rechazo, costo y evento; elimina el defecto de evento tras exploración rechazada. |
| Dibujo del mapa | Textos de consola; mapa esquemático en controles; canvas elaborado | Mapa esquemático WinForms primero | Presenta las dos realidades y sus atributos existentes con interacción clara; arte/animación se considera opcional. |

## Aplicabilidad al código de H3

El cambio parte de `Program.cs`, no de un juego nuevo. El menú de trece entradas se convierte en navegación persistente para mapa, misión, inventario, estado, análisis, búsqueda y extracción. `Cadete` aporta estadísticas e inventario; `Realidad` aporta los dos destinos, costo y umbrales; `Mision` aporta objetivos requeridos; las derivadas de `Objeto` especifican efectos; `Enemigo` y `Robot` sustentan encuentros. `Program` mantiene hoy orquestación y estado global: se reemplaza por `GameSession`/servicios y formularios, preservando primero las reglas observadas.

La propuesta reconoce mejoras que afectan coherencia: `Realidad.Explorar` debe devolver resultado; el consumo de un item debe depender de si el uso surtió efecto; aleatoriedad debe inyectarse para reproducir escenarios y guardar/reanudar de forma consistente; condición de victoria y flags del `Program` deben consolidarse en estado de misión. Cada cambio requiere documentar si conserva o corrige comportamiento accidental, no asumir equivalencia automática.

## Riesgos y mitigaciones

- **Migración grande:** convertir UI, reglas y persistencia simultáneamente dificulta encontrar regresiones. Avanzar por etapas: dominio, WinForms en memoria, luego SQL.
- **Eventos WinForms congelados:** evitar `Thread.Sleep` y operaciones SQL síncronas en el hilo visual; usar tareas async para IO, indicadores y manejo de cancelación.
- **Conexión no disponible:** capturar fallas de conexión/timeouts, dejar la sesión en memoria y permitir reintento. No comunicar guardado exitoso hasta commit.
- **Guardado incompleto:** persistir el agregado dentro de transacción, usar FKs/constraints y `RowVersion`.
- **Secreto en Git:** usar Windows Authentication local o secreto externo/config ignorada, nunca subir contraseña real.
- **Reglas no especificadas:** fijar comportamiento de partida al guardar en combate, derrota, inventario apilable, puntos de control y auto-guardado antes de implementar tablas dependientes.
- **Accesibilidad/percepción gráfica:** no depender del color, mantener lectura numérica y teclado, validar escala y contraste en la UI.

## Alternativas y límites

**WPF** también ofrece UI de escritorio y binding más declarativo; puede convenir si se priorizan animaciones o separación visual avanzada, pero no es la tecnología solicitada. **Unity/MonoGame** son apropiados para escenas/juego con renderizado intensivo, pero sobrepasan la meta explícita de WinForms. **Entity Framework Core** puede acelerar mapeo y migraciones; para el primer modelo acotado, ADO.NET con `Microsoft.Data.SqlClient` muestra claramente queries, parámetros y transacciones y reduce dependencias indirectas. Si el número de entidades crece, comparar EF Core en una revisión posterior, no mezclar con SQL ad hoc de formularios.

SQL Server local simplifica trabajo académico y no requiere red para una instancia local, pero limita la distribución a usuarios que instalen/configuren esa base. Si se distribuye a otras máquinas, deberá decidirse instalador/LocalDB/servidor remoto; LocalDB es una decisión de despliegue, no se asume que esté disponible.

## Fuentes técnicas consultadas

- Microsoft Learn, [Windows Forms overview](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/overview/): WinForms es UI de escritorio Windows y proporciona formularios, controles, gráficos, entrada y binding. Consultado el 26-10-2026.
- Microsoft Learn, [Microsoft.Data.SqlClient for SQL Server](https://learn.microsoft.com/en-us/sql/connect/ado-net): proveedor ADO.NET para conexiones, comandos, resultados, transacciones y SQL Server. Consultado el 26-10-2026.
- Microsoft Learn, [Configuring parameters](https://learn.microsoft.com/en-us/sql/connect/ado-net/configure-parameters?view=sql-server-ver17): parámetros para tipo/validación y tratamiento de valores, además de evitar que valores de entrada se ejecuten como SQL. Consultado el 26-10-2026.
- Microsoft Learn, [Primary and foreign key constraints](https://learn.microsoft.com/en-us/sql/relational-databases/tables/primary-and-foreign-key-constraints?view=sql-server-ver15): restricciones para integridad referencial; el índice de una FK no se crea automáticamente. Consultado el 26-10-2026.
- Microsoft Learn, [Local transactions](https://learn.microsoft.com/en-us/sql/connect/ado-net/local-transactions?view=sql-server-ver17): transacciones agrupan operaciones como unidad; los comandos deben asociarse a la transacción. Consultado el 26-10-2026.

Las páginas Microsoft documentan capacidades generales; la selección de WinForms/SQL Server como recomendación es una decisión del proyecto basada en la restricción del enunciado, no una conclusión que las fuentes hagan por sí mismas.

## Verificación de información obtenida con IA

Se contrastaron con la documentación oficial las afirmaciones técnicas sobre el alcance de WinForms, `Microsoft.Data.SqlClient`, parámetros SQL, integridad por PK/FK y transacciones. El diseño de juego y el análisis de clases se comprobaron contra `Program.cs`, `ConsoleApp3.csproj`, `README.md` y `docs/diagramas.md` del checkout. Las cantidades/umbrales del diseño proceden de las reglas de las clases `Cadete`, `Realidad`, `Mision` y objetos.

Las tablas y arquitectura son diseño propuesto, no funciones detectadas en el repositorio ni resultados de una IA tratados como hechos. Se recomienda verificar en compilación la plantilla WinForms y versión SDK instalada cuando comience la implementación, inspeccionar nuevamente reglas tras refactor, y validar cada script de esquema en una instancia SQL Server de desarrollo. No se verificó una instalación SQL Server local ni se ejecutó el proyecto en esta sesión; esta documentación no afirma que exista persistencia.
