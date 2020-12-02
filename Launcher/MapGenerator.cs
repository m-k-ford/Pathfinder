using System;

namespace Launcher
{
    public class MapGenerator
    {
        private int[,] _map;
        private int _width;
        private int _height;
        //private string _seed;

        public int this [int x, int y] { get { return _map[x, y]; } }
        public int[,] Map { get { return _map; } }
        //public string Seed { get { return _seed; } set { _seed = value; } } 

        public void GenerateMap(int width, int height, int fillPercent)
        {
            _width = width;
            _height = height;
            _map = new int[_width, _height];
            RandomFillMap(fillPercent);
            //for (int i = 0; i < 5; i++)
            //{
            //    SmoothMap();
            //}
        }

        public void RandomFillMap(int fillPercent)
        {
            Random random = new Random(DateTime.Now.ToString().GetHashCode());
            int value = 0;

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                    {
                        value = 1;
                    }
                    else
                    {
                        value = random.Next(0, 100) < fillPercent ? 1 : 0;
                    }
                    _map[x, y] = value;
                }
            }
        }

        public void RandomFillMap(int fillpercent, string seed)
        {
            Random random = new Random(seed.GetHashCode());
            int value = 0;

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                    {
                        value = 1;
                    }
                    else
                    {
                        value = random.Next(0, 100) < fillpercent ? 1 : 0;
                    }
                    _map[x, y] = value;
                }
            }
        }

        private void SmoothMap()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                    {
                        _map[x, y] = 1;
                    }
                    else if (neighbourWallTiles < 4)
                    {
                        Map[x, y] = 0;
                    }
                }
            }
        }

        private int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = -1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = -1; neighbourY <= gridY + 1; neighbourY++)
                {
                    // if the index is inside the 2D array
                    if (neighbourX >= 0 && neighbourX < _width && neighbourY >= 0 && neighbourY < _height)
                    {
                        // if the index is not the "current" node
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            // add the value of the the node to the wallcount
                            wallCount += _map[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        // else add 1 to the wallcount
                        wallCount++;
                    }
                }
            }
            return wallCount;
        }
    }
}
