using System;
using System.Collections.Generic;

namespace Chess.BoardCheck
{
    class PieceReach
    {
        public void RookReach(int sourceY, int sourceX, int destinationY, int destinationX, bool turn, Dictionary<int, HashSet<int>> reach)
        {
            // these will determine which direction the rook is checking the king
            int diffY = destinationY - sourceY;
            int diffX = destinationX - sourceX;

            if (diffY == 0) // rook move east or west
            {
                if (diffX < 0) // west
                {
                    for (int i = sourceX; i > destinationX; i--) // as it is assumed that rook is always checking the king in that direction, it will check all squares towards the king's direction
                    {
                        if (!reach.ContainsKey(sourceY))
                            reach.Add(sourceY, new HashSet<int>());
                        reach[sourceY].Add(i);
                    }
                }
                else // east
                {
                    for (int i = sourceX; i < destinationX; i++)
                    {
                        if (!reach.ContainsKey(sourceY))
                            reach.Add(sourceY, new HashSet<int>());
                        reach[sourceY].Add(i);
                    }
                }
            }
            else if (diffX == 0) // rook move north or south
            {
                if (diffY < 0) // north
                {
                    for (int i = sourceY; i > destinationY; i--)
                    {
                        if (!reach.ContainsKey(i))
                            reach.Add(i, new HashSet<int>());
                        reach[i].Add(sourceX);
                    }
                }
                else // south
                {
                    for (int i = sourceY; i < destinationY; i++)
                    {
                        if (!reach.ContainsKey(i))
                            reach.Add(i, new HashSet<int>());
                        reach[i].Add(sourceX);
                    }
                }
            }
        }

        public void BishopReach(int sourceY, int sourceX, int destinationY, int destinationX, bool turn, Dictionary<int, HashSet<int>> reach)
        {
            // these will determine which direction the bishop is checking the king
            int diffY = destinationY - sourceY;
            int diffX = destinationX - sourceX;
            int limit = Math.Abs(diffY);

            if (diffY < 0) // bishop can move north-east or north-west
            {
                if (diffX < 0) // north-west
                {
                    for (int i = 1; i < limit; i++) // as it is assumed that bishop is always checking the king in that direction, it will check all squares towards the king's direction
                    {
                        if (!reach.ContainsKey(sourceY - i))
                            reach.Add(sourceY - i, new HashSet<int>());
                        reach[sourceY - i].Add(sourceX - i);
                    }
                }
                else // north-east
                {
                    for (int i = 1; i < limit; i++)
                    {
                        if (!reach.ContainsKey(sourceY - i))
                            reach.Add(sourceY - i, new HashSet<int>());
                        reach[sourceY - i].Add(sourceX + i);
                    }
                }
            }
            else // bishop can move south-east or south-west
            {
                if (diffX < 0) // south-west
                {
                    for (int i = 1; i < limit; i++)
                    {
                        if (!reach.ContainsKey(sourceY + i))
                            reach.Add(sourceY + i, new HashSet<int>());
                        reach[sourceY + i].Add(sourceX - i);
                    }
                }
                else // south-east
                {
                    for (int i = 1; i < limit; i++)
                    {
                        if (!reach.ContainsKey(sourceY + i))
                            reach.Add(sourceY + i, new HashSet<int>());
                        reach[sourceY + i].Add(sourceX + i);
                    }
                }
            }
        }

        public void QueenReach(int sourceY, int sourceX, int destinationY, int destinationX, bool turn, Dictionary<int, HashSet<int>> reach)
        {
            // these will determine which direction the queen is checking the king
            int diffY = destinationY - sourceY;
            int diffX = destinationX - sourceX;
            double diffYX = diffX != 0 ? ((double)diffY / (double)diffX) : 0;
            if (diffYX == 1 || diffYX == -1) // diagonal direction
            {
                int limit = Math.Abs(diffY);
                if (diffY < 0) // queen can move north-west or north-east
                {
                    if (diffX < 0) // north-west
                    {
                        for (int i = 1; i < limit; i++) // as it is assumed that queen is always checking the king in that direction, it will check all squares towards the king's direction
                        {
                            if (!reach.ContainsKey(sourceY - i))
                                reach.Add(sourceY - i, new HashSet<int>());
                            reach[sourceY - i].Add(sourceX - i);
                        }
                    }
                    else // north-east
                    {
                        for (int i = 1; i < limit; i++)
                        {
                            if (!reach.ContainsKey(sourceY - i))
                                reach.Add(sourceY - i, new HashSet<int>());
                            reach[sourceY - i].Add(sourceX + i);
                        }
                    }
                }
                else // queen can move south-west or south-east
                {
                    if (diffX < 0) // south-west
                    {
                        for (int i = 1; i < limit; i++)
                        {
                            if (!reach.ContainsKey(sourceY + i))
                                reach.Add(sourceY + i, new HashSet<int>());
                            reach[sourceY + i].Add(sourceX - i);
                        }
                    }
                    else // south-east
                    {
                        for (int i = 1; i < limit; i++)
                        {
                            if (!reach.ContainsKey(sourceY + i))
                                reach.Add(sourceY + i, new HashSet<int>());
                            reach[sourceY + i].Add(sourceX + i);
                        }
                    }
                }
            }
            else // horizontal/vertical direction
            {
                if (diffY == 0) // queen can move west or east
                {
                    if (diffX < 0) // west
                    {
                        for (int i = sourceX; i > destinationX; i--)
                        {
                            if (!reach.ContainsKey(sourceY))
                                reach.Add(sourceY, new HashSet<int>());
                            reach[sourceY].Add(i);
                        }
                    }
                    else // east
                    {
                        for (int i = sourceX; i < destinationX; i++)
                        {
                            if (!reach.ContainsKey(sourceY))
                                reach.Add(sourceY, new HashSet<int>());
                            reach[sourceY].Add(i);
                        }
                    }
                }
                else if (diffX == 0) // queen can move north or south
                {
                    if (diffY < 0) // north
                    {
                        for (int i = sourceY; i > destinationY; i--)
                        {
                            if (!reach.ContainsKey(i))
                                reach.Add(i, new HashSet<int>());
                            reach[i].Add(sourceX);
                        }
                    }
                    else // south
                    {
                        for (int i = sourceY; i < destinationY; i++)
                        {
                            if (!reach.ContainsKey(i))
                                reach.Add(i, new HashSet<int>());
                            reach[i].Add(sourceX);
                        }
                    }
                }
            }
        }
    }
}
