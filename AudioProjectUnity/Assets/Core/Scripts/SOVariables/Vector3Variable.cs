using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Vars/Vector3", order = 1000)]
    public class Vector3Variable : ScriptableObject
    {
        public Vector3 Value; // capitalised so as not to get conflated with value keyword in Property setters
    }
}

