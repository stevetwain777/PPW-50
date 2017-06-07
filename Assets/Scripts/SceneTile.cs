using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

[Flags]
public enum SceneTileType {
	None        = 0x0,
	Ground      = 0x1,
	Ice         = 0x2,
	JumpThrough = 0x4,
}

public struct SceneTile {
	internal static class SceneTileDefaults {
		internal const string kIceParameterName = "ice";
		internal const float kTileQueryCastDistance = 0.1f;
	}
    
	private Tilemap _tilemap;
	public Tilemap tilemap {
		get { return _tilemap; }
	}

	private Int2 _gridPosition;
	public Int2 gridPosition {
		get { return _gridPosition; }
	}

	private Tile _tile;
	public Tile tile { 
		get { return _tile; } 
	}
	
	private SceneTileType _type;
	public SceneTileType type {
		get { return _type; }
	}

	public SceneTile(Tilemap tilemap, Int2 gridPosition) {
		_tilemap = tilemap;
		_gridPosition = gridPosition;

		_tile = TileForGridPosition(_tilemap, _gridPosition);
		_type = TileTypeForTile(_tile);
	}

	public static SceneTile SceneTileFromRaycastHit(RaycastHit2D hit) {
		Tilemap tilemap = hit.transform.gameObject.GetComponentInParent<Tilemap>();

		// Move the hit point in the opposite direction of the normal a little bit,
		// so that we move into the tile. NOTE: This will break if tileQueryCastDistance is 
		// more than the cell size.
		Vector2 localPosition = tilemap.transform.InverseTransformPoint(hit.point - (hit.normal * SceneTileDefaults.kTileQueryCastDistance));
		Int2 gridPosition = TileGridForLocalPosition(tilemap, localPosition);
		return new SceneTile(tilemap, gridPosition);
	}

	public void RemoveTile() {
		_tilemap.Erase(_gridPosition.x, _gridPosition.y);
		_tilemap.UpdateMesh();
	}




	private static Int2 TileGridForLocalPosition(Tilemap tilemap, Vector2 localPosition) {
		return new Int2(BrushUtil.GetGridX(localPosition, tilemap.CellSize),
						BrushUtil.GetGridY(localPosition, tilemap.CellSize));
	}

	private static Tile TileForGridPosition(Tilemap tilemap, Int2 gridPosition) {
		return tilemap.GetTile(gridPosition.x, gridPosition.y);
	}

	private static SceneTileType TileTypeForTile(Tile tile) {
		// Even if there's no tile, default to ground.
		if (tile == null) {
			return SceneTileType.Ground;
		}

		if (tile.paramContainer.GetBoolParam(SceneTileDefaults.kIceParameterName, false)) {
			return SceneTileType.Ice;
		}
		return SceneTileType.Ground;
	}
}
