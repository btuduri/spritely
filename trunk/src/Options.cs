using System;
using System.Collections.Generic;
using System.Text;

namespace Spritely
{
	public static class Options
	{
		public static bool Sprite_ShowPixelGrid = true;
		public static bool Sprite_ShowTileGrid = true;
		public static bool Sprite_ShowRedXForTransparent = true;
		public static bool Sprite_ShowPaletteIndex = false;

		public static bool Palette_ShowRedXForTransparent = true;
		public static bool Palette_ShowPaletteIndex = false;

		public static bool BackgroundMap_ShowGrid = true;
		public static bool BackgroundMap_ShowScreen = true;

		public enum PlatformType
		{
			GBA,
			NDS,
		};

		public static PlatformType Platform = PlatformType.GBA;
	}
}