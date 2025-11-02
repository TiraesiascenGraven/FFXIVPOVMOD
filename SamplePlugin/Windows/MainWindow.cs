using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Lumina.Excel.Sheets;
using Serilog;
using static Dalamud.Interface.Utility.Raii.ImRaii;
using static FFXIVClientStructs.FFXIV.Client.UI.RaptureAtkHistory.Delegates;
using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Lumina.Excel.Sheets.Experimental;
using SamplePlugin;
using System.Runtime.InteropServices;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly string goatImagePath;
    private readonly Plugin plugin;

    public static float arrowOffset = 0.75f;


    // We give this window a hidden ID using ##.
    // The user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, string goatImagePath)
        : base("First Person POV Mod", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.goatImagePath = goatImagePath;
        this.plugin = plugin;
    }

    private static void ResetSliderFloat(string id, ref float val, float min, float max, float reset, string format)
    {
        var save = true;


        ///This is the Reset button
        ImGui.PushFont(UiBuilder.IconFont);
        if (ImGui.Button($"{FontAwesomeIcon.UndoAlt.ToIconString()}##{id}"))
        {
            val = reset;
            save = true;
            Service.Log.Information($"===UI===");
        }
        ImGui.PopFont();

        ///This is the Slider
        ImGui.SameLine();
        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 160 * ImGuiHelpers.GlobalScale);
        save |= ImGui.SliderFloat(id, ref val, min, max, format);

        if (!save) return;
        //Service.Log.Information($"==={val}===");
        //    if (CurrentPreset == PresetManager.CurrentPreset)
        //        CurrentPreset.Apply();
    }

    private static void ResetSliderInt(string id, ref float val, float min, float max, float reset, string format)
    {
        var save = true;


        ///This is the Reset button
        ImGui.PushFont(UiBuilder.IconFont);
        if (ImGui.Button($"{FontAwesomeIcon.CalendarMinus.ToIconString()}##{id}"))
        {
            val --;
            save = true;
            Service.Log.Information($"===UI===");
        }
        ImGui.PopFont();

        ///This is the Slider
        ImGui.SameLine();
        ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X - 160 * ImGuiHelpers.GlobalScale);
        val = (int)val;
        save |= ImGui.SliderFloat(id, ref val, min, max, format);

        ImGui.SameLine();
        ImGui.PushFont(UiBuilder.IconFont);
        if (ImGui.Button($"{FontAwesomeIcon.CalendarPlus.ToIconString()}##{id}"))
        {
            val ++;
            save = true;
            Service.Log.Information($"===UI===");
        }
        ImGui.PopFont();

        if (!save) return;
        //Service.Log.Information($"==={val}===");
        //    if (CurrentPreset == PresetManager.CurrentPreset)
        //        CurrentPreset.Apply();

    }

    public void Dispose() { }

    //TEST VARIABLES
    public bool MyTest { get; set; } = false;

    public override void Draw()
    {
        ImGui.TextUnformatted($"Camera Offset:");

        ImGui.Spacing();
        ResetSliderFloat("FOV", ref arrowOffset, 0, 5, 0.75f, "%.2f");

        ImGui.Spacing();
        ResetSliderFloat("X Offset (Forwards/Back)", ref Configuration.OffsetX, -1, 1, 0f, "%.2f");

        ImGui.Spacing();
        ResetSliderFloat("Y Offset (Up Down)", ref Configuration.OffsetY, -1, 1, 0f, "%.2f");

        ImGui.Spacing();
        ResetSliderFloat("Z Offset (Left Right)", ref Configuration.OffsetZ, -1, 1, 0f, "%.2f");

        ImGui.Spacing();


        ImGui.TextUnformatted($"\nCamera Bone:\n1-POV / 26 - Head");

        ImGui.Spacing();
        ResetSliderInt("", ref Configuration.BoneToBind, 0, 300, 1, "%1f");

        ImGui.TextUnformatted($"\nCamera Binding:\nCOMING SOON?");

        //camera->minFoV = 1;
        //camera->maxFoV = 2;


        // Button Test

        ImGui.Spacing();

        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        
    }
}
