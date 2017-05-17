using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lazy_steam_server
{
    public partial class SettingsMenu : Form
    {
        public SettingsMenu()
        {
            InitializeComponent();
            textBoxSteamRunning.Text = Properties.Settings.Default.steam_exists.ToString();
            textBoxSteamNotRunning.Text = Properties.Settings.Default.steam_not_exists.ToString();
            checkBoxAutoPasteToSteam.Checked = Properties.Settings.Default.trigger_scrapping;
            checkBoxRunAtStartUp.Checked = Properties.Settings.Default.run_at_startup;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            int steamRunningDuration = 0;
            int steamNotRunningDuration = 0;

            if (Int32.TryParse(textBoxSteamRunning.Text, out steamRunningDuration) &&
                Int32.TryParse(textBoxSteamNotRunning.Text, out steamNotRunningDuration))
            {
                Properties.Settings.Default.steam_exists = steamRunningDuration;
                Properties.Settings.Default.steam_not_exists = steamNotRunningDuration;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("Duration time values must be integers", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBoxSteamNotRunning_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBoxSteamNotRunning_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void textBoxSteamRunning_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void checkBoxAutoPasteToSteam_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.trigger_scrapping = checkBoxAutoPasteToSteam.Checked;
            Properties.Settings.Default.Save();
        }

        private void checkBoxRunAtStartUp_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.run_at_startup = checkBoxRunAtStartUp.Checked;
            Properties.Settings.Default.Save();
            App.SetStartup();
        }
    }
}
