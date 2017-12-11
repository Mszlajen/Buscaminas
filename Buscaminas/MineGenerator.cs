namespace Generator
{
    class Square {
        private bool bomb = false,
                     flag = false,
                     visible = false;
        private short bombs = 0;

        public char asChar () {
            if (flag) return 'F';
            else if (visible) return ' ';
            else if (bomb) return 'B';
            else return bombs.ToString()[0];
        }

        public bool isBomb() { return bomb; }

        public void makeBomb() { bomb = true; }

        public void changeFlag() { if(!visible) flag = !flag; }

        public void increment() { bombs++; }

        public void makeVisible() { if (!flag) visible = true; }

        public bool isRight() { return !(bomb ^ flag); }
    } // Fin Class Square

    class Coordinate 
    {
        public int first = 0, second = 0;

        public Coordinate (int f, int s) 
        {
            first = f;
            second = s;
        }

        public Coordinate () {}
    }
    class Table
    {
        private Square[,] table;
        private int area;

        public Table (int _heigth, int _width, int _bombs)
        {
            this.table = new Square[_heigth, _width];
            area = _heigth * _width;

            System.Random rnd = new System.Random();
            int firstRnd, secondRnd;
            int[] around = { -1, 0, 1};

            if (_bombs > area) // Limita la cantidad de bombas si se piden más de las posibles
                _bombs = area;

            for(int i = 0; i < _bombs; i++) // Genera las coordenadas de las bombas, las coloca en la tabla y las guarda en el vector BPosition
            {
                bool repetido;
                do 
                {
                    repetido = false;
                    firstRnd = rnd.Next(0, _heigth);
                    secondRnd = rnd.Next(0, _width);
                    if(this.table[firstRnd, secondRnd].isBomb())
                        repetido = true;
                } while (repetido); //Repite hasta que generar una coordenada que no este ya ocupada
                
                // Coloca la bomba
                this.table[firstRnd, secondRnd].makeBomb();
            }

            Coordinate curSqr;
            for (int hIndex = 0; hIndex < _heigth; hIndex++) //Colocación de numeros
            {
                for (int wIndex = 0; wIndex < _width; wIndex++) 
                {
                    if(table[hIndex, wIndex].isBomb())
                    {
                        foreach(int x in around) foreach (int y in around) 
                        {
                            curSqr = new Coordinate(wIndex + x, hIndex + y);
                            if(0 <= curSqr.second && curSqr.second <= _heigth && 0 <= curSqr.first && curSqr.first <= _width)
                            {
                                    table[curSqr.first, curSqr.second].increment();
                            }
                        }
                    }
                }
            }
        }

        public Square[,] getTable ()
        {
            return table;
        }

        public void changeFlag (Coordinate co) 
        {
            table[co.first, co.second].changeFlag();
        }

        public void showSquare (Coordinate co) 
        {
            table[co.first, co.second].makeVisible();
        }

        public bool wins()
        {
            bool wins = true;
            foreach (Square sqr in table)
                wins = wins && sqr.isRight();
            return wins;
        }
    } // Fin Class Table
} // Fin NameSpace Generador