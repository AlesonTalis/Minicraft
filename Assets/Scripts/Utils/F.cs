using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class F
    {
        #region LOOPS

        public static bool Loop<T>(this T[] _, int SIZE, ref int i)
        {
            if (i < SIZE)
            {
                i++;

                return true;
            }

            return false;
        }

        public static bool Loop<T>(this T[] _, int SIZE, ref int i, ref int x, ref int y)
        {
            if (y < SIZE && x < SIZE)
            {
                x++;
                i++;

                if (x == SIZE)
                {
                    x = 0;
                    y++;

                    if (y == SIZE)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public static void Loop<T>(this T[] array, int SIZE, Action<T[], int, int, int, int> loopAction)
        {
            for (int z = 0, i = 0; z < SIZE; z++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    for (int x = 0; x < SIZE; x++, i++)
                    {
                        loopAction(array, i, x, y, z);
                    }
                }
            }
        }

        public static void Loop<T>(this T[,,] array, Func<Loop,LoopResult?> loopAction, string name = null, LoopSettings? settings = null)
        {
            settings ??= new LoopSettings()
            {
                ignoreBorders = false
            };

            //int ignoreBorders = (settings.Value.ignoreBorders ? 1 : 0);
            int X = array.GetLength(0);// - ignoreBorders;
            int Y = array.GetLength(1);// - ignoreBorders;
            int Z = array.GetLength(2);// - ignoreBorders;

            DateTime inicio = DateTime.Now;

            for (int x = 0, i = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    for (int z = 0; z < Z; z++, i++)
                    {
                        bool center = (x > 0 && y > 0 && z > 0) &&
                            (x < X - 1 && y < Y - 1 && z < Z - 1);

                        if (settings.Value.ignoreBorders && center == false) continue;
                        
                        var result = loopAction(new Loop
                        {
                            x = x,
                            y = y,
                            z = z,

                            i = i,

                            border = !center,
                        });

                        if (result is not null && result.Value.callBreak)
                        {
                            x = y = int.MaxValue; 
                            
                            break;
                        }
                    }
                }
            }

            var intervalo = TimeSpan.FromTicks(DateTime.Now.Ticks - inicio.Ticks).TotalMilliseconds;

            if (name is not null)
            {
                Debug.Log($"3D_LOOP: \"{name}\" - {intervalo}ms");
            }
        }

        #endregion

        public static int GetNeighbors(this ushort[] array, int SIZE, int x, int y, int z)
        {
            int faces = 63;

            //if ((x > 0 && y > 0 && z > 0) && (x < SIZE - 1 && y < SIZE - 1 && z < SIZE - 1))
            //{
            int IND = CalcIndex(SIZE, x, y, z);

            int TOP = CalcIndex(SIZE, x, y + 1, z);
            int BOT = CalcIndex(SIZE, x, y - 1, z);

            int RIG = CalcIndex(SIZE, x + 1, y, z);
            int LEF = CalcIndex(SIZE, x - 1, y, z);

            int FOR = CalcIndex(SIZE, x, y, z + 1);
            int BAC = CalcIndex(SIZE, x, y, z - 1);

            if (y < SIZE - 1 && array[TOP] == array[IND]) faces -= 1;
            if (y > 0        && array[BOT] == array[IND]) faces -= 2;

            if (x < SIZE - 1 && array[RIG] == array[IND]) faces -= 4;
            if (x > 0        && array[LEF] == array[IND]) faces -= 8;

            if (z < SIZE - 1 && array[FOR] == array[IND]) faces -= 16;
            if (z > 0        && array[BAC] == array[IND]) faces -= 32;
            //}

            return faces;
        }

        public static int GetNeighbors(this ushort[,,] array, int x, int y, int z)
        {
            int faces = 63;
            
            int O = 0;

            int X = array.GetLength(0) - 1;
            int Y = array.GetLength(1) - 1;
            int Z = array.GetLength(2) - 1;

            if (y < Y && array[x, y + 1, z] == array[x, y, z]) faces -= 1;
            if (y > O && array[x, y - 1, z] == array[x, y, z]) faces -= 2;

            if (x < X && array[x + 1, y, z] == array[x, y, z]) faces -= 4;
            if (x > O && array[x - 1, y, z] == array[x, y, z]) faces -= 8;

            if (z < Z && array[x, y, z + 1] == array[x, y, z]) faces -= 16;
            if (z > O && array[x, y, z - 1] == array[x, y, z]) faces -= 32;

            return faces;
        }

        public static int CalcIndex(int SIZE, int x, int y, int z) => y * z * SIZE + x;
    }

    public struct Loop
    {
        public int x;
        public int y;
        public int z;

        public int i;

        public bool border;
    }

    public struct LoopResult
    {
        public bool callBreak;
    }

    public struct LoopSettings
    {
        public bool ignoreBorders;
    }
}
