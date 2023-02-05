
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SC4Parser.Logging;
using SC4Parser.Region;
using SC4Parser;

using Logger = SC4Parser.Logging.Logger;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]
public class TerrainGenerator : MonoBehaviour
{

    public Material TerrainMaterial;

    private Terrain _terrain = null;
    private TerrainCollider _terrainCollider = null;

    private TerrainData _terrainData = null;
    private bool _loaded = false;

    void Start()
    {
        if(!gameObject.TryGetComponent(out _terrain))
        {
            Debug.LogError("[TerrainGenerator] Could not find Terrain component");
        }
        if (!gameObject.TryGetComponent(out _terrainCollider))
        {
            Debug.LogError("[TerrainGenerator] Could not find TerrainCollider component");
        }

        Debug.Assert(TerrainMaterial != null, "[TerrainGenerator] Terrain material cannot be null");

        LoadMap();
    }

    private void LoadMap()
    {
        _terrainData = new TerrainData();
        RegionViewSubfile regionView = null;
        TerrainMapSubfile terrainMap = null;

        Region region = new Region();
        region.Load(@"C:\Users\Shadowfax\Documents\SimCity 4\Regions\London");

        // Get data from save
        using (var save = new SC4SaveFile(@"C:\Users\Shadowfax\Documents\SimCity 4\Regions\London\City - Kensington.sc4"))
        {
            terrainMap = save.GetTerrainMapSubfile();
            regionView = save.GetRegionViewSubfile();
        }

        // Reformat height map
        //uint sizeX = regionView.CitySizeX;
        //uint sizeY = regionView.CitySizeY;
        //float[,] heightmap = new float[sizeY, sizeX];
        //float[][] savegameHeightmap = terrainMap.Map;
        //for (uint x = 0; x < sizeX; x++)
        //{
        //    for (uint y = 0; y < sizeY; y++)
        //    {
        //        heightmap[y, x] = Map(savegameHeightmap[x][y], 120, 2500, 0, 1);
        //    }
        //}

        uint sizeX = (uint) region.Terrain.Length;
        uint sizeY = (uint) region.Terrain[0].Length;
        float[,] heightmap = new float[sizeY, sizeX];
        float[][] savegameHeightmap = region.Terrain;
        for (uint x = 0; x < sizeX; x++)
        {
            for (uint y = 0; y < sizeY; y++)
            {
                heightmap[y, x] = Map(savegameHeightmap[x][y], 120, 2500, 0, 1);
            }
        }

        // Add heightmap to terrain data
        _terrainData.size = new Vector3(sizeX, sizeX * 4, sizeX);
        _terrainData.heightmapResolution = (int)sizeX;
        _terrainData.baseMapResolution = (int)sizeX;

        _terrainData.SetHeights(0, 0, heightmap);
        _terrainData.SyncHeightmap();

        // Add terrain data to terrain and setup
        _terrain.materialTemplate = TerrainMaterial;
        _terrain.terrainData = _terrainData;
        _terrainCollider.terrainData = _terrainData;

        _loaded = true;
    }

    public static float Map(float value, float valueMin, float valueMax, float outMin, float outMax)
    {
        return (value - valueMin) / (valueMax - valueMin) * (outMax - outMin) + outMin;
    }
}
