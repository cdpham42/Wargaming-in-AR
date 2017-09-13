using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	int rotationSpeed = 80;  //sets rate of rotation

	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3 (0, rotationSpeed, 0) * Time.deltaTime);
	}
}
