using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Spritely
{
	public partial class OptionsEdit : Form
	{
		public OptionsEdit(int nOptionPageIndex)
		{
			InitializeComponent();

			// Set the checkboxes as appropriate.
			cbSprite_ShowRedXForTransparent.Checked = Options.Sprite_ShowRedXForTransparent;
			cbSprite_ShowPaletteIndex.Checked = Options.Sprite_ShowPaletteIndex;
			cbPalette_ShowRedXForTransparent.Checked = Options.Palette_ShowRedXForTransparent;
			cbPalette_ShowPaletteIndex.Checked = Options.Palette_ShowPaletteIndex;

			// Set default result to 'No'.
			this.DialogResult = DialogResult.No;

			tcOptions.SelectedIndex = nOptionPageIndex;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			bool fHasChange = false;

			// Record the selected options.
			if (cbSprite_ShowRedXForTransparent.Checked != Options.Sprite_ShowRedXForTransparent)
			{
				Options.Sprite_ShowRedXForTransparent = cbSprite_ShowRedXForTransparent.Checked;
				fHasChange = true;
			}
			if (cbSprite_ShowPaletteIndex.Checked != Options.Sprite_ShowPaletteIndex)
			{
				Options.Sprite_ShowPaletteIndex = cbSprite_ShowPaletteIndex.Checked;
				fHasChange = true;
			}

			if (cbPalette_ShowRedXForTransparent.Checked != Options.Palette_ShowRedXForTransparent)
			{
				Options.Palette_ShowRedXForTransparent = cbPalette_ShowRedXForTransparent.Checked;
				fHasChange = true;
			}
			if (cbPalette_ShowPaletteIndex.Checked != Options.Palette_ShowPaletteIndex)
			{
				Options.Palette_ShowPaletteIndex = cbPalette_ShowPaletteIndex.Checked;
				fHasChange = true;
			}

			if (fHasChange)
			{
				// Set result to 'Yes' so that the caller knows that an option has changed.
				this.DialogResult = DialogResult.Yes;
			}

			this.Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

	}
}