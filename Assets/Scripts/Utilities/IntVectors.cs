using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Int2 {
	public int x, y;

	public Int2(int x, int y) {
		this.x = x;
		this.y = y;
	}
}

public static class Vector2Extensions {
    public static Int2 ToInt2 (this Vector2 vector2) {
        return new Int2(Mathf.RoundToInt(vector2.x),
						Mathf.RoundToInt(vector2.y));
    }
}