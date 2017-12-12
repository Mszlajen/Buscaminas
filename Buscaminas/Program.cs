using System;
using Generator;

namespace Juego
{
	enum acciones { comprobar, marcar, mostrar, invalida }
	enum estados { jugando, procesando, perdio, gano }

    class Program
    {
        static void Main()
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

            Coordinate co;
            while (estado == estados.jugando)
            {
                do
                {
                    switch (elegirAccion())
                    {
                        case acciones.mostrar:
                            co = leerCoordenadas(alto, ancho);
                            mostrarCuadro(alto, ancho, ref buscaMinas, ref co);
                            if (!buscaMinas.isFlagged(co) && buscaMinas.isBomb(co))
                                estado = estados.perdio;
                            break;

                        case acciones.marcar:
                            co = leerCoordenadas(alto, ancho);
                            buscaMinas.changeFlag(co);
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
                    }
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
            while (!getValue(out alto))
                Console.Write("Numero ingresado no es valido, intentelo otra vez: ");

            Console.Write("Por favor ingrese el ancho de la tabla: ");
            while (!getValue(out ancho))
                Console.Write("Numero ingresado no es valido, intentelo otra vez: ");

            Console.Write("Por favor la cantidad de bombas: ");
            while (!getValue(out bombas) || bombas > alto * ancho)
            {
                if (bombas > alto * ancho)
                    Console.Write("La cantidad de bombas excede el limite posible, intentelo otra vez: ");
                else
                    Console.Write("Numero ingresado no es valido, intentelo otra vez: ");
            }
        }

        private static bool getValue (out int value) 
        {
            return int.TryParse(Console.ReadLine(), out value);
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
            string letra = Console.ReadLine();
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
            }

        }
        private static void mostrarCuadro (int alto, int ancho, ref Table BM, ref Coordinate co)
        {
            if (BM.isVisible(co) || BM.isFlagged(co)) return;
            BM.showSquare(co);
            if (!BM.isZero(co)) return;

            int[] around = { -1, 0, 1 };
            foreach(int x in around) foreach(int y in around)
            {
                Coordinate temp = new Coordinate(co.first + x, co.second + y);
                if (temp != new Coordinate(0,0) && 0 <= temp.first && temp.first < alto && 0 <= temp.second && temp.second < ancho)
                        mostrarCuadro(alto, ancho, ref BM, ref temp);
            }
        }
    }
}
