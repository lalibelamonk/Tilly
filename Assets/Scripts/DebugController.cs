using UnityEngine;
using UnityEngine.UI;

public class DebugController {

	public GameObject CreateDebugPanel (GameObject parent) {
		GameObject panel = new GameObject("Debug Panel");
		panel.AddComponent<MeshRenderer>();
		panel.AddComponent<TextMesh>();
		panel.transform.SetParent(parent.transform);
		panel.transform.localPosition = new Vector3(0, 3, 0);
		panel.GetComponent<MeshRenderer>().sortingLayerName = "Enemies";

		return panel;
	}

	public void UpdatePanel (Transform panel, string text) {
		panel.gameObject.GetComponent<TextMesh>().text = text;
	}
}

