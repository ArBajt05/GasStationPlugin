using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using UnityEngine;

namespace GasStationSystem
{
    public class GasStationSystemZoneCreateCommand : IRocketCommand
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
            get { return "gaszone"; }
        }
        public string Syntax
        {
            get
            {
                return "/gaszone";
            }
        }
        public string Help
        {
            get { return "type /gaszone to add the gas station"; }
        }
        public List<string> Aliases
        {
            get { return new List<string> { "gaszone" }; }
        }

        //gaszone add 
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (command.Length == 0)
            {
                ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_usage"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }

            if (command[0].ToLower() == "add")
            {
                if (Class1.Instance.Configuration.Instance.FillFuelZones.Any(s => Vector3.Distance(s.Position, player.Position) <= Class1.Instance.Configuration.Instance.ZoneRadius * 2))
                {
                    ChatManager.serverSendMessage(Class1.Instance.Translate("Gas_station_zone_too_close"),Color.white,null, player.SteamPlayer(),EChatMode.SAY, null, true);
                    return;
                }

                var station = new Zone(player.Position);

                Class1.Instance.Configuration.Instance.FillFuelZones.Add(station);
                Class1.Instance.Configuration.Save();

                ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_added", Class1.Instance.Configuration.Instance.FillFuelZones.IndexOf(station)), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }
            if (command[0].ToLower() == "remove")
            {
                if (Class1.Instance.Configuration.Instance.FillFuelZones.Count == 0)
                {
                    ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_not_exist"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                    return;
                }

                if (command.Length > 1)
                {
                    if (!int.TryParse(command[1], out var index) || (Class1.Instance.Configuration.Instance.FillFuelZones.Count - 1 < index || index < 0))
                    {
                        ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_wrong_id"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                        return;
                    }

                    Class1.Instance.Configuration.Instance.FillFuelZones.RemoveAt(index);
                    Class1.Instance.Configuration.Save();

                    ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_removed"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                    return;
                }

                var station = Class1.Instance.Configuration.Instance.FillFuelZones.FirstOrDefault(s => Vector3.Distance(player.Position, s.Position) <= Class1.Instance.Configuration.Instance.ZoneRadius);

                if (station == null)
                {
                    ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_not_in"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                    return;
                }

                Class1.Instance.Configuration.Instance.FillFuelZones.Remove(station);
                Class1.Instance.Configuration.Save();

                ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_removed"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
                return;
            }
            ChatManager.serverSendMessage(Class1.Instance.Translate("gas_station_usage"), Color.white, null, player.SteamPlayer(), EChatMode.SAY, null, true);
        }
    }
}
