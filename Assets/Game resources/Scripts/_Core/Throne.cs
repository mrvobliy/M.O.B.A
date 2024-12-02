using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.AI;

public class Throne : Target
{
	[Header("Throne")]
	[SerializeField] private NavMeshObstacle _obstacle;

	public override float Radius => _obstacle.radius;
	public override void TryStun(int percentChanceStun, float timeStun) {}

	[ProButton]
	private void DebugDeath()
	{
		var player = FindObjectOfType<PlayerHero>();
		TakeDamage(player, 1000000);
	}
}
