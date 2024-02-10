using Assets.Scripts.Gen;
using Assets.Scripts.Scriptables;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.CE
{
    internal static class V2ICE
    {
        public static Vector2Int GetVector2Int(this V2I item) => new Vector2Int(item.x, item.y);
    }
}
