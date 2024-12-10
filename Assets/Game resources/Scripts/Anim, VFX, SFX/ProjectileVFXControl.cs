using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileVFXControl : MonoBehaviour 
{
	public GameObject muzzlePrefab;
	public GameObject hitPrefab;
	public AudioClip shotSFX;
	public AudioClip hitSFX;
	public List<GameObject> trails;

	private float speedRandomness;
	private Vector3 offset;

	private void Start () 
	{ 
		if (muzzlePrefab != null) 
		{
			var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = gameObject.transform.forward ;
			var ps = muzzleVFX.GetComponent<ParticleSystem>();

			if (ps != null)
			{
				Destroy (muzzleVFX, ps.main.duration);
			}
			else 
			{
				var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
			}
		}

		if (shotSFX != null && GetComponent<AudioSource>()) 
		{
			GetComponent<AudioSource>().PlayOneShot (shotSFX);
		}
	}

	public void SpawnHitEffect (Transform contactPoint)
	{
		if (shotSFX != null && GetComponent<AudioSource>()) 
			GetComponent<AudioSource> ().PlayOneShot (hitSFX);

		if (trails.Count > 0)
		{
			foreach (var trail in trails)
			{
				trail.transform.parent = null;
				var ps = trail.GetComponent<ParticleSystem> ();

				if (ps == null) continue;
				
				ps.Stop ();
				Destroy (ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
			}
		}

		if (hitPrefab != null) 
		{
			var hitVFX = Instantiate (hitPrefab, contactPoint.position, contactPoint.rotation);
			var ps = hitVFX.GetComponent<ParticleSystem> ();
				
			if (ps == null) 
			{
				var psChild = hitVFX.transform.GetChild (0).GetComponent<ParticleSystem> ();
				Destroy (hitVFX, psChild.main.duration);
			}
			else
			{
				Destroy (hitVFX, ps.main.duration);
			}
		}

		StartCoroutine (DestroyParticle(0f));
	}

	private IEnumerator DestroyParticle (float waitTime) 
	{
		if (transform.childCount > 0 && waitTime != 0) 
		{
			var tList = transform.GetChild(0).transform.Cast<Transform>().ToList();

			while (transform.GetChild(0).localScale.x > 0) 
			{
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				
				foreach (var t in tList)
				{
					t.localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
