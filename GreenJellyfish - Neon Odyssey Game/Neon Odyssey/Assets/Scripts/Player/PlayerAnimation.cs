﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerAnimation : MonoBehaviour
{

    //Effects
    //Setting up effects
    private bool m_inAir = true;
    private bool m_hitWall = true;

    //Store current effect
    private GameObject m_currentLandingEffect = null;
    private GameObject m_currentHitWallEffect = null;

    //store possible effects for wach player
    public List<GameObject> m_landingEffect;
    public List<GameObject> m_hitWallEffect;

    //Effect offset
    public Vector3 m_landingEffectSpawnPos = Vector3.up * 0.1f;
    public Vector3 m_hitWallEffectSpawnPos = Vector3.up * 0.1f;

    public List<GameObject> m_jumpingEffect;
    public Vector3 m_jumpingEffectSpawnPos = Vector3.up * 0.1f;

    Animator m_animator = null;
    PlayerController m_playerController = null;
    Player m_player = null;

    // Use this for initialization
    void Start ()
    {
        m_animator = gameObject.GetComponentInChildren<Animator>();

        m_playerController = GetComponent<PlayerController>();
        m_player = GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Landing animation
        if (m_inAir)
        {
            if (m_playerController.m_CollisionInfo.bottom) // Hit ground
            {
                if (GetComponent<ColourController>().m_firstBulletSlot)
                    m_currentLandingEffect = Instantiate(m_landingEffect[0], transform.TransformPoint(m_landingEffectSpawnPos), Quaternion.identity, transform);
                else
                    m_currentLandingEffect = Instantiate(m_landingEffect[1], transform.TransformPoint(m_landingEffectSpawnPos), Quaternion.identity, transform);

                StartCoroutine(PauseEffectFollow(0.1f, m_currentLandingEffect));
                m_inAir = false;
            }
        }
        else
        {
            if (XCI.GetButtonDown(m_player.jumpButton, m_player.controller)) //Just jumped
            {
                if (m_currentLandingEffect != null)//If player is no longer grounded remove the following effect
                {
                    m_currentLandingEffect.transform.parent = null;
                }
            }
            if(!m_playerController.m_CollisionInfo.bottom)
            {
                if (GetComponent<ColourController>().m_firstBulletSlot)
                    Destroy(Instantiate(m_jumpingEffect[0], transform.TransformPoint(m_jumpingEffectSpawnPos), Quaternion.identity, transform), 1.0f);
                else
                    Destroy(Instantiate(m_jumpingEffect[1], transform.TransformPoint(m_jumpingEffectSpawnPos), Quaternion.identity, transform), 1.0f);

                m_inAir = true;
            }
        }

        if (Mathf.Abs(GetComponent<Player>().velocity.x) > 0.1f && m_playerController.m_CollisionInfo.bottom) //moving animation
            m_animator.SetBool("Moving", true);
        else
            m_animator.SetBool("Moving", false);

        if (!m_playerController.m_CollisionInfo.bottom && !m_playerController.m_CollisionInfo.top && !m_playerController.m_CollisionInfo.left && !m_playerController.m_CollisionInfo.right)// Falling animation
            m_animator.SetBool("InAir", true);
        else
            m_animator.SetBool("InAir", false);

        if((m_playerController.m_CollisionInfo.left || m_playerController.m_CollisionInfo.right) && !m_playerController.m_CollisionInfo.bottom && !m_hitWall)//hit side of wall
        {
            m_hitWall = true;
            //Decide how to place effect
            int wallDirection = (m_playerController.m_CollisionInfo.left) ? -1 : 1; // Checks which direction the player is colliding with wall

            if (GetComponent<ColourController>().m_firstBulletSlot)
                m_currentHitWallEffect = Instantiate(m_hitWallEffect[0], transform.TransformPoint(m_hitWallEffectSpawnPos * wallDirection), Quaternion.identity);
            else
                m_currentHitWallEffect = Instantiate(m_hitWallEffect[1], transform.TransformPoint(m_hitWallEffectSpawnPos * wallDirection), Quaternion.identity);
            StartCoroutine(PauseEffectFollow(0.1f, m_currentHitWallEffect));
        }
        else
            m_hitWall = false;
    }

    IEnumerator PauseEffectFollow(float delay, GameObject effectHolder)
    {
        yield return new WaitForSeconds(delay);
        effectHolder.transform.parent = null;
        Destroy(effectHolder, 1.0f);
        effectHolder = null;
    }
}
