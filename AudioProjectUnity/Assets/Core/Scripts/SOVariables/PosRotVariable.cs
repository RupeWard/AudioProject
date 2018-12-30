using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Vars/PosRot", order = 1000)]
    public class PosRotVariable : ScriptableObject
    {
        public Maths.PosRot Value; // capitalised so as not to get conflated with value keyword in Property setters
    }
}

