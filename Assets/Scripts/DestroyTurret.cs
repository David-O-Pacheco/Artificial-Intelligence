using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTurret : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    // Destroy turret on bullet collisions //

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MinionBullet" || other.gameObject.tag == "MinionBullet2")
        {
            Destroy(gameObject);
        }
    }
}
