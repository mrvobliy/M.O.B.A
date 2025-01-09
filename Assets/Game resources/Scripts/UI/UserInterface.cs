using UnityEngine;

public class UserInterface : MonoBehaviour
{
	public static UserInterface Instance;

	private void Awake() => Instance = this;
}
