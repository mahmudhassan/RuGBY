﻿using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
	public int maxHealth = 100;
	public int curHealth = 100;
	public static HealthBar Instance;


	public float healthBarLength;
	// Use this for initialization
	void Start () 
	{
		healthBarLength = Screen.width / 2;
		if (networkView.isMine)
		{
			Instance = this;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		AdjustCurrentHealth(0);
	}

	void OnGUI()
	{
		if (networkView.isMine)
		{
			GUI.Box(new Rect(10, 10,healthBarLength, 20), curHealth + "/" + maxHealth);
		}
	}

	public void AdjustCurrentHealth(int adj) 
	{
		if (networkView.isMine){
			curHealth += adj;

			if (curHealth < 0)
				curHealth = 0;
			if (curHealth > maxHealth)
				curHealth = maxHealth;
			if (maxHealth < 1)
				maxHealth = 1;

			healthBarLength = (Screen.width / 2) * (curHealth / (float)maxHealth);
		}
	}

	public void ResetHealth() {
		if (networkView.isMine){
			curHealth = maxHealth;
			healthBarLength = (Screen.width / 2) * (curHealth / (float)maxHealth);
		}
	}
}
