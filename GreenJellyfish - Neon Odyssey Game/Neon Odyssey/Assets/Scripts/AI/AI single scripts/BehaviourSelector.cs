﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------------------------------------------
//-written by: Samuel
//-contributors:
//---------------------------------------------------------

public class BehaviourSelector : BehaviourComposite
{
    //--------------------------------------------------------------------------------------
    // Update behaviours in tree - OR like
    //
    // Return:
    //		Returns a enum BehaviourStatus, current status of behaviour, Success, failed, pending
    //--------------------------------------------------------------------------------------
    public override BehaviourBase.BehaviourStatus Execute()
    {
        if (!m_pendingBranch)
            BehaviourSetup(); ;

        while (m_branchNumber < m_behaviourBranches.Count)
        //Requires only one child to succeed
        {
            BehaviourBase currentBranch = m_behaviourBranches[m_branchNumber];

            if(!m_pendingBranch)
                currentBranch.BehaviourSetup();
            m_pendingBranch = false;
            BehaviourBase.BehaviourStatus branchStatus = currentBranch.Execute();

            if (branchStatus == BehaviourStatus.SUCCESS)
            {
                return BehaviourStatus.SUCCESS;
            }
            if (branchStatus == BehaviourStatus.PENDING)
            {
                m_pendingBranch = true;
                return BehaviourStatus.PENDING;
            }
            m_branchNumber++;
        }
        return BehaviourStatus.FAILURE;
    }
}
