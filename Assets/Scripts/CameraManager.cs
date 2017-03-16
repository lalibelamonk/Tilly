using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Camera cam;
	private Vector3 initialVectorBL;
	private Vector3 initialVectorTR;

	public Tree tree;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
		tree = GameObject.FindObjectOfType<Tree>();
		RecenterCamera();

		initialVectorBL = cam.ScreenToWorldPoint(new Vector3(0, 0, 30));
        initialVectorTR = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 30));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newVectorBL = cam.ScreenToWorldPoint(new Vector3(0, 0, 30));
        Vector3 newVectorTR = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 30));
		if (newVectorBL != initialVectorBL || newVectorTR != initialVectorTR) {
			RecenterCamera();
		}
	}

	void RecenterCamera() {
		Vector3 cameraPosition = cam.transform.position;
		Vector3 offset = tree.transform.position - cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

		cam.transform.position = new Vector3(cameraPosition.x + offset.x + 1, cameraPosition.y, cameraPosition.z);
	}
}
