using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleItem : MonoBehaviour
{
    public ParticleSystem particles;

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        Instantiate(particles, transform.position,transform.rotation);
    }

}
