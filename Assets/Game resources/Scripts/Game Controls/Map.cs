using UnityEngine;

public enum Team
{
	Neutral,
	Light,
	Dark,
	Map
}

public enum EntityType
{
	Hero,
	LaneMelee,
	LaneRange,
	NeutralMelee,
	NeutralRange,
	Tower,
	Throne,
	Boss,
	Fountain
}

public enum TowerTier
{
	T1,
	T2,
	T3
}

public class Map : MonoBehaviour {}