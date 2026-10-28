using Godot;

namespace godot1;

/// <summary>
/// Coupled-tank twin scene: industrial materials, glass vessels, and soft studio lighting.
/// Tank max level is 3.0 m (matches Lab 02).
/// </summary>
public partial class TwinWorld : Node3D
{
	public const float LevelMaxM = 3.0f;
	/// <summary>Keep liquid bottoms above the floor to avoid z-fighting sparkles.</summary>
	private const float LiquidBottomY = 0.06f;
	private const float TankHeight = 2.05f;
	private const float TankRadius = 0.52f;
	private const float LiquidRadius = 0.46f;

	private MeshInstance3D _liquid1 = null!;
	private MeshInstance3D _liquid2 = null!;
	private MeshInstance3D _liquidSurface1 = null!;
	private MeshInstance3D _liquidSurface2 = null!;
	private MeshInstance3D _valveBody = null!;
	private MeshInstance3D _valveStem = null!;
	private MeshInstance3D _setpointMarker = null!;
	private StandardMaterial3D _liquidMat1 = null!;
	private StandardMaterial3D _liquidMat2 = null!;
	private StandardMaterial3D _liquidSurfaceMat1 = null!;
	private StandardMaterial3D _liquidSurfaceMat2 = null!;
	private StandardMaterial3D _valveAccentMat = null!;

	// Opaque saturated fills — solid glass shells previously washed these to grey.
	private static readonly Color LiquidGreen = new(0.05f, 0.82f, 0.32f);
	private static readonly Color LiquidGreenSurface = new(0.35f, 0.98f, 0.55f);
	private static readonly Color LiquidBlue = new(0.08f, 0.42f, 0.98f);
	private static readonly Color LiquidBlueSurface = new(0.4f, 0.75f, 1.0f);
	private static readonly Color LiquidStale = new(0.48f, 0.5f, 0.52f);
	private static readonly Color LiquidStaleSurface = new(0.62f, 0.64f, 0.66f);

	private float _level1;
	private float _level2;
	private float _valvePct = 40f;
	private float _setpoint = 0.8f;
	private bool _stale;

	public override void _Ready()
	{
		BuildEnvironment();
		BuildScene();
		ApplyVisuals();
	}

	public void ApplyTelemetry(float level1M, float level2M, float valvePct, float setpointM, bool stale)
	{
		_level1 = level1M;
		_level2 = level2M;
		_valvePct = valvePct;
		_setpoint = setpointM;
		_stale = stale;
		ApplyVisuals();
	}

	private void BuildEnvironment()
	{
		var skyMat = new ProceduralSkyMaterial
		{
			SkyTopColor = new Color(0.18f, 0.22f, 0.28f),
			SkyHorizonColor = new Color(0.42f, 0.45f, 0.48f),
			GroundBottomColor = new Color(0.12f, 0.12f, 0.13f),
			GroundHorizonColor = new Color(0.28f, 0.29f, 0.30f),
			SkyEnergyMultiplier = 0.85f,
			GroundEnergyMultiplier = 0.35f
		};

		var env = new Godot.Environment
		{
			BackgroundMode = Godot.Environment.BGMode.Sky,
			Sky = new Sky { SkyMaterial = skyMat },
			AmbientLightSource = Godot.Environment.AmbientSource.Sky,
			AmbientLightEnergy = 0.55f,
			TonemapMode = Godot.Environment.ToneMapper.Aces,
			TonemapExposure = 1.05f,
			SsaoEnabled = true,
			SsaoRadius = 1.2f,
			SsaoIntensity = 0.55f,
			GlowEnabled = true,
			GlowIntensity = 0.25f,
			GlowStrength = 0.7f,
			FogEnabled = true,
			FogLightColor = new Color(0.55f, 0.58f, 0.62f),
			FogDensity = 0.012f,
			FogAerialPerspective = 0.35f
		};

		AddChild(new WorldEnvironment { Name = "WorldEnv", Environment = env });

		var key = new DirectionalLight3D
		{
			Name = "KeyLight",
			LightEnergy = 1.35f,
			LightColor = new Color(1.0f, 0.97f, 0.92f),
			ShadowEnabled = true,
			DirectionalShadowMode = DirectionalLight3D.ShadowMode.Parallel4Splits,
			ShadowBlur = 1.2f
		};
		key.RotationDegrees = new Vector3(-52f, -38f, 0f);
		AddChild(key);

		var fill = new DirectionalLight3D
		{
			Name = "FillLight",
			LightEnergy = 0.28f,
			LightColor = new Color(0.75f, 0.85f, 1.0f),
			ShadowEnabled = false
		};
		fill.RotationDegrees = new Vector3(-18f, 145f, 0f);
		AddChild(fill);

		var rim = new OmniLight3D
		{
			Name = "RimLight",
			LightEnergy = 1.8f,
			LightColor = new Color(0.85f, 0.92f, 1.0f),
			OmniRange = 8f,
			OmniAttenuation = 1.4f,
			Position = new Vector3(2.8f, 3.2f, -2.4f),
			ShadowEnabled = false
		};
		AddChild(rim);
	}

	private void BuildScene()
	{
		AddChild(BuildFloor());
		AddChild(BuildSkid(new Vector3(-1.45f, 0f, 0f), "Tank1Skid"));
		AddChild(BuildSkid(new Vector3(1.45f, 0f, 0f), "Tank2Skid"));

		_liquidMat1 = MakeLiquidMat(LiquidGreen);
		_liquidSurfaceMat1 = MakeLiquidSurfaceMat(LiquidGreenSurface, new Color(0.08f, 0.35f, 0.15f));
		_liquidMat2 = MakeLiquidMat(LiquidBlue);
		_liquidSurfaceMat2 = MakeLiquidSurfaceMat(LiquidBlueSurface, new Color(0.08f, 0.22f, 0.45f));

		AddChild(BuildTankAssembly(new Vector3(-1.45f, 0f, 0f), "Tank1", _liquidMat1, _liquidSurfaceMat1,
			out _liquid1, out _liquidSurface1));
		AddChild(BuildTankAssembly(new Vector3(1.45f, 0f, 0f), "Tank2", _liquidMat2, _liquidSurfaceMat2,
			out _liquid2, out _liquidSurface2));

		AddChild(BuildCouplingPipe());
		AddChild(BuildInletAndValve());

		_setpointMarker = BuildSetpointMarker();
		AddChild(_setpointMarker);

		var orbit = new OrbitCamera { Name = "OrbitCamera" };
		AddChild(orbit);
	}

	private Node3D BuildFloor()
	{
		var root = new Node3D { Name = "Floor" };

		var slabMat = MetalMat(
			albedo: new Color(0.32f, 0.33f, 0.35f),
			metallic: 0.05f,
			roughness: 0.78f);
		var slab = MakeBox(new Vector3(7.2f, 0.12f, 4.0f), slabMat);
		slab.Name = "ConcreteSlab";
		slab.Position = new Vector3(0f, -0.06f, 0f);
		root.AddChild(slab);

		var grateMat = MetalMat(
			albedo: new Color(0.22f, 0.24f, 0.26f),
			metallic: 0.65f,
			roughness: 0.55f);
		var grate = MakeBox(new Vector3(6.4f, 0.02f, 3.2f), grateMat);
		grate.Name = "DeckPlate";
		grate.Position = new Vector3(0f, 0.01f, 0f);
		root.AddChild(grate);

		// Subtle aisle stripe for industrial scale reference.
		var stripeMat = new StandardMaterial3D
		{
			AlbedoColor = new Color(0.85f, 0.62f, 0.12f),
			Roughness = 0.7f,
			Metallic = 0.1f
		};
		var stripe = MakeBox(new Vector3(6.0f, 0.005f, 0.08f), stripeMat);
		stripe.Position = new Vector3(0f, 0.025f, 1.35f);
		root.AddChild(stripe);

		return root;
	}

	private static Node3D BuildSkid(Vector3 position, string name)
	{
		var root = new Node3D { Name = name, Position = position };
		var beamMat = MetalMat(
			albedo: new Color(0.28f, 0.30f, 0.33f),
			metallic: 0.75f,
			roughness: 0.45f);

		foreach (var z in new[] { -0.42f, 0.42f })
		{
			var beam = MakeBox(new Vector3(1.15f, 0.08f, 0.1f), beamMat);
			beam.Position = new Vector3(0f, 0.05f, z);
			root.AddChild(beam);
		}

		foreach (var x in new[] { -0.42f, 0.42f })
		{
			var cross = MakeBox(new Vector3(0.1f, 0.08f, 0.95f), beamMat);
			cross.Position = new Vector3(x, 0.05f, 0f);
			root.AddChild(cross);
		}

		return root;
	}

	private Node3D BuildTankAssembly(
		Vector3 position,
		string name,
		StandardMaterial3D liquidMat,
		StandardMaterial3D surfaceMat,
		out MeshInstance3D liquid,
		out MeshInstance3D surface)
	{
		var root = new Node3D { Name = name, Position = position };

		// Use Godot CylinderMesh (smooth normals). Custom low-segment tubes looked ribbed.
		var glassMat = new StandardMaterial3D
		{
			AlbedoColor = new Color(0.82f, 0.90f, 0.95f, 0.14f),
			Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
			Metallic = 0.02f,
			Roughness = 0.12f,
			SpecularMode = BaseMaterial3D.SpecularModeEnum.SchlickGgx,
			CullMode = BaseMaterial3D.CullModeEnum.Disabled
		};

		var shell = MakeCylinder(TankRadius, TankHeight, glassMat, rings: 96);
		shell.Name = $"{name}Shell";
		shell.Position = new Vector3(0f, TankHeight * 0.5f + 0.12f, 0f);
		root.AddChild(shell);

		var metal = MetalMat(new Color(0.55f, 0.58f, 0.62f), metallic: 0.85f, roughness: 0.32f);
		var darkMetal = MetalMat(new Color(0.35f, 0.37f, 0.40f), metallic: 0.9f, roughness: 0.38f);

		var baseRing = MakeCylinder(TankRadius + 0.04f, 0.08f, darkMetal, rings: 64);
		baseRing.Position = new Vector3(0f, 0.12f, 0f);
		root.AddChild(baseRing);

		var midBand = MakeCylinder(TankRadius + 0.015f, 0.05f, metal, rings: 64);
		midBand.Position = new Vector3(0f, 1.15f, 0f);
		root.AddChild(midBand);

		var topRim = MakeCylinder(TankRadius + 0.035f, 0.07f, metal, rings: 64);
		topRim.Position = new Vector3(0f, TankHeight + 0.12f, 0f);
		root.AddChild(topRim);

		var lid = MakeCylinder(TankRadius - 0.02f, 0.04f, darkMetal, rings: 48);
		lid.Position = new Vector3(0f, TankHeight + 0.155f, 0f);
		root.AddChild(lid);

		// Level scale ticks (every ~0.5 m visual).
		var tickMat = MetalMat(new Color(0.75f, 0.78f, 0.8f), metallic: 0.4f, roughness: 0.5f);
		for (var i = 1; i <= 5; i++)
		{
			var y = 0.12f + (TankHeight * i / 6f);
			var tick = MakeBox(new Vector3(0.08f, 0.015f, 0.02f), tickMat);
			tick.Position = new Vector3(TankRadius + 0.01f, y, 0f);
			root.AddChild(tick);
		}

		liquid = MakeCylinder(LiquidRadius, 1f, liquidMat, rings: 64);
		liquid.Name = $"{name}Liquid";
		liquid.Position = new Vector3(0f, LiquidBottomY + 0.5f, 0f);
		root.AddChild(liquid);

		surface = new MeshInstance3D
		{
			Name = $"{name}Surface",
			Mesh = new CylinderMesh
			{
				TopRadius = LiquidRadius - 0.01f,
				BottomRadius = LiquidRadius - 0.01f,
				Height = 0.02f,
				RadialSegments = 64
			},
			MaterialOverride = surfaceMat
		};
		root.AddChild(surface);

		return root;
	}

	private Node3D BuildCouplingPipe()
	{
		var root = new Node3D { Name = "Coupling" };
		var pipe = MetalMat(new Color(0.62f, 0.64f, 0.68f), metallic: 0.88f, roughness: 0.28f);
		var flange = MetalMat(new Color(0.4f, 0.42f, 0.45f), metallic: 0.85f, roughness: 0.4f);

		var tube = MakeBox(new Vector3(1.85f, 0.1f, 0.1f), pipe);
		tube.Position = new Vector3(0f, 0.38f, 0f);
		root.AddChild(tube);

		foreach (var x in new[] { -0.85f, 0f, 0.85f })
		{
			var ring = MakeCylinder(0.12f, 0.04f, flange, rings: 24);
			ring.RotationDegrees = new Vector3(0f, 0f, 90f);
			ring.Position = new Vector3(x, 0.38f, 0f);
			root.AddChild(ring);
		}

		// Short vertical stubs into each tank.
		foreach (var x in new[] { -1.45f, 1.45f })
		{
			var stub = MakeCylinder(0.055f, 0.28f, pipe, rings: 20);
			stub.Position = new Vector3(x, 0.28f, 0f);
			root.AddChild(stub);

			var elbow = MakeBox(new Vector3(0.22f, 0.1f, 0.1f), pipe);
			elbow.Position = new Vector3(x + (x < 0 ? 0.12f : -0.12f), 0.38f, 0f);
			root.AddChild(elbow);
		}

		return root;
	}

	private Node3D BuildInletAndValve()
	{
		var root = new Node3D { Name = "InletValve" };
		var pipe = MetalMat(new Color(0.62f, 0.64f, 0.68f), metallic: 0.88f, roughness: 0.28f);
		var body = MetalMat(new Color(0.45f, 0.48f, 0.5f), metallic: 0.9f, roughness: 0.35f);
		_valveAccentMat = new StandardMaterial3D
		{
			AlbedoColor = new Color(0.18f, 0.72f, 0.42f),
			Metallic = 0.55f,
			Roughness = 0.35f,
			EmissionEnabled = true,
			Emission = new Color(0.05f, 0.25f, 0.12f),
			EmissionEnergyMultiplier = 0.4f
		};

		var inlet = MakeBox(new Vector3(0.85f, 0.09f, 0.09f), pipe);
		inlet.Position = new Vector3(-2.05f, 1.35f, 0f);
		root.AddChild(inlet);

		var drop = MakeCylinder(0.05f, 0.45f, pipe, rings: 18);
		drop.Position = new Vector3(-1.72f, 1.15f, 0f);
		root.AddChild(drop);

		_valveBody = new MeshInstance3D
		{
			Name = "ValveBody",
			Mesh = new CylinderMesh
			{
				TopRadius = 0.14f,
				BottomRadius = 0.14f,
				Height = 0.22f,
				RadialSegments = 28
			},
			MaterialOverride = body,
			Position = new Vector3(-2.45f, 1.35f, 0f),
			RotationDegrees = new Vector3(0f, 0f, 90f)
		};
		root.AddChild(_valveBody);

		var actuator = MakeBox(new Vector3(0.2f, 0.28f, 0.2f), _valveAccentMat);
		actuator.Name = "ValveActuator";
		actuator.Position = new Vector3(-2.45f, 1.62f, 0f);
		root.AddChild(actuator);
		_valveStem = actuator;

		var flangeL = MakeCylinder(0.16f, 0.04f, body, rings: 20);
		flangeL.RotationDegrees = new Vector3(0f, 0f, 90f);
		flangeL.Position = new Vector3(-2.58f, 1.35f, 0f);
		root.AddChild(flangeL);

		var flangeR = MakeCylinder(0.16f, 0.04f, body, rings: 20);
		flangeR.RotationDegrees = new Vector3(0f, 0f, 90f);
		flangeR.Position = new Vector3(-2.32f, 1.35f, 0f);
		root.AddChild(flangeR);

		return root;
	}

	private MeshInstance3D BuildSetpointMarker()
	{
		var mat = new StandardMaterial3D
		{
			AlbedoColor = new Color(1.0f, 0.78f, 0.15f, 0.85f),
			Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
			Metallic = 0.2f,
			Roughness = 0.25f,
			EmissionEnabled = true,
			Emission = new Color(0.85f, 0.55f, 0.05f),
			EmissionEnergyMultiplier = 0.9f,
			CullMode = BaseMaterial3D.CullModeEnum.Disabled
		};

		var marker = new MeshInstance3D
		{
			Name = "SetpointMarker",
			Mesh = new TorusMesh
			{
				InnerRadius = 0.48f,
				OuterRadius = 0.52f,
				Rings = 24,
				RingSegments = 32
			},
			MaterialOverride = mat,
			Position = new Vector3(1.45f, 0.8f, 0f),
			RotationDegrees = new Vector3(90f, 0f, 0f)
		};
		return marker;
	}

	private void ApplyVisuals()
	{
		SetLiquid(_liquid1, _liquidSurface1, _level1);
		SetLiquid(_liquid2, _liquidSurface2, _level2);

		var t = Mathf.Clamp(_valvePct / 100f, 0f, 1f);
		_valveStem.RotationDegrees = new Vector3(0f, Mathf.Lerp(0f, 90f, t), 0f);
		_valveAccentMat.AlbedoColor = _stale
			? new Color(0.45f, 0.45f, 0.48f)
			: Color.FromHsv(Mathf.Lerp(0.33f, 0.08f, t), 0.75f, 0.75f);
		_valveAccentMat.Emission = _stale
			? new Color(0.1f, 0.1f, 0.1f)
			: _valveAccentMat.AlbedoColor * 0.35f;

		var spY = 0.12f + Mathf.Clamp(_setpoint / LevelMaxM, 0f, 1f) * TankHeight;
		_setpointMarker.Position = new Vector3(1.45f, spY, 0f);

		_liquidMat1.AlbedoColor = _stale ? LiquidStale : LiquidGreen;
		_liquidSurfaceMat1.AlbedoColor = _stale ? LiquidStaleSurface : LiquidGreenSurface;
		_liquidMat2.AlbedoColor = _stale ? LiquidStale : LiquidBlue;
		_liquidSurfaceMat2.AlbedoColor = _stale ? LiquidStaleSurface : LiquidBlueSurface;
	}

	private static StandardMaterial3D MakeLiquidMat(Color albedo)
	{
		return new StandardMaterial3D
		{
			AlbedoColor = albedo,
			// Opaque — transparency through a solid glass volume washed colors to grey.
			Metallic = 0.0f,
			Roughness = 0.22f,
			SpecularMode = BaseMaterial3D.SpecularModeEnum.SchlickGgx,
			EmissionEnabled = true,
			Emission = albedo * 0.25f,
			EmissionEnergyMultiplier = 0.55f
		};
	}

	private static StandardMaterial3D MakeLiquidSurfaceMat(Color albedo, Color emission)
	{
		return new StandardMaterial3D
		{
			AlbedoColor = albedo,
			Metallic = 0.15f,
			Roughness = 0.08f,
			EmissionEnabled = true,
			Emission = emission,
			EmissionEnergyMultiplier = 0.7f
		};
	}

	private static void SetLiquid(MeshInstance3D liquid, MeshInstance3D surface, float levelM)
	{
		var ratio = Mathf.Clamp(levelM / LevelMaxM, 0f, 1f);
		var height = Mathf.Max(0.04f, ratio * (TankHeight - 0.12f));
		liquid.Scale = new Vector3(1f, height, 1f);
		liquid.Position = new Vector3(liquid.Position.X, LiquidBottomY + height * 0.5f, liquid.Position.Z);
		surface.Position = new Vector3(surface.Position.X, LiquidBottomY + height, surface.Position.Z);
		surface.Visible = ratio > 0.01f;
	}

	private static StandardMaterial3D MetalMat(Color albedo, float metallic, float roughness)
	{
		return new StandardMaterial3D
		{
			AlbedoColor = albedo,
			Metallic = metallic,
			Roughness = roughness,
			SpecularMode = BaseMaterial3D.SpecularModeEnum.SchlickGgx
		};
	}

	private static MeshInstance3D MakeBox(Vector3 size, Material mat)
	{
		return new MeshInstance3D
		{
			Mesh = new BoxMesh { Size = size },
			MaterialOverride = mat
		};
	}

	private static MeshInstance3D MakeCylinder(float radius, float height, Material mat, int rings = 32)
	{
		return new MeshInstance3D
		{
			Mesh = new CylinderMesh
			{
				TopRadius = radius,
				BottomRadius = radius,
				Height = height,
				RadialSegments = rings
			},
			MaterialOverride = mat
		};
	}
}
