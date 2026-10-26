# Diagramas de NEXUS

Los diagramas describen el código de `Program.cs`. Mermaid permite verlos en GitHub y en editores compatibles.

## Clases y relaciones

```mermaid
classDiagram
    class Objeto {
      <<abstract>>
      +string Nombre
      +TipoObjeto Tipo
      +string Descripcion
      +Usar(Cadete) void
    }
    class CelulaEnergia
    class Escudo
    class ModuloAtaque
    class Analizador
    class NucleoEnergetico
    Objeto <|-- CelulaEnergia
    Objeto <|-- Escudo
    Objeto <|-- ModuloAtaque
    Objeto <|-- Analizador
    Objeto <|-- NucleoEnergetico

    class Personaje {
      <<abstract>>
      +string Nombre
      +int Salud
      +MostrarInformacion()* void
      +RecibirDanio(int) void
      +EstaVivo() bool
    }
    class Cadete
    class Robot
    class Enemigo
    Personaje <|-- Cadete
    Personaje <|-- Robot
    Personaje <|-- Enemigo

    class Inventario {
      -List~Objeto~ objetos
      +Agregar(Objeto) void
      +Mostrar() void
      +Obtener(int) Objeto
      +Eliminar(Objeto) bool
      +Cantidad() int
      +TieneTipo(TipoObjeto) bool
    }
    Cadete *-- Inventario : contiene
    Inventario o-- Objeto : administra

    class Realidad
    class Mision
    Program --> Cadete : coordina
    Program --> Realidad : cambia y explora
    Program --> Mision : actualiza objetivos
```

## Flujo de exploración

```mermaid
flowchart TD
    A[Inicio: preparar estado y recursos] --> B{Cadete vivo y misión activa?}
    B -- No --> Z[Mostrar fin de sesión]
    B -- Sí --> C[Mostrar estado y menú]
    C --> D[Leer opción]
    D --> E{Opción = explorar?}
    E -- No --> X[Ejecutar otra opción del menú]
    X --> B
    E -- Sí --> F{Energía del cadete < 10?}
    F -- Sí --> G[Mostrar energía insuficiente]
    F -- No --> H[Realidad.Explorar: revisar energía y estado]
    H --> I{Exploración aceptada?}
    I -- No --> J[Rechazar exploración]
    I -- Sí --> K[Actualizar energía y estabilidad]
    J --> L[Generar evento aleatorio]
    K --> L
    L --> M[Aplicar evento de la realidad]
    M --> B
```

Nota de análisis: el método `Program.Explorar()` llama a `GenerarEvento()` después de `Realidad.Explorar()` sin recibir un resultado que indique si esta última aceptó la exploración. Por eso, con energía suficiente pero estadísticas demasiado bajas, se puede rechazar la exploración y aun así generar un evento. Es una oportunidad de mejora del diseño.

