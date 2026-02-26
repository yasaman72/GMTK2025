using System;

namespace Deviloop
{
    [Flags]
    public enum F_MaterialType
    {
        Unknown = 1 << 0,
        Metal = 1 << 1,
        Stone = 1 << 2,
        Wood = 1 << 3,
        Fabric = 1 << 4,
        Flesh = 1 << 5,
        Glass = 1 << 6,
        Explosive = 1 << 7,
        Vegetation = 1 << 8,
        Paper = 1 << 9
    }

    public enum MaterialType
    {
        Unknown = F_MaterialType.Unknown,
        Metal = F_MaterialType.Metal,
        Stone = F_MaterialType.Stone,
        Wood = F_MaterialType.Wood,
        Fabric = F_MaterialType.Fabric,
        Flesh = F_MaterialType.Flesh,
        Glass = F_MaterialType.Glass,
        Explosive = F_MaterialType.Explosive,
        Vegetation = F_MaterialType.Vegetation,
        Paper = F_MaterialType.Paper,
    }
}
