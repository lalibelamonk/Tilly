using UnityEngine;
using System.Collections;

public class BasicZombie : Enemy {

	// Use this for initialization
	public override void Start () {
		base.Start();
		base.state = State.Falling;
		attackSpeed = 1;
		attackDamage = 2;
		hp = 50;
	}
}
