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

            Color32[] colors = new Color32[width * height];

            heightMap.Loop((l) =>
            {
                int i = l.i;
                int x = l.x;
                int y = l.y;

                colors[i] = Color32.Lerp(Color.black, Color.white, heightMap[x, y]);

                return null;
            });

            var texture = GenerateTextureFromColorArray(width, height, colors);

            return texture;
        }

        public static Texture2D GenerateTextureForGradientColors(float[,] values, Gradient gradient)
        {
            int width = values.GetLength(0);
            int height = values.GetLength(1);

            Color32[] colors = new Color32[width * height];

            values.Loop((l) =>
            {
                int i = l.i;
                int x = l.x;
                int y = l.y;

                colors[i] = gradient.Evaluate(values[x, y]);

                return null;
            });

            var texture = GenerateTextureFromColorArray(width, height, colors);

            return texture;
        }

        static Texture2D GenerateTextureFromColorArray(int width, int height, Color32[] colors)
        {
            Texture2D texture = new Texture2D(width, height);

            texture.filterMode = FilterMode.Point;

            texture.SetPixels32(colors);
            texture.Apply();

            return texture;
        }
    }
}
