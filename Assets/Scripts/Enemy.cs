using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	protected static Enemy theGrabbedOne; // instance of the currently grabbed Enemy object
	protected Rigidbody2D body;
	protected Vector2 velocity;
	protected Vector2 lastPosition;
	protected Vector2 throwVelocity;
	protected int hp;
	protected enum State { Grabbed, Attacking, Walking, Falling, Dying, Stumbling, Paused };
	protected State state;
	protected AudioSource audioSource;
	protected Tree tree;
	protected GameController gc;
	protected int attackDamage;
	protected float attackSpeed;
	private EnemyPauseState pauseState;
	private Hashtable debugInfo;
	private DebugController debug;
	private bool beenThrown; // Limits bounds checks to after an enemy has been interacted with

	public AudioClip[] deathSounds;
	public AudioClip[] hurtSounds;

	// Use this for initialization
	public virtual void Start () {
		body = GetComponent<Rigidbody2D>();
		lastPosition = body.position;
		audioSource = GetComponent<AudioSource>();
		debugInfo = new Hashtable();
		debugInfo.Add("HP", 0);
		debug = new DebugController();
		beenThrown = false;
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
			case State.Grabbed:
				Vector3 position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				position.z = 0;
				body.position = position;
				throwVelocity = (body.position - lastPosition) / Time.deltaTime;
				lastPosition = body.position;
				break;
			case State.Walking:
				forceVelocity ();
				break;
			case State.Paused:
				break;
			case State.Falling:
				velocity = body.velocity;
				float cameraLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
				float cameraRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1)).x;
				if (body.position.x < cameraLeft && beenThrown) {
					body.position = new Vector2(cameraLeft, body.position.y);
					body.velocity = new Vector3(body.velocity.x * -1f * .5f, body.velocity.y); 
				} else if (body.position.x > cameraRight) {
					body.position =  new Vector2(cameraRight, body.position.y);
					body.velocity = new Vector3(body.velocity.x * -1f * .5f, body.velocity.y); //duplicate line
				}
				break;
		}
		if (gc.debug) {
			UpdateDebug();
		} else {
			
		}
	}

	public void UpdateDebug() {
		if (gameObject.transform.FindChild("Debug Panel") == null) {
			debug.CreateDebugPanel(gameObject);
		}
		debug.UpdatePanel(gameObject.transform.FindChild("Debug Panel"), "Health " + this.hp.ToString() );
	}

	public virtual void SortRendering() {
		
	}

	public void SetTree(Tree tree) {
		this.tree = tree;
	}

	public void SetGameController(GameController controller) {
		gc = controller;
	}	

	void OnCollisionEnter2D(Collision2D collision) {
		if (state == State.Grabbed) {
			return;
		}

		GameObject obj = collision.gameObject;
		if (state == State.Falling && obj.tag.Equals ("Ground")) {
			TakeDamage("ground", collision.relativeVelocity);
			state = State.Walking;
		}
		if (obj.tag.Equals ("Tree")) {
			//Debug.Log(this.GetInstanceID() + " collided with tree: ");
			if (state != State.Attacking) {
				InvokeRepeating ("AttackTreehouse", 0, attackSpeed);
			}
			state = State.Attacking;
		}
	}

	void forceVelocity(){
		body.velocity = new Vector2(5f, 0f);
	}

	public void PauseEnemy() {
		if (state == State.Grabbed) {
			state = State.Falling;
			theGrabbedOne = null;
		}
		Debug.Log(state.ToString());
		pauseState = new EnemyPauseState(state.ToString(), body.velocity);
		Debug.Log(pauseState);
		body.velocity = Vector3.zero;
		body.isKinematic = true;
		state = State.Paused;
	}

	public void UnPauseEnemy() {
		body.velocity = pauseState.velocity;
		state = FindStateFromString(pauseState.state);
		body.isKinematic = false;
	}

	void OnMouseDown () {
		if (theGrabbedOne == null) {
			theGrabbedOne = this;
			beenThrown = true;
			state = State.Grabbed;
			body.isKinematic = true;
			body.velocity = new Vector3(0, 0, 0);
			CancelInvoke();
		}
	}

	void OnMouseUp () {
		if (theGrabbedOne == this) {
			theGrabbedOne = null;
			state = State.Falling;
			body.isKinematic = false;
			body.velocity = clampVelocity(throwVelocity);
		}
	}

	void TakeDamage (string type, Vector3 relativeVelocity) {
		if (type == "ground") {
			hp = hp - System.Math.Abs((int) relativeVelocity.y);
			if (hp <= 0) {
				gc.ZombieKilled(this);
				transform.position = Vector3.one * 99999; // Move the sprite out of the way until death sound completes and it is destroyed
				Destroy(gameObject, PlayRandomSound(deathSounds));
			} else {
				PlayRandomSound(hurtSounds);
			}
		}
	}

	State FindStateFromString (string stateString) {
		foreach (State enemyState in System.Enum.GetValues(typeof(State))) {
			if (enemyState.ToString() == stateString) {
				Debug.Log(stateString + " has been selected as the returning state");
				return enemyState;
			}
		}
		return State.Falling;
	}

	float PlayRandomSound(AudioClip[] array) {
		int index = (int)Mathf.Min(Mathf.Floor(Random.value * (float)array.Length), array.Length - 1);
		audioSource.PlayOneShot(array[index], 1f);
		return array[index].length;
	}

	Vector3 clampVelocity (Vector3 velocity) {
		float maxSpeed = 50;
		Vector3 normalized = velocity.normalized;
		if (velocity.magnitude > maxSpeed) {
			return normalized * maxSpeed;
		} else {
			return velocity;
		}
	}

	void AttackTreehouse () {
		//audioSource.PlayOneShot(audioSource.clip, 1f);
		audioSource.Play();
		tree.TakeDamage(attackDamage);
	}
}

