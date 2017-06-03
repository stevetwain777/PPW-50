using System.Collections;
using System.Collections.Generic;

public static class GameLayers {
	public static readonly int Default 		= 0;
	public static readonly int Player 		= 8;
	public static readonly int Solid 		= 9;
	public static readonly int JumpThrough 	= 10;
	public static readonly int Breakable 	= 11;

	public static class Masks {
		public static readonly int Default 		= 0x1 << Default;
		public static readonly int Player 		= 0x1 << Player;
		public static readonly int Solid 		= 0x1 << Solid;
		public static readonly int JumpThrough 	= 0x1 << JumpThrough;
		public static readonly int Breakable 	= 0x1 << Breakable;
	}
}
