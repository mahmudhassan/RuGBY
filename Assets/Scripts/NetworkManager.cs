﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string typeName = "UniqueGameName";
	private const string gameName = "RoomName";
	
	private bool isRefreshingHostList = false;
	private HostData[] hostList;
	
	public GameObject playerPrefab;
	public GameObject cameraPrefab;
	
	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}
	
	private void StartServer()
	{
		Network.InitializeServer(5, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized()
	{
		SpawnPlayer();
	}
	
	void Update()
	{
		if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
		{
			isRefreshingHostList = false;
			hostList = MasterServer.PollHostList();
		}
	}
	
	private void RefreshHostList()
	{
		if (!isRefreshingHostList)
		{
			isRefreshingHostList = true;
			MasterServer.RequestHostList(typeName);
		}
	}
	
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}
	
	private void SpawnPlayer()
	{
		GameObject temp = (GameObject) Network.Instantiate(playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
		//Network.Instantiate(cameraPrefab, Vector3.up * 5, Quaternion.identity, 0);
		var original = GameObject.FindWithTag("MainCamera");
		Camera _cam = (Camera) Camera.Instantiate(original.camera, new Vector3(0, 0, 0), 
										Quaternion.FromToRotation(new Vector3(0, 0, 0), new Vector3(0, 0, 1)));
		DestroyImmediate(Camera.main.gameObject);


		GameObject.FindWithTag ("MainCamera").GetComponent<SmoothLookAt> ().target = temp.GetComponentInChildren<Transform>().Find("Head_Target");
		GameObject.FindWithTag("MainCamera").GetComponent<MouseOrbitImproved>().target = temp.GetComponentInChildren<Transform>().Find("Head_Target");; 
		GameObject.FindWithTag("MainCamera").GetComponent<SmoothFollow>().target = temp.GetComponentInChildren<Transform>().Find("Head_Target"); 
	}
}
