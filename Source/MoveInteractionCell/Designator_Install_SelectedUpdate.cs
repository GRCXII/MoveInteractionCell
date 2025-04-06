using HarmonyLib;
using RimWorld;
using Verse;

namespace MoveInteractionCell;

[HarmonyPatch(typeof(Designator_Install), nameof(Designator_Install.SelectedUpdate))]
public static class Designator_Install_SelectedUpdate
{
    public static void Prefix(Rot4 ___placingRot)
    {
        var selectedItem = MoveInteractionCell.GetSelectedItem();

        if (selectedItem == null)
        {
            return;
        }

        // 确保新蓝图的交互点不受之前建筑的影响
        var cellTracker = Current.Game.GetComponent<GameComponent_InteractionCellTracker>();
        if (cellTracker != null && cellTracker.CustomInteractionCells.TryGetValue(selectedItem, out var customOffset))
        {
            MoveInteractionCell.Overrides[MoveInteractionCell.BlueprintDummy] = new OverrideInfo(selectedItem, customOffset)
            {
                ThingCenter = UI.MouseCell(),
                Rotation = ___placingRot
            };
        }
        else if (!MoveInteractionCell.SetOverride(selectedItem, ___placingRot, true))
        {
            return;
        }

        MoveInteractionCell.Overrides[MoveInteractionCell.BlueprintDummy].ThingCenter = UI.MouseCell();
        MoveInteractionCell.Overrides[MoveInteractionCell.BlueprintDummy].Rotation = ___placingRot;
    }
}
