using System;
using System.Collections.Generic;
using System.Linq;

namespace Pry_Juegos_Final_Modulo1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Personaliza el aspecto visual
            // Fondo Azul oscuro y Texto blanco

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();  // Limpia la consola para aplicar los cambios


            int opcion;

            // Defino el Menù de de la aplicaciòn 
            // 1. Sudoku ==> llamo a la funciòn JugarSudoku()
            // 2. Piedra, Papel o Tijera ==> llamo a la funciòn JugarPiedraPapelTijera()
            // 3. Ahorcado ==> llamo a la funciòn JugarAhorcado()
            // 4. Salir ==> realizo un break y salgo del bucle.

            // Aqui manejo el bucle do..While para mantener  el menù hasta que el usuario elija la opciòn salir (4)

            do
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE JUEGOS ===");
                Console.WriteLine("1. Sudoku");
                Console.WriteLine("2. Piedra, Papel o Tijera");
                Console.WriteLine("3. Ahorcado");
                Console.WriteLine("4. Salir");
                Console.Write("Selecciona una opción: ");

                // Aqui valido con el bloque While que la opciòn ingresada se encuentre en 1 y 4
                while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 4)
                {
                    Console.Write("Opción inválida. Ingresa un número entre 1 y 4: ");
                }

                // Aqui mediante un Switch defino que opciòn escogio el usuario y lanzo la funciòn a seguir 
                // si el usuario selecciona 4 pues la aplicaciòn "Sale"
                switch (opcion)
                {
                    case 1:
                        JugarSudoku();
                        break;
                    case 2:
                        JugarPiedraPapelTijera();
                        break;
                    case 3:
                        JugarAhorcado();
                        break;
                    case 4:
                        Console.WriteLine("Gracias por jugar. ¡Hasta luego!");
                        break;
                }

            } while (opcion != 4);
        }

        // ================== Juego: Sudoku ==================
        static void JugarSudoku()
        {
            Console.Clear(); // Limpia la Consola

            // Solicita que el usuario ingrese el tamaño del bloque a definir para el juego

            Console.Write("Ingrese el tamaño del tablero de Sudoku (por ejemplo, 3 para 3x3): ");
            int tamaño = int.Parse(Console.ReadLine());

            // Crea un arreglo bidimensional para el tablero a jugar con el tamaño definido por el usuario
            int[,] tablero = new int[tamaño, tamaño];
            Random rand = new Random();

            // Llama a la funciòn RellenarCasilleros, para inicializar el Juego, eviando el arreglo establecido, el tamaño definido, y el valor aleatorio  creado
            RellenarCasilleros(tablero, tamaño, rand);

            // Aplico un bucle While (infinito) hasta que el usuario teclee "salir"
            while (true)
            {
                Console.Clear(); // Limpio la consola
                MostrarTablero(tablero, tamaño);   // Imprimir los numero en la consola
                Console.WriteLine("Ingrese fila,columna,valor (ej: 1,2,3) o 'salir' para terminar:");

                string entrada = Console.ReadLine();
                if (entrada.ToLower() == "salir")
                    break;

                string[] partes = entrada.Split(',');

                if (partes.Length == 3 &&
                    int.TryParse(partes[0].Trim(), out int fila) &&
                    int.TryParse(partes[1].Trim(), out int columna) &&
                    int.TryParse(partes[2].Trim(), out int valor))
                {
                    fila--; columna--;

                    if (valor < 1 || valor > tamaño || fila < 0 || fila >= tamaño || columna < 0 || columna >= tamaño)
                    {
                        Console.WriteLine("¡Datos fuera de rango! Presiona una tecla para continuar...");
                        Console.ReadKey();
                        continue;
                    }

                    if (EsMovimientoValido(tablero, fila, columna, valor, tamaño))
                    {
                        tablero[fila, columna] = valor;
                    }
                    else
                    {
                        Console.WriteLine("¡Movimiento inválido! Presiona una tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Usa el formato: fila,columna,valor");
                    Console.ReadKey();
                }
            }
        }

        static void MostrarTablero(int[,] tablero, int tamaño)
        {
            // Descripciòn de la funciòn :  imprime en la consola el tablero del juego Sudoku, mostrando:
            //                              "." - punto : si una celda está vacía.
            //                              nùmero : si la celda ya tiene un valor (del arreglo)

            for (int fila = 0; fila < tamaño; fila++)
            {
                // Es equivalente a for (int col = 0; col < tamaño; col++), , pero usando LINQ para generar
                // una secuencia de números consecutivos.
                // "Enumerable.Range(0, tamaño)" : se usa para generar una secuencia de números enteros consecutivos.
                foreach (var col in Enumerable.Range(0, tamaño))
                {
                    int val = tablero[fila, col];
                    Console.Write(val == 0 ? ". " : val + " ");
                }
                Console.WriteLine();
            }
        }

        static bool EsMovimientoValido(int[,] tablero, int fila, int columna, int valor, int tamaño)
        {
            // Descripciòn de la funciòn: Determinar si colocar un número (valor) en una celda específica (fila, columna)
            //                            es válido según las reglas básicas del Sudoku (sin repetir en fila ni columna).

            // Este bucle "for" recorre toda las filas y columnas.
            for (int i = 0; i < tamaño; i++)
            {
                // Revisa si ya existe el mismo número en la misma fila Y Revisa si ya existe el mismo número en la misma columna.
                if (tablero[fila, i] == valor || tablero[i, columna] == valor)
                    return false;
            }
            return true;
        }

        static void RellenarCasilleros(int[,] tablero, int tamaño, Random rand)
        {
            // Descripciòn de la funciòn : Colocar una cantidad inicial de números válidos de forma aleatoria en el tablero de Sudoku
            //                              para que el jugador tenga una base sobre la cual completar el resto.
            // La funciòn no retorna valor  : static void
            int cantidad = tamaño;

            for (int i = 0; i < cantidad; i++)
            {
                int fila = rand.Next(tamaño);   // Define la fila aleatoria
                int columna = rand.Next(tamaño);  // Define la Columna aleatoria
                int valor = rand.Next(1, tamaño + 1); // Define el Valor Aleatorio

                if (EsMovimientoValido(tablero, fila, columna, valor, tamaño))
                {
                    // Si es verdadero esto quiere decir valido, asigna el valor en el arrego 
                    tablero[fila, columna] = valor;
                }
                else
                {
                    i--; // intentar de nuevo
                }
            }
        }

        // ================== Juego: Piedra, Papel o Tijera ==================
        static void JugarPiedraPapelTijera()
        {
            string[] opciones = { "Piedra", "Papel", "Tijera" };
            bool seguir = true;

            while (seguir)
            {
                Console.Clear();
                Console.WriteLine("=== Piedra, Papel o Tijera ===");

                int i = 1;
                foreach (string opcion in opciones)
                {
                    Console.WriteLine($"{i}. {opcion}");
                    i++;
                }

                Console.Write("Elige una opción (1-3): ");

                int eleccionUsuario;
                while (!int.TryParse(Console.ReadLine(), out eleccionUsuario) || eleccionUsuario < 1 || eleccionUsuario > 3)
                {
                    Console.Write("Entrada inválida. Elige 1, 2 o 3: ");
                }

                string jugador = opciones[eleccionUsuario - 1];
                Random rnd = new Random();
                string pc = opciones[rnd.Next(3)];

                Console.WriteLine($"\nTú elegiste: {jugador}");
                Console.WriteLine($"PC eligió: {pc}");

                string resultado = ObtenerResultado(jugador, pc);
                Console.WriteLine($"Resultado: {resultado}");

                Console.Write("\n¿Jugar otra vez? (s/n): ");
                seguir = Console.ReadLine().ToLower() == "s";
            }
        }

        static string ObtenerResultado(string jugador, string pc)
        {
            if (jugador == pc) return "¡Empate!";
            else if ((jugador == "Piedra" && pc == "Tijera") ||
                     (jugador == "Papel" && pc == "Piedra") ||
                     (jugador == "Tijera" && pc == "Papel"))
                return "¡Ganaste!";
            else
                return "Perdiste.";
        }

        // ================== Juego: Ahorcado ==================
        static void JugarAhorcado()
        {
            string[] palabras = { "computadora", "teclado", "monitor", "programador", "internet" };
            Random rand = new Random();
            string palabra = palabras[rand.Next(palabras.Length)];

            char[] adivinada = new string('_', palabra.Length).ToCharArray();
            int intentos = 6;
            List<char> letrasUsadas = new List<char>();

            while (intentos > 0 && new string(adivinada) != palabra)
            {
                Console.Clear();
                Console.WriteLine("=== AHORCADO ===");
                Console.Write("Palabra: ");
                foreach (char letra in adivinada)
                {
                    Console.Write(letra + " ");
                }
                Console.WriteLine();

                Console.WriteLine("Letras usadas: " + string.Join(", ", letrasUsadas));
                Console.WriteLine($"Intentos restantes: {intentos}");

                Console.Write("Ingresa una letra: ");
                char letraIngresada = Console.ReadLine().ToLower()[0];

                if (letrasUsadas.Contains(letraIngresada))
                {
                    Console.WriteLine("Ya usaste esa letra.");
                    Console.ReadKey();
                    continue;
                }

                letrasUsadas.Add(letraIngresada);

                if (palabra.Contains(letraIngresada))
                {
                    for (int i = 0; i < palabra.Length; i++)
                    {
                        if (palabra[i] == letraIngresada)
                        {
                            adivinada[i] = letraIngresada;
                        }
                    }
                }
                else
                {
                    intentos--;
                }
            }

            Console.Clear();
            if (new string(adivinada) == palabra)
                Console.WriteLine($"¡Felicidades! Adivinaste la palabra: {palabra}");
            else
                Console.WriteLine($"¡Perdiste! La palabra era: {palabra}");

            Console.WriteLine("Presiona cualquier tecla para volver al menú.");
            Console.ReadKey();
        }
    }
}
