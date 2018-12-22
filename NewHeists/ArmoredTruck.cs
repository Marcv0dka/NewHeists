using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;

namespace NewHeists
{
    public class ArmoredTruck : Script
    {
        private List<Vehicle> listVehicle = new List<Vehicle>();
        private List<Blip> listBlip = new List<Blip>();
        private bool enabled = true;

        public ArmoredTruck()
        {
            this.KeyDown += onKeyDown;
        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad1)
            {
                Vector3 safePos = World.GetNextPositionOnStreet(Game.Player.Character.GetOffsetInWorldCoords(new Vector3(80, 80, 50)), true);

                Vehicle car = World.CreateVehicle(VehicleHash.Stockade, safePos, getRoadHeading(safePos));
                Blip carBlip = car.AddBlip();
                carBlip.Scale = 0.5f;
                listVehicle.Add(car);
                listBlip.Add(carBlip);
                UI.Notify(listVehicle.ToString());
            }
            if (e.KeyCode == Keys.NumPad3)
            {
                UI.Notify("" + enabled.ToString());
            }
        }

        private float getRoadHeading(Vector3 coords)
        {
            //Vector3 coords = Game.Player.Character.Position;
            Vector3 closestVehicleNodeCoords;
            float roadheading;

            OutputArgument tempcoords = new OutputArgument();
            OutputArgument temproadheading = new OutputArgument();

            Function.Call<Vector3>(Hash.GET_CLOSEST_VEHICLE_NODE_WITH_HEADING, coords.X, coords.Y, coords.Z, tempcoords, temproadheading, 1, 3, 0);
            closestVehicleNodeCoords = tempcoords.GetResult<Vector3>();
            roadheading = temproadheading.GetResult<float>();
            return roadheading;
        }

        private void removeVehicles()
        {
            for (int i = 0; i < listVehicle.Count(); i++)
            {
                listVehicle.ElementAt(i).Delete();
            }
            listVehicle.Clear();
        }

        private void removeBlips()
        {
            for (int i = 0; i < listBlip.Count(); i++)
            {
                listBlip.ElementAt(i).Remove();
            }
            listBlip.Clear();
        }

        public void disableArmoredTrucks()
        {
            this.removeBlips();
            this.removeVehicles();
            UI.Notify("removed");
        }

        public void setEnabled(bool enabled)
        {
            this.enabled = enabled;
        }

    }
}
