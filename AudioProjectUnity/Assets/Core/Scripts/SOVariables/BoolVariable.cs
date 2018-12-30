using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Vars/Bool", order = 1000)]
    public class BoolVariable : ScriptableObject
    {
        public bool Value; // capitalised so as not to get conflated with value keyword in Property setters
    }
}

