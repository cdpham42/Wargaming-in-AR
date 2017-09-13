using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class HelpWindowLauncher : MonoBehaviour {
	[SerializeField]
	public GameObject helpCanvas;
	bool isOpen;
	GameObject canvasCopy;

	public void launchHelpWindow() {
		if (isOpen) {
			return;
		}
		//Ray head = new Ray ();
	//	var pos = head.origin + head.direction * 4.0f;
		canvasCopy = Instantiate(helpCanvas, new Vector3(0,0,0), Quaternion.identity);
	//	transform.Rotate (Vector3.right * Time.deltaTime);
		isOpen = true;
	}

	public void dismissHelpWindow() {
		Destroy(canvasCopy);
		isOpen = false;
	}
}
