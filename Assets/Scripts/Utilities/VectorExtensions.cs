using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoundHelpers {

	public const float roundFactor = 10000.0f;

	public static float gameRounded(float value) {
		return Mathf.Round(value * roundFactor) / roundFactor;
	}

}

public static class Vector2ExtensionMethods {

	public static Vector2 rounded(this Vector2 vec) {
		return new Vector2(RoundHelpers.gameRounded(vec.x), RoundHelpers.gameRounded(vec.y));
	}
}

public static class Vector3ExtensionMethods {

	public static Vector3 rounded(this Vector3 vec) {
		return new Vector3(RoundHelpers.gameRounded(vec.x), RoundHelpers.gameRounded(vec.y), RoundHelpers.gameRounded(vec.z));
	}
	
}
