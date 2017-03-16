using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public BasicZombie basicZombie;
	public ArmoredZombie armoredZombie;
	public Tree tree;
	public Vector3 spawnLocation;
	private GameController gc;
	private float spawnRate;

	private int startingSlain;
	private int startingLifeTotal;

	// Use this for initialization
	void Start () {
		gc = GameObject.FindObjectOfType<GameController>();
		//USED FOR TESTING REMOVE LATER
		if (gc == null) {
			SceneManager.LoadScene("start_screen");
			return;
		}
		gc.levelManager = this;

		startingSlain = gc.enemiesSlain;
		startingLifeTotal = gc.tree.GetHealth();

		// set level specific spawn values
		spawnRate = 4f * Mathf.Pow(.8f, gc.level);
		gc.StartTimer();
		spawnLocation = new Vector3(-40, 0, 0);
		StartCoroutine (StartSpanws ());
		if (gc.tree != null) {
			tree = gc.tree;
		}
	}

	// Update is called once per frame
	void Update () {
		if (tree.GetHealth() <= 0) {
			gc.runTimer = false;
		}
		if (gc.timeLeft <= 0) {
			gc.IncreaseDifficulty();
			gc.GoToScene("transition_level");
		}
	}

	float SpawnTimer() {
		float sign = Random.value < .5f ? -1.0f : 1.0f;
		float timer = spawnRate + (spawnRate / 4 * sign);
		if (timer < 0) {
			timer = 0;
		}
		return timer;
	}

	float SetSpanwRate(float level) {
		float spawnRate = 2f * Mathf.Pow(.8f, level);
		return Mathf.Max(spawnRate, .33f);
	}

	Enemy SelectEnemy() {
		float choice = Random.value * gc.level;
		if (choice > .5) {
			//return (ArmoredZombie)Instantiate (armoredZombie, new Vector3(-16f, -1.72f, 0f), Quaternion.identity);
		}
		return (BasicZombie)Instantiate (basicZombie, spawnLocation, Quaternion.identity);
	}

	public void Restart() {
		gc.enemiesSlain = startingSlain;
		gc.tree.RepairHealth(startingLifeTotal - gc.tree.GetHealth()); //super hacky but its late and i dont care. TODO
		gc.timeLeft = gc.levelLength;
		gc.allEnemies.Clear();
		gc.GoToScene("default_level");
	}

	public void Quit() {
		gc.InitializeGameVariables();
		gc.GoToScene("start_screen");
	}

	IEnumerator StartSpanws () {
		while (true) {
			if (!gc.runTimer) {
				// Better way to pause spawning?
				// Issues...pause right after spawn? say hello to another spawn...
				yield return new WaitForSeconds(.05f);
			} else  {
				Enemy newEnemy = SelectEnemy();
				newEnemy.SetTree(tree);
				newEnemy.SetGameController(gc);
				gc.allEnemies.Add(newEnemy);
				gc.zIndex = gc.zIndex + 10;
				yield return new WaitForSeconds (SpawnTimer());
			}
		}
	}
}
