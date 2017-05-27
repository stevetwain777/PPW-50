using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using CreativeSpore.SuperTilemapEditor;

public static class CustomTileEditorExtensions {

	[MenuItem("Custom Tools/Add ice parameters to tile selection")]
	private static void AddIceParamsToTileSelection()
	{
		if (Selection.activeObject is Tileset)
		{
			Tileset tileset = Selection.activeObject as Tileset;
			foreach(uint tileId in tileset.TileSelection.selectionData)
			{
				ParameterContainer paramContainer = tileset.Tiles[(int)tileId].paramContainer;
				paramContainer.AddParam("ice", true);
			}
			EditorUtility.SetDirty(tileset);
		}
	}
}
