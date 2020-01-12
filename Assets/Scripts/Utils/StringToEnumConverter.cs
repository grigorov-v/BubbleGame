using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LevelValues;

namespace Utils {
    public class StringToEnumConverter {
        public static LevelTarget LevelTargetFromString(string value) {
            var levelTargetValues = Enum.GetValues(typeof(LevelTarget));
            foreach (LevelTarget target in levelTargetValues) {
                if ( target.ToString() == value ) {
                    return target;
                } 
            }

            return LevelTarget.All_Bubbles;
        }
    }
}