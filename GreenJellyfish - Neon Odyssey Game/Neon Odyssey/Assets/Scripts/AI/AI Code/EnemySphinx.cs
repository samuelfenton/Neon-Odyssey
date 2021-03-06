﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------
//-written by: Samuel
//-contributors:
//---------------------------------------------------------

public class EnemySphinx : Enemy
{

    //Points used to jump to
    public List<GameObject> m_jumpPoints;

    public float m_jumpTime = 1.0f;
    public float m_jumpHeight = 1.0f;

    public float m_landingTimer = 1.0f;

    //Laser
    public float m_laserFireDistance = 0.0f;
    public float m_laserChargeTime = 0.0f;
    public float m_laserCooldown = 0.0f;

    public bool m_laserFollowing = true;

    public Vector3 m_laserPos = Vector3.zero;
    public GameObject m_laserPrefab = null;

    public bool m_laserFriendlyFire = false;

    //Nodes
    private BehaviourSelector m_selectorTop;
    private BehaviourSequence m_gettingTarget;

    private BehaviourSelector m_selectorAction;
    private BehaviourBase m_actionGetTarget;

    private BehaviourSequence m_sequenceFiring;
    private BehaviourSequence m_sequenceLaser;

    //Laser
    private IsTargetCloseEnough m_actionGetDisLaser;
    private FireLaserbeam m_actionFireLaser;
    private CoolDown m_actionLaserCooldown;

    private JumpToPlatform m_jumpToPlatform;
    private CoolDown m_landingCooldown;

    //-----------------------------------------------------
    // Setting up all AI behaviour components
    //
    // Jump across platforms, fire laser at player, jump to new platform
    //-----------------------------------------------------
    void Start()
    {
        //Set health
        SetHealth(m_healthMax);

        m_selectorTop = gameObject.AddComponent<BehaviourSelector>();

        m_gettingTarget = gameObject.AddComponent<BehaviourSequence>();
        m_selectorAction = gameObject.AddComponent<BehaviourSelector>();
        m_sequenceFiring = gameObject.AddComponent<BehaviourSequence>();
        m_sequenceLaser = gameObject.AddComponent<BehaviourSequence>();

        //Laser
        m_actionGetDisLaser = gameObject.AddComponent<IsTargetCloseEnough>();
        m_actionFireLaser = gameObject.AddComponent<FireLaserbeam>();
        m_actionLaserCooldown = gameObject.AddComponent<CoolDown>();

        m_jumpToPlatform = gameObject.AddComponent<JumpToPlatform>();
        m_landingCooldown = gameObject.AddComponent<CoolDown>();

        //Set up get target
        if (GameObject.FindWithTag("GameController").GetComponent<GameManager>().m_singlePlayer)
            m_actionGetTarget = gameObject.AddComponent<GetTargetSinglePlayer>();
        else
        {
            switch (m_difficulty)
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

        m_selectorTop.m_behaviourBranches.Add(m_gettingTarget);
        m_selectorTop.m_behaviourBranches.Add(m_jumpToPlatform);

        m_gettingTarget.m_behaviourBranches.Add(m_actionGetTarget as BehaviourBase);
        m_gettingTarget.m_behaviourBranches.Add(m_selectorAction);
  
        m_selectorAction.m_behaviourBranches.Add(m_sequenceFiring);

        m_sequenceFiring.m_behaviourBranches.Add(m_sequenceLaser);
        m_sequenceFiring.m_behaviourBranches.Add(m_jumpToPlatform);
        m_sequenceFiring.m_behaviourBranches.Add(m_landingCooldown);

        m_sequenceLaser.m_behaviourBranches.Add(m_actionGetDisLaser);
        m_sequenceLaser.m_behaviourBranches.Add(m_actionFireLaser);
        m_sequenceLaser.m_behaviourBranches.Add(m_actionLaserCooldown);
        m_sequenceLaser.m_behaviourBranches.Add(m_jumpToPlatform);

        //Laser
        m_actionGetDisLaser.m_targetDistance = m_laserFireDistance;
        m_actionFireLaser.m_chargeRate = m_laserChargeTime;
        m_actionFireLaser.m_laserSpawnPos = m_laserPos;
        m_actionFireLaser.m_laserFriendlyFire = m_laserFriendlyFire;
        m_actionFireLaser.m_laserFollowing = m_laserFollowing;

        m_actionLaserCooldown.m_coolDown = m_laserCooldown;

        m_actionFireLaser.m_laserbeam = m_laserPrefab;

        //Jumping
        m_jumpToPlatform.m_jumpHeight = m_jumpHeight;
        m_jumpToPlatform.m_jumpTime = m_jumpTime;

        m_landingCooldown.m_coolDown = m_landingTimer;

        m_initalBehaviour = m_selectorTop;
    }
}
