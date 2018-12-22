using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using System.Windows.Forms;
using System.Drawing;
using GTA.Native;

namespace NewHeists
{
    public class Main : Script
    {
        private ArmoredTruck at = new ArmoredTruck();
        private ScriptSettings config;
        private Keys enableKey;
        private bool enabled = true;

        public Main()
        {
            this.Tick += onTick;
            this.KeyDown += onKeyDown;
            this.Aborted += onAborted;
            enableMod();
        }

        private void onAborted(object sender, EventArgs e)
        {
            disableMod();
            UI.Notify("~r~NewHeists ~w~Reloaded");
        }

        private void onTick(object sender, EventArgs e)
        {
            this.Interval = 10;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == this.enableKey)
            {
                if (!enabled)
                {
                    at.setEnabled(true);
                    enableMod();
                } else
                {
                    at.setEnabled(false);
                    disableMod();
                }
            }
        }

        private void enableMod()
        {
            enabled = true;
            loadSettings();
            sendMSG("NewHeists loaded", "successfully", "I have some work for you. Come over!", 1, "CHAR_LESTER");
        }

        private void disableMod()
        {
            enabled = false;
            sendMSG("NewHeists unloaded", "successfully", "WE WERE BUSTED. FUCK OFF!", 1, "CHAR_LESTER");
        }

        private void loadSettings()
        {
            this.config = ScriptSettings.Load("scripts\\NewHeists.ini");
            this.enableKey = (Keys)this.config.GetValue<Keys>("Options", "EnableModKey", Keys.PageDown);
        }

        public void sendMSG(string sender, string subject, string text, int icon, string texture = "CHAR_DEFAULT")
        {

            GTA.Native.Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, new InputArgument[] { "STRING" });
            GTA.Native.Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, new InputArgument[] { text });
            GTA.Native.Function.Call(Hash._SET_NOTIFICATION_MESSAGE, new InputArgument[] { texture, texture, 1, icon, sender, subject });
        }
    }
}
