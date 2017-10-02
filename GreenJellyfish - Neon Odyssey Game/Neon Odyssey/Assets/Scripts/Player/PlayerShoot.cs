﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerShoot : MonoBehaviour {

    public bool isFiring;

    public float bulletSpeed;
    public float timeBetweenShots = 0;
    public float fireRate = 2;

    private float shotCounter;


    public Player player;

    public Vector3 Aim;

    P1ColourController pcc1;
    P2ColourController pcc2;

    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;

    


    // Use this for initialization
    void Start ()
    {
        pcc1 = GetComponent<P1ColourController>();
        pcc2 = GetComponent<P2ColourController>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 rightInput = new Vector2(XCI.GetAxisRaw(XboxAxis.RightStickX), XCI.GetAxisRaw(XboxAxis.RightStickY));

        
        if (rightInput.x != 0 || rightInput.y != 0)
        {
            isFiring = true;
            Aim.x = rightInput.x;
            Aim.y = rightInput.y;
            Aim.z = 0;
            Aim.Normalize();

            
            Vector3 up = new Vector3(0, 0.9f);

            Debug.DrawRay(this.transform.position + up, Aim);

            timeBetweenShots += Time.deltaTime;

            if (isFiring)
            {
                if (pcc1.switchColour == 1)
                {
                    if (timeBetweenShots >= fireRate)
                    {
                        GameObject newBullet = Instantiate(bullet1, Aim + player.transform.position + up, Quaternion.Euler(Aim)) as GameObject;
                        newBullet.GetComponent<Rigidbody>().AddForce(Aim * bulletSpeed);

                        timeBetweenShots = 0;

                    }
                    
                }
                if (pcc1.switchColour == 2)
                {
                    if (timeBetweenShots >= fireRate)
                    {
                        
                        GameObject newBullet = Instantiate(bullet2, Aim + player.transform.position + up, Quaternion.Euler(Aim)) as GameObject;
                        
                        newBullet.GetComponent<Rigidbody>().AddForce(Aim * bulletSpeed);
                        timeBetweenShots = 0;
                    }
               
                }
            }


        }
        if (rightInput.x == 0 || rightInput.y == 0)
        {
            isFiring = false;
        }









        }
}
