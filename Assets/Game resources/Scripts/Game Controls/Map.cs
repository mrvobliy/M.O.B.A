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
	Throne
}

public class Map : MonoBehaviour {}