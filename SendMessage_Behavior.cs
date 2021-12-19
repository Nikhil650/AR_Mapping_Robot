﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class SendMessage_Behavior : MonoBehaviour {

	private static SendMessage_Behavior instance;

	public static SendMessage_Behavior Instance {
		get { return instance; }
	}

	[Header ("192.168.1.6")]
	public string serverIP;

	private const int PORT_NUM = 1998;
	private IPAddress serverAddr;
	private IPEndPoint endPoint;
	private Socket sock;
	private byte [] send_buffer;

	private void Awake () {
		instance = this;
	}

	void Start () {
		InitSocket ();
	}

	void OnDisable () {
		CloseSocket ();
	}

	void InitSocket () {
		//close socket in case its already open
		CloseSocket ();
		//init socket
		sock = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		serverAddr = IPAddress.Parse (serverIP);
		print (serverAddr);
		endPoint = new IPEndPoint (serverAddr, PORT_NUM);
	}

	void CloseSocket () {
		if (sock != null) {
			StopAllCoroutines ();
			sock.Disconnect (true);
		}
	}

	public void SendPacket (string message) {
		try {
			send_buffer = Encoding.ASCII.GetBytes (message);
			sock.SendTo (send_buffer, endPoint);
		} catch (SocketException s) {
			Debug.Log (s);
		}
		Debug.Log (message);
	}
}
