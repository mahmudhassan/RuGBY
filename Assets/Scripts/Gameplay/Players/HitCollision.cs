﻿using UnityEngine;
using System.Collections;

public class HitCollision : MonoBehaviour 
{
	private PlayerAnimator playeranimator;
	private Animator animator;
	private HashIDs hash;
	private PlayerHealth playerHealth;


	void Start() 
	{
		if (networkView.isMine)
		{
			playeranimator = GetComponent<PlayerAnimator>();
			playerHealth = GetComponent<PlayerHealth>();

		}
	}

	void Update() 
	{
		if (networkView.isMine)
		{
			if (playerHealth.currentHealth <= 0)
			{
				playeranimator.Die();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("I have collided!");
		Debug.Log ("networkView: "+networkView);
		//if (this.networkView.isMine)
		{
			Debug.Log ("this: " + this);
			//Debug.Log ("NetworkPlayer : "+Network.player.ToString());
			//Debug.Log (other.collider.transform.parent.gameObject);
			//Debug.Log ("OtherNetworkPlayer : "+other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner);
			//string player = other.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.gameObject.networkView.owner.ToString();
			//if(!player.Equals(Network.player.ToString()))
			//{
				playerHealth.TakeDamage(15);
				playeranimator.Hurt();
			//}

		}
	}

	public void OnEventAttackBegin()
	{
		if (networkView.isMine)
		{
			GameObject.FindGameObjectWithTag("Weapon").GetComponent<BoxCollider>().enabled = true;
			Debug.Log ("Attack enabled");
		}
	}

	public void OnEventAttackEnd()
	{
		if (networkView.isMine)
		{
			GameObject.FindGameObjectWithTag("Weapon").GetComponent<BoxCollider>().enabled = false;
			Debug.Log ("Attack disabled");
		}
	}

	public void HealthTest()
	{
		Debug.Log ("why are you hitting yourself?");
		playerHealth.TakeDamage(15);
	}
}
