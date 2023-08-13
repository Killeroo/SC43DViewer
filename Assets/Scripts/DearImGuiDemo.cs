using UnityEngine;
using UImGui;
using ImGuiNET;

public class DearImGuiDemo : MonoBehaviour
{
    void OnEnable()
    {
        UImGuiUtility.Layout += OnLayout;
    }

    void OnDisable()
    {
        UImGuiUtility.Layout -= OnLayout;
    }

    Vector3 newLocation = Vector3.zero;

    void OnLayout(UImGui.UImGui obj)
    {
        //https://github.com/ocornut/imgui/blob/d6ea56dfd97ac5d7caab2208b11e0e12fa2377d9/imgui_demo.cpp
        //ImGui.Begin("My First Tool", ref _isOpen, ImGuiWindowFlags.MenuBar);
        //if (ImGui.BeginMenuBar())
        //{
        //    if (ImGui.BeginMenu("File"))
        //    {
        //        if (ImGui.MenuItem("Open..", "Ctrl+O")) { /* Do stuff */ }
        //        if (ImGui.MenuItem("Save", "Ctrl+S")) { /* Do stuff */ }
        //        if (ImGui.MenuItem("Close", "Ctrl+W")) { _isOpen = false; }
        //        ImGui.EndMenu();
        //    }
        //    ImGui.EndMenuBar();
        //}

        //// Edit a color (stored as ~4 floats)
        //ImGui.ColorEdit4("Color", ref _myColor);

        //// Plot some values
        //float[] my_values = new float[] { 0.2f, 0.1f, 1.0f, 0.5f, 0.9f, 2.2f };
        //ImGui.PlotLines("Frame Times", ref my_values[0], my_values.Length);

        //// Display contents in a scrolling region
        //ImGui.TextColored(new Vector4(1, 1, 0, 1), "Important Stuff");
        //ImGui.BeginChild("Scrolling");
        //for (int n = 0; n < 50; n++)
        //    ImGui.Text($"{n}: Some text");
        //ImGui.EndChild();
        //ImGui.End();

        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.None;
        windowFlags |= ImGuiWindowFlags.NoResize;

        ImGui.ShowDemoWindow();

        ImGui.Begin("SimCity4 3D Viewer", windowFlags);

        string testString = "Test";
        ImGui.InputText("Test TExt", ref testString, 20);
        float test = 2.2f;
        ImGui.InputFloat("text", ref test);

        ImGui.DragFloat3("Position", ref newLocation);
        ImGui.SameLine();
        if (ImGui.Button("Teleport"))
        {

        }

        if (ImGui.TreeNode("test"))
        {

        }

        ImGui.Text("kasnfklsnfklnsf");

        ImGui.End();


    }
}