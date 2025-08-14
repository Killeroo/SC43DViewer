using System.IO;
using UnityEngine;
using UImGui;
using ImGuiNET;
using SC4Parser;

public class DearImGuiDemo : MonoBehaviour
{
    public GameObject TerrainObject;
    
    private TerrainGenerator _terrainGenerator;
    
    private static string SaveFileInput = "C:\\Users\\Shadowfax\\Documents\\SimCity 4\\Regions\\London\\City - Interpol.sc4";
    private static float ScaleInput = 1;
    private static float OffsetInput = 0;
    private static Vector3 TerrainScale = new Vector3(2f, 2f, 2f);
    
    void OnEnable()
    {
        UImGuiUtility.Layout += OnLayout;
    }

    void OnDisable()
    {
        UImGuiUtility.Layout -= OnLayout;
    }
    
    void OnLayout(UImGui.UImGui obj)
    {
        if (TerrainObject != null)
        {
            if (_terrainGenerator == null)
            {
                _terrainGenerator = TerrainObject.GetComponent<TerrainGenerator>();
            }
        }

        if (_terrainGenerator == null)
        {
            return;
        }

        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.None;
        windowFlags |= ImGuiWindowFlags.NoResize;
        
        ImGui.Begin("SimCity4 3D Viewer", windowFlags);

        ImGui.AlignTextToFramePadding();
        ImGui.Text("City");
        ImGui.SameLine();
        ImGui.InputText("", ref SaveFileInput, 256);
        ImGui.SameLine();
        if (ImGui.Button("Load"))
        {
            if (File.Exists(SaveFileInput))
            {
                _terrainGenerator.LoadSave(SaveFileInput);
            }
        }

        if (ImGui.Button("Clear"))
        {
            _terrainGenerator.Reload();
        }

        if (ImGui.Button("Show Terrain"))
        {
            _terrainGenerator.SetTerrainVisiblity(true);
        }
        ImGui.SameLine();
        if (ImGui.Button("Hide Terrain"))
        {
            _terrainGenerator.SetTerrainVisiblity(false);
        }
        
        if (ImGui.Button("Show Buildings"))
        {
            _terrainGenerator.SetBuildingVisiblity(true);
        }
        ImGui.SameLine();
        if (ImGui.Button("Hide Buildings"))
        {
            _terrainGenerator.SetBuildingVisiblity(false);
        }
        
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Building Scale");
        ImGui.SameLine();
        ImGui.PushID("BuildingScale");
        if (ImGui.SliderFloat("", ref ScaleInput, 1, 10))
        {
            _terrainGenerator.SetBuildingScale(ScaleInput);
        }
        ImGui.PopID();
        
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Building Offset");
        ImGui.SameLine();
        ImGui.PushID("BuildingOffset");
        if (ImGui.SliderFloat("", ref OffsetInput, 1, 1000))
        {
            _terrainGenerator.SetBuildingOffset(OffsetInput);
        }
        ImGui.PopID();
        
        ImGui.AlignTextToFramePadding();
        ImGui.Text("Terrain Scale");
        ImGui.SameLine();
        ImGui.PushID("TerrainScale");
        if (ImGui.InputFloat3("", ref TerrainScale))
        {
            _terrainGenerator.SetTerrainScale(TerrainScale);
        }
        ImGui.PopID();
        
        ImGui.End();

        // if (TerrainObject != null)
        // {
        //     if (_terrainGenerator == null)
        //     {
        //         _terrainGenerator = TerrainObject.GetComponent<TerrainGenerator>();
        //         if (_terrainGenerator == null) return;
        //     }
        //
        //     SC4SaveFile save = _terrainGenerator.SaveFile;
        //     if (save.ContainsBuildingsSubfile())
        //     {
        //         foreach (Building building in save.GetBuildingSubfile().Buildings)
        //         {
        //             ImGui.Text(
        //                 $"{building.TGI.ToString()} - x: {building.MinCoordinateX} y: {building.MinCoordinateY} z: {building.MinCoordinateZ}");
        //         }
        //     }
        //
        //
        //
        // }

    }
}