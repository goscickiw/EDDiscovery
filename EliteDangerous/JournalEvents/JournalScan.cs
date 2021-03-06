﻿/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Scan)]
    public class JournalScan : JournalEntry
    {
        public bool IsStar { get { return !String.IsNullOrEmpty(StarType); } }
        public string BodyDesignation { get; set; }

        // ALL
        public string ScanType { get; set; }                        // 3.0 scan type  Basic, Detailed, NavBeacon, NavBeaconDetail, (3.3) AutoScan, or empty for older ones
        public string BodyName { get; set; }                        // direct (meaning no translation)
        public int? BodyID { get; set; }                            // direct
        public double DistanceFromArrivalLS { get; set; }           // direct
        public string DistanceFromArrivalText { get { return string.Format("{0:0.00}AU ({1:0.0}ls)", DistanceFromArrivalLS / JournalScan.oneAU_LS, DistanceFromArrivalLS); } }
        public double? nRotationPeriod { get; set; }                // direct
        public double? nRotationPeriodDays { get; set; }      
        public double? nSurfaceTemperature { get; set; }            // direct
        public double? nRadius { get; set; }                        // direct
        public double? nRadiusSols { get; set; }
        public double? nRadiusEarths { get; set; }
        public bool HasRings { get { return Rings != null && Rings.Length > 0; } }
        public StarPlanetRing[] Rings { get; set; }
        public List<BodyParent> Parents { get; set; }
        public bool? WasDiscovered { get; set; }                    // direct, 3.4, indicates whether the body has already been discovered
        public bool? WasMapped { get; set; }                        // direct, 3.4, indicates whether the body has already been mapped

        // STAR
        public string StarType { get; set; }                        // null if no StarType, direct from journal, K, A, B etc
        public EDStar StarTypeID { get; }                           // star type -> identifier
        public string StarTypeText { get { return IsStar ? GetStarTypeName() : ""; } }   // Long form star name, from StarTypeID
        public double? nStellarMass { get; set; }                   // direct
        public double? nAbsoluteMagnitude { get; set; }             // direct
        public string Luminosity { get; set; }
        public double? nAge { get; set; }                           // direct
        public double? HabitableZoneInner { get; set; }             // calculated
        public double? HabitableZoneOuter { get; set; }             // calculated
        public double? MetalRichZoneInner { get; set; }
        public double? MetalRichZoneOuter { get; set; }
        public double? WaterWrldZoneInner { get; set; }
        public double? WaterWrldZoneOuter { get; set; }
        public double? EarthLikeZoneInner { get; set; }
        public double? EarthLikeZoneOuter { get; set; }
        public double? AmmonWrldZoneInner { get; set; }
        public double? AmmonWrldZoneOuter { get; set; }
        public double? IcyPlanetZoneInner { get; set; }
        public string IcyPlanetZoneOuter { get; set; }

        // All orbiting bodies (Stars/Planets), not main star
        public double? nSemiMajorAxis { get; set; }                              // direct
        public double? nSemiMajorAxisAU { get; set; }                            // direct
        public double? nEccentricity { get; set; }                               // direct
        public double? nOrbitalInclination { get; set; }                         // direct
        public double? nPeriapsis { get; set; }                                  // direct
        public double? nOrbitalPeriod { get; set; }                 // direct
        public double? nOrbitalPeriodDays { get; set; }
        public double? nAxialTilt { get; set; }                     // direct, radians
        public double? nAxialTiltDeg { get; set; }      
        public int? StarSubclass { get; set; }                      // star Subclass, direct, 3.4

        // Planets
        public string PlanetClass { get; set; }                     // planet class, direct

        public EDPlanet PlanetTypeID { get; }                       // planet class -> ID
        public bool AmmoniaWorld { get { return PlanetTypeID == EDPlanet.Ammonia_world; } }
        public bool Earthlike { get { return PlanetTypeID == EDPlanet.Earthlike_body; } }
        public bool WaterWorld { get { return PlanetTypeID == EDPlanet.Water_world; } }

        public bool? nTidalLock { get; set; }                       // direct
        public string TerraformState { get; set; }                  // direct, can be empty or a string
        public bool Terraformable { get { return TerraformState != null && TerraformState.ToLowerInvariant().Equals("terraformable"); } }
        public string Atmosphere { get; set; }                      // direct from journal, if not there or blank, tries AtmosphereType (Earthlikes)
        public EDAtmosphereType AtmosphereID { get; }               // Atmosphere -> ID (Ammonia, Carbon etc)
        public EDAtmosphereProperty AtmosphereProperty { get; set; }             // Atomsphere -> Property (None, Rich, Thick , Thin, Hot)
        public bool HasAtmosphericComposition { get { return AtmosphereComposition != null && AtmosphereComposition.Any(); } }
        public Dictionary<string, double> AtmosphereComposition { get; set; }
        public Dictionary<string, double> PlanetComposition { get; set; }
        public bool HasPlanetaryComposition { get { return PlanetComposition != null && PlanetComposition.Any(); } }

        public string Volcanism { get; set; }                       // direct from journal
        public EDVolcanism VolcanismID { get; }                     // Volcanism -> ID (Water_Magma, Nitrogen_Magma etc)
        public bool HasMeaningfulVolcanism { get { return VolcanismID != EDVolcanism.None && VolcanismID != EDVolcanism.Unknown; } }
        public EDVolcanismProperty VolcanismProperty { get; set; }               // Volcanism -> Property (None, Major, Minor)
        public double? nSurfaceGravity { get; set; }                // direct
        public double? nSurfaceGravityG { get; set; }
        public double? nSurfacePressure { get; set; }               // direct
        public double? nSurfacePressureEarth { get; set; }   
        public bool? nLandable { get; set; }                        // direct
        public bool IsLandable { get { return nLandable.HasValue && nLandable.Value; } }
        public double? nMassEM { get; set; }                        // direct, not in description of event, mass in EMs
        public double? nMassMM { get; set; }                        // only if nMassEM is set, moon mass in MMs

        public bool HasMaterials { get { return Materials != null && Materials.Any(); } }
        public Dictionary<string, double> Materials { get; set; }       // fdname and name is the same for materials on planets.  name is lower case
        public bool HasMaterial(string name) { return Materials != null && Materials.ContainsKey(name.ToLowerInvariant()); }
        public string MaterialList { get; private set; }                         // material list, names, comma separ, or null

        public bool IsEDSMBody { get; private set; }
        public string EDSMDiscoveryCommander { get; private set; }      // may be null if not known
        public DateTime EDSMDiscoveryUTC { get; private set; }

        public EDReserve ReserveLevel { get; set; }
        public string ReserveLevelStr
        {
            get
            {
                return ReserveLevel.ToString();
            }
            set
            {
                ReserveLevel = Bodies.ReserveStr2Enum(value);
            }
        }

        public bool Mapped { get { return mapped;}  }                          // affects prices
        public bool EfficientMapped { get { return efficientmapped; } }                          // affects prices
        public int EstimatedValue { get; private set; }           // Current Estimated Value
        public int EstimatedValueFirstDiscoveredFirstMapped { get; private set; }           // 0 if not mapped
        public int EstimatedValueFirstMapped { get; private set; }                          // 0 if not mapped

        public void SetMapped(bool m, bool e)
        {
            mapped = m; efficientmapped = e; EstimateScanValue(); 
        }

        private bool mapped, efficientmapped;

        // Constants:

        // stellar references
        public const double oneSolRadius_m = 695700000; // 695,700km

        // planetary bodies
        public const double oneEarthRadius_m = 6371000;
        public const double oneAtmosphere_Pa = 101325;
        public const double oneGee_m_s2 = 9.80665;
        public const double oneEarth_MT = 5.972e15;        // mega tons, 1 meta ton = 1e6 tons = 1e9 kg (google 5.972e21 tons)
        public const double oneMoon_MT = 7.34767309e13;     // mega tons, 1 meta ton = 1e6 tons = 1e9 kg
        public const double EarthMoonMassRatio = oneEarth_MT/oneMoon_MT; 

        // astrometric
        public const double oneLS_m = 299792458;
        public const double oneAU_m = 149597870700;
        public const double oneAU_LS = oneAU_m / oneLS_m;
        public const double oneDay_s = 86400;

        public class StarPlanetRing
        {
            public string Name;     // may be null
            public string RingClass;    // may be null
            public double MassMT;
            public double InnerRad;
            public double OuterRad;

            public string RingInformation(double scale = 1, string scaletype = " MT", bool parentIsStar = false)
            {
                StringBuilder scanText = new StringBuilder();
                scanText.AppendFormat("  {0} ({1})\n", Name.Alt("Unknown".T(EDTx.Unknown)), DisplayStringFromRingClass(RingClass));
                scanText.AppendFormat("  Mass: {0:N4}{1}\n".T(EDTx.StarPlanetRing_Mass), MassMT * scale, scaletype);
                if (parentIsStar && InnerRad > 3000000)
                {
                    scanText.AppendFormat("  Inner Radius: {0:0.00}ls\n".T(EDTx.StarPlanetRing_InnerRadius), (InnerRad / oneLS_m));
                    scanText.AppendFormat("  Outer Radius: {0:0.00}ls\n".T(EDTx.StarPlanetRing_OuterRadius), (OuterRad / oneLS_m));
                }
                else
                {
                    scanText.AppendFormat("  Inner Radius: {0}km\n".T(EDTx.StarPlanetRing_IK), (InnerRad / 1000).ToString("N0"));
                    scanText.AppendFormat("  Outer Radius: {0}km\n".T(EDTx.StarPlanetRing_OK), (OuterRad / 1000).ToString("N0"));
                }
                return scanText.ToNullSafeString();
            }

            public string RingInformationMoons(bool parentIsStar = false)
            {
                return RingInformation(1 / oneMoon_MT, " Moons".T(EDTx.StarPlanetRing_Moons), parentIsStar);
            }

            public static string DisplayStringFromRingClass(string ringClass)
            {
                switch (ringClass)
                {
                    case null:
                        return "Unknown".T(EDTx.Unknown);
                    case "eRingClass_Icy":
                        return "Icy".T(EDTx.StarPlanetRing_Icy);
                    case "eRingClass_Rocky":
                        return "Rocky".T(EDTx.StarPlanetRing_Rocky);
                    case "eRingClass_MetalRich":
                        return "Metal Rich".T(EDTx.StarPlanetRing_MetalRich);
                    case "eRingClass_Metalic":
                        return "Metallic".T(EDTx.StarPlanetRing_Metallic);
                    case "eRingClass_RockyIce":
                        return "Rocky Ice".T(EDTx.StarPlanetRing_RockyIce);
                    default:
                        return ringClass.Replace("eRingClass_", "");
                }
            }
        }

        public class BodyParent
        {
            public string Type;
            public int BodyID;
        }

        public JournalScan(JObject evt, long edsmid) : this(evt)
        {
            EdsmID = edsmid;
        }

        public JournalScan(JObject evt) : base(evt, JournalTypeEnum.Scan)
        {
            ScanType = evt["ScanType"].Str();
            BodyName = evt["BodyName"].Str();
            BodyID = evt["BodyID"].IntNull();
            StarType = evt["StarType"].StrNull();

            DistanceFromArrivalLS = evt["DistanceFromArrivalLS"].Double();

            nAge = evt["Age_MY"].DoubleNull();
            nStellarMass = evt["StellarMass"].DoubleNull();
            nRadius = evt["Radius"].DoubleNull();
            if (nRadius != null)
            {
                nRadiusSols = nRadius.Value / oneSolRadius_m;
                nRadiusEarths = nRadius.Value / oneEarthRadius_m;
            }
            nAbsoluteMagnitude = evt["AbsoluteMagnitude"].DoubleNull();
            Luminosity = evt["Luminosity"].StrNull();

            nRotationPeriod = evt["RotationPeriod"].DoubleNull();
            if (nRotationPeriod != null)
                nRotationPeriodDays = nRotationPeriod.Value / oneDay_s;

            nOrbitalPeriod = evt["OrbitalPeriod"].DoubleNull();
            if (nOrbitalPeriod != null)
                nOrbitalPeriodDays = nOrbitalPeriod.Value / oneDay_s;

            nSemiMajorAxis = evt["SemiMajorAxis"].DoubleNull();
            if (nSemiMajorAxis != null)
                nSemiMajorAxisAU = nSemiMajorAxis.Value / oneAU_m;

            nEccentricity = evt["Eccentricity"].DoubleNull();
            nOrbitalInclination = evt["OrbitalInclination"].DoubleNull();
            nPeriapsis = evt["Periapsis"].DoubleNull();
            nAxialTilt = evt["AxialTilt"].DoubleNull();
            if ( nAxialTilt!= null)
                nAxialTiltDeg = nAxialTilt.Value * 180.0 / Math.PI;

            Rings = evt["Rings"]?.ToObjectProtected<StarPlanetRing[]>(); // may be Null

            nTidalLock = evt["TidalLock"].Bool();
            TerraformState = evt["TerraformState"].StrNull();
            if (TerraformState != null && TerraformState.Equals("Not Terraformable", StringComparison.InvariantCultureIgnoreCase)) // EDSM returns this, normalise to journal
                TerraformState = String.Empty;
            PlanetClass = evt["PlanetClass"].StrNull();

            Atmosphere = evt["Atmosphere"].StrNull();
            if (Atmosphere == null || Atmosphere.Length == 0)             // Earthlikes appear to have empty atmospheres but AtmosphereType
                Atmosphere = evt["AtmosphereType"].StrNull();
            if (Atmosphere != null)
                Atmosphere = Atmosphere.SplitCapsWordFull();

            AtmosphereID = Bodies.AtmosphereStr2Enum(Atmosphere, out EDAtmosphereProperty ap);
            AtmosphereProperty = ap;

            Volcanism = evt["Volcanism"].StrNull();
            VolcanismID = Bodies.VolcanismStr2Enum(Volcanism, out EDVolcanismProperty vp);
            VolcanismProperty = vp;

            nMassEM = evt["MassEM"].DoubleNull();
            if (nMassEM.HasValue)
                nMassMM = nMassEM.Value * EarthMoonMassRatio;
            nSurfaceGravity = evt["SurfaceGravity"].DoubleNull();
            if (nSurfaceGravity.HasValue)
                nSurfaceGravityG = nSurfaceGravity / oneGee_m_s2;
            nSurfaceTemperature = evt["SurfaceTemperature"].DoubleNull();
            nSurfacePressure = evt["SurfacePressure"].DoubleNull();
            if (nSurfacePressure != null)
                nSurfacePressureEarth = nSurfacePressure.Value / oneAtmosphere_Pa;

            nLandable = evt["Landable"].BoolNull();

            WasMapped = evt["WasMapped"].BoolNull();            // new 3.4
            WasDiscovered = evt["WasDiscovered"].BoolNull();        // new 3.4

            ReserveLevelStr = evt["ReserveLevel"].Str();

            if (IsStar)
            {
                StarTypeID = Bodies.StarStr2Enum(StarType);
                StarSubclass = evt["Subclass"].IntNull();

                if (nRadius.HasValue && nSurfaceTemperature.HasValue)
                {
                    // values initially calculated by Jackie Silver (https://forums.frontier.co.uk/member.php/37962-Jackie-Silver)

                    HabitableZoneInner = DistanceForBlackBodyTemperature(315); // this is the goldilocks zone, where is possible to expect to find planets with liquid water. 
                    HabitableZoneOuter = DistanceForBlackBodyTemperature(223);
                    MetalRichZoneInner = DistanceForNoMaxTemperatureBody(oneSolRadius_m); // we don't know the maximum temperature that the galaxy simulation take as possible...
                    MetalRichZoneOuter = DistanceForBlackBodyTemperature(1100);
                    WaterWrldZoneInner = DistanceForBlackBodyTemperature(307);
                    WaterWrldZoneOuter = DistanceForBlackBodyTemperature(156);
                    EarthLikeZoneInner = DistanceForBlackBodyTemperature(281); // I enlarged a bit the range to fit my and other CMDRs discoveries.
                    EarthLikeZoneOuter = DistanceForBlackBodyTemperature(227);
                    AmmonWrldZoneInner = DistanceForBlackBodyTemperature(193);
                    AmmonWrldZoneOuter = DistanceForBlackBodyTemperature(117);
                    IcyPlanetZoneInner = DistanceForBlackBodyTemperature(150);
                    IcyPlanetZoneOuter = "\u221E"; // practically infinite, at least until the body suffer from the gravitational bond with its host star
                }
            }
            else if (PlanetClass != null)
            {
                PlanetTypeID = Bodies.PlanetStr2Enum(PlanetClass);
                // Fix naming to standard and fix case..
                PlanetClass = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                        ToTitleCase(PlanetClass.ToLowerInvariant()).Replace("Ii ", "II ").Replace("Iv ", "IV ").Replace("Iii ", "III ");
            }
            else
            {
                PlanetTypeID = EDPlanet.Unknown_Body_Type;
            }

            JToken mats = (JToken)evt["Materials"];

            if (mats != null)
            {
                if (mats.Type == JTokenType.Object)
                {
                    Materials = mats?.ToObjectProtected<Dictionary<string, double>>();  // name in fd logs is lower case
                }
                else
                {
                    Materials = new Dictionary<string, double>();
                    foreach (JObject jo in mats)                                        // name in fd logs is lower case
                    {
                        Materials[jo["Name"].Str().ToLowerInvariant()] = jo["Percent"].Double();
                    }
                }

                var na = (from x in Materials select x.Key).ToArray();
                MaterialList = String.Join(",", na);
            }

            JToken atmos = (JToken)evt["AtmosphereComposition"];

            if (atmos != null)
            {
                if (atmos.Type == JTokenType.Object)
                {
                    AtmosphereComposition = atmos?.ToObjectProtected<Dictionary<string, double>>();
                }
                else
                {
                    AtmosphereComposition = new Dictionary<string, double>();
                    foreach (JObject jo in atmos)
                    {
                        AtmosphereComposition[(string)jo["Name"]] = jo["Percent"].Double();
                    }
                }
            }

            JToken composition = evt["Composition"];

            if (composition != null)
            {
                PlanetComposition = new Dictionary<string, double>();
                foreach (JProperty jp in composition)
                {
                    PlanetComposition[jp.Name] = (double)jp.Value;
                }
            }

            if (evt["Parents"] != null)
            {
                Parents = new List<BodyParent>();
                foreach (JObject parent in evt["Parents"])
                {
                    JProperty prop = parent.Properties().First();
                    Parents.Add(new BodyParent { Type = prop.Name, BodyID = prop.Value.Int() });
                }
            }

            // EDSM bodies fields

            IsEDSMBody = evt["EDDFromEDSMBodie"].Bool(false);           // Bodie? Who is bodie?  Did you mean Body Finwen ;-)

            JToken discovery = evt["discovery"];
            if (discovery != null)
            {
                EDSMDiscoveryCommander = discovery["commander"].StrNull();
                EDSMDiscoveryUTC = discovery["date"].DateTimeUTC();
            }

            EstimateScanValue();                        // do the basic no mapped no efficency map as the basic formula
        }

        #region Information Returns

        public string RadiusText()  // null if not set, or the best representation
        {
            if (nRadius != null)
            {
                if (nRadius >= oneSolRadius_m / 5)
                    return nRadiusSols.Value.ToString("0.#" + "SR");
                else
                    return (nRadius.Value / 1000).ToString("0.#") + "km";
            }
            else
                return null;
        }

        public string MassEMText()
        {
            if (nMassEM.HasValue)
            {
                if (nMassEM.Value < 0.01)
                    return nMassMM.Value.ToString("0.####") + "MM";
                else
                    return nMassEM.Value.ToString("0.##") + "EM";
            }
            else
                return null;
        }

        public override string SummaryName(ISystem sys)
        {
            string text = "Scan of {0}".T(EDTx.JournalScan_Scanof);
            if (ScanType == "AutoScan")
                text = "Autoscan of {0}".T(EDTx.JournalScan_Autoscanof);
            else if (ScanType == "Detailed")
                text = "Detailed scan of {0}".T(EDTx.JournalScan_Detailedscanof);
            else if (ScanType == "Basic")
                text = "Basic scan of {0}".T(EDTx.JournalScan_Basicscanof);
            else if (ScanType.Contains("Nav"))
                text = "Nav scan of {0}".T(EDTx.JournalScan_Navscanof);

            return string.Format(text, BodyName.ReplaceIfStartsWith(sys.Name)); 
        }

        public override string EventFilterName
        {
            get
            {
                if (ScanType == "AutoScan" )
                    return "Scan Auto";
                else if (ScanType == "Basic")
                    return "Scan Basic";
                else if (ScanType.Contains("Nav"))
                    return "Scan Nav";
                else
                    return base.EventFilterName;
            }
        }

        static public List<Tuple<string, string, Image>> FilterItems()
        {
            return new List<Tuple<string, string, Image>>()
            {
                new Tuple<string, string,Image>( "Scan Auto", "Scan Auto".T(EDTx.JournalScan_ScanAuto), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan] ),
                new Tuple<string,string,Image>( "Scan Basic", "Scan Basic".T(EDTx.JournalScan_ScanBasic), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan] ),
                new Tuple<string,string,Image>( "Scan Nav", "Scan Nav".T(EDTx.JournalScan_ScanNav), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan] ),
            };
        }

        public override void FillInformation(out string info, out string detailed)  
        {
            if (IsStar)
            {
                info = BaseUtils.FieldBuilder.Build("", GetStarTypeName(), "Mass:;SM;0.00".T(EDTx.JournalScan_MSM), nStellarMass, 
                                                "Age:;my;0.0".T(EDTx.JournalScan_Age), nAge,
                                                "Radius:".T(EDTx.JournalScan_RS), RadiusText(),
                                                "Dist:;ls;0.0".T(EDTx.JournalScan_DISTA), DistanceFromArrivalLS,
                                                "Name:".T(EDTx.JournalScan_BNME), BodyName);
            }
            else
            {
                info = BaseUtils.FieldBuilder.Build( "", PlanetClass, "Mass:".T(EDTx.JournalScan_MASS), MassEMText(),
                                                "<;, Landable".T(EDTx.JournalScan_Landable), IsLandable, 
                                                "<;, Terraformable".T(EDTx.JournalScan_Terraformable), TerraformState == "Terraformable", "", Atmosphere, 
                                                 "Gravity:;G;0.0".T(EDTx.JournalScan_Gravity), nSurfaceGravityG,
                                                 "Radius:".T(EDTx.JournalScan_RS), RadiusText(),
                                                 "Dist:;ls;0.0".T(EDTx.JournalScan_DISTA), DistanceFromArrivalLS,
                                                 "Name:".T(EDTx.JournalScan_SNME), BodyName);
            }

            detailed = DisplayString(0, false);
        }

        public string DisplayString(int indent = 0, bool includefront = true , MaterialCommoditiesList historicmatlist = null, MaterialCommoditiesList currentmatlist = null)//, bool mapped = false, bool efficiencyBonus = false)
        {
            string inds = new string(' ', indent);

            StringBuilder scanText = new StringBuilder();

            scanText.Append(inds);

            if (includefront)
            {
                scanText.AppendFormat("{0} {1}\n\n", BodyName, IsEDSMBody ? " (EDSM)" : "");

                if (IsStar)
                {
                    scanText.AppendFormat(GetStarTypeName());
                }
                else if (PlanetClass != null)
                {
                    scanText.AppendFormat("{0}", PlanetClass);

                    if (!PlanetClass.ToLowerInvariant().Contains("gas"))
                    {
                        scanText.AppendFormat((Atmosphere == null || Atmosphere == String.Empty) ? ", No Atmosphere".T(EDTx.JournalScan_NoAtmosphere) : (", " + Atmosphere));
                    }
                }

                if (IsLandable)
                    scanText.AppendFormat(", Landable".T(EDTx.JournalScan_LandC));

                scanText.AppendFormat("\n");

                if (HasAtmosphericComposition)
                    scanText.Append("\n" + DisplayAtmosphere(2));
                    
                if (HasPlanetaryComposition)
                    scanText.Append("\n" + DisplayComposition(2));

                if (HasPlanetaryComposition || HasAtmosphericComposition)
                    scanText.Append("\n\n");
                                
                if (nAge.HasValue)
                    scanText.AppendFormat("Age: {0} my\n".T(EDTx.JournalScan_AMY), nAge.Value.ToString("N0"));

                if (nStellarMass.HasValue)
                    scanText.AppendFormat("Solar Masses: {0:0.00}\n".T(EDTx.JournalScan_SolarMasses), nStellarMass.Value);

                if (nMassEM.HasValue)
                    scanText.AppendFormat("Mass:".T(EDTx.JournalScan_MASS) + " " + MassEMText() + "\n");

                if (nRadius.HasValue)
                    scanText.AppendFormat("Radius:".T(EDTx.JournalScan_RS) + " " + RadiusText() + "\n");

                if (DistanceFromArrivalLS > 0)
                    scanText.AppendFormat("Distance from Arrival Point {0:N1}ls\n".T(EDTx.JournalScan_DistancefromArrivalPoint), DistanceFromArrivalLS);
            }

            if (nSurfaceTemperature.HasValue)
                scanText.AppendFormat("Surface Temp: {0}K\n".T(EDTx.JournalScan_SurfaceTemp), nSurfaceTemperature.Value.ToString("N0"));

            if (Luminosity != null)
                scanText.AppendFormat("Luminosity: {0}\n".T(EDTx.JournalScan_Luminosity), Luminosity);

            if (nSurfaceGravity.HasValue)
                scanText.AppendFormat("Gravity: {0:0.00}g\n".T(EDTx.JournalScan_GV), nSurfaceGravityG.Value );

            if (nSurfacePressure.HasValue && nSurfacePressure.Value > 0.00 && !PlanetClass.ToLowerInvariant().Contains("gas"))
            {
                if (nSurfacePressure.Value > 1000)
                {
                    scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n".T(EDTx.JournalScan_SPA), nSurfacePressureEarth.Value.ToString("N2"));
                }
                else
                {
                    scanText.AppendFormat("Surface Pressure: {0} Pa\n".T(EDTx.JournalScan_SPP), (nSurfacePressure.Value).ToString("N2"));
                }
            }

            if (Volcanism != null)
                scanText.AppendFormat("Volcanism: {0}\n".T(EDTx.JournalScan_Volcanism), Volcanism == String.Empty ? "No Volcanism".T(EDTx.JournalScan_NoVolcanism) : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                                                                            ToTitleCase(Volcanism.ToLowerInvariant()));


            if (nOrbitalPeriodDays.HasValue && nOrbitalPeriodDays > 0)
                scanText.AppendFormat("Orbital Period: {0} days\n".T(EDTx.JournalScan_OrbitalPeriod), nOrbitalPeriodDays.Value.ToString("N1"));

            if (nSemiMajorAxis.HasValue)
            {
                if (IsStar || nSemiMajorAxis.Value > oneAU_m / 10)
                    scanText.AppendFormat("Semi Major Axis: {0:0.00}AU\n".T(EDTx.JournalScan_SMA), nSemiMajorAxisAU.Value );
                else
                    scanText.AppendFormat("Semi Major Axis: {0}km\n".T(EDTx.JournalScan_SMK), (nSemiMajorAxis.Value / 1000).ToString("N1"));
            }

            if (nEccentricity.HasValue)
                scanText.AppendFormat("Orbital Eccentricity: {0:0.000}\n".T(EDTx.JournalScan_OrbitalEccentricity), nEccentricity.Value);

            if (nOrbitalInclination.HasValue)
                scanText.AppendFormat("Orbital Inclination: {0:0.000}°\n".T(EDTx.JournalScan_OrbitalInclination), nOrbitalInclination.Value);

            if (nPeriapsis.HasValue)
                scanText.AppendFormat("Arg Of Periapsis: {0:0.000}°\n".T(EDTx.JournalScan_ArgOfPeriapsis), nPeriapsis.Value);

            if (nAbsoluteMagnitude.HasValue)
                scanText.AppendFormat("Absolute Magnitude: {0:0.00}\n".T(EDTx.JournalScan_AbsoluteMagnitude), nAbsoluteMagnitude.Value);

            if (nAxialTiltDeg.HasValue)
                scanText.AppendFormat("Axial tilt: {0:0.00}°\n".T(EDTx.JournalScan_Axialtilt), nAxialTiltDeg.Value);

            if (nRotationPeriodDays.HasValue)
                scanText.AppendFormat("Rotation Period: {0} days\n".T(EDTx.JournalScan_RotationPeriod), nRotationPeriodDays.Value.ToString("N1"));

            if (nTidalLock.HasValue && nTidalLock.Value)
                scanText.Append("Tidally locked\n".T(EDTx.JournalScan_Tidallylocked));

            if (Terraformable)
                scanText.Append("Candidate for terraforming\n".T(EDTx.JournalScan_Candidateforterraforming));

            if (HasRings)
            {
                scanText.Append("\n");
                if (IsStar)
                {
                    scanText.AppendFormat(Rings.Count() == 1 ? "Belt".T(EDTx.JournalScan_Belt) : "Belts".T(EDTx.JournalScan_Belts), ""); // OLD translator files had "Belt{0}" so supply an empty string just in case
                    for (int i = 0; i < Rings.Length; i++)
                    {
                        if (Rings[i].MassMT > (oneMoon_MT / 10000))
                        {
                            scanText.Append("\n" + RingInformation(i, 1.0 / oneMoon_MT, " Moons".T(EDTx.JournalScan_Moons)));
                        }
                        else
                        {
                            scanText.Append("\n" + RingInformation(i));
                        }
                    }
                }
                else
                {
                    scanText.AppendFormat(Rings.Count() == 1 ? "Ring".T(EDTx.JournalScan_Ring) : "Rings".T(EDTx.JournalScan_Rings), ""); // OLD translator files had "Rings{0}" so supply an empty string just in case

                    for (int i = 0; i < Rings.Length; i++)
                        scanText.Append("\n" + RingInformation(i));
                }
            }

            if (HasMaterials)
            {
                scanText.Append("\n" + DisplayMaterials(2, historicmatlist , currentmatlist) + "\n");
            }

            if (CircumstellarZonesString() != null)
                scanText.Append("\n" + CircumstellarZonesString());

			if (IsStar && HabZoneOtherStarsString() != null)
				scanText.Append(HabZoneOtherStarsString());
			
            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            if (EstimatedValue > 0)
            {
                scanText.AppendFormat("\nEstimated value: {0:N0}".T(EDTx.JournalScan_EV), EstimatedValue);
                if (Mapped)
                {
                    scanText.Append(" " + "Mapped".T(EDTx.JournalScan_MPI));
                    if (EfficientMapped)
                        scanText.Append(" " + "Efficiently".T(EDTx.JournalScan_MPIE));

                    scanText.AppendFormat("\nFirst Discovered+Mapped value: {0:N0}".T(EDTx.JournalScan_EVFD), EstimatedValueFirstDiscoveredFirstMapped);
                    scanText.AppendFormat("\nFirst Mapped value: {0:N0}".T(EDTx.JournalScan_EVFM), EstimatedValueFirstMapped);
                }

                if (WasDiscovered.HasValue && WasDiscovered.Value)
                    scanText.AppendFormat("\nAlready Discovered".T(EDTx.JournalScan_EVAD));
                if (WasMapped.HasValue && WasMapped.Value)
                    scanText.AppendFormat("\nAlready Mapped".T(EDTx.JournalScan_EVAM));
            }

            if (EDSMDiscoveryCommander != null)
                scanText.AppendFormat("\n\nDiscovered by {0} on {1}".T(EDTx.JournalScan_DB), EDSMDiscoveryCommander, EDSMDiscoveryUTC.ToStringZulu());

            scanText.AppendFormat("\nScan Type: {0}".T(EDTx.JournalScan_SCNT), ScanType);

            return scanText.ToNullSafeString().Replace("\n", "\n" + inds);
        }

		// goldilocks zone
        public string GetHabZoneStringLs()
        {
            if (IsStar && HabitableZoneInner.HasValue && HabitableZoneOuter.HasValue)
            {
                return $"{HabitableZoneInner:N0}-{HabitableZoneOuter:N0}ls";
            }
            else
            {
                return string.Empty;
            }
        }

        public string CircumstellarZonesString()
        {
            if (IsStar && HabitableZoneInner.HasValue && HabitableZoneOuter.HasValue)
            {
                StringBuilder habZone = new StringBuilder();

				habZone.Append("Inferred Circumstellar zones:\n".T(EDTx.JournalScan_InferredCircumstellarzones));

				habZone.AppendFormat(" - Habitable Zone, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_HabitableZone),
									 GetHabZoneStringLs(),
									 (HabitableZoneInner.Value / oneAU_LS).ToString("N2"),
									 (HabitableZoneOuter.Value / oneAU_LS).ToString("N2"));

				habZone.AppendFormat(" - Metal Rich planets, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_MetalRichplanets),
									 GetMetalRichZoneStringLs(),
									 (MetalRichZoneInner.Value / oneAU_LS).ToString("N2"),
									 (MetalRichZoneInner.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Water Worlds, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_WaterWorlds),
									 GetWaterWorldZoneStringLs(),
									 (WaterWrldZoneInner.Value / oneAU_LS).ToString("N2"),
									 (WaterWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Earth Like Worlds, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_EarthLikeWorlds),
									 GetEarthLikeZoneStringLs(),
									 (EarthLikeZoneInner.Value / oneAU_LS).ToString("N2"),
									 (EarthLikeZoneOuter.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Ammonia Worlds, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_AmmoniaWorlds),
									 GetAmmoniaWorldZoneStringLs(),
									 (AmmonWrldZoneInner.Value / oneAU_LS).ToString("N2"),
									 (AmmonWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Icy Planets, {0} (from {1} AU)\n\n".T(EDTx.JournalScan_IcyPlanets),
									 GetIcyPlanetsZoneStringLs(),
				(IcyPlanetZoneInner.Value / oneAU_LS).ToString("N2"));

                return habZone.ToNullSafeString();
            }
            else
                return null;
        }

		// string which tell us that other stars are not considered in the habitable zone calculations.
		public string HabZoneOtherStarsString()
		{
			StringBuilder habZoneAddend = new StringBuilder();
			if (nSemiMajorAxis.HasValue && nSemiMajorAxis.Value > 0)
				habZoneAddend.Append("(Others stars not considered)\n\n".T(EDTx.JournalScan_Othersstarsnotconsidered));
			
			return habZoneAddend.ToNullSafeString();
		}

		// metal rich zone
		public string GetMetalRichZoneStringLs()
		{
			if (IsStar && MetalRichZoneInner.HasValue && MetalRichZoneOuter.HasValue)
			{
				return $"{MetalRichZoneInner:N0}-{MetalRichZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string MetalRichZoneString()
		{
			if (IsStar && MetalRichZoneInner.HasValue && MetalRichZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Metal Rich Planets: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_MRP),
									 GetMetalRichZoneStringLs(), 
									 (MetalRichZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (MetalRichZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

		// water world zone
		public string GetWaterWorldZoneStringLs()
		{
			if (IsStar && WaterWrldZoneInner.HasValue && WaterWrldZoneOuter.HasValue)
			{
				return $"{WaterWrldZoneInner:N0}-{WaterWrldZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string WaterWorldZoneString()
		{
			if (IsStar && WaterWrldZoneInner.HasValue && WaterWrldZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Water Worlds: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_WWP),
									 GetWaterWorldZoneStringLs(), 
									 (WaterWrldZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (WaterWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

		// earth like world zone
		public string GetEarthLikeZoneStringLs()
		{
			if (IsStar && EarthLikeZoneInner.HasValue && EarthLikeZoneOuter.HasValue)
			{
				return $"{EarthLikeZoneInner:N0}-{EarthLikeZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string EarthLikeZoneString()
		{
			if (IsStar && EarthLikeZoneInner.HasValue && EarthLikeZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Earth Like Worlds: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_ELWP),
									 GetEarthLikeZoneStringLs(), 
									 (EarthLikeZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (EarthLikeZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

		// ammonia world zone
		public string GetAmmoniaWorldZoneStringLs()
		{
			if (IsStar && AmmonWrldZoneInner.HasValue && AmmonWrldZoneOuter.HasValue)
			{
				return $"{AmmonWrldZoneInner:N0}-{AmmonWrldZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string AmmoniaWorldZoneString()
		{
			if (IsStar && AmmonWrldZoneInner.HasValue && AmmonWrldZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Ammonia Worlds: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_AWP),
									 GetAmmoniaWorldZoneStringLs(), 
									 (AmmonWrldZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (AmmonWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

		// icy planets zone
		public string GetIcyPlanetsZoneStringLs()
		{
			if (IsStar && IcyPlanetZoneInner.HasValue && IcyPlanetZoneOuter != null)
			{
				return $"{IcyPlanetZoneInner:N0}ls to {IcyPlanetZoneOuter:N0}";
			}
			else
			{
				return string.Empty;
			}
		}

		public string IcyPlanetsZoneString()
		{
			if (IsStar && IcyPlanetZoneInner.HasValue && IcyPlanetZoneOuter != null)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Icy Planets: {0} ({1} AU to {2})\n".T(EDTx.JournalScan_ICYP),
									 GetIcyPlanetsZoneStringLs(), 
									 (IcyPlanetZoneInner.Value / oneAU_LS).ToString("N2"),
									 IcyPlanetZoneOuter);
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

        // optionally, show material counts at the historic point and current.
        public string DisplayMaterials(int indent = 0, MaterialCommoditiesList historicmatlist = null, MaterialCommoditiesList currentmatlist = null)
        {
            StringBuilder scanText = new StringBuilder();

            if (HasMaterials)
            {
                string indents = new string(' ', indent);

                scanText.Append("Materials:\n".T(EDTx.JournalScan_Materials));
                foreach (KeyValuePair<string, double> mat in Materials)
                {
                    scanText.Append(indents + DisplayMaterial(mat.Key, mat.Value, historicmatlist, currentmatlist));
                }

                if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                    scanText.Remove(scanText.Length - 1, 1);
            }

            return scanText.ToNullSafeString();
        }

        public string DisplayMaterial(string fdname, double percent, MaterialCommoditiesList historicmatlist = null, MaterialCommoditiesList currentmatlist = null)
        {
            StringBuilder scanText = new StringBuilder();

            MaterialCommodityData mc = MaterialCommodityData.GetByFDName(fdname);

            if (mc != null && (historicmatlist != null || currentmatlist != null))
            {
                MaterialCommodities historic = historicmatlist?.Find(mc);
                MaterialCommodities current = ReferenceEquals(historicmatlist,currentmatlist) ? null : currentmatlist?.Find(mc);
                int? limit = mc.MaterialLimit();

                string matinfo = historic?.Count.ToString() ?? "0";
                if (limit != null)
                    matinfo += "/" + limit.Value.ToString();

                if (current != null && (historic == null || historic.Count != current.Count) )
                    matinfo += " Cur " + current.Count.ToString();

                scanText.AppendFormat("{0} ({1}) {2} {3}% {4}\n", mc.Name, mc.Shortname, mc.TranslatedType, percent.ToString("N1"), matinfo);
            }
            else
                scanText.AppendFormat("{0} {1}%\n", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(fdname.ToLowerInvariant()),
                                                            percent.ToString("N1"));

            return scanText.ToNullSafeString();
        }

        public string DisplayAtmosphere(int indent = 0)
        {
            StringBuilder scanText = new StringBuilder();
            string indents = new string(' ', indent);

            scanText.Append("Atmospheric Composition:\n".T(EDTx.JournalScan_AtmosphericComposition));
            foreach (KeyValuePair<string, double> comp in AtmosphereComposition)
            {
                scanText.AppendFormat(indents + "{0} - {1}%\n", comp.Key, comp.Value.ToString("N2"));
            }

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            return scanText.ToNullSafeString();
        }

        public string DisplayComposition(int indent = 0)
        {
            StringBuilder scanText = new StringBuilder();
            string indents = new string(' ', indent);

            scanText.Append("Planetary Composition:\n".T(EDTx.JournalScan_PlanetaryComposition));
            foreach (KeyValuePair<string, double> comp in PlanetComposition)
            {
                if (comp.Value > 0)
                    scanText.AppendFormat(indents + "{0} - {1}%\n", comp.Key, (comp.Value * 100).ToString("N2"));
            }

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            return scanText.ToNullSafeString();
        }

        public string RingInformationMoons(int ringno)
        {
            return RingInformation(ringno, 1 / oneMoon_MT, " Moons".T(EDTx.JournalScan_Moons));
        }

        public string RingInformation(int ringno, double scale = 1, string scaletype = " MT")
        {
            StarPlanetRing ring = Rings[ringno];
            return ring.RingInformation(scale, scaletype, IsStar);
        }

        public string GetStarTypeName()           // give description to star class
        {
            return Bodies.StarName(StarTypeID);
        }

        public System.Drawing.Image GetStarTypeImage()           // give image and description to star class
        {
            return EDDiscovery.Icons.IconSet.GetIcon("Stars." + StarTypeID.ToString());
        }

        static public System.Drawing.Image GetStarImageNotScanned()
        {
            return EDDiscovery.Icons.IconSet.GetIcon("Stars.Unknown");
        }

        public System.Drawing.Image GetPlanetClassImage()
        {
            if (PlanetClass == null)
            {
                return GetPlanetImageNotScanned();
            }

            string iconname = PlanetTypeID.ToString();

            // adjust image type dependent for certain classes.

            if (PlanetTypeID == EDPlanet.High_metal_content_body || PlanetTypeID == EDPlanet.Metal_rich_body)
            {
                if (AtmosphereProperty == (EDAtmosphereProperty.Hot | EDAtmosphereProperty.Thick))
                {
                    iconname = "High_metal_content_body_hot_thick";
                }
                else if (PlanetTypeID == EDPlanet.High_metal_content_body && nSurfaceTemperature > 700)
                {
                    iconname = "High_metal_content_body_700";
                }
                else if (PlanetTypeID == EDPlanet.High_metal_content_body && nSurfaceTemperature > 250)
                {
                    iconname = "High_metal_content_body_250";
                }
            }

            return EDDiscovery.Icons.IconSet.GetIcon("Planets." + iconname);
        }

        static public System.Drawing.Image GetPlanetImageNotScanned()
        {
            return EDDiscovery.Icons.IconSet.GetIcon("Planets.Unknown");
        }

        static public System.Drawing.Image GetMoonImageNotScanned()
        {
            return EDDiscovery.Icons.IconSet.GetIcon("Planets.Unknown");
        }

        public double GetMaterial(string v)
        {
            if (Materials == null)
                return 0.0;

            if (!Materials.ContainsKey(v.ToLowerInvariant()))
                return 0.0;

            return Materials[v.ToLowerInvariant()];
        }

        public double? GetAtmosphereComponent(string c)
        {
            if (!HasAtmosphericComposition)
                return null;

            if (!AtmosphereComposition.ContainsKey(c))
                return 0.0;

            return AtmosphereComposition[c];

        }

        public double? GetCompositionPercent(string c)
        {
            if (!HasPlanetaryComposition)
                return null;

            if (!PlanetComposition.ContainsKey(c))
                return 0.0;

            return PlanetComposition[c] * 100;
        }

        public bool IsStarNameRelated(string starname, string designation = null)
        {
            if (designation == null)
            {
                designation = BodyName;
            }

            if (designation.Length >= starname.Length)
            {
                string s = designation.Substring(0, starname.Length);
                return starname.Equals(s, StringComparison.InvariantCultureIgnoreCase);
            }
            else
                return false;
        }

        public string IsStarNameRelatedReturnRest(string starname)          // null if not related, else rest of string
        {
            string designation = BodyDesignation ?? BodyName;
            if (designation.Length >= starname.Length)
            {
                string s = designation.Substring(0, starname.Length);
                if (starname.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                    return designation.Substring(starname.Length).Trim();
            }

            return null;
        }

        // Habitable zone calculations, formula cribbed from JackieSilver's HabZone Calculator with permission
        private double DistanceForBlackBodyTemperature(double targetTemp)
        {
            double top = Math.Pow(nRadius.Value, 2.0) * Math.Pow(nSurfaceTemperature.Value, 4.0);
            double bottom = 4.0 * Math.Pow(targetTemp, 4.0);
            double radius_metres = Math.Pow(top / bottom, 0.5);
            return radius_metres / oneLS_m;
        }

		private double DistanceForNoMaxTemperatureBody(double radius)
		{
			return radius / oneLS_m;
		}

        #endregion

        #region Estimated Value

        private void EstimateScanValue()       // call to estimate scan value given these parameters. Updates EstimatedValue
        {
            // see https://forums.frontier.co.uk/showthread.php/232000-Exploration-value-formulae/ for detail

            if (EventTimeUTC < new DateTime(2017, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc))
            {
                EstimatedValue = EstimatedValueED22();
                return;
            }

            if (EventTimeUTC < new DateTime(2018, 12, 11, 9, 0, 0, DateTimeKind.Utc))
            {
                EstimatedValue = EstimatedValue32();
                return;
            }

            // 3.3 onwards

            //System.Diagnostics.Debug.WriteLine("Scan calc " + mapped + " ef " + efficient + " Current " + EstimatedValue);

            double kValue;

            if (IsStar)
            {
                switch (StarTypeID)
                {
                    // white dwarf
                    case EDStar.D:
                    case EDStar.DA:
                    case EDStar.DAB:
                    case EDStar.DAO:
                    case EDStar.DAZ:
                    case EDStar.DAV:
                    case EDStar.DB:
                    case EDStar.DBZ:
                    case EDStar.DBV:
                    case EDStar.DO:
                    case EDStar.DOV:
                    case EDStar.DQ:
                    case EDStar.DC:
                    case EDStar.DCV:
                    case EDStar.DX:
                        kValue = 14057;
                        break;

                    case EDStar.N:
                    case EDStar.H:
                        kValue = 22628;
                        break;

                    case EDStar.SuperMassiveBlackHole:
                        // this is applying the same scaling to the 3.2 value as a normal black hole, not confirmed in game
                        kValue = 33.5678;   
                        break;

                    default:
                        kValue = 1200;
                        break;
                }

                EstimatedValue = (int)StarValue32And33(kValue, nStellarMass.HasValue ? nStellarMass.Value : 1.0);
            }
            else
            {
                EstimatedValue = 0;

                if (PlanetClass != null)  //Asteroid belt is null
                {
                    switch (PlanetTypeID)
                    {
                        case EDPlanet.Metal_rich_body:
                            // CFT value is scaled same as WW/ELW from 3.2, not confirmed in game
                            // They're like hen's teeth anyway....
                            kValue = 21790;
                            if (Terraformable) kValue += 65631;
                            break;
                        case EDPlanet.Ammonia_world:
                            kValue = 96932;
                            break;
                        case EDPlanet.Sudarsky_class_I_gas_giant:
                            kValue = 1656;
                            break;
                        case EDPlanet.Sudarsky_class_II_gas_giant:
                        case EDPlanet.High_metal_content_body:
                            kValue = 9654;
                            if (Terraformable) kValue += 100677;
                            break;
                        case EDPlanet.Water_world:
                            kValue = 64831;
                            if (Terraformable) kValue += 116295;
                            break;
                        case EDPlanet.Earthlike_body:
                            // Always terraformable so WW + bonus
                            kValue = 64831 + 116295;
                            break;
                        default:
                            kValue = 300;
                            if (Terraformable) kValue += 93328;
                            break;
                    }

                    double mass = nMassEM.HasValue ? nMassEM.Value : 1.0;

                    if (Mapped)
                    {
                        double effmapped = EfficientMapped ? 1.25 : 1;

                        EstimatedValueFirstDiscoveredFirstMapped = (int)(PlanetValue33(kValue, mass, 3.699622554 * effmapped) * 2.6);

                        EstimatedValueFirstMapped = (int)PlanetValue33(kValue, mass, 8.0956 * effmapped);

                        EstimatedValue = (int)PlanetValue33(kValue, mass, 3.3333333333 * effmapped);
                    }
                    else
                    {
                        EstimatedValue = (int)PlanetValue33(kValue, mass, 1);
                        EstimatedValueFirstDiscoveredFirstMapped = EstimatedValueFirstMapped = 0;
                    }

                }
            }
        }

        private double StarValue32And33(double k, double m)
        {
            return k + (m * k / 66.25);
        }

        private double PlanetValue33(double k, double m, double map)
        {
            const double q = 0.56591828;
            return Math.Max((k + (k * Math.Pow(m, 0.2) * q)) * map, 500);
        }

        #endregion

        #region ED 3.2 values

        private int EstimatedValue32()
        {
            double kValue;
            double kBonus = 0;

            if (IsStar)
            {
                switch (StarTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    // white dwarf
                    case EDStar.D:
                    case EDStar.DA:
                    case EDStar.DAB:
                    case EDStar.DAO:
                    case EDStar.DAZ:
                    case EDStar.DAV:
                    case EDStar.DB:
                    case EDStar.DBZ:
                    case EDStar.DBV:
                    case EDStar.DO:
                    case EDStar.DOV:
                    case EDStar.DQ:
                    case EDStar.DC:
                    case EDStar.DCV:
                    case EDStar.DX:
                        kValue = 33737;
                        break;

                    case EDStar.N:
                    case EDStar.H:
                        kValue = 54309;
                        break;

                    case EDStar.SuperMassiveBlackHole:
                        kValue = 80.5654;
                        break;

                    default:
                        kValue = 2880;
                        break;
                }

                return (int)StarValue32And33(kValue, nStellarMass.HasValue ? nStellarMass.Value : 1.0);
            }
            else if (PlanetClass == null)  //Asteroid belt
                return 0;
            else   // Planet
            {
                switch (PlanetTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    
                    case EDPlanet.Metal_rich_body:
                        kValue = 52292;
                        if (Terraformable) { kBonus = 245306; }
                        break;
                    case EDPlanet.High_metal_content_body:
                    case EDPlanet.Sudarsky_class_II_gas_giant:
                        kValue = 23168;
                        if (Terraformable) { kBonus = 241607; }
                        break;
                    case EDPlanet.Earthlike_body:
                        kValue = 155581;
                        kBonus = 279088;
                        break;
                    case EDPlanet.Water_world:
                        kValue = 155581;
                        if (Terraformable) { kBonus = 279088; }
                        break;
                    case EDPlanet.Ammonia_world:
                        kValue = 232619;
                        break;
                    case EDPlanet.Sudarsky_class_I_gas_giant:
                        kValue = 3974;
                        break;
                    default:
                        kValue = 720;
                        if (Terraformable) { kBonus = 223971; }
                        break;
                }

                double mass = nMassEM.HasValue ? nMassEM.Value : 1.0;       // some old entries don't have mass, so just presume 1

                int val = (int)PlanetValueED32(kValue, mass);
                if (Terraformable || PlanetTypeID == EDPlanet.Earthlike_body)
                {
                    val += (int)PlanetValueED32(kBonus, mass);
                }

                return val;
            }
        }

        private double PlanetValueED32(double k, double m)
        {
            return k + (3 * k * Math.Pow(m, 0.199977) / 5.3);
        }

        #endregion

        #region ED 22 

        private int EstimatedValueED22()
        {
            if (IsStar)
            {
                switch (StarTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    case EDStar.O:
                        //low = 3677;
                        //high = 4465;
                        return 4170;

                    case EDStar.B:
                        //low = 2992;
                        //high = 3456;
                        return 3098;

                    case EDStar.A:
                        //low = 2938;
                        //high = 2986;
                        return 2950;

                    case EDStar.F:
                        //low = 2915;
                        //high = 2957;
                        return 2932;

                    case EDStar.G:
                        //low = 2912;
                        //high = 2935;
                        // also have a G8V
                        return 2923;

                    case EDStar.K:
                        //low = 2898;
                        //high = 2923;
                        return 2911;
                    case EDStar.M:
                        //low = 2887;
                        //high = 2905;
                        return 2911;

                    // dwarfs
                    case EDStar.L:
                        //low = 2884;
                        //high = 2890;
                        return 2887;
                    case EDStar.T:
                        //low = 2881;
                        //high = 2885;
                        return 2883;
                    case EDStar.Y:
                        //low = 2880;
                        //high = 2882;
                        return 2881;

                    // proto stars
                    case EDStar.AeBe:    // Herbig
                        //                ??
                        //low = //high = 0;
                        return 2500;
                    case EDStar.TTS:
                        //low = 2881;
                        //high = 2922;
                        return 2900;

                    // wolf rayet
                    case EDStar.W:
                    case EDStar.WN:
                    case EDStar.WNC:
                    case EDStar.WC:
                    case EDStar.WO:
                        //low = //high = 7794;
                        return 7794;

                    // Carbon
                    case EDStar.CS:
                    case EDStar.C:
                    case EDStar.CN:
                    case EDStar.CJ:
                    case EDStar.CHd:
                        //low = //high = 2920;
                        return 2920;

                    case EDStar.MS: //seen in log
                    case EDStar.S:   // seen in log
                                     //                ??
                                     //low = //high = 0;
                        return 2000;


                    // white dwarf
                    case EDStar.D:
                    case EDStar.DA:
                    case EDStar.DAB:
                    case EDStar.DAO:
                    case EDStar.DAZ:
                    case EDStar.DAV:
                    case EDStar.DB:
                    case EDStar.DBZ:
                    case EDStar.DBV:
                    case EDStar.DO:
                    case EDStar.DOV:
                    case EDStar.DQ:
                    case EDStar.DC:
                    case EDStar.DCV:
                    case EDStar.DX:
                        //low = 25000;
                        //high = 27000;

                        return 26000;

                    case EDStar.N:
                        //low = 43276;
                        //high = 44619;
                        return 43441;

                    case EDStar.H:
                        //low = 44749;
                        //high = 80305;
                        return 61439;

                    case EDStar.X:
                    case EDStar.A_BlueWhiteSuperGiant:
                    case EDStar.F_WhiteSuperGiant:
                    case EDStar.M_RedSuperGiant:
                    case EDStar.M_RedGiant:
                    case EDStar.K_OrangeGiant:
                    case EDStar.RoguePlanet:

                    default:
                        //low = 0;
                        //high = 0;
                        return 2000;
                }
            }
            else   // Planet
            {
                switch (PlanetTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    case EDPlanet.Icy_body:
                        //low = 792; // (0.0001 EM)
                        //high = 1720; // 89.17
                        return 933; // 0.04

                    case EDPlanet.Rocky_ice_body:
                        //low = 792; // (0.0001 EM)
                        //high = 1720; // 89.17
                        return 933; // 0.04

                    case EDPlanet.Rocky_body:
                        if (TerraformState != null && TerraformState.ToLowerInvariant().Equals("terraformable"))
                        {
                            //low = 36000;
                            //high = 36500;
                            return 37000;
                        }
                        else
                        {
                            //low = 792; // (0.0001 EM)
                            //high = 1720; // 89.17
                            return 933; // 0.04
                        }
                    case EDPlanet.Metal_rich_body:
                        //low = 9145; // (0.0002 EM)
                        //high = 14562; // (4.03 EM)
                        return 12449; // 0.51 EM
                    case EDPlanet.High_metal_content_body:
                        if (TerraformState != null && TerraformState.ToLowerInvariant().Equals("terraformable"))
                        {
                            //low = 36000;
                            //high = 54000;
                            return 42000;
                        }
                        else
                        {
                            //low = 4966; // (0.0015 EM)
                            //high = 9632;  // 31.52 EM
                            return 6670; // 0.41
                        }

                    case EDPlanet.Earthlike_body:
                        //low = 65000; // 0.24 EM
                        //high = 71885; // 196.60 EM
                        return 67798; // 0.47 EM

                    case EDPlanet.Water_world:
                        //low = 26589; // (0.09 EM)
                        //high = 43437; // (42.77 EM)
                        return 30492; // (0.82 EM)
                    case EDPlanet.Ammonia_world:
                        //low = 37019; // 0.09 EM
                        //high = 71885; //(196.60 EM)
                        return 40322; // (0.41 EM)
                    case EDPlanet.Sudarsky_class_I_gas_giant:
                        //low = 2472; // (2.30 EM)
                        //high = 4514; // (620.81 EM
                        return 3400;  // 62.93 EM

                    case EDPlanet.Sudarsky_class_II_gas_giant:
                        //low = 8110; // (5.37 EM)
                        //high = 14618; // (949.98 EM)
                        return 12319;  // 260.84 EM

                    case EDPlanet.Sudarsky_class_III_gas_giant:
                        //low = 1368; // (10.16 EM)
                        //high = 2731; // (2926 EM)
                        return 2339; // 990.92 EM

                    case EDPlanet.Sudarsky_class_IV_gas_giant:
                        //low = 2739; //(2984 EM)
                        //high = 2827; // (3697 EM)
                        return 2782; // 3319 em

                    case EDPlanet.Sudarsky_class_V_gas_giant:
                        //low = 2225; // 688.2 EM
                        //high = 2225;
                        return 2225;

                    case EDPlanet.Water_giant:
                    case EDPlanet.Water_giant_with_life:
                    case EDPlanet.Gas_giant_with_water_based_life:
                    case EDPlanet.Gas_giant_with_ammonia_based_life:
                    case EDPlanet.Helium_rich_gas_giant:
                    case EDPlanet.Helium_gas_giant:
                        //low = 0;
                        //high = 0;
                        return 2000;

                    default:
                        //low = 0;
                        //high = 2000;
                        return 0;
                }
            }

        }
    }

    #endregion

    public class ScansAreForSameBody : EqualityComparer<JournalScan>
    {
        public override bool Equals(JournalScan x, JournalScan y)
        {
            return x.BodyName == y.BodyName;
        }

        public override int GetHashCode(JournalScan obj)
        {
            return obj.BodyName.GetHashCode();
        }
    }
}
