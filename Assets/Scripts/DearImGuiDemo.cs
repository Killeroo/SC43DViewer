using UnityEngine;
using ImGuiNET;

public class DearImGuiDemo : MonoBehaviour
{
    void OnEnable()
    {
        ImGuiUn.Layout += OnLayout;
    }

    void OnDisable()
    {
        ImGuiUn.Layout -= OnLayout;
    }

    void OnLayout()
    {
        //https://github.com/ocornut/imgui/blob/d6ea56dfd97ac5d7caab2208b11e0e12fa2377d9/imgui_demo.cpp
        ImGui.ShowDemoWindow();

        //ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoResize;

        //ImGui.Begin("SC43DViewer", windowFlags);
        ////if (ImGui.BeginMenu("Menu"))
        ////{

        ////}
        //ImGui.Text("Welcome to SC43DViewer v1.0");
        //ImGui.Spacing();

        //ImGui.End();

    }
}