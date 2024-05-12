using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GasStationSystem
{
    public class Configuration : IRocketPluginConfiguration
    {
        public float ZoneRadius;
        public uint GasPriceForOneProcent;
        public List<Zone> FillFuelZones;

        public void LoadDefaults()
        {
            ZoneRadius = 5f;
            GasPriceForOneProcent = 3;
            FillFuelZones = new List<Zone> { };
        }
    }
}
