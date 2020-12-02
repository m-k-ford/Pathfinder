using System;
using System.Collections.Generic;
using System.Threading;

namespace Pathfinder
{
    public class Cartographer<T> where T : ICartographer<T>
    {
        //private readonly int[] DEFAULT = new int[] { 0, 3, 5, 6, 7, 4, 2, 1 };
        private readonly int[] DEFAULT = new int[] { 1, 2, 4, 7, 6, 5, 3, 0 };
        private ArrayList<T> _borderSet;
        private T[,] _grid;
        private ArrayList<T> _renderSet;

        Point _lastBorderPosition = Point.Nil;

        private int[] GetRotatedArray(Direction start)
        {
            int[] rotatedArray = new int[DEFAULT.Length];
            Array.Copy(DEFAULT, rotatedArray, DEFAULT.Length);

            int rotations = Array.IndexOf(DEFAULT, (int)start);
            for (int i = 0; i < rotations; i++)
            {
                int temp = rotatedArray[0];
                for (int j = 0; j < (rotatedArray.Length - 1); j++)
                {
                    rotatedArray[j] = rotatedArray[j + 1];
                }
                rotatedArray[rotatedArray.Length - 1] = temp;
            }
            return rotatedArray;
        }

        private int[] GetReversedArray(int[] arrayToReverse)
        {
            int[] temp = new int[arrayToReverse.Length];
            int j = arrayToReverse.Length;
            for (int i = 0; i < arrayToReverse.Length; i++)
            {
                j--;
                temp[i] = arrayToReverse[j];
            }
            return temp;
        }

        private bool GetStart(out T start)
        {
            for (int y = 0; y < _grid.GetLength(1); y++)
            {
                for (int x = 0; x < _grid.GetLength(0); x++)
                {
                    if (_grid[x, y].Type != NodeType.Normal)
                    {
                        start = _grid[x, y];
                        return true;
                    }
                    #region Rendering
                    _grid[x, y].Draw(ConsoleColor.Magenta);
                    Thread.Sleep(16);
                    #endregion
                }
            }
            start = default;
            return false;
        }


        private void NewMapObstacle(T current)
        {
            HashSet<T> obstacleSet = new HashSet<T>();
            // for a new obstacle node to be valid, it must be a neighbour to one of the explored obstacle nodes




        }

        private bool MapObstacle(T current, int[] rotation)
        {

            // try move right
            // if not move right, next try down
            // if move down, next try right
            // if move right, next try up
            // if move up, next try left
            // if move left, next try down


            // H cost
            // adjust H cost based on last move direction

            // highest value is best move option
            // neighbours are sorted and categorised based on if they're adjacent an obstacle
            // if a node is not adjacent an obstacle it is not put in the open set
            // 




            int length = current.Neighbours.Length;
            int i = 0;

            Point pos = current.Position;
            if (pos.X >= 0 && pos.X < _grid.GetLength(0) && pos.Y >= 0 && pos.Y < _grid.GetLength(1))
            {
                _renderSet.Add(current);
                //current.Draw(ConsoleColor.Yellow);
            }

            // skip obstacles and border nodes so the search can start on an empty node
            while (current.Neighbours[rotation[i]].Type != NodeType.Normal || _borderSet.Contains(current.Neighbours[rotation[i]]))
            {
                i++;
            }

            for (; i < length; i++)
            {
                #region Rendering      
                //Console.ReadKey();
                //Thread.Sleep(16);
                pos = current.Neighbours[rotation[i]].Position;
                if (pos.X >= 0 && pos.X < _grid.GetLength(0) && pos.Y >= 0 && pos.Y < _grid.GetLength(1))
                {
                    if (current.Neighbours[rotation[i]].Type != NodeType.Normal != true && _borderSet.Contains(current.Neighbours[rotation[i]]) != true)
                    {
                        _renderSet.Add(current.Neighbours[rotation[i]]);
                        //current.Neighbours[rotation[i]].Draw(ConsoleColor.Magenta);
                    }
                }
                #endregion

                // loop through the empty nodes until we find an obstacle node
                if (current.Neighbours[rotation[i]].Type != NodeType.Normal)
                {

                    //int xDistance = Math.Abs(_lastBorderPosition.X - current.Neighbours[rotation[i]].Position.X);
                    //int yDistance = Math.Abs(_lastBorderPosition.Y - current.Neighbours[rotation[i]].Position.Y);

                    //if(xDistance > 1 || yDistance > 1)
                    //{
                    //    continue;
                    //}

                    //_lastBorderPosition = current.Neighbours[rotation[i]].Position;


                    // set the indexOfBorder to the last empty node
                    // if the current node is obstacle, the only possibility is the last node was empty
                    int indexOfBorder = (i - 1);
                    if (i < 0) { i = (length - 1); }

                    // if the new border node is the same as the first border node, then the border is complete
                    // the obstacle has been explored
                    if (current.Neighbours[rotation[indexOfBorder]].Equals(_borderSet[0]))
                    {
                        return true;
                    }

                    //current.Neighbours[rotation[indexOfBorder]].Draw(ConsoleColor.Yellow);

                    // if the border is not complete yet, add the new border tile to the borderSet
                    _borderSet.Add(current.Neighbours[rotation[indexOfBorder]]);

                    // get the index of the current border node in the neighbours array of the new border node
                    // use this index to determine the direction from current to new
                    // get an array that starts with the current node          

                    int nextRotationStartIndex = 0;
                    T newBorderNode = current.Neighbours[rotation[indexOfBorder]];
                    for (int l = 0; l < length; l++)
                    {
                        if (newBorderNode.Neighbours[l].Position == current.Position)
                        {
                            nextRotationStartIndex = l;
                            break;
                        }
                    }
                    return MapObstacle(current.Neighbours[rotation[indexOfBorder]], GetRotatedArray((Direction)nextRotationStartIndex));
                }
            }
            return false;
        }

        public void GetCartogram(T[,] grid)
        {
            T start = default;
            _grid = grid;
            _borderSet = new ArrayList<T>(_grid.Length);
            _renderSet = new ArrayList<T>(_grid.Length);

            if (GetStart(out start))
            {
                Direction dir = (start.Position.X > 0) ? Direction.West : Direction.North;
                T next = start.Neighbours[(int)dir];
                _borderSet.Add(next);

                _lastBorderPosition = start.Position;



                //NewMapObstacle(next);

                //if (MapObstacle(next, GetRotatedArray(dir)))
                //{
                //    for (int i = 0; i < _renderSet.Length; i++)
                //    {
                //        ConsoleColor colour = (_borderSet.Contains(_renderSet[i]) ? ConsoleColor.Yellow : ConsoleColor.Magenta);
                //        _renderSet[i].Draw(colour);
                //        Thread.Sleep(16);
                //    }
                //}
            }
        }
    }

    public interface ICartographer<T>
    {
        Point Position { get; }
        T[] Neighbours { get; }
        NodeType Type { get; }
        int H { get; set; }
        int F { get; }
        void Draw(ConsoleColor colour);
    }
}
