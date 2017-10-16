﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public bool m_singlePlayer = false;

    public GameObject m_player1 = null;

    public GameObject m_player2 = null;

    public Vector3 offset = new Vector3(0, 1.0f, -10);

    public float m_zoomMin = 10.0f;
    public float m_zoomSpeed = 0.02f;

    public float m_maxHorizontalDistance = 20.0f;
    public float m_maxVerticalDistance = 14.0f;

    // Use this for initialization
    void Awake ()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length == 1)
        {
            m_player1 = players[0];
        }
        else if(players.Length == 2)
        {
 
            m_player1 = players[0];

            m_player2 = players[1];
        }

        if (m_player2 == null)
            m_singlePlayer = true;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Single player
        if (m_singlePlayer)
            transform.position = m_player1.transform.position + offset;
        //Multiplayer
        else
            transform.position = (m_player1.transform.position + m_player2.transform.position) / 2 + offset;
    }
}
