﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerShoot : MonoBehaviour
{

    public float m_bulletSpeed = 1000;
    private float m_timeBetweenShots = 0;
    public float m_fireRate = 0.2f;

    public Vector3 m_aim;

    public GameObject m_bullet1;
    public GameObject m_bullet2;

    public XboxController controller;

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Player>().isDead)
        {
            Vector2 rightInput = new Vector2(XCI.GetAxisRaw(XboxAxis.RightStickX, controller), XCI.GetAxisRaw(XboxAxis.RightStickY, controller));

            if (rightInput.x != 0 || rightInput.y != 0)
            {
                m_aim.x = rightInput.x;
                m_aim.y = rightInput.y;
                m_aim.z = 0;
                m_aim.Normalize();

                Vector3 up = new Vector3(0, 0.9f);

                Debug.DrawRay(this.transform.position + up, m_aim);

                m_timeBetweenShots += Time.deltaTime;

                if (GetComponent<ColourController>().m_firstBulletSlot && m_timeBetweenShots >= m_fireRate)
                {
                    GameObject newBullet = Instantiate(m_bullet1, m_aim + transform.position + up, Quaternion.Euler(m_aim)) as GameObject;
                
                    newBullet.GetComponent<Rigidbody>().AddForce(m_aim * m_bulletSpeed);
                    newBullet.GetComponent<Bullet>().SetTeam(Bullet.TEAM.PLAYER);
                    m_timeBetweenShots = 0;
                }
                if (!GetComponent<ColourController>().m_firstBulletSlot && m_timeBetweenShots >= m_fireRate)
                {
                    GameObject newBullet = Instantiate(m_bullet2, m_aim + transform.position + up, Quaternion.Euler(m_aim)) as GameObject;

                    newBullet.GetComponent<Rigidbody>().AddForce(m_aim * m_bulletSpeed);
                    newBullet.GetComponent<Bullet>().SetTeam(Bullet.TEAM.PLAYER);
                    m_timeBetweenShots = 0;
                }
            }
        }
    }
}
