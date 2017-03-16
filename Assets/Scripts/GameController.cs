using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : Persistent {

	public float levelLength { get; set; }
	public float level { get; set; }
	public float timeLeft { get; set; }
	public bool runTimer { get; set; }
	public LevelManager levelManager { get; set; }
	public int enemiesSlain { get; set; }
	public Tree tree { get; set; }
	public int schmeckles { get; set; }
	public int zIndex { get; set; }

	public AudioSource audioSource;
	private IEnumerator fade;
	public bool isPaused { get; set; }
	public bool debug;
	public List<Enemy> allEnemies { get; set; }

	void Awake () {
		base.Awake();

		//USED FOR TESTING REMOVE LATER
		if (GameObject.FindObjectOfType<Tree>()) {
			this.tree = GameObject.FindObjectOfType<Tree>();
		}

		// Ignore collisions between zombies
		int layer = LayerMask.NameToLayer("Enemy");
		Debug.Log("LAYER " + layer);
		Physics2D.IgnoreLayerCollision(layer, layer, true);

		DontDestroyOnLoad(gameObject);
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0;
		audioSource.Play();
		fade = FadeIn(audioSource);
		StartCoroutine (FadeIn(audioSource));
		allEnemies = new List<Enemy>();
		zIndex = 20;
		debug = false;
	}

	// Use this for initialization
	void Start () {
		InitializeGameVariables();
	}
	
	// Update is called once per frame
	void Update () {
		if (audioSource.volume >= 1) {
			StopCoroutine(fade);
		}
		if (runTimer && !isPaused) {
			timeLeft -= Time.deltaTime;
		}
	}

	public void InitializeGameVariables() {
		enemiesSlain = 0;
		allEnemies.Clear();
		levelLength = 20f;
		timeLeft = levelLength;
		level = 1f;
		runTimer = false;
		schmeckles = 0;
		if (tree != null) {
			tree.Reset();
		}
	}

	public void ZombieKilled(Enemy enemy) {
		 enemiesSlain += 1;
		 allEnemies.Remove(enemy);
	}

	public void IncreaseDifficulty() {
		level += 1;
		levelLength += 2;
		timeLeft = levelLength;
		runTimer = false;
		allEnemies.Clear();
		zIndex = 20;
	}

	public void GoToScene(string level) {
		SceneManager.LoadScene(level);
	}

	public void StartTimer(){
		runTimer = true;
	}

	public void TogglePause(bool paused) {
		isPaused = paused;
		runTimer = !paused;
		if (paused) {
			foreach (Enemy enemy in allEnemies) {
				enemy.PauseEnemy();
			}
		}
		else {
			foreach (Enemy enemy in allEnemies) {
				enemy.UnPauseEnemy();
			}
		}
	}

	IEnumerator FadeIn(AudioSource source) {
		while (source.volume < 0.65f) {
			source.volume += 0.02f;
			yield return new WaitForSeconds(.05f);
		}
	}

}
