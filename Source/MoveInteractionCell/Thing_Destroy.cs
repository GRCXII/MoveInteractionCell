using HarmonyLib;
using Verse;
using RimWorld; // 添加此命名空间以确保 Blueprint 和 Frame 可用

namespace MoveInteractionCell;

[HarmonyPatch(typeof(Thing), nameof(Thing.Destroy))]
public static class Thing_Destroy
{
    public static void Prefix(Thing __instance)
    {
        // 确保在建筑销毁时清理自定义交互点
        MoveInteractionCell.ResetOverrideCell(__instance);

        // 如果是蓝图或框架，清理 BlueprintDummy 的交互点
        if (__instance is Blueprint || __instance is Frame)
        {
            MoveInteractionCell.Overrides.Remove(MoveInteractionCell.BlueprintDummy);
        }
    }
}
