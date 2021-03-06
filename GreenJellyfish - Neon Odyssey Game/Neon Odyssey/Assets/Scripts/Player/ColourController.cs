﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

//---------------------------------------------------------
//-written by: Edward,
//-contributors: Samuel, Nick
//---------------------------------------------------------

//-----------------------------------------------------------------------------
// Attach script to player gameobject
//-----------------------------------------------------------------------------

public class ColourController : MonoBehaviour
{
    public XboxController controller;
    public float m_switchTime = 1.0f;
    public float m_switchTimer = 0.0f;
    //All four materials, Purple, Pink, Orange, Green
    public Material[] playerMaterial;

    //MISC VARIABLES
    public bool m_firstBulletSlot = true;

    public void Start()
    {
        toggleColour();
    }

    void Update()
    {
        //Disable player input
        if (GameObject.FindWithTag("GameController").GetComponent<GameManager>().m_inputOn == false)
            return;

        if (!GetComponent<Player>().IsDead())
        {
            //TOGGLE PLAYER COLOUR
            m_switchTimer += Time.deltaTime;

            if ((XCI.GetButton(XboxButton.Y, controller) || XCI.GetButton(XboxButton.RightBumper, controller) || XCI.GetButton(XboxButton.LeftBumper, controller) || XCI.GetAxisRaw(XboxAxis.LeftTrigger, controller) > 0 || XCI.GetAxisRaw(XboxAxis.RightTrigger, controller) > 0) && m_switchTimer >= m_switchTime)
            {
                toggleColour();
                m_switchTimer = 0.0f;
                GameObject.FindWithTag("GameController").GetComponent<GameManager>().GetComponentsInChildren<UI>()[0].ChangeUIColour(GetComponent<Player>().isFirstPlayer, m_firstBulletSlot);
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // Toggle character colour 
    // based off player weapon selected slot and first player or not, change colour
    //--------------------------------------------------------------------------------------
    void toggleColour()
    {
        m_firstBulletSlot = !m_firstBulletSlot; // Toggle colour

        if (GetComponent<Player>().isFirstPlayer)
        {
            if(m_firstBulletSlot)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = playerMaterial[0];
                gameObject.layer = LayerMask.NameToLayer("Purple");
            }
            else
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = playerMaterial[1];
                gameObject.layer = LayerMask.NameToLayer("Orange");
            }
        }
        else
        {
            if (m_firstBulletSlot)
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = playerMaterial[0];
                gameObject.layer = LayerMask.NameToLayer("Green");
            }
            else
            {
                GetComponentInChildren<SkinnedMeshRenderer>().material = playerMaterial[1];
                gameObject.layer = LayerMask.NameToLayer("Pink");
            }
        }
    }
}
