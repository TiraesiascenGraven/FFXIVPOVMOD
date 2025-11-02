using Dalamud.Configuration;
using System;

namespace SamplePlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsConfigWindowMovable { get; set; } = true;
    public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

    public float MinFovValue = 1;
    public bool TestVar1 { get; set; } = true;

    public static float OffsetX = 0;
    public static float OffsetY = 0;
    public static float OffsetZ = 0;

    public static float BoneToBind = 1;

    public static float InitOnlyOncePls = 0;









    // The below exist just to make saving less cumbersome
    public void Save()
    {
        Service.PluginInterface.SavePluginConfig(this);
    }
}
