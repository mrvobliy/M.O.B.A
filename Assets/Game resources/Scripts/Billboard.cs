using UnityEngine;

public class Billboard : MonoBehaviour
{
	private void LateUpdate()
	{
		transform.rotation = Quaternion.Inverse(Camera.main.transform.rotation);
	}
}
