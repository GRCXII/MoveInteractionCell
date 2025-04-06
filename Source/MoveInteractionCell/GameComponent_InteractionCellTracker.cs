using System.Collections.Generic;
using Verse;

namespace MoveInteractionCell;

public class GameComponent_InteractionCellTracker : GameComponent
{
	public Dictionary<Thing, IntVec3> CustomInteractionCells = new Dictionary<Thing, IntVec3>();

	private List<Thing> customInteractionCellsKeys;

	private List<IntVec3> customInteractionCellsValues;

	public GameComponent_InteractionCellTracker(Game game)
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			List<Thing> list = new List<Thing>();
			foreach (KeyValuePair<Thing, IntVec3> customInteractionCell in CustomInteractionCells)
			{
				if (customInteractionCell.Key == null || customInteractionCell.Key.Destroyed)
				{
					list.Add(customInteractionCell.Key);
				}
			}
			foreach (Thing item in list)
			{
				CustomInteractionCells.Remove(item);
			}
		}

		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			// 确保加载后清理无效的交互点
			List<Thing> invalidKeys = new List<Thing>();
			foreach (var kvp in CustomInteractionCells)
			{
				if (kvp.Key == null || kvp.Key.Destroyed)
				{
					invalidKeys.Add(kvp.Key);
				}
			}
			foreach (var key in invalidKeys)
			{
				CustomInteractionCells.Remove(key);
			}
		}

		Scribe_Collections.Look(ref CustomInteractionCells, "CustomInteractionCells", LookMode.Reference, LookMode.Value, ref customInteractionCellsKeys, ref customInteractionCellsValues);
	}
}
