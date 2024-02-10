using Assets.Scripts.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.CE
{
    internal static class ItemSettingsCE
    {
        public static int GetFace(this ItemSettings item, int face)
        {
            int count = item.itemFaces.Length;

            return count switch
            {
                1 => item.itemFaces[0],
                2 => face == 0 || face == 1 ? item.itemFaces[0] : item.itemFaces[1],
                3 => face switch
                {
                    0 or 1 => item.itemFaces[0],
                    2 or 3 => item.itemFaces[1],
                    _ => item.itemFaces[2],
                },
                _ => item.itemFaces[face],
            };
        }
    }
}
