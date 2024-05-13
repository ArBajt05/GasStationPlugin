using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GasStationSystem
{
    public class GasStationSystemUseCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() {};
            }
        }
        public bool RunFromConsole
        {
            get { return false; }
        }

        public string Name
        {
            get { return "gas"; }
        }
        public string Syntax
        {
            get
            {
                return "/gas";
            }
        }
        public string Help
        {
            get { return "Będąc w odpowiedniej strefie wpisz /gas aby zatankować auto"; }
        }
        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(IRocketPlayer caller, params string[] command)
        {
            var player = (UnturnedPlayer)caller;

            var zone = Class1.Instance.Configuration.Instance.FillFuelZones.FirstOrDefault(s => Vector3.Distance(player.Position, s.Position) <= Class1.Instance.Configuration.Instance.ZoneRadius);

            if (!player.IsInVehicle)
            {
                return;
            }

            if (zone == null)
            {
                return;
            }

            if (player.CurrentVehicle.fuel <= player.CurrentVehicle.asset.fuel)
            {
                if (player.Experience >= Class1.Instance.Configuration.Instance.GasPriceForOneProcent)
                {
                    var litrbenzyny = ushort.Parse((player.CurrentVehicle.asset.fuel * 0.01).ToString()); // 1% of gas
                    var kalkulator = ushort.Parse((player.CurrentVehicle.asset.fuel - player.CurrentVehicle.fuel).ToString()) / litrbenzyny; // how much of % we need to 100
                    uint koszt1 = (uint)(kalkulator * Class1.Instance.Configuration.Instance.GasPriceForOneProcent); // cost of gas in configuration for example 3 exp. For example i have 80% in the tank in the car then in this case there will be 60 exp to pay.
                    var count = 0;

                    while (player.CurrentVehicle.fuel < player.CurrentVehicle.asset.fuel && player.Experience >= Class1.Instance.Configuration.Instance.GasPriceForOneProcent)
                    {
                        count++;
                        player.Experience -= Class1.Instance.Configuration.Instance.GasPriceForOneProcent;
                        player.CurrentVehicle.askFillFuel((ushort)(player.CurrentVehicle.asset.fuel * 0.01));
                    }

                    var all = count * Class1.Instance.Configuration.Instance.GasPriceForOneProcent;
                    ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_tanked", all), Color.green, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                }
                else
                {
                    ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_not_money"), Color.green, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                    return;
                }
            }
        }
    }
}
