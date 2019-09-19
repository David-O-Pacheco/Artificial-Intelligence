using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBase : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

        // Rotate Base1 and Base2 heads //

        transform.Rotate(new Vector3(0, 50 * Time.deltaTime,0));

	}
}
