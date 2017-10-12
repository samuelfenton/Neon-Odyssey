﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkBullet : MonoBehaviour {

    public float BulletTimer = 2.0f;
    // Use this for initialization
    void Start () {
   
	}
	
	// Update is called once per frame
	void Update()
    {
        Destroy(gameObject, BulletTimer);
        if (gameObject.GetComponentInChildren<Renderer>().isVisible == false)
        {
            Destroy(gameObject);
        }
    }

    //function called when object collides
    void OnTriggerEnter(Collider col)
    {
        //if BULLET is same colour as ENEMY, destroy ENEMY and BULLET
        if (col.gameObject.tag == "Enemy" && col.gameObject.layer == 10)
        {
            col.GetComponent<Enemy>().PlayDeath();
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

        //if BULLET collides with other coloured ENEMY, destroy bullet
        if (col.gameObject.tag == "Enemy" && (col.gameObject.layer == 9 ||  //purple 
                                              col.gameObject.layer == 11 || //orange
                                              col.gameObject.layer == 12))  //green
        {
            Destroy(gameObject);
        }

        //if BULLET collides with WALLS, destroy bullet
        if (col.gameObject.layer == 8 && col.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }

    }

}
