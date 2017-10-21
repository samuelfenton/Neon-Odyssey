﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGun : BehaviourBase
{

    public GameObject m_bullet;

    public int m_numberOfBullets = 0;
    private int m_bulletCount = 0;
    public float m_bulletSpeed = 0.0f;

    public float m_timeBetweenShots = 0.0f;
    private float m_time = 0.0f;

    private bool m_behaviourSetup = true;

    public Vector3 m_bulletSpawnPos = Vector3.up * 0.5f;

    //--------------------------------------------------------------------------------------
    // Update behaviours - Fire Towards target
    //
    // Return:
    //		Returns a enum BehaviourStatus, current status of behaviour, Success, failed, pending
    //--------------------------------------------------------------------------------------
    public override BehaviourBase.BehaviourStatus Execute()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f, -9.8f, 0.0f);
        if (m_behaviourSetup)
        {
            //Start numbers of bullets to fire
            m_bulletCount = 0;
            m_time = 0.0f;
            m_behaviourSetup = false;
        }
	
        m_time -= Time.deltaTime;

        if (m_time < 0.0f)
        {
            m_time = m_timeBetweenShots;
            m_bulletCount++;

            //Fire bullet
            Vector3 bulletDir = (GetComponent<Enemy>().m_target.transform.position - transform.position).normalized;
            FireBullet(bulletDir);
        }

        if (m_bulletCount < m_numberOfBullets)
            return BehaviourStatus.PENDING;

        //Reset all varibles
        m_behaviourSetup = true;
        return BehaviourStatus.SUCCESS;
    }

    void FireBullet(Vector3 bulletDir)
    {
        GameObject newBullet = Instantiate(m_bullet, this.transform.position + new Vector3(transform.up.x * m_bulletSpawnPos.x, transform.up.y * m_bulletSpawnPos.y, transform.up.z * m_bulletSpawnPos.z), Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = bulletDir * m_bulletSpeed;
        newBullet.GetComponent<Bullet>().SetTeam(Bullet.TEAM.ENEMY);
    }
}
