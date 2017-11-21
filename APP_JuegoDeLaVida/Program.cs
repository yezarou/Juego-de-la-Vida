/* ===========================================
 * AUTOR:       Rubén Zúñiga García
 * FECHA:       21/11/2017
 * VERSION:     1.0
 * DESCRIPCION: EL JUEGO DE LA VIDA
 * =========================================== */

using System;
using System.Threading;

namespace APP_JuegoDeLaVida
{
    class Program
    {
        #region Variables
        // Declaración de variables de la clase Porgram.
        static int MAXIMOFILA = Console.LargestWindowHeight - 12;       // Filas máximas del tablero.
        static int MAXIMOCOLUMNA = Console.LargestWindowWidth - 10;     // Columnas máximas del tablero.
        static int celulasVivas = 0;                                    // Numero de celulas vivas en el tablero.
        static string celulaViva = "*";                                 // Representación de una celula viva.
        static string celulaMuerta = " ";                               // Representación de una celula muerta.
        #endregion

        static void Main(string[] args)
        {
            // Declaración de variables del main.
            string[,] Tablero = new string[MAXIMOFILA, MAXIMOCOLUMNA];                      // Tablero principal.
            string[,] TableroSiguienteGeneracion = new string[MAXIMOFILA, MAXIMOCOLUMNA];   // Tablero con la información de la siguiente generación.
            int generaciones = 0;
            bool salir = false;                 // Salida del programa.
            bool introduccionTecla = false;     // Comprobación de una tecla correcta.
            bool automatico = false;            // Automatizar generaciones.
            bool reseteo = true;                // Resetear tablero (True para generar el primero).
            ConsoleKeyInfo tecla;               // Información de la tecla pulsada.

            // Logo y marco del programa.
            Console.CursorVisible = false;
            MarcoInformativo();

            // Primer Do-While para salir del programa con el botón escape.
            do
            {
                // Segundo Do-While para automatizar el programa.
                do
                {
                    // Generar nuevo tablero.
                    if (reseteo == true)
                    {
                        generaciones = 0;
                        InicializarMatrizAlea(Tablero);
                        CopiarMatriz(Tablero, TableroSiguienteGeneracion);
                        reseteo = false;
                    }

                    // Mostrar tablero e información de él.
                    Console.SetCursorPosition(4, 4);
                    MostrarMatriz(Tablero);
                    Console.SetCursorPosition(4, 1);
                    Console.WriteLine("Celulas vivas:   {0:D4}", celulasVivas);
                    celulasVivas = 0;
                    Console.SetCursorPosition(4, 2);
                    Console.WriteLine("Generación:     {0:D5}", ++generaciones);

                    // Generar siguiente generación de celulas.
                    CambiarCelulas(Tablero, TableroSiguienteGeneracion);
                    CopiarMatriz(TableroSiguienteGeneracion, Tablero);
                    if (automatico == true)
                        Thread.Sleep(50);

                    // Si se pulsa una tecla cualquiera, el sistema deja de ser automatico.
                    if (Console.KeyAvailable)
                        automatico = false;
                } while (automatico == true);

                // Do-While para comprobar que se introduce una tecla correcta.
                do
                {
                    introduccionTecla = false;
                    tecla = Console.ReadKey(true);
                    switch (tecla.Key)
                    {
                        case ConsoleKey.Escape:
                            introduccionTecla = true;
                            salir = true;
                            break;
                        case ConsoleKey.I:
                            introduccionTecla = true;
                            break;
                        case ConsoleKey.A:
                            introduccionTecla = true;
                            automatico = true;
                            break;
                        case ConsoleKey.R:
                            introduccionTecla = true;
                            automatico = false;
                            reseteo = true;
                            break;
                    }
                } while (!introduccionTecla && !salir);
            } while (!salir);
        }

        #region Funciones De Matrices.
        /// <summary>
        /// Inicializa una matriz, asignando la vida de la celula de forma aleatoria.
        /// </summary>
        /// <param name="matriz"></param>
        static void InicializarMatrizAlea(string[,] matriz)
        {
            Random rnd = new Random();
            for (int i = 0; i < MAXIMOFILA; i++)
                for (int j = 0; j < MAXIMOCOLUMNA; j++)
                    if (rnd.Next(4) == 0)
                        matriz[i, j] = celulaViva;
                    else
                        matriz[i, j] = celulaMuerta;
        }

        /// <summary>
        /// Muestra la matriz del tablero en pantalla.
        /// </summary>
        /// <param name="array"></param>
        static void MostrarMatriz(string[,] array)
        {
            for (int i = 0; i < MAXIMOFILA; i++)
            {
                for (int j = 0; j < MAXIMOCOLUMNA; j++)
                {
                    Console.Write(array[i, j]);
                    if (array[i, j] == "*")
                        celulasVivas++;
                }
                Console.WriteLine();
                Console.CursorLeft = 4;
            }
        }

        /// <summary>
        /// Función que copia los datos de la primera matriz a la segunda.
        /// </summary>
        /// <param name="matriz1"></param>
        /// <param name="matriz2"></param>
        static void CopiarMatriz(string[,] matriz1, string[,] matriz2)
        {
            for (int i = 0; i < MAXIMOFILA; i++)
                for (int j = 0; j < MAXIMOCOLUMNA; j++)
                    matriz2[i,j] = matriz1[i,j];
        }

        /// <summary>
        /// Función que comprueba la vida de la siguiente generación de celulas de una matriz.
        /// </summary>
        /// <param name="matrizOriginal"></param>
        /// <param name="matrizCambiado"></param>
        static void CambiarCelulas(string[,] matrizOriginal, string[,] matrizCambiado)
        {
            int celulasVecinas = 0;
            for (int i = 0; i < MAXIMOFILA; i++)
                for (int j = 0; j < MAXIMOCOLUMNA; j++)
                {
                    celulasVecinas = 0;
                    if (matrizOriginal[((MAXIMOFILA + i) - 1) % MAXIMOFILA, j] == celulaViva)
                        celulasVecinas++;
                    if (matrizOriginal[((MAXIMOFILA + i) + 1) % MAXIMOFILA, j] == celulaViva)
                        celulasVecinas++;
                    if (matrizOriginal[i,((MAXIMOCOLUMNA + j) - 1) % MAXIMOCOLUMNA] == celulaViva)
                        celulasVecinas++;
                    if (matrizOriginal[i,((MAXIMOCOLUMNA + j) + 1) % MAXIMOCOLUMNA] == celulaViva)
                        celulasVecinas++;
                    if (matrizOriginal[((MAXIMOFILA + i) - 1) % MAXIMOFILA, ((MAXIMOCOLUMNA + j) + 1) % MAXIMOCOLUMNA] == celulaViva)
                        celulasVecinas++;
                    if (matrizOriginal[((MAXIMOFILA + i) - 1) % MAXIMOFILA, ((MAXIMOCOLUMNA + j) - 1) % MAXIMOCOLUMNA] == celulaViva)
                        celulasVecinas++;
                    if (matrizOriginal[((MAXIMOFILA + i) + 1) % MAXIMOFILA, ((MAXIMOCOLUMNA + j) + 1) % MAXIMOCOLUMNA] == celulaViva)
                        celulasVecinas++;
                    if (matrizOriginal[((MAXIMOFILA + i) + 1) % MAXIMOFILA, ((MAXIMOCOLUMNA + j) - 1) % MAXIMOCOLUMNA] == celulaViva)
                        celulasVecinas++;
                    if ((matrizOriginal[i,j]) == celulaViva)
                        if (celulasVecinas >= 2 && celulasVecinas <= 3)
                            matrizCambiado[i,j] = celulaViva;
                        else
                            matrizCambiado[i,j] = celulaMuerta;
                    else if (celulasVecinas == 3)
                        matrizCambiado[i,j] = celulaViva;
                }
        }

        #endregion

        #region Decoracion.

        /// <summary>
        /// Marco del programa e introducción.
        /// </summary>
        static void MarcoInformativo()
        {
            Console.WindowHeight = MAXIMOFILA + 11;
            Console.WindowWidth = MAXIMOCOLUMNA + 9;
            Console.BackgroundColor = ConsoleColor.Blue;
            for (int i = 0; i < MAXIMOFILA + 11; i++)
            {
                for (int j = 0; j < MAXIMOCOLUMNA + 8; j++)
                    Console.Write(" ");
                Console.WriteLine();
            }
            Console.SetCursorPosition(0, 0);

            LogoJuegoDeLaVida();

            Console.ReadKey(true);

            Console.SetCursorPosition(Console.WindowWidth/2 - 6, MAXIMOFILA + 5);
            Console.WriteLine("INSTRUCCIONES");
            Console.CursorLeft = 4;
            Console.WriteLine("i:    (Iterar)     Se avanza una generación.");
            Console.CursorLeft = 4;
            Console.WriteLine("a:    (Automático) Mientras no se pulse una tecla se van produciendo nuevas generaciones.");
            Console.CursorLeft = 4;
            Console.WriteLine("r:    (Reset)      Se genera un tablero nuevo de forma aleatoria.");
            Console.CursorLeft = 4;
            Console.WriteLine("ESC:  (Finalizar)  Finaliza el programa.");

            Console.ResetColor();
        }

        /// <summary>
        /// Logo del programa.
        /// </summary>
        static void LogoJuegoDeLaVida()
        {
            Console.SetCursorPosition(Console.LargestWindowWidth / 2 - 16, Console.LargestWindowHeight / 2 - 9);
            Console.WriteLine("       _                        ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 16;
            Console.WriteLine("      | |                       ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 16;
            Console.WriteLine("      | |_   _  ___  __ _  ___  ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 16;
            Console.WriteLine("  _   | | | | |/ _ \\/ _` |/ _ \\ ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 16;
            Console.WriteLine(" | |__| | |_| |  __/ (_| | (_) |");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 16;
            Console.WriteLine("  \\____/ \\__,_|\\___|\\__, |\\___/ ");

            Console.WriteLine();

            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("      _        _       ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("     | |      | |      ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("   __| | ___  | | __ _ ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("  / _` |/ _ \\ | |/ _` |");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine(" | (_| |  __/ | | (_| |");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("  \\__,_|\\___| |_|\\__,_|");

            Console.WriteLine();

            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("        _     _       ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("       (_)   | |      ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine(" __   ___  __| | __ _ ");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine(" \\ \\ / / |/ _` |/ _` |");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("  \\ V /| | (_| | (_| |");
            Console.CursorLeft = Console.LargestWindowWidth / 2 - 11;
            Console.WriteLine("   \\_/ |_|\\__,_|\\__,_| \n\n\n");

            Console.CursorLeft = Console.LargestWindowWidth / 2 - 16;
            Console.WriteLine("Pulse una tecla para continuar . . .");
        }
        #endregion
    }
}
