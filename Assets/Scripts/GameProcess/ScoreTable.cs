using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Utils;

namespace GameProcess {
    public class ScoreTable : MonoSingleton<ScoreTable> {
        [SerializeField] Transform _pointForRewardAnimation = null;

        public Transform PointForRewardAnimation {
            get {
                return _pointForRewardAnimation;
            }
        }
    }
}