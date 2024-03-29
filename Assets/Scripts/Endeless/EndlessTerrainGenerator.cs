﻿using Assets.Scripts.CE;
using Assets.Scripts.Concurents;
using Assets.Scripts.Gen;
using Assets.Scripts.Model;
using Assets.Scripts.Scriptables;
using Assets.Scripts.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Endless
{
    public class EndlessTerrainGenerator : MonoBehaviour
    {
        [SerializeField]
        private int m_ThreadAmount = 4;
        [SerializeField]
        private int m_ChunkAmountTeste = 1;
        [SerializeField]
        private int m_ThreadSleepMiliseconds = 200;

        [SerializeField]
        private float m_GlobalScale = 1f;

        [SerializeField]
        private Material m_TerrainMaterial;
        [SerializeField]
        private TextAsset m_BiomesSettings;
        [SerializeField]
        private TextAsset m_BlockPallet;
        [SerializeField]
        private string seed;

        [Space]
        [SerializeField]
        private bool debug = false;

        private WorldSettings worldSettings;

        private ConcurrentQueue<Vector2Int> chunksGenerateQueue = new();
        private ConcurrentQueue<ChunkData> chunkDataGenerateQueue = new();
        private ConcurrentDictionary<Vector2Int, RegionData> regionDataDictionary = new();

        private BiomeSetting[] biomeSettings;
        private ItemSettings[] itemSettings;

        private bool runningThread = false;

        void Start()
        {
            LoadBiomeSettings();
            LoadItemSettings();

            worldSettings = WorldSettingsCE.Init(seed, biomeSettings, itemSettings, m_GlobalScale);

            ThreadInit();
        }

        void LateUpdate()
        {
            if (chunkDataGenerateQueue.TryDequeue(out ChunkData chunkData) == false) {
                return;
            }

            GameObject dad = new GameObject($"CHUNK_{chunkData.chunkUID}_{chunkData.GetPosition()}");
            dad.transform.position = chunkData.GetPosition();
            dad.transform.parent = transform;

            if (debug) GUIUtility.systemCopyBuffer = chunkData.debugData;// debug only

            for (int i = 0; i < chunkData.subChunks.Length; i++)
            {
                var pos = new Vector3(0, i * SubChunk.CHUNK_SIZE, 0);
                GameObject child = new GameObject($"SUBCHUNK_{i}");

                child.transform.localPosition = pos;
                child.transform.SetParent(dad.transform, false);

                var renderer = child.AddComponent<MeshRenderer>();
                var filter = child.AddComponent<MeshFilter>();

                var mesh = chunkData.subChunks[i].GetMeshData();

                filter.sharedMesh = mesh;
                renderer.sharedMaterial = m_TerrainMaterial;
            }
        }

        #region Threads

        void ThreadInit()
        {
            int[,] megaLoop = new int[m_ChunkAmountTeste, m_ChunkAmountTeste];

            megaLoop.Loop((l) =>
            {
                chunksGenerateQueue.Enqueue(new Vector2Int(l.x, l.y));

                return null;
            });

            runningThread = true;

            ThreadLoop();


            Application.quitting += Application_quitting;
        }


        private void Application_quitting()
        {
            runningThread = false;
        }

        void ThreadLoop()
        {
            for (int i = 0; i < m_ThreadAmount; i++)
            {
                ThreadStart thread = delegate
                {
                    ThreadAction(i);
                };

                new Thread(thread).Start();
            }
        }

        void ThreadAction(int threadId)
        {
            var rnd = new System.Random();
            var frm = $" - from THREAD_{threadId}_{rnd.Next(0, 1000)}";

            while (runningThread)
            {
                if (chunksGenerateQueue.TryDequeue(out Vector2Int chunkPosition) == false)
                {
                    Thread.Sleep(m_ThreadSleepMiliseconds);
                    continue;
                }

                var init = DateTime.Now;

                ChunkGenerationLogic(chunkPosition);

                var end = TimeSpan.FromTicks(DateTime.Now.Ticks - init.Ticks).TotalMilliseconds;

                Thread.Sleep(m_ThreadSleepMiliseconds);
            }

            Debug.Log($"Quitting with {chunksGenerateQueue.Count} items on queue {frm}");
        }

        #endregion

        void LoadBiomeSettings()
        {
            biomeSettings = JsonConvert.DeserializeObject<BiomeSetting[]>(m_BiomesSettings.text);
            biomeSettings.FillDictionary();
        }

        void LoadItemSettings()
        {
            itemSettings = JsonConvert.DeserializeObject<ItemSettings[]>(m_BlockPallet.text);
            itemSettings.FillItemsDictionary();
        }

        void ChunkGenerationLogic(Vector2Int chunkPosition)
        {
            ChunkData chunkData = new ChunkData();

            chunkData.GenerateChunkData(chunkPosition * SubChunk.CHUNK_SIZE, worldSettings, debug);

            chunkDataGenerateQueue.Enqueue(chunkData);
        }
    }
}