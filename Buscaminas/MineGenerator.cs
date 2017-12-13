namespace Generator
{
    class Square {
        private bool bomb = false,
                     flag = false,
                     visible = false;
        private short bombs = 0;

        public char asChar () {
            if (flag) return 'F';
            else if (!visible) return ' ';
            else if (bomb) return 'B';
            else return bombs.ToString()[0];
        }
        public char asCharDebug () {
            if (bomb) return 'B';
            else return bombs.ToString()[0];
        }

        public bool isBomb() { return bomb; }

        public void makeBomb() { bomb = true; }

        public void changeFlag() { if(!visible) flag = !flag; }

        public void increment() { bombs++; }

        public void makeVisible() { if (!flag) visible = true; }

        public bool isRight() { return !(bomb ^ flag); }

        public bool isZero() { return bombs == 0; }

        public bool isVisible() { return visible; }

        public bool isFlagged() { return flag; }
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

        public static bool operator == (Coordinate co1, Coordinate co2)
        {
            return co1.first == co2.first && co1.second == co2.second;
        }

        public static bool operator != (Coordinate co1, Coordinate co2)
        {
            return !(co1 == co2);
        }

        public static Coordinate operator + (Coordinate co1, Coordinate co2)
        {
            return new Coordinate(co1.first + co2.first, co1.second + co2.second);
        }

    }
    class Table
    {
        private Square[,] table;
        private int area;

        public Table (int _heigth, int _width, int _bombs)
        {
            this.initializeTable(_heigth, _width);
            area = _heigth * _width;

            this.putBombs(ref _heigth, ref _width, ref _bombs);

            this.putNumbers(ref _heigth, ref _width); 
        }

        private void initializeTable (int _heigth, int _width) {
            this.table = new Square[_heigth, _width];
            for (int i = 0; i < _heigth; i++) for (int j = 0; j < _width; j++)
            {
                    table[i, j] = new Square();
            }
        }

        private void putBombs (ref int _heigth, ref int _width, ref int _bombs)
        {
            System.Random rnd = new System.Random();
			int firstRnd, secondRnd;

			if (_bombs > area) // Limita la cantidad de bombas si se piden más de las posibles
				_bombs = area;

			for (int i = 0; i < _bombs; i++) // Genera las coordenadas de las bombas, las coloca en la tabla y las guarda en el vector BPosition
			{
				bool repetido;
				do
				{
					repetido = false;
					firstRnd = rnd.Next(0, _heigth);
					secondRnd = rnd.Next(0, _width);
					if (this.table[firstRnd, secondRnd].isBomb())
						repetido = true;
				} while (repetido); //Repite hasta que generar una coordenada que no este ya ocupada

				// Coloca la bomba
				this.table[firstRnd, secondRnd].makeBomb();
			}
        }

        private void putNumbers (ref int _heigth, ref int _width)
        {
			Coordinate curSqr;
			int[] around = { -1, 0, 1 };
			for (int hIndex = 0; hIndex < _heigth; hIndex++) //Colocación de numeros
			{
				for (int wIndex = 0; wIndex < _width; wIndex++)
				{
					if (table[hIndex, wIndex].isBomb())
					{
						foreach (int x in around) foreach (int y in around)
						{
                            curSqr = new Coordinate(hIndex + x, wIndex + y);
                            if (0 <= curSqr.second && curSqr.second < _width && 0 <= curSqr.first && curSqr.first < _heigth)
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
            bool win = true;
            foreach (Square sqr in table)
                win = win && sqr.isRight();
            return win;
        }

        public bool isBomb (Coordinate co)
        {
            return table[co.first, co.second].isBomb();
        }

        public bool isZero (Coordinate co)
        {
            return table[co.first, co.second].isZero();
        }

        public bool isVisible (Coordinate co)
        {
            return table[co.first, co.second].isVisible();
        }

        public bool isFlagged (Coordinate co)
        {
            return table[co.first, co.second].isFlagged();
        }
        public void showAll () 
        {
            foreach (Square sqr in table)
                sqr.makeVisible();
        }
    } // Fin Class Table
} // Fin NameSpace Generador