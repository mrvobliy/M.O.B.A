using UnityEngine;

public class TowerHealthControl : BuildHealthControl
{
    protected override void PlayDestroyAnimation()
    {
        _explosionEffect.Play();

        foreach (var element in _rigidBodies)
        {
            element.isKinematic = false;

            var exploreDir = new Vector3
            {
                x = Random.Range(-1.0f, 1.0f),
                y = Random.Range(0f, 1.5f),
                z = Random.Range(-1.0f, 1.0f)
            };

            element.AddForce(exploreDir * 200);
        }
        
        Invoke(nameof(DisableComponents), DiveDelay);
    }
}
