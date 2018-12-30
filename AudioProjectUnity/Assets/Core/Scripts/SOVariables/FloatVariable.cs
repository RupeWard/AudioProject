using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Vars/Float", order = 1000)]
    public class FloatVariable : ScriptableObject
    {
        public float Value; // capitalised so as not to get conflated with value keyword in Property setters
    }
}

