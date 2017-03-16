using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellTracker : MonoBehaviour {

	bool tracking;
	Vector2 lastPos;
	List<Vector2> positions;
	ParticleSystem particles;

	void Awake () {
		particles = GetComponent<ParticleSystem>();
	}

	// Use this for initialization
	void Start () {
		tracking = false;
		lastPos = new Vector2(0, 0);
		positions = new List<Vector2>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(1)) {
			particles.Play();
			Vector2 newPos = Camera.main.ViewportToScreenPoint
			this.gameObject.transform.position = newPos;
			if (Vector2.Distance(lastPos, newPos) > 2) {
				positions.Add(newPos);
				lastPos = newPos;
				Debug.Log(positions.ToString());
			}
		}

		if (Input.GetMouseButtonUp(1)) {
			particles.Stop();
			Debug.Log("Done TRACKING");
			Debug.Log(positions.ToString());
			positions.Clear();
		}
	}
}
