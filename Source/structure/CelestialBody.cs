using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Utilities;
using kOS.Suffixed;
using System.Linq;

namespace kOS.AddOns.kOSAstrogator.structure
{
    /// <summary>
    /// A structure wrapper for CelestialBody with enough to return to kOS to be useful
    /// </summary>
    [KOSNomenclature("AstrogatorCelestialBody")]
    public class CelestialBodyStructure : Structure
    {
        private readonly CelestialBody body;
        private readonly SharedObjects shared;

        private CelestialBodyStructure(CelestialBody body, SharedObjects shared)
        {
            this.body = body;
            this.shared = shared;
            InitializeSuffixes();
        }

        /// <summary>
        /// Creates a wrapper around CelestialBody
        /// </summary>
        /// <param name="body">The body to wrap</param>
        /// <param name="shared">Game shared data</param>
        /// <returns></returns>
        public static CelestialBodyStructure Create(CelestialBody body, SharedObjects shared) => new CelestialBodyStructure(body, shared);

        private void InitializeSuffixes()
        {
            AddSuffix("bodyName", new Suffix<StringValue>(() => body.bodyName));
            AddSuffix("bodyDisplayName", new Suffix<StringValue>(() => body.bodyDisplayName));
            AddSuffix("bodyDescription", new Suffix<StringValue>(() => body.bodyDescription));
            AddSuffix("GeeASL", new Suffix<ScalarValue>(() => body.GeeASL));
            AddSuffix("Radius", new Suffix<ScalarValue>(() => body.Radius));
            AddSuffix("Mass", new Suffix<ScalarValue>(() => body.Mass));
            AddSuffix("Density", new Suffix<ScalarValue>(() => body.Density));
            AddSuffix("SurfaceArea", new Suffix<ScalarValue>(() => body.SurfaceArea));
            AddSuffix("gravParameter", new Suffix<ScalarValue>(() => body.gravParameter));
            AddSuffix("sphereOfInfluence", new Suffix<ScalarValue>(() => body.sphereOfInfluence));
            AddSuffix("hillSphere", new Suffix<ScalarValue>(() => body.hillSphere));
            AddSuffix("gMagnitudeAtCenter", new Suffix<ScalarValue>(() => body.gMagnitudeAtCenter));
            AddSuffix("atmDensityASL", new Suffix<ScalarValue>(() => body.atmDensityASL));
            AddSuffix("atmPressureASL", new Suffix<ScalarValue>(() => body.atmPressureASL));
            AddSuffix("bodyAdjectiveDisplayName", new Suffix<StringValue>(() => body.bodyAdjectiveDisplayName));
            AddSuffix("scaledEllipsoid", new Suffix<BooleanValue>(() => body.scaledEllipsoid));
            AddSuffix("scaledElipRadMult", new Suffix<Vector>(() => new Vector(body.scaledElipRadMult)));
            AddSuffix("scaledRadiusHorizonMultiplier", new Suffix<ScalarValue>(() => body.scaledRadiusHorizonMultiplier));
            AddSuffix("navballSwitchRadiusMult", new Suffix<ScalarValue>(() => body.navballSwitchRadiusMult));
            AddSuffix("navballSwitchRadiusMultLow", new Suffix<ScalarValue>(() => body.navballSwitchRadiusMultLow));
            AddSuffix("isHomeWorld", new Suffix<BooleanValue>(() => body.isHomeWorld));
            AddSuffix("isCometPerturber", new Suffix<BooleanValue>(() => body.isCometPerturber));
            AddSuffix("ocean", new Suffix<BooleanValue>(() => body.ocean));
            AddSuffix("oceanUseFog", new Suffix<BooleanValue>(() => body.oceanUseFog));
            AddSuffix("oceanFogPQSDepth", new Suffix<ScalarValue>(() => body.oceanFogPQSDepth));
            AddSuffix("oceanFogPQSDepthRecip", new Suffix<ScalarValue>(() => body.oceanFogPQSDepthRecip));
            AddSuffix("oceanFogDensityStart", new Suffix<ScalarValue>(() => body.oceanFogDensityStart));
            AddSuffix("oceanFogDensityEnd", new Suffix<ScalarValue>(() => body.oceanFogDensityEnd));
            AddSuffix("oceanFogDensityPQSMult", new Suffix<ScalarValue>(() => body.oceanFogDensityPQSMult));
            AddSuffix("oceanFogDensityAltScalar", new Suffix<ScalarValue>(() => body.oceanFogDensityAltScalar));
            AddSuffix("oceanFogDensityExponent", new Suffix<ScalarValue>(() => body.oceanFogDensityExponent));
            AddSuffix("oceanFogColorStart", new Suffix<RgbaColor>(() => new RgbaColor(body.oceanFogColorStart.r, body.oceanFogColorStart.g, body.oceanFogColorStart.b, body.oceanFogColorStart.a)));
            AddSuffix("oceanFogColorEnd", new Suffix<RgbaColor>(() => new RgbaColor(body.oceanFogColorEnd.r, body.oceanFogColorEnd.g, body.oceanFogColorEnd.b, body.oceanFogColorEnd.a)));
            AddSuffix("oceanFogDawnFactor", new Suffix<ScalarValue>(() => body.oceanFogDawnFactor));
            AddSuffix("oceanSkyColorMult", new Suffix<ScalarValue>(() => body.oceanSkyColorMult));
            AddSuffix("oceanSkyColorOpacityBase", new Suffix<ScalarValue>(() => body.oceanSkyColorOpacityBase));
            AddSuffix("oceanSkyColorOpacityAltMult", new Suffix<ScalarValue>(() => body.oceanSkyColorOpacityAltMult));
            AddSuffix("oceanDensity", new Suffix<ScalarValue>(() => body.oceanDensity));
            AddSuffix("oceanAFGBase", new Suffix<ScalarValue>(() => body.oceanAFGBase));
            AddSuffix("oceanAFGAltMult", new Suffix<ScalarValue>(() => body.oceanAFGAltMult));
            AddSuffix("oceanAFGMin", new Suffix<ScalarValue>(() => body.oceanAFGMin));
            AddSuffix("oceanSunBase", new Suffix<ScalarValue>(() => body.oceanSunBase));
            AddSuffix("oceanSunAltMult", new Suffix<ScalarValue>(() => body.oceanSunAltMult));
            AddSuffix("oceanSunMin", new Suffix<ScalarValue>(() => body.oceanSunMin));
            AddSuffix("oceanAFGLerp", new Suffix<BooleanValue>(() => body.oceanAFGLerp));
            AddSuffix("oceanMinAlphaFogDistance", new Suffix<ScalarValue>(() => body.oceanMinAlphaFogDistance));
            AddSuffix("oceanMaxAlbedoFog", new Suffix<ScalarValue>(() => body.oceanMaxAlbedoFog));
            AddSuffix("oceanMaxAlphaFog", new Suffix<ScalarValue>(() => body.oceanMaxAlphaFog));
            AddSuffix("oceanAlbedoDistanceScalar", new Suffix<ScalarValue>(() => body.oceanAlbedoDistanceScalar));
            AddSuffix("oceanAlphaDistanceScalar", new Suffix<ScalarValue>(() => body.oceanAlphaDistanceScalar));
            AddSuffix("minOrbitalDistance", new Suffix<ScalarValue>(() => body.minOrbitalDistance));
            AddSuffix("atmosphere", new Suffix<BooleanValue>(() => body.atmosphere));
            AddSuffix("atmosphereContainsOxygen", new Suffix<BooleanValue>(() => body.atmosphereContainsOxygen));
            AddSuffix("atmosphereDepth", new Suffix<ScalarValue>(() => body.atmosphereDepth));
            AddSuffix("atmosphereTemperatureSeaLevel", new Suffix<ScalarValue>(() => body.atmosphereTemperatureSeaLevel));
            AddSuffix("atmospherePressureSeaLevel", new Suffix<ScalarValue>(() => body.atmospherePressureSeaLevel));
            AddSuffix("atmosphereMolarMass", new Suffix<ScalarValue>(() => body.atmosphereMolarMass));
            AddSuffix("atmosphereAdiabaticIndex", new Suffix<ScalarValue>(() => body.atmosphereAdiabaticIndex));
            AddSuffix("atmosphereTemperatureLapseRate", new Suffix<ScalarValue>(() => body.atmosphereTemperatureLapseRate));
            AddSuffix("atmosphereGasMassLapseRate", new Suffix<ScalarValue>(() => body.atmosphereGasMassLapseRate));
            AddSuffix("atmosphereUseTemperatureCurve", new Suffix<BooleanValue>(() => body.atmosphereUseTemperatureCurve));
            AddSuffix("atmosphereTemperatureCurveIsNormalized", new Suffix<BooleanValue>(() => body.atmosphereTemperatureCurveIsNormalized));
            //AddSuffix("atmosphereTemperatureCurve", new Suffix<ScalarValue>(() => body.atmosphereTemperatureCurve));
            //AddSuffix("latitudeTemperatureBiasCurve", new Suffix<ScalarValue>(() => body.latitudeTemperatureBiasCurve));
            //AddSuffix("latitudeTemperatureSunMultCurve", new Suffix<ScalarValue>(() => body.latitudeTemperatureSunMultCurve));
            //AddSuffix("axialTemperatureSunMultCurve", new Suffix<ScalarValue>(() => body.axialTemperatureSunMultCurve));
            //AddSuffix("axialTemperatureSunBiasCurve", new Suffix<ScalarValue>(() => body.axialTemperatureSunBiasCurve));
            //AddSuffix("atmosphereTemperatureSunMultCurve", new Suffix<ScalarValue>(() => body.atmosphereTemperatureSunMultCurve));
            AddSuffix("maxAxialDot", new Suffix<ScalarValue>(() => body.maxAxialDot));
            //AddSuffix("eccentricityTemperatureBiasCurve", new Suffix<ScalarValue>(() => body.eccentricityTemperatureBiasCurve));
            AddSuffix("albedo", new Suffix<ScalarValue>(() => body.albedo));
            AddSuffix("emissivity", new Suffix<ScalarValue>(() => body.emissivity));
            AddSuffix("coreTemperatureOffset", new Suffix<ScalarValue>(() => body.coreTemperatureOffset));
            AddSuffix("convectionMultiplier", new Suffix<ScalarValue>(() => body.convectionMultiplier));
            AddSuffix("shockTemperatureMultiplier", new Suffix<ScalarValue>(() => body.shockTemperatureMultiplier));
            AddSuffix("bodyTemperature", new Suffix<ScalarValue>(() => body.bodyTemperature));
            AddSuffix("atmosphereUsePressureCurve", new Suffix<BooleanValue>(() => body.atmosphereUsePressureCurve));
            AddSuffix("atmospherePressureCurveIsNormalized", new Suffix<BooleanValue>(() => body.atmospherePressureCurveIsNormalized));
            //AddSuffix("atmospherePressureCurve", new Suffix<ScalarValue>(() => body.atmospherePressureCurve));
            AddSuffix("radiusAtmoFactor", new Suffix<ScalarValue>(() => body.radiusAtmoFactor));
            AddSuffix("hasSolidSurface", new Suffix<BooleanValue>(() => body.hasSolidSurface));
            AddSuffix("isStar", new Suffix<BooleanValue>(() => body.isStar));
            AddSuffix("transformRight", new Suffix<Vector>(() => new Vector(body.transformRight)));
            AddSuffix("transformUp", new Suffix<Vector>(() => new Vector(body.transformUp)));
            AddSuffix("rotation", new Suffix<Direction>(() => new Direction(body.rotation)));
            //AddSuffix("orbitDriver", new Suffix<ScalarValue>(() => body.orbitDriver));
            //AddSuffix("pqsController", new Suffix<ScalarValue>(() => body.pqsController));
            //AddSuffix("pqsSurfaceObjects", new Suffix<ScalarValue>(() => body.pqsSurfaceObjects));
            //AddSuffix("scaledBody", new Suffix<ScalarValue>(() => body.scaledBody));
            //AddSuffix("afg", new Suffix<ScalarValue>(() => body.afg));
            AddSuffix("rotates", new Suffix<BooleanValue>(() => body.rotates));
            AddSuffix("rotationPeriod", new Suffix<ScalarValue>(() => body.rotationPeriod));
            AddSuffix("rotPeriodRecip", new Suffix<ScalarValue>(() => body.rotPeriodRecip));
            AddSuffix("solarDayLength", new Suffix<ScalarValue>(() => body.solarDayLength));
            AddSuffix("solarRotationPeriod", new Suffix<BooleanValue>(() => body.solarRotationPeriod));
            AddSuffix("initialRotation", new Suffix<ScalarValue>(() => body.initialRotation));
            AddSuffix("rotationAngle", new Suffix<ScalarValue>(() => body.rotationAngle));
            AddSuffix("directRotAngle", new Suffix<ScalarValue>(() => body.directRotAngle));
            AddSuffix("angularVelocity", new Suffix<Vector>(() => new Vector(body.angularVelocity)));
            AddSuffix("zUpAngularVelocity", new Suffix<Vector>(() => new Vector(body.zUpAngularVelocity)));
            AddSuffix("tidallyLocked", new Suffix<BooleanValue>(() => body.tidallyLocked));
            AddSuffix("clampInverseRotThreshold", new Suffix<BooleanValue>(() => body.clampInverseRotThreshold));
            AddSuffix("inverseRotation", new Suffix<BooleanValue>(() => body.inverseRotation));
            AddSuffix("inverseRotThresholdAltitude", new Suffix<ScalarValue>(() => body.inverseRotThresholdAltitude));
            AddSuffix("angularV", new Suffix<ScalarValue>(() => body.angularV));
            AddSuffix("timeWarpAltitudeLimits", new Suffix<ListValue>(() => (ListValue)body.timeWarpAltitudeLimits.Select(x => new ScalarDoubleValue(x))));
            AddSuffix("atmosphericAmbientColor", new Suffix<RgbaColor>(() => new RgbaColor(body.atmosphericAmbientColor.r, body.atmosphericAmbientColor.g, body.atmosphericAmbientColor.b, body.atmosphericAmbientColor.a)));
            AddSuffix("orbitingBodies", new Suffix<ListValue>(() => (ListValue)body.orbitingBodies.Select(x => Create(x, shared))));
            //AddSuffix("BodyFrame", new Suffix<ScalarValue>(() => body.BodyFrame));
            //AddSuffix("progressTree", new Suffix<ScalarValue>(() => body.progressTree));
            //AddSuffix("bodyType", new Suffix<ScalarValue>(() => body.bodyType));
            //AddSuffix("scienceValues", new Suffix<ScalarValue>(() => body.scienceValues));
            //AddSuffix("BiomeMap", new Suffix<ScalarValue>(() => body.BiomeMap));
            //AddSuffix("MiniBiomes", new Suffix<ScalarValue>(() => body.MiniBiomes));
            //AddSuffix("bodyTransform", new Suffix<ScalarValue>(() => body.bodyTransform));
            AddSuffix("displayName", new Suffix<StringValue>(() => body.displayName));
            AddSuffix("name", new Suffix<StringValue>(() => body.name));
            AddSuffix("flightGlobalsIndex", new Suffix<ScalarValue>(() => body.flightGlobalsIndex));
            AddSuffix("position", new Suffix<Vector>(() => new Vector(body.position)));
            AddSuffix("orbit", new Suffix<OrbitInfo>(() => new OrbitInfo(body.orbit, shared)));
            //AddSuffix("MapObject", new Suffix<ScalarValue>(() => body.MapObject));
            AddSuffix("referenceBody", new Suffix<CelestialBodyStructure>(() => Create(body.referenceBody, shared)));
            //AddSuffix("ResourceMap", new Suffix<ScalarValue>(() => body.ResourceMap));
            //AddSuffix("DiscoveryInfo", new Suffix<ScalarValue>(() => body.DiscoveryInfo));
            AddSuffix("RotationAxis", new Suffix<Vector>(() => new Vector(body.RotationAxis)));

            // Functions
            AddSuffix("GetPressure", new OneArgsSuffix<ScalarValue, ScalarValue>((a) => body.GetPressure(a.GetDoubleValue())));
            AddSuffix("GetPressureAtm", new OneArgsSuffix<ScalarValue, ScalarValue>((a) => body.GetPressureAtm(a.GetDoubleValue())));
            AddSuffix("GetTemperature", new OneArgsSuffix<ScalarValue, ScalarValue>((a) => body.GetTemperature(a.GetDoubleValue())));
            AddSuffix("GetDensity", new TwoArgsSuffix<ScalarValue, ScalarValue, ScalarValue>((p, t) => body.GetDensity(p.GetDoubleValue(), t.GetDoubleValue())));
            AddSuffix("GetSpeedOfSound", new TwoArgsSuffix<ScalarValue, ScalarValue, ScalarValue>((p, t) => body.GetSpeedOfSound(p.GetDoubleValue(), t.GetDoubleValue())));
            AddSuffix("GetSolarPowerFactor", new OneArgsSuffix<ScalarValue, ScalarValue>((d) => body.GetSolarPowerFactor(d.GetDoubleValue())));
            AddSuffix("CBUpdate", new NoArgsVoidSuffix(() => body.CBUpdate()));
            //AddSuffix("getBounds", new Suffix<ScalarValue>(() => body.getBounds()));
            AddSuffix("GetFrameVel", new Suffix<Vector>(() => new Vector(body.GetFrameVel())));
            AddSuffix("GetFrameVelAtUT", new OneArgsSuffix<Vector, ScalarValue>((t) => new Vector(body.GetFrameVelAtUT(t.GetDoubleValue()))));
            AddSuffix("getRFrmVel", new OneArgsSuffix<Vector, Vector>((wp) => new Vector(body.getRFrmVel(wp.ToVector3D()))));
            //AddSuffix("getRFrmVelOrbit", new OneArgsSuffix<OrbitInfo, OrbitInfo>((o) => new Vector(body.getRFrmVelOrbit(o))));
            AddSuffix("getTruePositionAtUT", new OneArgsSuffix<Vector, ScalarValue>((t) => new Vector(body.getTruePositionAtUT(t.GetDoubleValue()))));
            AddSuffix("getPositionAtUT", new OneArgsSuffix<Vector, ScalarValue>((t) => new Vector(body.getPositionAtUT(t.GetDoubleValue()))));
            AddSuffix("GetRelSurfaceNVector", new TwoArgsSuffix<Vector, ScalarValue, ScalarValue>((lat, lng) => new Vector(body.GetRelSurfaceNVector(lat.GetDoubleValue(), lng.GetDoubleValue()))));
            AddSuffix("GetSurfaceNVector", new TwoArgsSuffix<Vector, ScalarValue, ScalarValue>((lat, lng) => new Vector(body.GetSurfaceNVector(lat.GetDoubleValue(), lng.GetDoubleValue()))));
            
            // TODO: convert to optional to overload
            AddSuffix("GetRelSurfacePosition", new ThreeArgsSuffix<Vector, ScalarValue, ScalarValue, ScalarValue>((lat, lng, alt) => new Vector(body.GetRelSurfacePosition(lat.GetDoubleValue(), lng.GetDoubleValue(), alt.GetDoubleValue()))));
            //AddSuffix("GetRelSurfacePosition", new Suffix<ScalarValue>(() => body.GetRelSurfacePosition()));
            //AddSuffix("GetRelSurfacePosition", new Suffix<ScalarValue>(() => body.GetRelSurfacePosition()));
            AddSuffix("GetRelSurfaceDirection", new OneArgsSuffix<Vector, Vector>((p) => new Vector(body.GetRelSurfaceDirection(p.ToVector3D()))));
            AddSuffix("GetWorldSurfacePosition", new ThreeArgsSuffix<Vector, ScalarValue, ScalarValue, ScalarValue>((lat, lng, alt) => new Vector(body.GetWorldSurfacePosition(lat.GetDoubleValue(), lng.GetDoubleValue(), alt.GetDoubleValue()))));
            AddSuffix("GetLatitude", new TwoArgsSuffix<ScalarValue, Vector, BooleanValue>((p, r) => body.GetLatitude(p.ToVector3D(), r)));
            AddSuffix("GetLongitude", new TwoArgsSuffix<ScalarValue, Vector, BooleanValue>((p, r) => body.GetLongitude(p.ToVector3D(), r)));
            //AddSuffix("GetLatitudeAndLongitude", new Suffix<ScalarValue>(() => body.GetLatitudeAndLongitude()));
            //AddSuffix("GetImpactLatitudeAndLongitude", new Suffix<BooleanValue, Vector, Vector, ScalarValue, ScalarValue>(() => body.GetImpactLatitudeAndLongitude()));
            AddSuffix("GetAltitude", new OneArgsSuffix<ScalarValue, Vector>((p) => body.GetAltitude(p.ToVector3D())));
            //AddSuffix("GetLatLonAlt", new Suffix<ScalarValue>(() => body.GetLatLonAlt()));
            //AddSuffix("GetLatLonAltOrbital", new Suffix<ScalarValue>(() => body.GetLatLonAltOrbital()));
            //AddSuffix("GetRandomLatitudeAndLongitude", new Suffix<ScalarValue>(() => body.GetRandomLatitudeAndLongitude()));
            AddSuffix("TerrainAltitude", new ThreeArgsSuffix<ScalarValue, ScalarValue, ScalarValue, BooleanValue>((lat, lng, an) => body.TerrainAltitude(lat.GetDoubleValue(), lng.GetDoubleValue(), an)));
            AddSuffix("HasParent", new OneArgsSuffix<BooleanValue, CelestialBodyStructure>((b) => body.HasParent(b.body)));
            AddSuffix("HasChild", new OneArgsSuffix<BooleanValue, CelestialBodyStructure>((b) => body.HasChild(b.body)));
            //AddSuffix("GetTransform", new Suffix<ScalarValue>(() => body.GetTransform()));
            AddSuffix("GetObtVelocity", new Suffix<Vector>(() => new Vector(body.GetObtVelocity())));
            AddSuffix("GetSrfVelocity", new Suffix<Vector>(() => new Vector(body.GetSrfVelocity())));
            AddSuffix("GetFwdVector", new Suffix<Vector>(() => new Vector(body.GetFwdVector())));
            //AddSuffix("GetVessel", new Suffix<ScalarValue>(() => body.GetVessel()));
            AddSuffix("GetName", new Suffix<StringValue>(() => body.GetName()));
            AddSuffix("GetDisplayName", new Suffix<StringValue>(() => body.GetDisplayName()));
            AddSuffix("GetOrbit", new Suffix<OrbitInfo>(() => new OrbitInfo(body.GetOrbit(), shared)));
            //AddSuffix("GetOrbitDriver", new Suffix<ScalarValue>(() => body.GetOrbitDriver()));
            //AddSuffix("GetTargetingMode", new Suffix<ScalarValue>(() => body.GetTargetingMode()));
            AddSuffix("GetActiveTargetable", new Suffix<BooleanValue>(() => body.GetActiveTargetable()));
            //AddSuffix("SetResourceMap", new Suffix<ScalarValue>(() => body.SetResourceMap()));
            //AddSuffix("HideSurfaceResource", new Suffix<ScalarValue>(() => body.HideSurfaceResource()));
            AddSuffix("RevealName", new Suffix<StringValue>(() => body.RevealName()));
            AddSuffix("RevealDisplayName", new Suffix<StringValue>(() => body.RevealDisplayName()));
            AddSuffix("RevealSpeed", new Suffix<ScalarValue>(() => body.RevealSpeed()));
            AddSuffix("RevealAltitude", new Suffix<ScalarValue>(() => body.RevealAltitude()));
            AddSuffix("RevealSituationString", new Suffix<StringValue>(() => body.RevealSituationString()));
            AddSuffix("RevealType", new Suffix<StringValue>(() => body.RevealType()));
            AddSuffix("RevealMass", new Suffix<ScalarValue>(() => body.RevealMass()));

        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (body == null) return "CelestialBody(null)";
            return string.Format("CelestialBody(name: {0})", body.name);
        }

    }
}
