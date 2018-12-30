﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Vars/Vector2", order = 1000)]
    public class Vector2Variable : ScriptableObject
    {
        public Vector2 Value; // capitalised so as not to get conflated with value keyword in Property setters
    }
}
