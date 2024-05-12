using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace GasStationSystem
{
    public class Zone
    {
        [XmlAttribute]
        public float X;
        [XmlAttribute]
        public float Y;
        [XmlAttribute]
        public float Z;

        public Vector3 Position => new Vector3(X, Y, Z);

        public Zone(Vector3 postion)
        {
            X = postion.x;
            Y = postion.y;
            Z = postion.z;
        }
        
        public Zone()
        {
        }
    }
}
