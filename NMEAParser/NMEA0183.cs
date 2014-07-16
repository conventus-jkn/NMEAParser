using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;



namespace NMEAParser
{

    class NMEA0183
    {
        internal static void parseNmea0183(string message, Vessel pinniped, DateTime messagetime)
        {
            if (parseNmea0183CheckSum(message))
            { 
                string[] nmeaparam = message.Split(',');
                string nmeatalker, nmeacode;
                if (nmeaparam[0].Substring(1, 1)=="P")
                {
                       nmeatalker = nmeaparam[0].Substring(1, 1);
                       nmeacode = nmeaparam[0].Substring(2);
                }
                else
                {
                    nmeatalker = nmeaparam[0].Substring(1, 2);
                    nmeacode = nmeaparam[0].Substring(3);
                }
                string nmeamessage = message.Substring(7);
                switch (nmeacode)
                {
                    case "RMC":
                        //                Console.Clear();

                        Console.SetCursorPosition(0, 0);
                        DateTime fixtime;
                        
                        parseNmea0183RMC(message, pinniped,messagetime,out fixtime);

                        Console.WriteLine("Location: {0:f4} {1:f4}, SOG: {2:f1} m/s, COG: {3:f0}°", pinniped.curLat, pinniped.curLong, pinniped.SOG, pinniped.COG);
                        Console.WriteLine("Distance from Original Loc ({0:f4} {1:f4}) {2:f0}m", pinniped.OrgLat, pinniped.OrgLong, pinniped.CurtoOrgDistance);
                        Console.SetCursorPosition(0, 12);
                        Console.WriteLine("FixTime: {0}, internal clock delta:{1}", fixtime, fixtime - messagetime);

                        //                if (pinniped.CurtoOrgDistance > 10) Console.Write("\a");
                        break;
                    case "DBT":
                        parseNmea0183DBT(message, pinniped);
                        Console.SetCursorPosition(0, 2);
                        Console.WriteLine("Depth below keel: {0:f1}m, below surface {1:f1}m", pinniped.DBK, pinniped.DBS);
                        break;
                    case "DPT":
                        parseNmea0183DPT(message, pinniped);
                        Console.SetCursorPosition(0, 2);
                        Console.WriteLine("Depth below keel: {0:f1}m, below surface {1:f1}m", pinniped.DBK, pinniped.DBS);
                        break;
                    case "HDG":
                        parseNmea0183HDG(message, pinniped);
                        Console.SetCursorPosition(0, 3);
                        Console.WriteLine("Heading: {0:f0}°", pinniped.HDG);
                        break;
                    case "MWV":
                        parseNmea0183MVW(message, pinniped);
                        Console.SetCursorPosition(0, 4);
                        Console.WriteLine("Relative Wind Direction: {0:f0}°, Strength: {1:f1} m/s", pinniped.RWD, pinniped.RWS);
                        Console.WriteLine("True Wind Direction: {0:f0}°, Strength: {1:f1} m/s", pinniped.TWD, pinniped.RWS);
                        break;
                    case "ROT":
                        parseNmea0183ROT(message, pinniped);
                        Console.SetCursorPosition(0, 6);
                        Console.WriteLine("Rate of Turn: {0:f0}°/min", pinniped.ROT);
                        break;
                    case "MTW":
                        parseNmea0183MTW(message, pinniped);
                        Console.SetCursorPosition(0, 7);
                        Console.WriteLine("Water Temp: {0:f1}°c, Air Temp {1:f1}°c", pinniped.WTemp, pinniped.ATemp);
                        break;
                    case "MDA":
                        parseNmea0183MDA(message, pinniped);
                        Console.SetCursorPosition(0, 7);
                        Console.WriteLine("Water Temp: {0:f1}°c, Air Temp {1:f1}°c", pinniped.WTemp, pinniped.ATemp);
                        break;
                    case "VLW":
                        parseNmea0183VLW(message, pinniped);
                        Console.SetCursorPosition(0, 8);
                        Console.WriteLine("Water Log Total: {0:f0}nm, Trip: {1:f0}nm", pinniped.LogWaterTotal, pinniped.LogWaterTotal);
                        break;
                    case "VHW":
                        parseNmea0183VHW(message, pinniped);
                        Console.SetCursorPosition(0, 3);
                        Console.WriteLine("Heading: {0:f0}°", pinniped.HDG);
                        Console.SetCursorPosition(0, 9);
                        Console.WriteLine("Current Speed through water: {0:f0} knots", pinniped.STW);
                        break;
                    case "APB":
                        parseNmea0183APB(message, pinniped);
                        Console.SetCursorPosition(0, 11);
                        Console.WriteLine("Wp ID: {0}, Org BrtoWP: {1:f0}, Cur BrtoWp: {2:f0}, Cur HdtoWP: {3:f0}, XTE: {4:f2}", pinniped.TWPID,pinniped.TWOrigCrstoWP,pinniped.TWCurBeartoWP,pinniped.TWCurHeadtoWP,pinniped.XTE);
                        break;
                    case "XTE":
                        parseNmea0183XTE(message, pinniped);
                        Console.SetCursorPosition(0, 11);
                        Console.WriteLine("Wp ID: {0}, Org BrtoWP: {1:f0}, Cur BrtoWp: {2:f0}, Cur HdtoWP: {3:f0}, XTE: {4:f2}", pinniped.TWPID,pinniped.TWOrigCrstoWP,pinniped.TWCurBeartoWP,pinniped.TWCurHeadtoWP,pinniped.XTE);
                        break;
                    case "VDO":
                        Console.SetCursorPosition(0, 14);
                        Console.WriteLine("Talker: {0}, Code: {1}, Message: {2}", nmeatalker, nmeacode, nmeamessage);
                        break;
                    case "VDM":
                        Console.SetCursorPosition(0, 15);
                        Console.WriteLine("Talker: {0}, Code: {1}, Message: {2}", nmeatalker, nmeacode, nmeamessage);
                        break;
                    case "RM":
                        Console.SetCursorPosition(0, 17);
                        Console.WriteLine("Talker: {0}, Code: {1}, Message: {2}", nmeatalker, nmeacode, nmeamessage);
                        break;
                    default:
                        Console.SetCursorPosition(0, 16);
                        Console.WriteLine("Talker: {0}, Code: {1}, Message: {2}", nmeatalker, nmeacode, nmeamessage);
                        break;
                }
            }

        }

        private static void parseNmea0183RMC(string message, NMEAParser.Vessel ves,DateTime messagetime, out DateTime fixtime)
        /* NMEA0183 Sentence: GP RMC (Recomended Minimum Specific GNSS Data)
         * Number of fields =13
         * Field 0: Talker + NMEA Sentence
         * Field 1: UTC of position fix
         * Field 2: Status
         * Field 3: Latitude
         * Field 4: N or S
         * Field 5: Longditude
         * Field 6: E or W
         * Field 7: Speed over ground
         * Field 8: Course of ground
         * Field 9: ddmmyy
         * Field 10: Magnetic variation
         * Field 11: E or W
         * Field 12: Positiong system mode indicator
         * Field 13: CheckSum
         */
            
            //parse GPS data to get location, vesSOG and course.
        {
            fixtime = DateTime.MinValue;
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 13)
            {
                //check if valid fix
                if (nmeaparam[2] == "A")
                {
                    //set fix time to time reurned by gps
                    if ((nmeaparam[1].Length > 0) && (nmeaparam[9].Length > 0))
                        fixtime = new DateTime(2000 + int.Parse(nmeaparam[9].Substring(4, 2)), int.Parse(nmeaparam[9].Substring(2, 2)), int.Parse(nmeaparam[9].Substring(0, 2)), int.Parse(nmeaparam[1].Substring(0, 2)), int.Parse(nmeaparam[1].Substring(2, 2)), int.Parse(nmeaparam[1].Substring(4, 2)));
                    TimeSpan fixtimedelta = fixtime - messagetime;
                    if ((fixtimedelta > TimeSpan.FromSeconds(-120)) && (fixtimedelta < TimeSpan.FromSeconds(120)))
                    {
                        double lat = 0, longd = 0;
                        if (nmeaparam[3].Length > 0) lat = double.Parse(nmeaparam[3].Substring(0, 2)) + double.Parse(nmeaparam[3].Substring(2)) / 60;
                        if (nmeaparam[4].Length > 0) if (nmeaparam[4].StartsWith("S")) lat = lat * -1;
                        if (nmeaparam[5].Length > 0) longd = double.Parse(nmeaparam[5].Substring(0, 3)) + double.Parse(nmeaparam[5].Substring(3)) / 60; ;
                        if (nmeaparam[6].Length > 0) if (nmeaparam[6].StartsWith("W")) longd = longd * -1;

                        ves.SetCurPos(lat, longd);
                        if (nmeaparam[7].Length > 0) ves.SOG = double.Parse(nmeaparam[7]) * 0.5144444446;
                        if (nmeaparam[8].Length > 0) ves.COG = double.Parse(nmeaparam[8]);
                    }
                }
            }
        }

        private static void parseNmea0183DBT(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: SD DBT (Depth Below Transducer)
             * Number of fields = 7
             * Field 0: Talker + NMEA Sentence
             * Field 1: Depth
             * Field 2: Feet
             * Field 3: Depth
             * Field 4: Meters
             * Field 5: Depth
             * Field 6: Fathoms
             * Field 7: CheckSum
             */
            //sets depth below tranducer in m.
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 7)
                if (nmeaparam[3].Length > 0) ves.DBT = double.Parse(nmeaparam[3]);
        }

        private static void parseNmea0183DPT(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: SD DPT (Depth)
             * Number of fields = 4
             * Field 0: Talker + NMEA Sentence
             * Field 1: Depth
             * Field 2: Offset (+ve to waterline -ve to keel)
             * Field 3: Range
             * Field 4: CheckSum
             */
            //sets depth below tranducer in M.
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 4) 
                if (nmeaparam[1].Length>0) ves.DBT = double.Parse(nmeaparam[1]);
        }

        private static void parseNmea0183HDG(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: HC HDG (Heading, Deviation & Variation)
             * Number of fields = 6
             * Field 0: Talker + NMEA Sentence
             * Field 1: Magnetic sensor heading 
             * Field 2: Magnetic deviation
             * Field 3: E or W
             * Field 4: Magnetic variation
             * Field 5: E or W
             * Field 6: Checksum
             */
            //returns current heading
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 6)
                if (nmeaparam[1].Length > 0) ves.HDG = double.Parse(nmeaparam[1]);
        }

        private static void parseNmea0183MVW(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: II MVW (Wind Speed and Angle )
             * Number of fields = 6
             * Field 0: Talker + NMEA Sentence
             * Field 1: Wind Angle 
             * Field 2: R or T
             * Field 3: Wind Speed
             * Field 4: 
             * Field 5: 
             * Field 6: Checksum
             */
            //sets current wind
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 6)
            {
                if (nmeaparam[1].Length > 0) ves.RWD = double.Parse(nmeaparam[1]);
                if (nmeaparam[3].Length > 0) ves.RWS = double.Parse(nmeaparam[3]);
            }
        }

        private static void parseNmea0183ROT(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: II ROT (Rate of Turn )
             * Number of fields = 3
             * Field 0: Talker + NMEA Sentence
             * Field 1: Rate in Degrees/Min
             * Field 2: Checksum
             */
            //sets current wind
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 3)
                if (nmeaparam[1].Length > 0) ves.ROT = double.Parse(nmeaparam[1]);
        }

        private static void parseNmea0183MTW(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: II MDATW (Water Temperature )
             * Number of fields = 3
             * Field 0: Talker + NMEA Sentence
             * Field 1: Water Temperature in C
             * Field 2: Checksum
             */
            //sets current wind
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 3)
                if (nmeaparam[1].Length > 0) ves.WTemp = double.Parse(nmeaparam[1]);
        }

        private static void parseNmea0183MDA(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: II MDA (Meterological Composite )
             * Number of fields = 21
             * Field 0: Talker + NMEA Sentence
             * Field 1: Barometic Pressure, Inches Hg
             * Field 2: Inches Hg
             * Field 3: Barometic Pressure
             * Field 4: Bar
             * Field 5: Air Temperature
             * Field 6: c
             * Field 7: Water Temperature
             * Field 8: c
             * Field 9: Relative Humidity
             * Field 10: Absolute Humidity
             * Field 11: Due point
             * Field 12: c
             * Field 13: Wind direction
             * Field 14: True
             * Field 15: Window direction
             * Field 16: Mangetic
             * Field 17: Wind speed
             * Field 18: Knots
             * Field 19: Wind speed
             * Field 20: m/s
             * Field 21: Checksum
             */
            //sets current wind
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 21)
            {
                if (nmeaparam[7].Length > 0) ves.WTemp = double.Parse(nmeaparam[7]);
                if (nmeaparam[5].Length > 0) ves.ATemp = double.Parse(nmeaparam[5]);
            }
        }

        private static void parseNmea0183VLW(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: II VLW (Dual Ground / Water Distance)
             * Number of fields = 9
             * Field 0: Talker + NMEA Sentence
             * Field 1: Total cumulative water distance
             * Field 2: nautical miles
             * Field 3: Water distance since reset
             * Field 4: nautical miles
             * Field 5: Total cumulative ground distance
             * Field 6: nautical miles
             * Field 7: Ground distance since reset
             * Field 8: nautical miles
             * Field 9: CheckSum
             */
            //sets water log in nm.
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 9)
            {
                if (nmeaparam[1].Length > 0) ves.LogWaterTotal = double.Parse(nmeaparam[1]);
                if (nmeaparam[3].Length > 0) ves.LogWaterTrip = double.Parse(nmeaparam[3]);
            }
        }
        private static void parseNmea0183VHW(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: II VLW (Water Speen and Heading)
             * Number of fields = 9
             * Field 0: Talker + NMEA Sentence
             * Field 1: Heading
             * Field 2: Deg T
             * Field 3: Heading
             * Field 4: Deg M
             * Field 5: Water Speed
             * Field 6: knots
             * Field 7: Water speed
             * Field 8: kmh
             * Field 9: CheckSum
             */
            //sets water heading and water speed.
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 9)
            {
                if (nmeaparam[3].Length > 0) ves.HDG = double.Parse(nmeaparam[3]);
                if (nmeaparam[5].Length > 0) ves.STW = double.Parse(nmeaparam[5]);
            }
        }
        private static void parseNmea0183APB(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: LC APB (Heading / Track Controller (Autopilot) Sentence "B" )
             * Number of fields = 16
             * Field 0: Talker + NMEA Sentence
             * Field 1: Loran Warning, General Warning
             * Field 2: Loran?
             * Field 3: Magniute of Cross track error
             * Field 4: Steer L or R to get back on track
             * Field 5: Units (N nautical miles)
             * Field 6: Arival status 
             * Field 7: Perpendicular passed at waypoint
             * Field 8: Bearing origin to destinaion
             * Field 9: M or T
             * Field 10: Destination Waypoint ID
             * Field 11: Bearing, present position to destination
             * Field 12: M or T
             * Field 13: Heading to Steer to destination waypoint
             * Field 14: M or T
             * Field 15: Mode indicator
             * Field 16: Checksum
             */
            //sets current wind
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 16)
            {
                if (nmeaparam[3].Length > 0) ves.XTE = double.Parse(nmeaparam[3]);
                if (nmeaparam[4] == "L") ves.XTE = ves.XTE * -1;
                if (nmeaparam[8].Length > 0) ves.TWOrigCrstoWP = double.Parse(nmeaparam[8]);
                if (nmeaparam[11].Length > 0) ves.TWCurBeartoWP = double.Parse(nmeaparam[11]);
                if (nmeaparam[13].Length > 0) ves.TWCurHeadtoWP = double.Parse(nmeaparam[13]);
            }
        }
        private static void parseNmea0183XTE(string message, NMEAParser.Vessel ves)
        {
            /* NMEA0183 Sentence: LC APB (Heading / Track Controller (Autopilot) Sentence "B" )
             * Number of fields = 7
             * Field 0: Talker + NMEA Sentence
             * Field 1: Loran or general warning
             * Field 2: Loran ?
             * Field 3: Magniute of Cross track error
             * Field 4: Steer L or R to get back on track
             * Field 5: Units (N nautical miles)
             * Field 6: Mode indicator
             * Field 7: Checksum
             */
            //sets current wind
            string[] nmeaparam = message.Split(',');
            if (nmeaparam.Length == 7)
            {
                if (nmeaparam[3].Length > 0) ves.XTE = double.Parse(nmeaparam[3]);
                if (nmeaparam[4] == "L") ves.XTE = ves.XTE * -1;
            }
        }
        private static bool parseNmea0183CheckSum(string message)
        {
            message = message.Trim();
            string[] nmeaparam = message.Split('*');
            if (nmeaparam.Length==2)
            {
                string mess=nmeaparam[0];
                int chsum = mess[0];
                foreach (int element  in mess)
                {
                    chsum ^= element;
                }
                //Console.SetCursorPosition(0, 18);
                //Console.WriteLine("Provided Checksum: {0}, Calculated {1:x}",Convert.ToInt32(nmeaparam[1],16), chsum);
                if (Convert.ToInt32(nmeaparam[1], 16) == chsum) return true;
                else return false;
            }
            else
                return false;
        }

    }
}
