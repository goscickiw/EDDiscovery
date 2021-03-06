﻿/*
 * Copyright © 2019 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore
{
    public class RoutePlotter
    {
        public bool StopPlotter { get; set; } = false;
        public float MaxRange;
        public Point3D Coordsfrom;
        public Point3D Coordsto;
        public string FromSystem;
        public string ToSystem;
        public int RouteMethod;

        // METRICs defined by systemclass GetSystemNearestTo function
        public static string[] metric_options = {
            "Nearest to Waypoint",
            "Minimum Deviation from Path",
            "Nearest to Waypoint with dev<=100ly",
            "Nearest to Waypoint with dev<=250ly",
            "Nearest to Waypoint with dev<=500ly",
            "Nearest to Waypoint + Deviation / 2"
        };

        public class ReturnInfo
        {
            public string name;             // always
            public double dist;             // always
            public Point3D pos;             // 3dpos can be null
            public double waypointdist;     // can be Nan
            public double deviation;        // can be Nan
            public ISystem system;          // only if its a real system

            public ReturnInfo(string s, double d, Point3D p = null, double way = double.NaN, double dev = double.NaN, ISystem sys = null)
            {
                name = s;dist = d;pos = p;waypointdist = way;deviation = dev; system = sys;
            }
        }

        public List<ISystem> RouteIterative(Action<ReturnInfo> info)
        {
            double traveldistance = Point3D.DistanceBetween(Coordsfrom, Coordsto);      // its based on a percentage of the traveldistance
            List<ISystem> routeSystems = new List<ISystem>();
            System.Diagnostics.Debug.WriteLine("From " + FromSystem + " to  " + ToSystem);

            routeSystems.Add(new SystemClass(FromSystem, Coordsfrom.X, Coordsfrom.Y, Coordsfrom.Z));

            info(new ReturnInfo(FromSystem, double.NaN, Coordsfrom,double.NaN,double.NaN,routeSystems[0]));

            Point3D curpos = Coordsfrom;
            int jump = 1;
            double actualdistance = 0;

            float maxfromwanted = (MaxRange<100) ? (MaxRange-1) : (100+MaxRange * 1 / 5);       // if <100, then just make sure we jump off by 1 yr, else its a 100+1/5

            do
            {
                double distancetogo = Point3D.DistanceBetween(Coordsto, curpos);      // to go

                if (distancetogo <= MaxRange)                                         // within distance, we can go directly
                    break;

                Point3D travelvector = new Point3D(Coordsto.X - curpos.X, Coordsto.Y - curpos.Y, Coordsto.Z - curpos.Z); // vector to destination
                Point3D travelvectorperly = new Point3D(travelvector.X / distancetogo, travelvector.Y / distancetogo, travelvector.Z / distancetogo); // per ly travel vector

                Point3D nextpos = new Point3D(curpos.X + MaxRange * travelvectorperly.X,
                                              curpos.Y + MaxRange * travelvectorperly.Y,
                                              curpos.Z + MaxRange * travelvectorperly.Z);   // where we would like to be..
                

                ISystem bestsystem = DB.SystemCache.GetSystemNearestTo(curpos, nextpos, MaxRange, maxfromwanted, RouteMethod, 1000);     // at least get 1/4 way there, otherwise waypoint.  Best 1000 from waypoint checked

                string sysname = "WAYPOINT";
                double deltafromwaypoint = 0;
                double deviation = 0;

                if (bestsystem != null)
                {
                    Point3D bestposition = new Point3D(bestsystem.X, bestsystem.Y, bestsystem.Z);
                    deltafromwaypoint = Point3D.DistanceBetween(bestposition, nextpos);     // how much in error
                    deviation = Point3D.DistanceBetween(curpos.InterceptPoint(nextpos, bestposition), bestposition);
                    nextpos = bestposition;
                    sysname = bestsystem.Name;
                    routeSystems.Add(bestsystem);
                }

                info(new ReturnInfo(sysname, Point3D.DistanceBetween(curpos, nextpos), nextpos, deltafromwaypoint, deviation , bestsystem));

                actualdistance += Point3D.DistanceBetween(curpos, nextpos);
                curpos = nextpos;
                jump++;

            } while ( !StopPlotter);

            routeSystems.Add(new SystemClass(ToSystem, Coordsto.X, Coordsto.Y, Coordsto.Z));

            actualdistance += Point3D.DistanceBetween(curpos, Coordsto);

            info(new ReturnInfo(ToSystem, Point3D.DistanceBetween(curpos, Coordsto), Coordsto, double.NaN, double.NaN, routeSystems.Last()));

            info(new ReturnInfo("Straight Line Distance", traveldistance));
            info(new ReturnInfo("Travelled Distance", actualdistance));

            return routeSystems;
        }
    }

}
