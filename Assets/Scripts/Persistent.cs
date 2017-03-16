using UnityEngine;
using System;

public class Persistent : MonoBehaviour {

	public static GameObject gc;
	public static GameObject tree;

	public virtual void Awake () {
		switch (this.GetType().ToString()) {
			case "GameController":
				CheckInstance(gc, this.gameObject);
				break;
			case "Tree":
				CheckInstance(tree, this.gameObject);
				Debug.Log("tree checked");
				break;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	private void CheckInstance(GameObject obj, GameObject newObj) {
		if (obj != null) {
			Destroy(newObj);
		} else {
			obj = newObj;
		}
	}
}

