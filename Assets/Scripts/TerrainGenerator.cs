
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SC4Parser.Logging;
using SC4Parser.Region;
using SC4Parser;
using UnityEditor;
using Logger = SC4Parser.Logging.Logger;

[RequireComponent(typeof(Terrain))]
[RequireComponent(typeof(TerrainCollider))]
public class TerrainGenerator : MonoBehaviour
{
    public Material TerrainMaterial;

    public SC4SaveFile SaveFile
    {
        get { return _loadedSaveFile; }
    }
    
    private SC4SaveFile _loadedSaveFile;
    
    private List<GameObject> _buildingObjects = new List<GameObject>();
    private Terrain _terrain = null;
    private TerrainCollider _terrainCollider = null;
    private TerrainData _terrainData = null;
    private float _buildingScale = 1f;
    private float _buildingOffset = 266f;
    // private float _terrainScale = 2f;
    private Vector3 _terrainScale = new Vector3(2f, 2f, 2f);
    
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

        LoadSave("C:\\Users\\Shadowfax\\Documents\\SimCity 4\\Regions\\London\\City - Interpol.sc4");
    }

    public void LoadSave(string pathToSave)
    {
        if (!File.Exists(pathToSave))
            return;

        _loadedSaveFile = new SC4SaveFile(pathToSave);

        Reload();
        
        if (_loadedSaveFile.ContainsTerrainMapSubfile() && _loadedSaveFile.ContainsRegionViewSubfile())
            GenerateTerrain(_loadedSaveFile.GetTerrainMapSubfile().Map, _loadedSaveFile.GetRegionViewSubfile().CitySizeX, _loadedSaveFile.GetRegionViewSubfile().CitySizeY);
        if (_loadedSaveFile.ContainsBuildingsSubfile())
            GenerateBuildings(_loadedSaveFile.GetBuildingSubfile().Buildings);
    }

    public void SetTerrainVisiblity(bool isVisible)
    {
        _terrain.enabled = isVisible;
    }

    public void SetBuildingVisiblity(bool isVisible)
    {
        foreach (GameObject building in _buildingObjects)
        {
            building.gameObject.SetActive(isVisible);
        }
    }

    public void SetBuildingOffset(float offset)
    {
        _buildingOffset = offset;
        Reload();
    }
    
    public void SetTerrainScale(Vector3 scale)
    {
        _terrainScale = scale;
        Reload();
    }

    public void Reload()
    {
        foreach (GameObject building in _buildingObjects)
        {
            GameObject.Destroy(building);
        }
        
        GenerateTerrain(_loadedSaveFile.GetTerrainMapSubfile().Map, _loadedSaveFile.GetRegionViewSubfile().CitySizeX, _loadedSaveFile.GetRegionViewSubfile().CitySizeY);
        GenerateBuildings(_loadedSaveFile.GetBuildingSubfile().Buildings);
    }

    public void SetBuildingScale(float scale)
    {
        _buildingScale = scale;
        Reload();
    }

    private void GenerateTerrain(float[][] heightMap, uint citySizeX, uint citySizeY)
    {
        // Reformat height map
        float[,] heightmap = new float[citySizeY, citySizeX];
        float[][] savegameHeightmap = heightMap;
        for (uint x = 0; x < citySizeX; x++)
        {
            for (uint y = 0; y < citySizeY; y++)
            {
                heightmap[y, x] = Map(savegameHeightmap[x][y], 120, 2500, 0, 1);
            }
        }

        //uint sizeX = (uint) region.Terrain.Length;
        //uint sizeY = (uint) region.Terrain[0].Length;
        //float[,] heightmap = new float[sizeY, sizeX];
        //float[][] savegameHeightmap = region.Terrain;
        //for (uint x = 0; x < sizeX; x++)
        //{
        //    for (uint y = 0; y < sizeY; y++)
        //    {
        //        heightmap[y, x] = Map(savegameHeightmap[x][y], 120, 2500, 0, 1);
        //    }
        //}

        // Add heightmap to terrain data
        _terrainData = new TerrainData();
        _terrainData.size = new Vector3(citySizeX * _terrainScale.x, citySizeX * _terrainScale.y /** x 4 */, citySizeX * _terrainScale.z);
        _terrainData.heightmapResolution = (int)citySizeX;
        _terrainData.baseMapResolution = (int)citySizeX;

        _terrainData.SetHeights(0, 0, heightmap);
        _terrainData.SyncHeightmap();

        // Add terrain data to terrain and setup
        _terrain.materialTemplate = TerrainMaterial;
        _terrain.terrainData = _terrainData;
        _terrainCollider.terrainData = _terrainData;

        _loaded = true;
    }

    private void GenerateBuildings(List<Building> buildings)
    {
        Vector3 rawPos = new Vector3();
        Vector3 rawSize = new Vector3();
        
        Quaternion Quat = Quaternion.Euler(new Vector3(0, 90, 0));
        foreach (Building building in buildings)
        {
            rawPos.y = building.MinCoordinateY;
            rawPos.x = building.MinCoordinateZ;
            rawPos.z = building.MinCoordinateX;
            
            rawSize.x = building.MaxCoordinateZ - building.MinCoordinateZ;
            rawSize.z = building.MaxCoordinateX - building.MinCoordinateX;
            rawSize.y = building.MaxCoordinateY - building.MinCoordinateY;
            
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // rawPos * Quat;
            Vector3 pos = _terrain.transform.position + (rawPos / _buildingScale);
            pos.y += rawSize.y / 2;
            pos.y -= _buildingOffset;
            
            cube.transform.position = pos;
            cube.transform.localScale = rawSize / _buildingScale;// / 2; // /8
            
            _buildingObjects.Add(cube);
        }
    }

    public static float Map(float value, float valueMin, float valueMax, float outMin, float outMax)
    {
        return (value - valueMin) / (valueMax - valueMin) * (outMax - outMin) + outMin;
    }

    private void OnDrawGizmos()
    {
        if (_terrain == null || _loadedSaveFile == null)
        {
            return;
        }
        
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(_terrain.transform.position, 30.0f);
        
        // Vector3 rawPos = new Vector3();
        // Vector3 rawSize = new Vector3();
        // foreach (Building building in _loadedSaveFile.GetBuildingSubfile().Buildings)
        // {
        //     rawPos.y = building.MinCoordinateY;
        //     rawPos.z = building.MinCoordinateZ;
        //     rawPos.x = building.MaxCoordinateX;
        //     
        //     rawSize.z = building.MaxCoordinateZ - building.MinCoordinateZ;
        //     rawSize.x = building.MaxCoordinateX - building.MinCoordinateX;
        //     rawSize.y = building.MaxCoordinateY - building.MinCoordinateY;
        //     
        //     Gizmos.DrawSphere(rawPos, 3.0f);
        // }
    }
}
