using System;
using System.Collections.Generic;
using System.Linq;

namespace Pry_Juegos_Final_Modulo1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ DE JUEGOS ===");
                Console.WriteLine("1. Sudoku");
                Console.WriteLine("2. Piedra, Papel o Tijera");
                Console.WriteLine("3. Ahorcado");
                Console.WriteLine("4. Salir");
                Console.Write("Selecciona una opción: ");

                while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < 1 || opcion > 4)
                {
                    Console.Write("Opción inválida. Ingresa un número entre 1 y 4: ");
                }

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
            Console.Clear();
            Console.Write("Ingrese el tamaño del tablero de Sudoku (por ejemplo, 3 para 3x3): ");
            int tamaño = int.Parse(Console.ReadLine());

            int[,] tablero = new int[tamaño, tamaño];
            Random rand = new Random();

            RellenarCasilleros(tablero, tamaño, rand);

            while (true)
            {
                Console.Clear();
                MostrarTablero(tablero, tamaño);
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
            for (int fila = 0; fila < tamaño; fila++)
            {
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
            for (int i = 0; i < tamaño; i++)
            {
                if (tablero[fila, i] == valor || tablero[i, columna] == valor)
                    return false;
            }
            return true;
        }

        static void RellenarCasilleros(int[,] tablero, int tamaño, Random rand)
        {
            int cantidad = tamaño;

            for (int i = 0; i < cantidad; i++)
            {
                int fila = rand.Next(tamaño);
                int columna = rand.Next(tamaño);
                int valor = rand.Next(1, tamaño + 1);

                if (EsMovimientoValido(tablero, fila, columna, valor, tamaño))
                {
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
