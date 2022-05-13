using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SC4Parser.Files;
using SC4Parser.Subfiles;

[RequireComponent(typeof(Terrain))]
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
            Debug.Assert(false, "[TerrainGenerator] Could not find Terrain component");
        }
        if (!gameObject.TryGetComponent(out _terrainCollider))
        {
            Debug.Assert(false, "[TerrainGenerator] Could not find TerrainCollider component");
        }

        Debug.Assert(TerrainMaterial != null, "[TerrainGenerator] Terrain material cannot be null");

    }

    void Update()
    {
        if (!_loaded)
        {
            _terrainData = new TerrainData();
            RegionViewSubfile regionView = null;
            TerrainMapSubfile terrainMap = null;

            // Get data from save
            using (var save = new SC4SaveFile(@"C:\Users\Shadowfax\Documents\SimCity 4\Regions\London\City - Kensington.sc4"))
            {
                terrainMap = save.GetTerrainMapSubfile();
                regionView = save.GetRegionViewSubfile();
            }

            // Reformat height map
            uint sizeX = regionView.CitySizeX;
            uint sizeY = regionView.CitySizeY;
            float[,] heightmap = new float[sizeY, sizeX];
            float[][] savegameHeightmap = terrainMap.Map;
            for (uint x = 0; x < sizeX; x++)
            {
                for (uint y = 0; y < sizeY; y++)
                {
                    heightmap[y, x] = Map(savegameHeightmap[x][y], 120, 2500, 0, 1);
                }
            }

            // Add heightmap to terrain data
            _terrainData.size = new Vector3(sizeX, sizeX*4, sizeX);
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
    }

    public static float Map(float value, float valueMin, float valueMax, float outMin, float outMax)
    {
        return (value - valueMin) / (valueMax - valueMin) * (outMax - outMin) + outMin;
    }
}
