using Assets.Scripts.CE;
using Assets.Scripts.Concurents;
using Assets.Scripts.Model;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Gen
{
    public static class Decoration
    {
        struct ChangeBlocks
        {
            public int x, y, z;
            public ushort block;
        }

        public static void FillBiomeBlocks(this ChunkData chunk)
        {
            // comeca com tudo sendo stone. substitui a camada superior pelo bloco do bioma. grass e dirt por padrao, ou o configurado em BiomeSetting dictionary (se nao existe a configuracao)
            List<ChangeBlocks> blocks = new List<ChangeBlocks>();

            chunk.BlockColumnLoop((blk) =>
            {
                int biomeId = chunk.biomeMapData[blk.x, blk.z];
                int crtHeight = blk.h;
                
                if (BiomeSettingsDictionary.biomes.TryGetValue(biomeId, out var biome) is false) return;

                System.Random rnd = new (chunk.chunkSeed);

                foreach (var rule in biome.SurfaceFiller)
                {
                    var block = rule.Key;
                    var range = rule.Value;
                    int amount = 1;

                    if (range.Length > 1)
                    {
                        amount = rnd.Next(range[0], range[1]);
                    }

                    for (int i = 0; i < amount; i++)
                    {
                        blocks.Add(new ChangeBlocks() { x = blk.x, y = crtHeight, z = blk.z, block = block });

                        crtHeight--;
                    }
                }
            });

            blocks.ForEach(block =>
            {
                chunk.SetBlock(block.x, block.y, block.z, block.block);
            });
        }

        public static void BlockColumnLoop(this ChunkData chunk, Action<ColumnData> action)
        {
            chunk.biomeMapData.Loop((l) =>
            {
                int height = chunk.subChunks.GetHeight(l.x, l.y);

                action(new ColumnData
                {
                    x = l.x,
                    z = l.y,
                    h = height,
                });

                return null;
            });
        }

        public static int GetHeight(this SubChunkData[] subchunks, int x, int z)
        {
            for (int i = subchunks.Length - 1; i >= 0; i--)
            {
                for (int blockHeight = SubChunk.CHUNK_SIZE+1; blockHeight >= 0; blockHeight--)
                {
                    if (subchunks[i].BlockArray[x,blockHeight,z] != Chunk.AIR_BLOCK)
                    {
                        return blockHeight + (i * SubChunk.CHUNK_SIZE);
                    }
                }
            }

            return 0;
        }

        public struct ColumnData
        {
            public int x, z;

            public int h;
        }
    }
}
