using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Spritely
{
	public class Sprite
	{
		private Document m_doc;
		private SpriteList m_sl;

		private int m_nPaletteID;
		private Tile[] m_Tiles;
		private string m_strName;
		private string m_strDescription;

		/// <summary>
		/// Width of the sprite (in tiles)
		/// </summary>
		private int m_tileWidth = 1;

		/// <summary>
		/// Height of the sprite (in tiles)
		/// </summary>
		private int m_tileHeight = 1;

		/// <summary>
		/// This class should contain all of the user-editable data for the Sprite.
		/// It is used by the Undo class
		/// </summary>
		public class UndoData
		{
			public int palette;
			public Tile.UndoData[] tiles;
			public string name;
			public string desc;
			public int width, height;

			private UndoData() { }

			public UndoData(int nWidth, int nHeight)
			{
				palette = 0;
				name = "";
				desc = "";
				width = nWidth;
				height = nHeight;

				int nTiles = nWidth * nHeight;
				tiles = new Tile.UndoData[nTiles];
			}

			public UndoData(UndoData data)
			{
				palette = data.palette;
				name = data.name;
				desc = data.desc;
				width = data.width;
				height = data.height;

				int nTiles = width * height;
				tiles = new Tile.UndoData[nTiles];
				for (int i = 0; i < nTiles; i++)
					tiles[i] = new Tile.UndoData(data.tiles[i]);
			}

			public bool Equals(UndoData data)
			{
				if (palette != data.palette
					|| name != data.name
					|| desc != data.desc
					|| width != data.width
					|| height != data.height
					)
					return false;

				int nTiles = width * height;
				for (int i = 0; i < nTiles; i++)
					if (!tiles[i].Equals(data.tiles[i]))
						return false;

				return true;
			}
		}

		/// <summary>
		/// A snapshot of the sprite data from the last undo checkpoint.
		/// </summary>
		private UndoData m_snapshot;

		/// <summary>
		/// Internal ID to assign to next sprite.
		/// This is used internally to manage sprites and is assigned in the order that the sprites are created.
		/// This is also used to assign a default name to the sprite.
		/// It is not the same as the SpriteID that is exported into the C code.
		/// </summary>
		private static int m_nSpriteID = 1;

		/// <summary>
		/// Exported ID of the sprite.
		/// This is only assigned when exporting the sprites.
		/// </summary>
		private int m_nSpriteExportID = 0;

		/// <summary>
		/// ID of the first tile for this sprite.
		/// This is only assigned when exporting the sprites.
		/// </summary>
		private int m_nFirstTileID = 0;

		//  SQUARE  00    SIZE_8   00    8 x 8              1
		//  SQUARE  00    SIZE_16  01    16 x 16            4
		//  SQUARE  00    SIZE_32  10    32 x 32            16
		//  SQUARE  00    SIZE_64  11    64 x 64            64
		//   WIDE   01    SIZE_8   00    16 x 8             2
		//   WIDE   01    SIZE_16  01    32 x 8             4
		//   WIDE   01    SIZE_32  10    32 x 16            8
		//   WIDE   01    SIZE_64  11    64 x 32            32
		//   TALL   10    SIZE_8   00    8 x 16             2
		//   TALL   10    SIZE_16  01    8 x 32             4
		//   TALL   10    SIZE_32  10    16 x 32            8
		//   TALL   10    SIZE_64  11    32 x 64            32
		public enum GBASize
		{
			Size8,
			Size16,
			Size32,
			Size64,
		}

		public enum GBAShape
		{
			Square,
			Wide,
			Tall,
		}

		// This assumes that width x height are valid.
		public Sprite(Document doc, SpriteList sl, int nWidth, int nHeight, string strName, string strDescription)
		{
			m_doc = doc;
			m_sl = sl;

			if (strName == ""
				|| doc.GetSprites(MainForm.Tab.Sprites).HasNamedSprite(strName)
				|| doc.GetSprites(MainForm.Tab.BackgroundSprites).HasNamedSprite(strName)
				)
				strName = AutoGenerateSpriteName();
			m_strName = strName;

			m_nPaletteID = 0;
			m_strDescription = strDescription;
			m_tileWidth = nWidth;
			m_tileHeight = nHeight;

			int nTiles = NumTiles;
			m_Tiles = new Tile[nTiles];
			for (int i=0; i < nTiles; i++)
				m_Tiles[i] = new Tile(this);

			// Make an initial snapshot of the (empty) sprite.
			m_snapshot = GetUndoData();
		}

		public void UpdateDocument(Document doc)
		{
			m_doc = doc;
		}

		public static string AutoGenerateSpriteName()
		{
			return String.Format("S{0}", m_nSpriteID++);
		}

		public void Duplicate(Sprite sToCopy)
		{
			CopyData(sToCopy);
			m_nPaletteID = sToCopy.m_nPaletteID;
		}

		/// <summary>
		/// Copy the tile data from the specified sprite
		/// </summary>
		/// <param name="sToCopy">Sprite to copy</param>
		public void CopyData(Sprite sToCopy)
		{
			// Make sure that the sprites have the same dimensions
			if (TileWidth != sToCopy.TileWidth || TileHeight != sToCopy.TileHeight)
				return;

			// Copy over the tile data
			for (int i = 0; i < NumTiles; i++)
				m_Tiles[i].CopyData(sToCopy.m_Tiles[i]);
		}

		/// <summary>
		/// Width of sprite (in tiles)
		/// </summary>
		public int TileWidth
		{
			get { return m_tileWidth; }
		}

		/// <summary>
		/// Height of sprite (in tiles)
		/// </summary>
		public int TileHeight
		{
			get { return m_tileHeight; }
		}

		/// <summary>
		/// Total number of tiles for sprite
		/// </summary>
		public int NumTiles
		{
			get { return m_tileWidth * m_tileHeight; }
		}

		/// <summary>
		/// Width of sprite (in pixels)
		/// </summary>
		public int PixelWidth
		{
			get { return m_tileWidth * Tile.TileSize; }
		}

		/// <summary>
		/// Height of sprite (in pixels)
		/// </summary>
		public int PixelHeight
		{
			get { return m_tileHeight * Tile.TileSize; }
		}

		public bool IsSize(int tileWidth, int tileHeight)
		{
			return tileWidth == m_tileWidth && tileHeight == m_tileHeight;
		}

		/// <summary>
		/// The sprite ID assigned to this sprite.
		/// This value is valid only after the sprites have been loaded, exported or saved.
		/// Editing operations (adding/removing sprites) will cause this to become invalid.
		/// </summary>
		public int ExportID
		{
			get { return m_nSpriteExportID; }
			set { m_nSpriteExportID = value; }
		}

		/// <summary>
		/// The tile ID for the first tile in this sprite.
		/// This value is valid only after the sprites have been loaded, exported or saved.
		/// Editing operations (adding/removing sprites) will cause this to become invalid.
		/// </summary>
		public int FirstTileID
		{
			get { return m_nFirstTileID; }
			set { m_nFirstTileID = value; }
		}

		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}

		public string Description
		{
			get { return m_strDescription; }
			set { m_strDescription = value; }
		}

		public int PaletteID
		{
			get { return m_nPaletteID; }
			set { m_nPaletteID = value; }
		}

		public Palette Palette
		{
			get { return m_sl.Palettes.GetPalette(m_nPaletteID); }
		}

		public Tile GetTile(int nTileIndex)
		{
			if (nTileIndex < 0 || nTileIndex >= NumTiles)
				nTileIndex = 0;
			return m_Tiles[nTileIndex];
		}

		public int GetPixel(int pxSpriteX, int pxSpriteY)
		{
			// Convert sprite pixel coords (x,y) into tile index (x,y) and tile pixel coords (x,y).
			int tileX = pxSpriteX / Tile.TileSize;
			int pxX = pxSpriteX % Tile.TileSize;
			int tileY = pxSpriteY / Tile.TileSize;
			int pxY = pxSpriteY % Tile.TileSize;

			int nTileIndex = (tileY * TileWidth) + tileX;

			return m_Tiles[nTileIndex].GetPixel(pxX, pxY);
		}

		public void SetPixel(int pxSpriteX, int pxSpriteY, int color)
		{
			// Convert sprite pixel coords (x,y) into tile index (x,y) and tile pixel coords (x,y).
			int tileX = pxSpriteX / Tile.TileSize;
			int pxX = pxSpriteX % Tile.TileSize;
			int tileY = pxSpriteY / Tile.TileSize;
			int pxY = pxSpriteY % Tile.TileSize;

			int nTileIndex = (tileY * TileWidth) + tileX;

			m_Tiles[nTileIndex].SetPixel(pxX, pxY, color);
		}

		/// <summary>
		/// Is the sprite empty?
		/// </summary>
		/// <returns>True if the sprite has no data</returns>
		public bool IsEmpty()
		{
			int nTiles = NumTiles;

			// Check each tile for emptiness
			for (int i = 0; i < nTiles; i++)
				if (!m_Tiles[i].IsEmpty())
					return false;

			// All the tiles are empty, therefore the sprite is empty
			return true;
		}

		public bool Resize(int tileWidth, int tileHeight)
		{
			// If the new size == the old size
			if (tileWidth == m_tileWidth && tileHeight == m_tileHeight)
				return false;

			int oldX = m_tileWidth;		// AKA: TileWidth
			int oldY = m_tileHeight;	// AKA: TileHeight
			int old_tiles = m_tileWidth * m_tileHeight;	// AKA: NumTiles
			int newX = tileWidth;
			int newY = tileHeight;
			int new_tiles = tileWidth * tileHeight;

			// If we are clipping any tiles with the new size
			if (oldX > newX || oldY > newY)
			{
				// Check to see if any of the clipped tiles contain data
				bool fHasData = false;

				// Handle case where the old sprite is wider than the new sprite:
				// e.g. old=4,2  xxOO     old=4,2  xxOO    old=4,4  xxOO
				//      new=2,4  xxOO     new=2,2  xxOO    new=2,2  xxOO
				//               nn                                 ooOO
				//               nn                                 ooOO
				// (O = tile only in old sprite - checked by this loop,
				//  o = tile only in old sprite - NOT checked by this loop,
				//  n = tile only in new sprite - not checked,
				//  x = tile shared in both sprites - not checked)
				for (int ix = newX; ix < oldX; ix++)
				{
					for (int iy = 0; iy < oldY; iy++)
					{
						if (!m_Tiles[(iy * oldX) + ix].IsEmpty())
							fHasData = true;
					}
				}

				// Handle the case where the old sprite is taller than the new sprite:
				// e.g. old=2,4  xxnn     old=2,4  xx      old=4,4  xxoo
				//      new=4,2  xxnn     new=2,2  xx      new=2,2  xxoo
				//               OO                OO               OOoo
				//               OO                OO               OOoo
				// Note that we can cut the x-loop short when oldX > newX since we've covered that case
				// in the above loop.
				int minX = (oldX < newX ? oldX : newX);
				for (int iy = newY; iy < oldY; iy++)
				{
					for (int ix = 0; ix < minX; ix++)
					{
						if (!m_Tiles[(iy * oldX) + ix].IsEmpty())
							fHasData = true;
					}
				}

				// Make sure it's OK with the user before discarding clipped data
				//if (fHasData && !m_doc.Owner.AskYesNo("Resizing the sprite will cause some tiles to be clipped and the associated data will be lost.\nAre you sure you want to resize the sprite?"))
				if (fHasData && !m_doc.Owner.AskYesNo(ResourceMgr.GetString("WarnSpriteResizeClip")))
					return false;
			}

			// Allocate the new tiles (copying over from the current tiles as needed)
			Tile[] newTiles = new Tile[new_tiles];
			for (int ix = 0; ix < newX; ix++)
			{
				for (int iy = 0; iy < newY; iy++)
				{
					newTiles[(iy * newX) + ix] = new Tile(this);
					if (ix < oldX && iy < oldY)
						newTiles[(iy * newX) + ix].CopyData(m_Tiles[(iy * oldX) + ix]);
				}
			}

			// Switch over to the newly resized tile data
			m_tileWidth = newX;
			m_tileHeight = newY;
			m_Tiles = newTiles;

			return true;
		}

		/// <summary>
		/// Handle a click at the specified (x,y) pixel coords
		/// </summary>
		/// <param name="pxSpriteX">Click x-position (in pixels)</param>
		/// <param name="pxSpriteY">Click y-position (in pixels)</param>
		/// <returns>True if the sprite changes as a result of this click, false otherwise.</returns>
		public bool Click(int pxSpriteX, int pxSpriteY, Toolbox.ToolType tool)
		{
			// Ignore if pixel is outside bounds of current sprite.
			if (pxSpriteX >= PixelWidth || pxSpriteY >= PixelHeight)
				return false;

			if (tool == Toolbox.ToolType.FloodFill)
				return FloodFillClick(pxSpriteX, pxSpriteY);

			// Convert sprite pixel coords (x,y) into tile index (x,y) and tile pixel coords (x,y).
			int tileX = pxSpriteX / Tile.TileSize;
			int pxX = pxSpriteX % Tile.TileSize;
			int tileY = pxSpriteY / Tile.TileSize;
			int pxY = pxSpriteY % Tile.TileSize;

			int nTileIndex = (tileY * TileWidth) + tileX;

			if (tool == Toolbox.ToolType.Eyedropper)
				return m_Tiles[nTileIndex].SelectColorClick(pxX, pxY);

			return m_Tiles[nTileIndex].Click(pxX, pxY, tool == Toolbox.ToolType.Eraser);
		}

		/// <summary>
		/// Finish an editing operation.
		/// </summary>
		public void FinishEdit(Toolbox.ToolType tool)
		{
			switch (tool)
			{
				case Toolbox.ToolType.Eraser:
				case Toolbox.ToolType.Eyedropper:
				case Toolbox.ToolType.FloodFill:
				case Toolbox.ToolType.Pencil:
					// These are immediate tools - nothing to finish up.
					return;
			}

			//return m_Tiles[nTileIndex].Click(pxX, pxY, tool == Toolbox.ToolType.Eraser);
		}

		public class PixelCoord
		{
			public int X, Y;

			public PixelCoord(int x, int y)
			{
				X = x;
				Y = y;
			}
		}

		/// <summary>
		/// Perform a floodfill click at the specified (x,y) pixel coords
		/// </summary>
		/// <param name="pxSpriteX">Click x-position (in pixels)</param>
		/// <param name="pxSpriteY">Click y-position (in pixels)</param>
		/// <returns>True if the sprite changes as a result of this click, false otherwise.</returns>
		public bool FloodFillClick(int pxSpriteX, int pxSpriteY)
		{
			int colorOld = GetPixel(pxSpriteX, pxSpriteY);
			int colorNew = Palette.CurrentColor;
			if (colorOld == colorNew)
				return false;

			// Stack of pixels to process.
			Stack<PixelCoord> stackPixels = new Stack<PixelCoord>();

			PixelCoord p = new PixelCoord(pxSpriteX, pxSpriteY);
			stackPixels.Push(p);

			while (stackPixels.Count != 0)
			{
				p = stackPixels.Pop();
				SetPixel(p.X, p.Y, colorNew);

				FloodFill_CheckPixel(p.X-1, p.Y,   colorOld, ref stackPixels);
				FloodFill_CheckPixel(p.X + 1, p.Y, colorOld, ref stackPixels);
				FloodFill_CheckPixel(p.X, p.Y - 1, colorOld, ref stackPixels);
				FloodFill_CheckPixel(p.X, p.Y + 1, colorOld, ref stackPixels);
			}

			FlushBitmaps();
			return true;
		}

		private void FloodFill_CheckPixel(int x, int y, int color, ref Stack<PixelCoord> stack)
		{
			if (x >= 0 && x < PixelWidth && y >= 0 && y < PixelHeight
				&& GetPixel(x, y) == color)
			{
				stack.Push(new PixelCoord(x, y));
			}
		}

		public void ShiftPixels(Toolbox.ShiftArrow shift)
		{
			if (shift == Toolbox.ShiftArrow.Left)
				ShiftPixels_Left();
			if (shift == Toolbox.ShiftArrow.Right)
				ShiftPixels_Right();
			if (shift == Toolbox.ShiftArrow.Up)
				ShiftPixels_Up();
			if (shift == Toolbox.ShiftArrow.Down)
				ShiftPixels_Down();
			FlushBitmaps();
		}

		public void ShiftPixels_Left()
		{
			int pxWidth = PixelWidth;
			int pxHeight = PixelHeight;

			for (int pxX = 1; pxX < pxWidth; pxX++)
				for (int pxY = 0; pxY < pxHeight; pxY++)
					SetPixel(pxX - 1, pxY, GetPixel(pxX, pxY));
			for (int pxY = 0; pxY < pxHeight; pxY++)
				SetPixel(pxWidth - 1, pxY, 0);
		}

		public void ShiftPixels_Right()
		{
			int pxWidth = PixelWidth;
			int pxHeight = PixelHeight;

			for (int pxX = pxWidth-1; pxX > 0; pxX--)
				for (int pxY = 0; pxY < pxHeight; pxY++)
					SetPixel(pxX, pxY, GetPixel(pxX-1, pxY));
			for (int pxY = 0; pxY < pxHeight; pxY++)
				SetPixel(0, pxY, 0);
		}

		public void ShiftPixels_Up()
		{
			int pxWidth = PixelWidth;
			int pxHeight = PixelHeight;

			for (int pxY = 1; pxY < pxHeight; pxY++)
				for (int pxX = 0; pxX < pxWidth; pxX++)
					SetPixel(pxX, pxY-1, GetPixel(pxX, pxY));
			for (int pxX = 0; pxX < pxWidth; pxX++)
				SetPixel(pxX, pxHeight-1, 0);
		}

		public void ShiftPixels_Down()
		{
			int pxWidth = PixelWidth;
			int pxHeight = PixelHeight;

			for (int pxY = pxHeight - 1; pxY > 0; pxY--)
				for (int pxX = 0; pxX < pxWidth; pxX++)
					SetPixel(pxX, pxY, GetPixel(pxX, pxY-1));
			for (int pxX = 0; pxX < pxWidth; pxX++)
				SetPixel(pxX, 0, 0);
		}

		public void Clear()
		{
			int nTiles = NumTiles;
			for (int i = 0; i < nTiles; i++)
				m_Tiles[i].Clear();
		}

		public bool ImportTile(int nTileIndex, uint[] b)
		{
			if (nTileIndex >= NumTiles)
			{
				//m_doc.Owner.Warning(String.Format("Too many tiles specified for sprite '{0}'. Ignoring extra tiles.", m_strName));
				m_doc.Owner.Warning(String.Format(ResourceMgr.GetString("ErrorSpriteTooManyTiles"), m_strName));
				
				// Return true because the error isn't fatal.
				return true;
			}
			m_Tiles[nTileIndex].Import(b);
			return true;
		}

		public bool ImportBitmap(Bitmap b)
		{
			int nHeight = b.Height;
			int nWidth = b.Width;
			int flags = b.Flags;

			// Determine the number of unique colors in the bitmap.
			Dictionary<Color, int> pal = new Dictionary<Color, int>();
			int nTransparent = 0;
			for (int ix = 0; ix < nWidth; ix++)
			{
				for (int iy = 0; iy < nHeight; iy++)
				{
					Color c = b.GetPixel(ix, iy);
					if (c == Color.Transparent)
						nTransparent++;
					if (!pal.ContainsKey(c))
						pal.Add(c, 0);
					pal[c]++;
				}
			}
			return true;
		}

		public void FlushBitmaps()
		{
			int nTiles = NumTiles;
			for (int i=0; i < nTiles; i++)
				m_Tiles[i].FlushBitmaps();
		}

		public void DrawSmallSprite(Graphics g, int nX0, int nY0)
		{
			int nX, nY;

			for (int iRow = 0; iRow < TileHeight; iRow++)
			{
				for (int iColumn = 0; iColumn < TileWidth; iColumn++)
				{
					nX = (iColumn * Tile.SmallBitmapScreenSize);
					nY = (iRow * Tile.SmallBitmapScreenSize);
					m_Tiles[(iRow * TileWidth) + iColumn].DrawSmallTile(g, nX0 + nX, nY0 + nY);
				}
			}
		}

		public void DrawBigSprite(Graphics g)
		{
			int nX, nY;

			for (int iRow = 0; iRow < TileHeight; iRow++)
			{
				for (int iColumn = 0; iColumn < TileWidth; iColumn++)
				{
					nX = (iColumn * Tile.BigBitmapScreenSize);
					nY = (iRow * Tile.BigBitmapScreenSize);
					m_Tiles[(iRow * TileWidth) + iColumn].DrawBigTile(g, nX, nY);
				}
			}
		}

		public void RecordUndoAction(string strDesc)
		{
			UndoMgr undo = m_doc.Undo();
			if (undo == null)
				return;

			UndoData data = GetUndoData();

			// Don't record anything if there aren't any changes
			if (!data.Equals(m_snapshot))
			{
				UndoAction_SpriteEdit action = new UndoAction_SpriteEdit(undo, m_sl, this, m_snapshot, data, strDesc);
				undo.Push(action);

				// Update the snapshot for the next UndoAction
				RecordSnapshot();
			}
		}

		public UndoData GetUndoData()
		{
			UndoData undo = new UndoData(m_tileWidth, m_tileHeight);
			RecordUndoData(ref undo);
			return undo;
		}

		public void RecordSnapshot()
		{
			RecordUndoData(ref m_snapshot);
		}

		private void RecordUndoData(ref UndoData undo)
		{
			// If the given undo struct isn't the right size, resize it.
			if (undo.width * undo.height != m_tileWidth * m_tileHeight)
				undo = new UndoData(m_tileWidth, m_tileHeight);

			undo.name = m_strName;
			undo.desc = m_strDescription;
			undo.palette = m_nPaletteID;
			undo.width = m_tileWidth;
			undo.height = m_tileHeight;

			int nTiles = NumTiles;
			for (int i = 0; i < nTiles; i++)
				undo.tiles[i] = m_Tiles[i].GetUndoData();
		}

		public void ApplyUndoData(UndoData undo)
		{
			// If the sprite isn't the same size (in tiles) as the undo data, resize it.
			if (undo.width * undo.height != m_tileWidth * m_tileHeight)
			{
				int nTiles = undo.width * undo.height;
				m_Tiles = new Tile[nTiles];
				for (int i = 0; i < nTiles; i++)
					m_Tiles[i] = new Tile(this);
			}

			m_strName = undo.name;
			m_strDescription = undo.desc;
			m_nPaletteID = undo.palette;
			m_tileWidth = undo.width;
			m_tileHeight = undo.height;

			for (int i = 0; i < NumTiles; i++)
				m_Tiles[i].ApplyUndoData(undo.tiles[i]);

			RecordSnapshot();
		}

		public void Save(System.IO.TextWriter tw, int nSpriteExportID, int nFirstTileID)
		{
			// Record the export ID and the first tile ID so that the other export routines can use it.
			m_nSpriteExportID = nSpriteExportID;
			m_nFirstTileID = nFirstTileID;

			tw.Write("\t<sprite");
			tw.Write(String.Format(" name=\"{0}\"", m_strName));
			tw.Write(String.Format(" desc=\"{0}\"", m_strDescription));
			tw.Write(String.Format(" size=\"{0}x{1}\"", m_tileWidth, m_tileHeight));
			tw.Write(String.Format(" palette=\"{0}\"", m_nPaletteID));
			tw.Write(String.Format(" id=\"{0}\"", nSpriteExportID));
			tw.Write(String.Format(" firsttileid=\"{0}\"", nFirstTileID));
			tw.WriteLine(">");

			int nTileID = m_nFirstTileID;
			foreach (Tile t in m_Tiles)
				t.Save(tw, nTileID++);

			tw.WriteLine("\t</sprite>");
		}

		public void ExportGBAHeader_SpriteIDs(System.IO.TextWriter tw)
		{
			tw.WriteLine(String.Format("#define kSprite_{0} {1}", m_strName, m_nSpriteExportID));
		}

		public void ExportGBASource_AssignIDs(int nSpriteExportID, int nFirstTileID)
		{
			// Record the export ID and the first tile ID so that the other export routines can use it.
			m_nSpriteExportID = nSpriteExportID;
			m_nFirstTileID = nFirstTileID;
		}

		public void ExportGBASource_SpriteInfo(System.IO.TextWriter tw, GBASize size, GBAShape shape)
		{
			if (tw == null)
				return;

			string strShape = "INVALID";
			switch (shape)
			{
				case GBAShape.Square: strShape = "ATTR0_SQUARE"; break;
				case GBAShape.Wide: strShape = "ATTR0_WIDE"; break;
				case GBAShape.Tall: strShape = "ATTR0_TALL"; break;
			}

			string strSize = "INVALID";
			switch (size)
			{
				case GBASize.Size8: strSize = "ATTR1_SIZE_8"; break;
				case GBASize.Size16: strSize = "ATTR1_SIZE_16"; break;
				case GBASize.Size32: strSize = "ATTR1_SIZE_32"; break;
				case GBASize.Size64: strSize = "ATTR1_SIZE_64"; break;
			}

			tw.WriteLine(String.Format("\t{{{0,4},{1,4},{2,4},{3,4},{4,4},{5,16},{6,16} }}, // Sprite_{7}",
				m_nFirstTileID, NumTiles,
				PixelWidth, PixelHeight, m_nPaletteID,
				strShape, strSize, m_strName));
		}

		public void ExportGBASource_SpriteData(System.IO.TextWriter tw)
		{
			if (m_nSpriteExportID != 0)
				tw.WriteLine("");
			
			tw.WriteLine("\t// Sprite : " + m_strName);
			if (m_strDescription != "")
				tw.WriteLine("\t// Description : " + m_strDescription);
			tw.WriteLine(String.Format("\t// Size : {0}x{1} = {2} tiles", TileWidth, TileHeight, NumTiles));

			int nIndex = m_nFirstTileID;
			foreach (Tile t in m_Tiles)
				t.ExportGBA(tw, nIndex++);
		}

	}
}