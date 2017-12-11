using System;
using System.Collections.Generic;
using Generator;

namespace Juego
{
    class Program
    {
        enum acciones { comprobar, marcar, mostrar, invalida }
        enum estados { jugando, procesando, perdio, gano }

        static void Main(string[] args)
        {
            Table buscaMinas;
            int alto, ancho, bombas;
            estados estado = estados.jugando;

            Console.Title = "BuscaMinas";
            
            obtenerDatos(out alto, out ancho, out bombas);

            buscaMinas = new Table(alto, ancho, bombas);

            Console.Clear();

            dibujarTabla(alto, ancho, buscaMinas);

            Console.Title = "BuscaMinas - Cant. Bombas: " + bombas;

            while (estado == estados.jugando)
            {
                do
                {
                    switch (elegirAccion())
                    {
                        case acciones.mostrar:
                            buscaMinas.showSquare(leerCoordenadas(alto, ancho));
                            break;

                        case acciones.marcar:
                            buscaMinas.changeFlag(leerCoordenadas(alto, ancho));
                            break;

                        case acciones.comprobar:
                            if (buscaMinas.wins())
                                estado = estados.gano;
                            else
                                estado = estados.jugando;
                            break;

                        default:
                            estado = estados.procesando;
                            break;
                    };
                } while (estado == estados.procesando);
                Console.Clear();
                dibujarTabla(alto, ancho, buscaMinas);
            }
            if (estado == estados.gano)
                Console.WriteLine("GANO");
            else
                Console.WriteLine("PERDIO");
            Console.WriteLine("Presione cualquier tecla para continuar.");
            Console.ReadKey();

        }
        private static void obtenerDatos (out int alto, out int ancho, out int bombas)
        {
            Console.Write("Por favor ingrese el alto de la tabla: ");
            while (!int.TryParse(Console.ReadLine(), out alto))
                Console.Write("Numero ingresado no es valido, intentelo otra vez: ");

            Console.Write("Por favor ingrese el ancho de la tabla: ");
            while (!int.TryParse(Console.ReadLine(), out ancho))
                Console.Write("Numero ingresado no es valido, intentelo otra vez: ");

            Console.Write("Por favor la cantidad de bombas: ");
            while (!int.TryParse(Console.ReadLine(), out bombas) || bombas > alto * ancho)
            {
                if (bombas > alto * ancho)
                    Console.Write("La cantidad de bombas excede el limite posible, intentelo otra vez: ");
                else
                    Console.Write("Numero ingresado no es valido, intentelo otra vez: ");
            }
        }
        private static void dibujarTabla (int alto, int ancho, Table tabla)
        {
            int x, y;
            Console.CursorLeft = (Console.WindowWidth - 2 * ancho) / 2;
            for (int i = 0; i < ancho; i++)
            {
                Console.Write(" {0}", i);
            }
            Console.Write('\n');
            for (x = 0; x < alto; x++)
            {
                Console.CursorLeft = (Console.WindowWidth - 2 * ancho - 2) / 2;
                Console.Write(x);
                for (y = 0; y < ancho; y++)
                {
                    Console.Write('|');
                    Console.Write(tabla.getTable()[x, y].asChar());
                }
                Console.Write("|\n");
            }
            Console.CursorLeft = 0;
        }
        private static Coordinate leerCoordenadas(int alto, int ancho)
        {
            Coordinate retorno = new Coordinate();
            char[] separadores = { ' ', '\t' };
            bool coValidadas = false;
            string[] coordenadas;

            Console.Write("Por favor ingrese las coordenadas en el siguiente formato \"fila columna\": ");
            
            do
            {
                coordenadas = Console.ReadLine().Split(separadores);
                if (coordenadas.Length >= 2)
                {
                    bool validesCoUno = int.TryParse(coordenadas[0], out retorno.first);
                    bool validesCoDos = int.TryParse(coordenadas[1], out retorno.second);

                    if (!validesCoUno || !validesCoDos)
                        Console.Write("El formato no es correcto, intentelo nuevamente: ");
                    else if (retorno.first >= alto || retorno.second >= ancho)
                        Console.Write("Las coordenadas no existen, intentelo nuevamente: ");
                    else
                        coValidadas = true;
                }
                else
                    Console.Write("No se ingreso un elemento, intentelo nuevamente: ");
            } while (!coValidadas);
            return retorno;
        }
        private static acciones elegirAccion ()
        {
            Console.WriteLine("1 - Mostrar casilla \n2 - (Des)marcar casilla \n3 - Comprobar marcas");
            char[] letra = Console.ReadLine().ToCharArray();
            switch(letra[0])
            {
                case '1':                    
                    return acciones.mostrar;
                case '2':
                    return acciones.marcar;
                case '3':
                    return acciones.comprobar;
                default:
                    return acciones.invalida;
            };

        }
        
    }
}
