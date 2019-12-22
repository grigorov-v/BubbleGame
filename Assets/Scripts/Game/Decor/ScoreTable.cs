using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core.Singleton;

namespace Game {
    public class ScoreTable : MonoSingleton<ScoreTable> {
        [SerializeField] Transform _pointForRewardAnimation = null;

        public Transform PointForRewardAnimation {
            get {
                return _pointForRewardAnimation;
            }
        }
    }
}