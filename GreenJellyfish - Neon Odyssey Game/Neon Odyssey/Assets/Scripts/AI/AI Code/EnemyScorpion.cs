﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------
//-written by: Samuel
//-contributors:
//---------------------------------------------------------

public class EnemyScorpion : Enemy
{
    //Gun
    public float m_gunFireDistance = 0.0f;
    public int m_gunNumberOfShots = 0;
    public float m_gunBulletSpeed = 0.0f;
    public float m_gunTimeBetweenShots = 0.0f;
    public float m_gunCooldown = 0.0f;

    public Vector3 m_leftClawPos = Vector3.zero;
    public Vector3 m_rightClawPos = Vector3.zero;

    public GameObject m_bulletPrefab = null;

    //Laser
    public float m_laserFireDistance = 0.0f;
    public float m_laserChargeTime = 0.0f;
    public float m_laserCooldown = 0.0f;

    public bool m_laserFollowing = true;

    public Vector3 m_laserPos = Vector3.zero;
    public GameObject m_laserPrefab = null;

    public bool m_laserFriendlyFire = false;

    //Nodes
    private BehaviourSequence m_sequenceTop;

    private BehaviourParallel m_firingParallel;

    private BehaviourSequence m_sequenceLeftGun;
    private BehaviourSequence m_sequenceRightGun;
    private BehaviourSequence m_sequenceLaser;

    private BehaviourBase m_actionGetTarget;

    //LeftClaw
    private FireGun m_actionFireLeftGun;

    //RightClaw
    private FireGun m_actionFireRightGun;

    //Both Claws
    private IsTargetCloseEnough m_actionGetDisGun;
    private CoolDown m_actionGunCooldown;

    //Laser
    private IsTargetCloseEnough m_actionGetDisLaser;
    private FireLaserbeam m_actionFireLaser;
    private CoolDown m_actionLaserCooldown;

    //-----------------------------------------------------
    // Setting up all AI behaviour components
    //
    // Fire left claw, fire right and laser at same time
    //-----------------------------------------------------
    void Start()
    {
        //Set health
        SetHealth(m_healthMax);

        m_sequenceTop = gameObject.AddComponent<BehaviourSequence>();

        m_firingParallel = gameObject.AddComponent<BehaviourParallel>();

        m_sequenceLeftGun = gameObject.AddComponent<BehaviourSequence>();
        m_sequenceRightGun = gameObject.AddComponent<BehaviourSequence>();
        m_sequenceLaser = gameObject.AddComponent<BehaviourSequence>();

        m_actionGetTarget = gameObject.AddComponent<BehaviourBase>();

        //LeftClaw
        m_actionFireLeftGun = gameObject.AddComponent<FireGun>();

        //RightClaw
        m_actionFireRightGun = gameObject.AddComponent<FireGun>();

        //Both Claws
        m_actionGetDisGun = gameObject.AddComponent<IsTargetCloseEnough>();
        m_actionGunCooldown = gameObject.AddComponent<CoolDown>();

        //Laser
        m_actionGetDisLaser = gameObject.AddComponent<IsTargetCloseEnough>();
        m_actionFireLaser = gameObject.AddComponent<FireLaserbeam>();
        m_actionLaserCooldown = gameObject.AddComponent<CoolDown>();

        //Set up get target
        if (GameObject.FindWithTag("GameController").GetComponent<GameManager>().m_singlePlayer)
            m_actionGetTarget = gameObject.AddComponent<GetTargetSinglePlayer>();
        else
        {
            switch(m_difficulty)
            {
                case Difficulty.Easy:
                    {
                        m_actionGetTarget = gameObject.AddComponent<GetTargetEasy>();
                        break;
                    }
                case Difficulty.Medium:
                    {
                        m_actionGetTarget = gameObject.AddComponent<GetTargetMedium>();
                        break;
                    }
                case Difficulty.Hard:
                    {
                        m_actionGetTarget = gameObject.AddComponent<GetTargetHard>();
                        break;
                    }
            }
        }

        //Left gun
        m_actionGetDisGun.m_targetDistance = m_gunFireDistance;
        m_actionFireLeftGun.m_numberOfBullets = m_gunNumberOfShots;
        m_actionFireLeftGun.m_timeBetweenShots = m_gunTimeBetweenShots;
        m_actionFireLeftGun.m_bulletSpeed = m_gunBulletSpeed;
        m_actionFireLeftGun.m_bulletSpawnPos = m_leftClawPos;
        m_actionFireLeftGun.m_bullet = m_bulletPrefab;
        m_actionGunCooldown.m_coolDown = m_gunCooldown;


        //Right gun
        m_actionGetDisGun.m_targetDistance = m_gunFireDistance;
        m_actionFireRightGun.m_numberOfBullets = m_gunNumberOfShots;
        m_actionFireRightGun.m_timeBetweenShots = m_gunTimeBetweenShots;
        m_actionFireRightGun.m_bulletSpeed = m_gunBulletSpeed;
        m_actionFireRightGun.m_bulletSpawnPos = m_rightClawPos;
        m_actionFireRightGun.m_bullet = m_bulletPrefab;
        m_actionGunCooldown.m_coolDown = m_gunCooldown;

        //Laser
        m_actionGetDisLaser.m_targetDistance = m_laserFireDistance;
        m_actionFireLaser.m_chargeRate = m_laserChargeTime;
        m_actionFireLaser.m_laserSpawnPos = m_laserPos;
        m_actionFireLaser.m_laserFriendlyFire = m_laserFriendlyFire;
        m_actionFireLaser.m_laserFollowing = m_laserFollowing;
        m_actionFireLaser.m_laserbeam = m_laserPrefab;

        m_actionLaserCooldown.m_coolDown = m_laserCooldown;

        //Set up branches
        m_sequenceTop.m_behaviourBranches.Add(m_actionGetTarget as BehaviourBase);
        m_sequenceTop.m_behaviourBranches.Add(m_sequenceLeftGun);
        m_sequenceTop.m_behaviourBranches.Add(m_firingParallel);

        m_firingParallel.m_behaviourBranches.Add(m_sequenceLaser);
        m_firingParallel.m_behaviourBranches.Add(m_sequenceRightGun);

        m_sequenceLeftGun.m_behaviourBranches.Add(m_actionGetDisGun);
        m_sequenceLeftGun.m_behaviourBranches.Add(m_actionFireLeftGun);
        m_sequenceLeftGun.m_behaviourBranches.Add(m_actionGunCooldown);

        m_sequenceRightGun.m_behaviourBranches.Add(m_actionGetDisGun);
        m_sequenceRightGun.m_behaviourBranches.Add(m_actionFireRightGun);
        m_sequenceRightGun.m_behaviourBranches.Add(m_actionGunCooldown);

        m_sequenceLaser.m_behaviourBranches.Add(m_actionGetDisLaser);
        m_sequenceLaser.m_behaviourBranches.Add(m_actionFireLaser);
        m_sequenceLaser.m_behaviourBranches.Add(m_actionLaserCooldown);

        m_initalBehaviour = m_sequenceTop;
    }
}
