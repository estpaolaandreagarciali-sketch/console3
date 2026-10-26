#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;

namespace NEXUS
{
    enum TipoObjeto
    {
        CelulaEnergia,
        Escudo,
        ModuloAtaque,
        Analizador,
        NucleoEnergetico
    }

    abstract class Objeto
    {
        public string Nombre { get; protected set; }
        public TipoObjeto Tipo { get; protected set; }
        public string Descripcion { get; protected set; }

        protected Objeto(string nombre, TipoObjeto tipo, string descripcion)
        {
            Nombre = nombre;
            Tipo = tipo;
            Descripcion = descripcion;
        }

        public abstract void Usar(Cadete cadete);
    }

    class CelulaEnergia : Objeto
    {
        public CelulaEnergia()
            : base("Célula de energía", TipoObjeto.CelulaEnergia,
                  "Recupera energía.")
        {
        }

        public override void Usar(Cadete cadete)
        {
            if (cadete.Energia >= 100)
            {
                Console.WriteLine("\nLa energía ya está al máximo.");
                return;
            }

            int anterior = cadete.Energia;

            cadete.Energia += 25;

            if (cadete.Energia > 100)
                cadete.Energia = 100;

            Console.WriteLine("\n🔋 CÉLULA DE ENERGÍA UTILIZADA");
            Console.WriteLine($"Energía: {anterior} → {cadete.Energia}");
        }
    }

    class Escudo : Objeto
    {
        public Escudo()
            : base("Escudo", TipoObjeto.Escudo,
                  "Aumenta la protección del Cadete.")
        {
        }

        public override void Usar(Cadete cadete)
        {
            if (cadete.Escudo >= 100)
            {
                Console.WriteLine("\nEl escudo ya está al máximo.");
                return;
            }

            int anterior = cadete.Escudo;

            cadete.Escudo += 30;

            if (cadete.Escudo > 100)
                cadete.Escudo = 100;

            Console.WriteLine("\n🛡️ ESCUDO ACTIVADO");
            Console.WriteLine($"Escudo: {anterior} → {cadete.Escudo}");
        }
    }

    class ModuloAtaque : Objeto
    {
        public ModuloAtaque()
            : base("Módulo de ataque", TipoObjeto.ModuloAtaque,
                  "Aumenta el daño del próximo ataque.")
        {
        }

        public override void Usar(Cadete cadete)
        {
            cadete.DanoExtra += 15;

            Console.WriteLine("\n🔫 MÓDULO DE ATAQUE ACTIVADO");
            Console.WriteLine("Daño adicional preparado: +15");
        }
    }

    class Analizador : Objeto
    {
        public Analizador()
            : base("Analizador", TipoObjeto.Analizador,
                  "Permite analizar entidades.")
        {
        }

        public override void Usar(Cadete cadete)
        {
            Console.WriteLine("\n🔬 ANALIZADOR ACTIVADO");
            Console.WriteLine("Sistema listo para analizar entidades.");
        }
    }

    class NucleoEnergetico : Objeto
    {
        public NucleoEnergetico()
            : base("Núcleo energético", TipoObjeto.NucleoEnergetico,
                  "Objetivo principal de la misión.")
        {
        }

        public override void Usar(Cadete cadete)
        {
            Console.WriteLine("\n⚡ NÚCLEO ENERGÉTICO");
            Console.WriteLine(
                "El núcleo debe ser llevado al punto de extracción.");
        }
    }

    class Inventario
    {
        private List<Objeto> objetos;

        public Inventario()
        {
            objetos = new List<Objeto>();
        }

        public void Agregar(Objeto objeto)
        {
            objetos.Add(objeto);

            Console.WriteLine(
                $"\n🎁 OBJETO AGREGADO: {objeto.Nombre}");
        }

        public void Mostrar()
        {
            Console.WriteLine("\n========== INVENTARIO ==========");

            if (objetos.Count == 0)
            {
                Console.WriteLine("El inventario está vacío.");
            }
            else
            {
                for (int i = 0; i < objetos.Count; i++)
                {
                    Console.WriteLine(
                        $"{i + 1}. {objetos[i].Nombre}");
                }
            }

            Console.WriteLine("================================");
        }

        public Objeto Obtener(int indice)
        {
            if (indice < 0 || indice >= objetos.Count)
            {
                return null;
            }

            return objetos[indice];
        }

        public bool Eliminar(Objeto objeto)
        {
            return objetos.Remove(objeto);
        }

        public int Cantidad()
        {
            return objetos.Count;
        }

        public bool TieneTipo(TipoObjeto tipo)
        {
            foreach (Objeto objeto in objetos)
            {
                if (objeto.Tipo == tipo)
                    return true;
            }

            return false;
        }
    }

    abstract class Personaje
    {
        public string Nombre { get; protected set; }
        public int Salud { get; protected set; }

        protected Personaje(string nombre, int salud)
        {
            Nombre = nombre;
            Salud = salud;
        }

        public abstract void MostrarInformacion();

        public virtual void RecibirDanio(int dano)
        {
            Salud -= dano;

            if (Salud < 0)
                Salud = 0;
        }

        public bool EstaVivo()
        {
            return Salud > 0;
        }
    }

    class Cadete : Personaje
    {
        public int Energia { get; set; }
        public int Estabilidad { get; set; }
        public int Escudo { get; set; }
        public int XP { get; private set; }
        public int DanoExtra { get; set; }

        public Inventario Inventario { get; private set; }

        public Cadete(string nombre)
            : base(nombre, 100)
        {
            Energia = 100;
            Estabilidad = 100;
            Escudo = 50;
            XP = 0;
            DanoExtra = 0;

            Inventario = new Inventario();
        }

        public override void MostrarInformacion()
        {
            Console.WriteLine("\n========== CADETE ==========");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Salud: {Salud}/100");
            Console.WriteLine($"Energía: {Energia}/100");
            Console.WriteLine($"Estabilidad: {Estabilidad}/100");
            Console.WriteLine($"Escudo: {Escudo}/100");
            Console.WriteLine($"XP: {XP}");
            Console.WriteLine("============================");
        }

        public void AgregarXP(int cantidad)
        {
            XP += cantidad;

            Console.WriteLine($"\n⭐ +{cantidad} XP");
        }

        public void RecibirDanioCadete(int dano)
        {
            if (Escudo > 0)
            {
                int absorbido = Math.Min(Escudo, dano);

                Escudo -= absorbido;
                dano -= absorbido;

                Console.WriteLine(
                    $"🛡️ Escudo absorbió {absorbido} de daño.");
            }

            if (dano > 0)
            {
                Salud -= dano;

                if (Salud < 0)
                    Salud = 0;

                Console.WriteLine($"❤️ Salud perdida: {dano}");
            }
        }
    }

    class Robot : Personaje
    {
        public int Defensa { get; private set; }

        public Robot(string nombre)
            : base(nombre, 80)
        {
            Defensa = 25;
        }

        public override void MostrarInformacion()
        {
            Console.WriteLine("\n========== ROBOT ==========");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Salud: {Salud}/80");
            Console.WriteLine($"Defensa: {Defensa}");
            Console.WriteLine("===========================");
        }

        public void ActivarDefensa()
        {
            Console.WriteLine("\n🤖 SISTEMA DEFENSIVO ACTIVADO");
            Console.WriteLine("El robot aumenta su defensa.");
        }
    }

    class Enemigo : Personaje
    {
        public int Dano { get; private set; }

        public Enemigo(string nombre, int salud, int dano)
            : base(nombre, salud)
        {
            Dano = dano;
        }

        public override void MostrarInformacion()
        {
            Console.WriteLine("\n========== ENEMIGO ==========");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Salud: {Salud}");
            Console.WriteLine($"Daño: {Dano}");
            Console.WriteLine("=============================");
        }

        public void Atacar(Cadete cadete)
        {
            Console.WriteLine($"\n👾 {Nombre} ATACA");

            cadete.RecibirDanioCadete(Dano);
        }
    }

    class Realidad
    {
        public string Codigo { get; private set; }
        public string Nombre { get; private set; }
        public int Estabilidad { get; private set; }
        public int NivelRiesgo { get; private set; }
        public string Estado { get; private set; }

        public Realidad(
            string codigo,
            string nombre,
            int estabilidad,
            int riesgo)
        {
            Codigo = codigo;
            Nombre = nombre;
            Estabilidad = estabilidad;
            NivelRiesgo = riesgo;

            ActualizarEstado();
        }

        public void Explorar(Cadete cadete)
        {
            Console.WriteLine(
                $"\n[NEXUS] Explorando {Codigo} - {Nombre}...");

            Thread.Sleep(500);

            if (cadete.Energia < 10)
            {
                Console.WriteLine("\n⚠ Energía insuficiente.");
                return;
            }

            if (cadete.Energia >= 70 &&
                cadete.Estabilidad >= 70)
            {
                Console.WriteLine("\n🟢 EXPLORACIÓN SEGURA");

                cadete.Energia -= 10 + NivelRiesgo;

                cadete.Estabilidad -=
                    NivelRiesgo <= 20 ? 3 : 5;

                Estabilidad -= 2;
            }
            else if (cadete.Energia >= 40 &&
                     cadete.Estabilidad >= 40)
            {
                Console.WriteLine("\n🟡 EXPLORACIÓN INESTABLE");

                cadete.Energia -= 15 + NivelRiesgo;

                cadete.Estabilidad -=
                    6 + NivelRiesgo / 10;

                Estabilidad -= 5;
            }
            else
            {
                Console.WriteLine("\n🔴 EXPLORACIÓN RECHAZADA.");
                return;
            }

            LimitarCadete(cadete);

            if (Estabilidad < 0)
                Estabilidad = 0;

            ActualizarEstado();
        }

        public void MostrarInformacion()
        {
            Console.WriteLine("\n========== REALIDAD ==========");
            Console.WriteLine($"Código: {Codigo}");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Estabilidad: {Estabilidad}/100");
            Console.WriteLine($"Riesgo: {NivelRiesgo}/100");
            Console.WriteLine($"Estado: {Estado}");
            Console.WriteLine("==============================");
        }

        private void ActualizarEstado()
        {
            if (Estabilidad >= 70)
                Estado = "ESTABLE";
            else if (Estabilidad >= 40)
                Estado = "INESTABLE";
            else
                Estado = "CRÍTICA";
        }

        private void LimitarCadete(Cadete cadete)
        {
            if (cadete.Energia < 0)
                cadete.Energia = 0;

            if (cadete.Estabilidad < 0)
                cadete.Estabilidad = 0;

            if (cadete.Energia > 100)
                cadete.Energia = 100;

            if (cadete.Estabilidad > 100)
                cadete.Estabilidad = 100;
        }
    }

    class Mision
    {
        public string Nombre { get; private set; }

        public bool AnomaliaEncontrada { get; private set; }
        public bool NucleoEncontrado { get; private set; }
        public bool EnemigoSuperado { get; private set; }
        public bool ExtraccionRealizada { get; private set; }
        public bool Completada { get; private set; }

        public Mision(string nombre)
        {
            Nombre = nombre;
        }

        public void CompletarAnomalia()
        {
            AnomaliaEncontrada = true;
        }

        public void CompletarNucleo()
        {
            NucleoEncontrado = true;
        }

        public void CompletarEnemigo()
        {
            EnemigoSuperado = true;
        }

        public void CompletarExtraccion()
        {
            ExtraccionRealizada = true;
            Verificar();
        }

        private void Verificar()
        {
            if (AnomaliaEncontrada &&
                NucleoEncontrado &&
                EnemigoSuperado &&
                ExtraccionRealizada)
            {
                Completada = true;
            }
        }

        public void Mostrar()
        {
            Console.WriteLine("\n========== MISIÓN ==========");
            Console.WriteLine($"MISIÓN: {Nombre}");

            Console.WriteLine(
                $"{Marca(AnomaliaEncontrada)} Llegar a la zona de anomalía");

            Console.WriteLine(
                $"{Marca(NucleoEncontrado)} Encontrar el núcleo energético");

            Console.WriteLine(
                $"{Marca(EnemigoSuperado)} Sobrevivir al encuentro");

            Console.WriteLine(
                $"{Marca(ExtraccionRealizada)} Regresar al punto de extracción");

            Console.WriteLine(
                $"Estado: {(Completada ? "COMPLETADA" : "EN PROGRESO")}");

            Console.WriteLine("============================");
        }

        private string Marca(bool completado)
        {
            return completado ? "✓" : "□";
        }
    }

    class Program
    {
        static Random azar = new Random();

        static bool sesionActiva = true;

        static Cadete cadete =
            new Cadete("NEXUS-CADETE-01");

        static Realidad realidadPrincipal =
            new Realidad(
                "R-17",
                "Ciudad de los Ecos",
                90,
                15);

        static Realidad realidadAlterna =
            new Realidad(
                "R-18",
                "Sector de las Sombras",
                60,
                35);

        static Realidad realidadActual;

        static Mision mision =
            new Mision("La anomalía perdida");

        static Enemigo enemigoActual = null;

        static bool tieneNucleo = false;

        static bool jefeDerrotado = false;

        static void Main()
        {
            realidadActual = realidadPrincipal;

            Console.Title = "NEXUS v1.0";

            Inicio();

            cadete.Inventario.Agregar(
                new CelulaEnergia());

            cadete.Inventario.Agregar(
                new Escudo());

            cadete.Inventario.Agregar(
                new Analizador());

            while (sesionActiva)
            {
                if (!cadete.EstaVivo())
                {
                    Derrota(
                        "El Cadete ha perdido toda su salud.");

                    break;
                }

                ComprobarIRIS();

                if (!sesionActiva)
                    break;

                MostrarEstado();

                MostrarMenu();

                string opcion =
                    Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        Explorar();
                        break;

                    case "2":
                        cadete.Inventario.Mostrar();
                        break;

                    case "3":
                        UsarObjeto();
                        break;

                    case "4":
                        MostrarEstadoCompleto();
                        break;

                    case "5":
                        MostrarMapa();
                        break;

                    case "6":
                        CambiarRealidad();
                        break;

                    case "7":
                        mision.Mostrar();
                        break;

                    case "8":
                        AnalizarAnomalia();
                        break;

                    case "9":
                        BuscarNucleo();
                        break;

                    case "10":
                        Extraer();
                        break;

                    case "11":
                        realidadActual.MostrarInformacion();
                        break;

                    case "12":
                        cadete.MostrarInformacion();
                        break;

                    case "13":
                        Console.WriteLine(
                            "\nDesconectando de NEXUS...");

                        sesionActiva = false;
                        break;

                    default:
                        Console.WriteLine(
                            "\n⚠ OPCIÓN NO VÁLIDA.");
                        break;
                }

                if (sesionActiva)
                {
                    Console.WriteLine(
                        "\nPresiona ENTER para continuar...");

                    Console.ReadLine();
                }
            }

            Console.WriteLine(
                "\n======================================");

            Console.WriteLine(
                "        SESIÓN NEXUS FINALIZADA");

            Console.WriteLine(
                "======================================");
        }

        static void Inicio()
        {
            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "==========================================");

            Console.WriteLine(
                "              NEXUS v1.0");

            Console.WriteLine(
                "          AÑO 2297");

            Console.WriteLine(
                "     SISTEMA DE EXPLORACIÓN");

            Console.WriteLine(
                "==========================================");

            Console.ResetColor();

            Console.WriteLine(
                "\nBienvenido, Cadete Explorer.");

            Console.WriteLine(
                "Identificación: NEXUS-CADETE-01");

            Console.WriteLine(
                "Sistema operativo: NEXUS");

            Console.WriteLine(
                "\nNueva misión recibida:");

            Console.WriteLine(
                "\"LA ANOMALÍA PERDIDA\"");

            Console.WriteLine(
                "\nObjetivo:");

            Console.WriteLine(
                "Investigar la anomalía y regresar con vida.");

            Console.WriteLine(
                "\nPresiona ENTER para iniciar.");

            Console.ReadLine();
        }

        static void MostrarMenu()
        {
            Console.WriteLine(
                "\n=============== MENÚ ===============");

            Console.WriteLine(
                "1. Explorar realidad");

            Console.WriteLine(
                "2. Consultar inventario");

            Console.WriteLine(
                "3. Utilizar objeto");

            Console.WriteLine(
                "4. Consultar estado");

            Console.WriteLine(
                "5. Ver mapa");

            Console.WriteLine(
                "6. Cambiar de realidad");

            Console.WriteLine(
                "7. Consultar misión");

            Console.WriteLine(
                "8. Analizar anomalía");

            Console.WriteLine(
                "9. Buscar núcleo energético");

            Console.WriteLine(
                "10. Punto de extracción");

            Console.WriteLine(
                "11. Información de realidad");

            Console.WriteLine(
                "12. Información del Cadete");

            Console.WriteLine(
                "13. Desconectarse");

            Console.WriteLine(
                "====================================");

            Console.Write(
                "Selecciona una opción: ");
        }

        static void MostrarEstado()
        {
            Console.WriteLine(
                "\n------------- ESTADO NEXUS -------------");

            Console.WriteLine(
                $"Realidad: {realidadActual.Codigo} - {realidadActual.Nombre}");

            Barra(
                "Salud",
                cadete.Salud);

            Barra(
                "Energía",
                cadete.Energia);

            Barra(
                "Estabilidad",
                cadete.Estabilidad);

            Barra(
                "Escudo",
                cadete.Escudo);

            Console.WriteLine(
                $"XP: {cadete.XP}");

            if (tieneNucleo)
            {
                Console.WriteLine(
                    "⚡ Núcleo energético: OBTENIDO");
            }

            Console.WriteLine(
                "----------------------------------------");
        }

        static void Barra(
            string nombre,
            int valor)
        {
            int bloques = valor / 10;

            Console.Write(
                $"{nombre}: [");

            for (int i = 0; i < 10; i++)
            {
                if (i < bloques)
                    Console.Write("█");
                else
                    Console.Write("░");
            }

            Console.WriteLine(
                $"] {valor}/100");
        }

        static void Explorar()
        {
            if (cadete.Energia < 10)
            {
                Console.WriteLine(
                    "\n⚠ No tienes suficiente energía.");

                return;
            }

            realidadActual.Explorar(cadete);

            GenerarEvento();
        }

        static void GenerarEvento()
        {
            int evento =
                azar.Next(0, 100);

            if (realidadActual == realidadAlterna)
            {
                if (evento < 25)
                {
                    EncontrarAnomalia();
                    return;
                }

                if (evento < 50)
                {
                    EncontrarEnemigo();
                    return;
                }

                if (evento < 70)
                {
                    EncontrarObjeto();
                    return;
                }

                if (evento < 85)
                {
                    EncontrarRobot();
                    return;
                }

                PerdidaEnergia();
                return;
            }

            if (evento < 25)
            {
                EncontrarObjeto();
            }
            else if (evento < 45)
            {
                EncontrarRobot();
            }
            else if (evento < 60)
            {
                PerdidaEnergia();
            }
            else if (evento < 70)
            {
                EncontrarAnomalia();
            }
            else
            {
                Console.WriteLine(
                    "\n🌌 No ocurrió nada relevante.");
            }
        }

        static void EncontrarObjeto()
        {
            int tipo =
                azar.Next(0, 4);

            Objeto objeto;

            switch (tipo)
            {
                case 0:
                    objeto =
                        new CelulaEnergia();
                    break;

                case 1:
                    objeto =
                        new Escudo();
                    break;

                case 2:
                    objeto =
                        new ModuloAtaque();
                    break;

                default:
                    objeto =
                        new Analizador();
                    break;
            }

            Console.WriteLine(
                $"\n🎁 OBJETO ENCONTRADO: {objeto.Nombre}");

            Console.WriteLine(
                objeto.Descripcion);

            cadete.Inventario.Agregar(
                objeto);
        }

        static void EncontrarRobot()
        {
            Robot robot =
                new Robot("ROBOT GUARDIÁN");

            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "\n🤖 ENTIDAD DETECTADA");

            Console.ResetColor();

            robot.MostrarInformacion();

            robot.ActivarDefensa();

            Console.WriteLine(
                "\nEl robot no es hostil.");
        }

        static void EncontrarEnemigo()
        {
            enemigoActual =
                new Enemigo(
                    "GUARDIÁN DE LA ANOMALÍA",
                    80,
                    15);

            Console.ForegroundColor =
                ConsoleColor.Red;

            Console.WriteLine(
                "\n👾 ENTIDAD HOSTIL DETECTADA");

            Console.ResetColor();

            enemigoActual.MostrarInformacion();

            Combate();
        }

        static void Combate()
        {
            while (
                enemigoActual != null &&
                enemigoActual.EstaVivo() &&
                cadete.EstaVivo())
            {
                Console.WriteLine(
                    "\n=========== COMBATE ===========");

                Console.WriteLine(
                    $"Enemigo: {enemigoActual.Nombre}");

                Console.WriteLine(
                    $"Salud enemigo: {enemigoActual.Salud}");

                Console.WriteLine(
                    $"Salud Cadete: {cadete.Salud}");

                Console.WriteLine(
                    "\n1. Atacar");

                Console.WriteLine(
                    "2. Escapar");

                Console.WriteLine(
                    "3. Utilizar objeto");

                Console.WriteLine(
                    "4. Analizar entidad");

                Console.Write(
                    "\nAcción: ");

                string opcion =
                    Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1":
                        Atacar();
                        break;

                    case "2":
                        if (Escapar())
                            return;
                        break;

                    case "3":
                        UsarObjeto();
                        break;

                    case "4":
                        AnalizarEnemigo();
                        break;

                    default:
                        Console.WriteLine(
                            "\n⚠ Acción inválida.");
                        continue;
                }

                if (
                    enemigoActual != null &&
                    enemigoActual.EstaVivo())
                {
                    enemigoActual.Atacar(
                        cadete);
                }
            }

            if (
                enemigoActual != null &&
                !enemigoActual.EstaVivo())
            {
                Console.ForegroundColor =
                    ConsoleColor.Green;

                Console.WriteLine(
                    "\n🟢 ENEMIGO DERROTADO");

                Console.ResetColor();

                cadete.AgregarXP(50);

                jefeDerrotado = true;

                mision.CompletarEnemigo();

                enemigoActual = null;
            }
        }

        static void Atacar()
        {
            if (enemigoActual == null)
            {
                Console.WriteLine(
                    "\n⚠ No existe ningún enemigo.");

                return;
            }

            if (cadete.Energia < 5)
            {
                Console.WriteLine(
                    "\n⚠ Energía insuficiente para atacar.");

                return;
            }

            int dano =
                20 + cadete.DanoExtra;

            cadete.Energia -= 5;

            enemigoActual.RecibirDanio(
                dano);

            Console.WriteLine(
                "\n🔫 ATAQUE REALIZADO");

            Console.WriteLine(
                $"Daño causado: {dano}");

            Console.WriteLine(
                $"Salud enemiga: {enemigoActual.Salud}");

            cadete.DanoExtra = 0;
        }

        static bool Escapar()
        {
            int posibilidad =
                azar.Next(0, 100);

            if (posibilidad < 70)
            {
                Console.WriteLine(
                    "\n🏃 ESCAPE EXITOSO");

                enemigoActual = null;

                return true;
            }

            Console.WriteLine(
                "\n⚠ No pudiste escapar.");

            return false;
        }

        static void AnalizarEnemigo()
        {
            if (enemigoActual == null)
            {
                Console.WriteLine(
                    "\n⚠ No existe una entidad.");

                return;
            }

            if (
                !cadete.Inventario.TieneTipo(
                    TipoObjeto.Analizador))
            {
                Console.WriteLine(
                    "\n⚠ No tienes un analizador.");

                return;
            }

            Console.WriteLine(
                "\n🔬 ANÁLISIS DE ENTIDAD");

            enemigoActual.MostrarInformacion();

            Console.WriteLine(
                "\nDebilidad detectada: ataque directo.");
        }

        static void UsarObjeto()
        {
            if (cadete.Inventario.Cantidad() == 0)
            {
                Console.WriteLine(
                    "\n⚠ El inventario está vacío.");

                return;
            }

            cadete.Inventario.Mostrar();

            Console.Write(
                "\nNúmero del objeto: ");

            string entrada =
                Console.ReadLine() ?? "";

            if (
                !int.TryParse(
                    entrada,
                    out int numero))
            {
                Console.WriteLine(
                    "\n⚠ Debes introducir un número.");

                return;
            }

            numero--;

            Objeto objeto =
                cadete.Inventario.Obtener(
                    numero);

            if (objeto == null)
            {
                Console.WriteLine(
                    "\n⚠ Ese objeto no existe.");

                return;
            }

            objeto.Usar(cadete);

            if (
                objeto.Tipo ==
                TipoObjeto.CelulaEnergia ||
                objeto.Tipo ==
                TipoObjeto.Escudo ||
                objeto.Tipo ==
                TipoObjeto.ModuloAtaque)
            {
                cadete.Inventario.Eliminar(
                    objeto);

                Console.WriteLine(
                    "Objeto consumido.");
            }
        }

        static void EncontrarAnomalia()
        {
            Console.ForegroundColor =
                ConsoleColor.Magenta;

            Console.WriteLine(
                "\n⚡ ANOMALÍA DETECTADA");

            Console.ResetColor();

            Console.WriteLine(
                "Una perturbación desconocida aparece.");

            Console.WriteLine(
                "La señal coincide con el objetivo de la misión.");

            mision.CompletarAnomalia();
        }

        static void AnalizarAnomalia()
        {
            if (!mision.AnomaliaEncontrada)
            {
                Console.WriteLine(
                    "\n⚠ Todavía no has encontrado una anomalía.");

                return;
            }

            if (cadete.Energia < 10)
            {
                Console.WriteLine(
                    "\n⚠ No hay suficiente energía.");

                return;
            }

            cadete.Energia -= 10;

            Console.WriteLine(
                "\n🔬 ANALIZANDO ANOMALÍA...");

            Thread.Sleep(500);

            Console.WriteLine(
                "\nAnálisis completado.");

            Console.WriteLine(
                "Se detectó una fuerte señal energética.");

            Console.WriteLine(
                "Posible ubicación: sector central.");
        }

        static void BuscarNucleo()
        {
            if (!mision.AnomaliaEncontrada)
            {
                Console.WriteLine(
                    "\n⚠ Primero debes encontrar la anomalía.");

                return;
            }

            if (realidadActual != realidadAlterna)
            {
                Console.WriteLine(
                    "\n⚠ La señal del núcleo se encuentra en R-18.");

                Console.WriteLine(
                    "Debes dirigirte al Sector de las Sombras.");

                return;
            }

            if (tieneNucleo)
            {
                Console.WriteLine(
                    "\nYa tienes el núcleo energético.");

                return;
            }

            if (!jefeDerrotado)
            {
                Console.WriteLine(
                    "\n⚠ El Guardián todavía protege el núcleo.");

                Console.WriteLine(
                    "Debes sobrevivir al encuentro antes de recogerlo.");

                return;
            }

            Console.ForegroundColor =
                ConsoleColor.Yellow;

            Console.WriteLine(
                "\n⚡ NÚCLEO ENERGÉTICO ENCONTRADO");

            Console.ResetColor();

            Console.WriteLine(
                "El núcleo estaba oculto detrás del Guardián.");

            tieneNucleo = true;

            mision.CompletarNucleo();

            cadete.Inventario.Agregar(
                new NucleoEnergetico());

            cadete.AgregarXP(50);
        }

        static void Extraer()
        {
            Console.WriteLine(
                "\n========== PUNTO DE EXTRACCIÓN ==========");

            if (realidadActual != realidadPrincipal)
            {
                Console.WriteLine(
                    "\n⚠ Debes regresar a R-17.");

                return;
            }

            if (!tieneNucleo)
            {
                Console.WriteLine(
                    "\n⚠ No tienes el núcleo energético.");

                return;
            }

            if (!mision.EnemigoSuperado)
            {
                Console.WriteLine(
                    "\n⚠ Debes sobrevivir al encuentro.");

                return;
            }

            Console.WriteLine(
                "\n🚀 Regresando al punto de extracción...");

            Thread.Sleep(700);

            mision.CompletarExtraccion();

            if (mision.Completada)
            {
                MisionCompletada();
            }
        }

        static void MisionCompletada()
        {
            Console.ForegroundColor =
                ConsoleColor.Green;

            Console.WriteLine(
                "\n==========================================");

            Console.WriteLine(
                "         🟢 MISIÓN COMPLETADA");

            Console.WriteLine(
                "==========================================");

            Console.WriteLine(
                "\nLA ANOMALÍA PERDIDA");

            Console.WriteLine(
                "\n✓ Llegaste a la zona de anomalía");

            Console.WriteLine(
                "✓ Encontraste el núcleo energético");

            Console.WriteLine(
                "✓ Sobreviviste al encuentro");

            Console.WriteLine(
                "✓ Regresaste al punto de extracción");

            Console.WriteLine(
                "\nRECOMPENSAS");

            Console.WriteLine(
                "+100 XP");

            Console.WriteLine(
                "+1 módulo de ataque");

            cadete.AgregarXP(100);

            cadete.Inventario.Agregar(
                new ModuloAtaque());

            Console.WriteLine(
                "\n🎉 EL CADETE REGRESÓ CON VIDA.");

            Console.WriteLine(
                "MISIÓN REGISTRADA POR NEXUS.");

            Console.WriteLine(
                "\n==========================================");

            Console.ResetColor();

            sesionActiva = false;
        }

        static void ComprobarIRIS()
        {
            if (
                cadete.Estabilidad <= 35 ||
                (
                    cadete.Energia <= 40 &&
                    cadete.Estabilidad <= 40
                ))
            {
                Console.ForegroundColor =
                    ConsoleColor.Red;

                Console.WriteLine(
                    "\n🚨 ANOMALÍA IRIS DETECTADA");

                Console.WriteLine(
                    "La estabilidad del Cadete es crítica.");

                Console.ResetColor();
            }

            if (cadete.Estabilidad <= 10)
            {
                Derrota(
                    "La estabilidad alcanzó un nivel crítico.");
            }
        }

        static void Derrota(string motivo)
        {
            Console.ForegroundColor =
                ConsoleColor.Red;

            Console.WriteLine(
                "\n==========================================");

            Console.WriteLine(
                "           🔴 MISIÓN FALLIDA");

            Console.WriteLine(
                "==========================================");

            Console.WriteLine(
                $"\nMotivo: {motivo}");

            Console.WriteLine(
                "\nNEXUS no pudo completar la misión.");

            Console.WriteLine(
                "==========================================");

            Console.ResetColor();

            sesionActiva = false;
        }

        static void CambiarRealidad()
        {
            if (realidadActual == realidadPrincipal)
            {
                realidadActual =
                    realidadAlterna;

                Console.WriteLine(
                    $"\n[NEXUS] Has ingresado a " +
                    $"{realidadActual.Codigo} - " +
                    $"{realidadActual.Nombre}");
            }
            else
            {
                realidadActual =
                    realidadPrincipal;

                Console.WriteLine(
                    $"\n[NEXUS] Has regresado a " +
                    $"{realidadActual.Codigo} - " +
                    $"{realidadActual.Nombre}");
            }
        }

        static void MostrarMapa()
        {
            Console.WriteLine(
                $"\n========== MAPA {realidadActual.Codigo} ==========");

            Console.WriteLine();

            for (int fila = 0; fila < 8; fila++)
            {
                for (int columna = 0; columna < 12; columna++)
                {
                    int valor =
                        azar.Next(0, 4);

                    switch (valor)
                    {
                        case 0:
                            Console.Write(". ");
                            break;

                        case 1:
                            Console.Write("■ ");
                            break;

                        case 2:
                            Console.Write("● ");
                            break;

                        default:
                            Console.Write("? ");
                            break;
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine(
                "\n========== LEYENDA ==========");

            Console.WriteLine(
                ". = Zona");

            Console.WriteLine(
                "■ = Base");

            Console.WriteLine(
                "● = Recurso");

            Console.WriteLine(
                "? = Anomalía");

            Console.WriteLine(
                "==============================");
        }

        static void MostrarEstadoCompleto()
        {
            cadete.MostrarInformacion();

            realidadActual.MostrarInformacion();

            mision.Mostrar();

            if (enemigoActual != null)
            {
                Console.WriteLine(
                    "\n⚠ Existe un enemigo activo.");
            }
        }

        static void PerdidaEnergia()
        {
            int perdida = azar.Next(5, 16);

            cadete.Energia -= perdida;

            if (cadete.Energia < 0)
                cadete.Energia = 0;

            Console.WriteLine(
                $"\n⚠ PÉRDIDA DE ENERGÍA: -{perdida}");

            Console.WriteLine(
                $"Energía actual: {cadete.Energia}/100");
        }
    }
}