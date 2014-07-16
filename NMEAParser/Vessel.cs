using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAParser
{
    static class SignalK
    {
       
        //unique identifier of this vessel - e.g. MMSI
        static private string skself="235078477";
        //timestamp of last update 
        static private DateTime sktimestamp= new DateTime(2014,07,11,20,50,00);
        //version of update;
        static private int skversion=1;
        //source of date;
        static private string sksource="self";

        static private IDictionary<string, Vessel> skvessel;

        static internal string self { get { return skself; } }
        static internal DateTime timestamp { get { return sktimestamp; } }
        static internal int version{ get { return skversion; } }
        static internal string source { get { return sksource; } }

        static internal Vessel vessel(string id)
        {
            Vessel ves;
            if (!(skvessel.TryGetValue(id, out ves)))
            {
                ves = new Vessel();
                skvessel.Add(id, ves);
            }
            return ves;
        }
    }
    
    class Vessel
    {
        public Vessel ()
        {
            vesOrgLoc = new GeoCoordinate();
            vesCurLoc = new GeoCoordinate();
        }
        
        /// <summary>
        /// current heading in deg
        /// </summary>
        private double vesHDG;
        /// <summary>
        /// current depth below transducer in m
        /// </summary>
        private double vesDBT;
        /// <summary>
        /// current relative wind direction in deg
        /// </summary>
        private double vesRWD;
        /// <summary>
        /// current relative wind speed in m/s
        /// </summary>
        private double vesRWS;
        /// <summary>
        /// current location, speed, course of vessel
        /// </summary>
        private GeoCoordinate vesCurLoc;
        /// <summary>
        /// original location of vessel
        /// </summary>
        private GeoCoordinate vesOrgLoc;
        /// <summary>
        /// current speed through water
        /// </summary>
        private double vesSTW;
        /// <summary>
        /// rate of turn in deg/min
        /// </summary>
        private double vesROT;
        /// <summary>
        /// water temp in deg C
        /// </summary>
        private double vesWTemp;
        /// <summary>
        /// air temp in deg C
        /// </summary>
        private double vesATemp;
        /// <summary>
        /// Total distance in water nautical miles
        /// </summary>
        private double vesLogWaterTotal;
        /// <summary>
        /// trip distance in water nautical miles
        /// </summary>
        private double vesLogWaterTrip;        
        /// <summary>
        /// cross track error in nautical miles -ve to port +ve to starboard
        /// </summary>
        private double vesXTE;        
        /// <summary>
        /// curernt target waypoint ID
        /// </summary>
        private double vesTargetWaypointID;        
        /// <summary>
        /// Bearing origin to destinaion waypoint
        /// </summary>
        private double vesOrigCourseToWaypoint;        
        /// <summary>
        /// Current bearing to destinaion waypoint
        /// </summary>
        private double vesCurrentBearingToWaypoint;        
        /// <summary>
        /// Current heading to steer to destinaion waypoint
        /// </summary>
        private double vesCurrentHeadingtoSteerToWaypoint;        

        public double SOG
        {
            get {return vesCurLoc.Speed;}
            set {vesCurLoc.Speed = value;}
        }

        public double HDG
        {
            get {return vesHDG;}
            set { vesHDG = value; }
        }

        public double COG
        {
            get{ return vesCurLoc.Course;}
            set{ vesCurLoc.Course = value;}
        }

        public double DBT
        {
            get { return vesDBT; }
            set { vesDBT = value;}
        }
        public double DBS
        {
            //Todo add depth of transducer as a vessel attribute
            get { return vesDBT+0.4; }
        }
        public double DBK
        {
            //Todo add depth of keel from transducer as a vessel attribute
            get { return vesDBT - 1.1; }
        }

        public double RWD
        {
            get { return vesRWD;}
            set { vesRWD = value; }
        }

        public double TWD
        {
            get
            {
                double truewinddir = this.HDG + this.RWD;
                if (truewinddir > 360) truewinddir = truewinddir - 360;
                return truewinddir;
            }
        }

        public double RWS
        {
            get {return vesRWS;}
            set { vesRWS = value; }
        }

        public double curLat
        {
            get { return vesCurLoc.Latitude; }
        }

        public double curLong
        {
            get { return vesCurLoc.Longitude; }
        }
        public double OrgLat
        {
            get { return vesOrgLoc.Latitude; }
        }

        public double OrgLong
        {
            get { return vesOrgLoc.Longitude; }
        }
        public double CurtoOrgDistance
        {
            get 
            {
                if ((vesCurLoc.IsUnknown) || (vesOrgLoc.IsUnknown)) return 0;
                else return vesCurLoc.GetDistanceTo(vesOrgLoc);
            }
        }
        public double STW
        {
            get { return vesSTW; }
            set { vesSTW = value; }
        }
        public double ROT
        {
            get { return vesROT; }
            set { vesROT = value; }
        }
        public double WTemp
        {
            get { return vesWTemp; }
            set { vesWTemp = value; }
        }
        public double ATemp
        {
            get { return vesATemp; }
            set { vesATemp = value; }
        }
        public double LogWaterTotal
        {
            get { return vesLogWaterTotal; }
            set { vesLogWaterTotal = value; }
        }
        public double LogWaterTrip
        {
            get { return vesLogWaterTrip; }
            set { vesLogWaterTrip = value; }
        }

        public double XTE
        {
            get { return vesXTE;}
            set { vesXTE = value; }
        }
        public double TWPID
        {
            get { return vesTargetWaypointID; }
            set { vesTargetWaypointID = value; }
        }
        public double TWOrigCrstoWP
        {
            get { return vesOrigCourseToWaypoint; }
            set { vesOrigCourseToWaypoint = value; }
        }
        public double TWCurBeartoWP
        {
            get { return vesCurrentBearingToWaypoint; }
            set { vesCurrentBearingToWaypoint = value; }
        }
        public double TWCurHeadtoWP
        {
            get { return vesCurrentHeadingtoSteerToWaypoint; }
            set { vesCurrentHeadingtoSteerToWaypoint = value; }
        }
 
        public void SetCurPos(Double Lat, double Long)
        {
            vesCurLoc.Latitude=Lat;
            vesCurLoc.Longitude = Long;
            if (vesOrgLoc.IsUnknown)
            {
                vesOrgLoc.Latitude = Lat;
                vesOrgLoc.Longitude = Long;
            }

        }
    
    }
}
