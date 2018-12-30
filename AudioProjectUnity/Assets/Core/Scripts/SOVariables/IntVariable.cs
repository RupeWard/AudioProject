using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Vars/Int", order = 1000)]
    public class IntVariable : ScriptableObject
    {
        public int Value; // capitalised so as not to get conflated with value keyword in Property setters
    }
}

