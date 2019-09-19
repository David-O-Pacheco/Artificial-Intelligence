using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBullet : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

        // Destroy Bullets after 1 second timer //

        Destroy(gameObject, 1);

	}
}
