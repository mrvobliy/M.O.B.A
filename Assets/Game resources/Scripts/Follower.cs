using UnityEngine;

public class Follower : MonoBehaviour
{
	[SerializeField] private Transform _target;

	private void LateUpdate()
	{
		transform.position = _target.position;
	}
}
