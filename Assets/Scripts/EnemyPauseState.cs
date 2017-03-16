using System;
using UnityEngine;

public class EnemyPauseState : System.Object
{
    public string state;
    public Vector3 velocity { get; set; }

    public EnemyPauseState (string state, Vector3 velocity) {
    	this.state = state;
    	this.velocity = velocity;	
    }
}
