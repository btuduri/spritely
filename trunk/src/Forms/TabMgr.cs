using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Spritely
{
	public class TabMgr
	{
		private ProjectMainForm m_owner;
		private TabId m_eId;

		public enum TabId
		{
			Sprites=0,
			BackgroundMaps,
			BackgroundImages,
			MAX,
		};

		enum FormListType
		{
			Spritesets=0,
			Sprites,
			Palettes,
			Maps,
			BgImageLists,
			BgImages,
			MAX,
		};

		private List<Form>[] m_winLists;

		public TabMgr(ProjectMainForm owner, TabId id)
		{
			m_owner = owner;
			m_eId = id;

			m_winLists = new List<Form>[(int)FormListType.MAX];
			for (int i=0; i<(int)FormListType.MAX; i++)
				m_winLists[(int)i] = new List<Form>();
		}

		public TabId Id
		{
			get { return m_eId; }
		}

		private List<Form> WinList(FormListType eWinType)
		{
			return m_winLists[(int)eWinType];
		}

		private List<Form> WinList(int id)
		{
			return m_winLists[id];
		}

		public void AddSpritesetWindow(Form f)
		{
			List<Form> winlist = WinList(FormListType.Spritesets);
			if (!winlist.Contains(f))
				winlist.Add(f);
		}

		public void AddSpriteWindow(Form f)
		{
			List<Form> winlist = WinList(FormListType.Sprites);
			if (!winlist.Contains(f))
				winlist.Add(f);
		}

		public void AddPaletteWindow(Form f)
		{
			List<Form> winlist = WinList(FormListType.Palettes);
			if (!winlist.Contains(f))
				winlist.Add(f);
		}

		public void AddMapWindow(Form f)
		{
			List<Form> winlist = WinList(FormListType.Maps);
			if (!winlist.Contains(f))
				winlist.Add(f);
		}

		public void AddBgImageListWindow(Form f)
		{
			List<Form> winlist = WinList(FormListType.BgImageLists);
			if (!winlist.Contains(f))
				winlist.Add(f);
		}

		public void AddBgImageWindow(Form f)
		{
			List<Form> winlist = WinList(FormListType.BgImages);
			if (!winlist.Contains(f))
				winlist.Add(f);
		}

		public void RemoveWindow(Form f)
		{
			for (int i = 0; i < (int)FormListType.MAX; i++)
			{
				if (WinList(i).Contains(f))
					WinList(i).Remove(f);
			}
		}

		public void RemoveAllWindows()
		{
			for (int i = 0; i < (int)FormListType.MAX; i++)
			{
				foreach (Form f in WinList(i))
					f.Dispose();
				WinList(i).Clear();
			}
		}

		public void ShowWindows()
		{
			for (int i = 0; i < (int)FormListType.MAX; i++)
			{
				foreach (Form f in WinList(i))
					f.Show();
			}
		}

		public void HideWindows()
		{
			for (int i = 0; i < (int)FormListType.MAX; i++)
			{
				foreach (Form f in WinList(i))
					f.Hide();
			}
		}

		public void CloseWindows()
		{
			for (int i = 0; i < (int)FormListType.MAX; i++)
			{
				foreach (Form f in WinList(i))
					f.Close();
			}
		}

		/// <summary>
		/// The width of a sprite window that is best suited for allowing
		/// the editing of a single tile. This is used on the Background Map
		/// editing tab to size the sprite window and to calculate the x-offset
		/// for the map window.
		/// </summary>
		const int k_pxSprite1IdealWidth = 206;
		const int k_pxSprite2IdealWidth = 330;
		const int k_pxSprite4IdealWidth = 591;
		const int k_pxBackgroundMapIdealWidth = 591;
		const int k_pxBackgroundImageIdealWidth = 591;

		public void ArrangeWindows()
		{
			if (m_eId == TabId.Sprites)
			{
				int nSpritesets = WinList(FormListType.Spritesets).Count;
				int nSprites = WinList(FormListType.Sprites).Count;
				int nPalettes = WinList(FormListType.Palettes).Count;

				if (nSpritesets == 0 || nSprites == 0 || nPalettes == 0)
					return;

				Form ss = WinList(FormListType.Spritesets)[0];
				ss.Top = 0;
				ss.Left = 0;

				Form p = WinList(FormListType.Palettes)[0];
				p.Top = ss.Height;
				p.Left = 0;

				Form s = WinList(FormListType.Sprites)[0];
				s.Top = 0;
				s.Left = ss.Right;
				s.Height = m_owner.ContentHeight;

				// Expand window to fill all remaining space.
				s.Width = m_owner.ContentWidth - s.Left;
			}

			if (m_eId == TabId.BackgroundMaps)
			{
				int nSpritesets = WinList(FormListType.Spritesets).Count;
				int nSprites = WinList(FormListType.Sprites).Count;
				int nPalettes = WinList(FormListType.Palettes).Count;
				int nMaps = WinList(FormListType.Maps).Count;

				if (nSpritesets == 0 || nSprites == 0 || nPalettes == 0 || nMaps == 0)
					return;

				Form ss = WinList(FormListType.Spritesets)[0];
				ss.Top = 0;
				ss.Left = 0;

				Form p = WinList(FormListType.Palettes)[0];
				p.Top = ss.Height;
				p.Left = 0;

				Form s = WinList(FormListType.Sprites)[0];
				s.Top = 0;
				s.Left = ss.Right;
				s.Height = m_owner.ContentHeight;

				Form m = WinList(FormListType.Maps)[0];
				m.Top = 0;
				m.Height = m_owner.ContentHeight;

				// Balance width of sprite and map windows in the remaining space.
				int pxSpace = m_owner.ContentWidth - s.Left;

				// Is there room for a map and 2-tile wide sprite?
				if (pxSpace >= k_pxSprite2IdealWidth + k_pxBackgroundMapIdealWidth)
				{
					s.Width = k_pxSprite2IdealWidth;
					m.Left = s.Right;
					m.Width = k_pxBackgroundMapIdealWidth;
				}
				else
				{
					// Make bgsprite window = 1 tile width.
					s.Width = k_pxSprite1IdealWidth;
					m.Left = s.Right;
					// Make map window as big as possible.
					if (pxSpace >= k_pxSprite1IdealWidth + k_pxBackgroundMapIdealWidth)
						m.Width = k_pxBackgroundMapIdealWidth;
					else
						m.Width = m_owner.ContentWidth - s.Right;
				}
			}

			if (m_eId == TabId.BackgroundImages)
			{
				int nBgImageLists = WinList(FormListType.BgImageLists).Count;
				int nBgImages = WinList(FormListType.BgImages).Count;

				if (nBgImageLists == 0 || nBgImages == 0)
					return;

				Form bglist = WinList(FormListType.BgImageLists)[0];
				bglist.Top = 0;
				bglist.Left = 0;

				Form bgi = WinList(FormListType.BgImages)[0];
				bgi.Top = 0;
				bgi.Left = bglist.Right;
				bgi.Height = m_owner.ContentHeight;

				int pxSpace = m_owner.ContentWidth - bgi.Left;

				if (pxSpace >= k_pxBackgroundImageIdealWidth)
					bgi.Width = k_pxBackgroundMapIdealWidth;
				else
					bgi.Width = m_owner.ContentWidth - bgi.Left;
			}
		}

	}
}
