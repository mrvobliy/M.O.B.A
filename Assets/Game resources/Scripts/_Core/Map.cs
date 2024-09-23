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

	[Header("Dark")]
	[SerializeField] private Throne _darkThrone;
	[SerializeField] private Tower _darkBottomTower1;
	[SerializeField] private Tower _darkBottomTower2;
	[SerializeField] private Tower _darkMiddleTower1;
	[SerializeField] private Tower _darkMiddleTower2;
	[SerializeField] private Tower _darkTopTower1;
	[SerializeField] private Tower _darkTopTower2;

	[Header("Detectors")]
	[SerializeField] private DetectionZone _topZone;
	[SerializeField] private DetectionZone _middleZone;
	[SerializeField] private DetectionZone _bottomZone;

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

	public Tower GetTower(Team team, Lane lane, int tier)
	{
		switch (team)
		{
			case Team.Light:
			{
				return lane switch
				{
					Lane.Top when tier == 1 => _lightTopTower1,
					Lane.Top when tier == 2 => _lightTopTower2,

					Lane.Middle when tier == 1 => _lightMiddleTower1,
					Lane.Middle when tier == 2 => _lightMiddleTower2,

					Lane.Bottom when tier == 1 => _lightBottomTower1,
					Lane.Bottom when tier == 2 => _lightBottomTower2,

					_ => null
				};
			}

			case Team.Dark:
			{
				return lane switch
				{
					Lane.Top when tier == 1 => _darkTopTower1,
					Lane.Top when tier == 2 => _darkTopTower2,

					Lane.Middle when tier == 1 => _darkMiddleTower1,
					Lane.Middle when tier == 2 => _darkMiddleTower2,

					Lane.Bottom when tier == 1 => _darkBottomTower1,
					Lane.Bottom when tier == 2 => _darkBottomTower2,

					_ => null
				};
			}
		}

		Debug.LogError("Tower not found");
		return null;
	}

	public DetectionZone GetDetector(Lane lane)
	{
		return lane switch
		{
			Lane.Top => _topZone,
			Lane.Middle => _middleZone,
			Lane.Bottom => _bottomZone,

			_ => null
		};
	}
}