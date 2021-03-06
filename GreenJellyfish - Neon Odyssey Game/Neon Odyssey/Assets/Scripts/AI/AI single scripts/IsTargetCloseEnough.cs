﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------
//-written by: Samuel
//-contributors:
//---------------------------------------------------------

public class IsTargetCloseEnough : BehaviourBase
{
    public float m_targetDistance = 0.0f;

    //--------------------------------------------------------------------------------------
    // Update behaviours - Get the distance to an object
    //
    // Return:
    //		Returns a enum BehaviourStatus, current status of behaviour, Success, failed, pending
    //--------------------------------------------------------------------------------------
    public override BehaviourBase.BehaviourStatus Execute()
    {
        if ((transform.position - GetComponent<Enemy>().m_target.transform.position).magnitude < m_targetDistance)
            return BehaviourStatus.SUCCESS;
        return BehaviourStatus.FAILURE;
    }
}
