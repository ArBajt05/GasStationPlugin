using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using UnityEngine;
using System.Security.Policy;
using SDG.Unturned;

namespace GasStationSystem
{
    public class Class1 : RocketPlugin<Configuration>
    {
        public static Class1 Instance;
        private Dictionary<CSteamID, Zone> PlayerInGasStationZone;

        protected override void Load()
        {
            Instance = this;
            PlayerInGasStationZone = new Dictionary<CSteamID, Zone>();
            UnturnedPlayerEvents.OnPlayerUpdatePosition += UnturnedPlayerEvents_OnPlayerUpdatePosition;
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdatePosition -= UnturnedPlayerEvents_OnPlayerUpdatePosition;
        }

        public void UnturnedPlayerEvents_OnPlayerUpdatePosition(UnturnedPlayer player, Vector3 position)
        {
            var station = Configuration.Instance.FillFuelZones.FirstOrDefault(s => Vector3.Distance(position, s.Position) <= Configuration.Instance.ZoneRadius);

            if (station == null) return;

            PlayerInGasStationZone[player.CSteamID] = station;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
                {"gas_station_usage","Use: /gaszone add/remove"},
                {"gas_station_added","Gas Station zone added with Id: {0}"},
                {"gas_station_removed","Gas Station zone removed"},
                {"gas_station_wrong_id","Wrong id of the zone"},
                {"gas_station_not_exist","There are no gas stations"},
                {"gas_station_not_in","You are not at gas station"},
                {"gas_station_to_much_of_gas","You can't fill up for more than your car's fuel tank can hold"},
                {"gas_station_how_much_you_can_tank_for_now","For now you can fill up: {0}% of gas for: {1} exp"},
                {"Gas_station_zone_too_close","You are too close to existing gas station"},
                {"gas_station_not_enough_money","You don't have enough money to pay for gas"},
                {"gas_station_tanked","You tanked for {0} exp"}
        };
    }
}
