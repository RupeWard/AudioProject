using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Vars/PosRotSet", order = 1000)]
    public class PosRotSet : ScriptableObject
    {
        public int Num
        {
            get { return posrots.Length;  }
        }

        public Maths.PosRot[] posrots = new Maths.PosRot[0];            
    }
}

