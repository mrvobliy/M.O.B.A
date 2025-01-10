using UnityEngine;

public enum Team
{
	Neutral,
	Light,
	Dark,
}

public enum Lane
{
	Top,
	Bottom,
	Middle
}

public enum EntityType
{
	Hero,
	LaneMelee,
	LaneRange,
	NeutralMelee,
	NeutralRange,
	Tower,
	Trone
}

public class Map : MonoBehaviour
{
	[Header("Light")]
	[SerializeField] private Throne _lightThrone;
	[SerializeField] private Tower _lightBottomTower1;
	[SerializeField] private Tower _lightBottomTower2;
	[SerializeField] private Tower _lightMiddleTower1;
	[SerializeField] private Tower _lightMiddleTower2;
	[SerializeField] private Tower _lightTopTower1;
	[SerializeField] private Tower _lightTopTower2;
	[SerializeField] private Transform[] _lightTopWaypoints;
	[SerializeField] private Transform[] _lightMiddleWaypoints;
	[SerializeField] private Transform[] _lightBottomWaypoints;

	[Header("Dark")]
	[SerializeField] private Throne _darkThrone;
	[SerializeField] private Tower _darkBottomTower1;
	[SerializeField] private Tower _darkBottomTower2;
	[SerializeField] private Tower _darkMiddleTower1;
	[SerializeField] private Tower _darkMiddleTower2;
	[SerializeField] private Tower _darkTopTower1;
	[SerializeField] private Tower _darkTopTower2;
	[SerializeField] private Transform[] _darkTopWaypoints;
	[SerializeField] private Transform[] _darkMiddleWaypoints;
	[SerializeField] private Transform[] _darkBottomWaypoints;

	public static Map Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	public Throne GetThrone(Team team)
	{
		return team switch
		{
			Team.Light => _lightThrone,
			Team.Dark => _darkThrone,
			_ => null
		};
	}

	public Target GetFirstAliveBuilding(Team team, Lane lane)
	{
		switch (team)
		{
			case Team.Light:
			{
				return lane switch
				{
					Lane.Top when _lightTopTower2.IsDead == false => _lightTopTower2,
					Lane.Top when _lightTopTower1.IsDead == false => _lightTopTower1,

					Lane.Middle when _lightMiddleTower2.IsDead == false => _lightMiddleTower2,
					Lane.Middle when _lightMiddleTower1.IsDead == false => _lightMiddleTower1,

					Lane.Bottom when _lightBottomTower2.IsDead == false => _lightBottomTower2,
					Lane.Bottom when _lightBottomTower1.IsDead == false => _lightBottomTower1,

					_ => _lightThrone
				};
			}

			case Team.Dark:
			{
				return lane switch
				{
					Lane.Top when _darkTopTower2.IsDead == false => _darkTopTower2,
					Lane.Top when _darkTopTower1.IsDead == false => _darkTopTower1,

					Lane.Middle when _darkMiddleTower2.IsDead == false => _darkMiddleTower2,
					Lane.Middle when _darkMiddleTower1.IsDead == false => _darkMiddleTower1,

					Lane.Bottom when _darkBottomTower2.IsDead == false => _darkBottomTower2,
					Lane.Bottom when _darkBottomTower1.IsDead == false => _darkBottomTower1,

					_ => _darkThrone
				};
			}
		}

		Debug.LogError("Building not found");
		return null;
	}

	public Transform[] GetWaypoints(Team team, Lane lane)
	{
		switch (team)
		{
			case Team.Light:
			{
				return lane switch
				{
					Lane.Top => _lightTopWaypoints,
					Lane.Middle => _lightMiddleWaypoints,
					Lane.Bottom => _lightBottomWaypoints,

					_ => null
				};
			}

			case Team.Dark:
			{
				return lane switch
				{
					Lane.Top => _darkTopWaypoints,
					Lane.Middle => _darkMiddleWaypoints,
					Lane.Bottom => _darkBottomWaypoints,

					_ => null
				};
			}
		}

		Debug.LogError("Waypoints not found");
		return null;
	}
}