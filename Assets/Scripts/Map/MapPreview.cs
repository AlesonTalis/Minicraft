using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Map
{
    public static class MapPreview
    {
        public static Texture2D GenerateTextureFromHeightMap(float[,] heightMap)
        {
            int width = heightMap.GetLength(0);
            int height = heightMap.GetLength(1);

            Texture2D texture = new Texture2D(width, height);
            Color32[] colors = new Color32[width * height];
            
            texture.filterMode = FilterMode.Point;

            heightMap.Loop((l) =>
            {
                int i = l.i;
                int x = l.x;
                int y = l.y;

                colors[i] = Color32.Lerp(Color.black, Color.white, heightMap[x, y]);

                return null;
            });


            texture.SetPixels32(colors);
            texture.Apply();

            return texture;
        }
    }
}
