﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public GameObject[] SpawnPoints = new GameObject[4];

	private int lastSpawnPoint;
	private int beforeLastSpawnPoint;

	private int spawn;

	private NetworkManager network;

	void Awake()
	{
		network = GetComponent<NetworkManager>();
	}

	public void Respawn()
	{
		while (spawn == lastSpawnPoint || spawn == beforeLastSpawnPoint)
			spawn = Random.Range(0, 4);
		beforeLastSpawnPoint = lastSpawnPoint;
		lastSpawnPoint = spawn;
		network.SpawnPlayer(SpawnPoints[spawn].transform.position);

	}
}
