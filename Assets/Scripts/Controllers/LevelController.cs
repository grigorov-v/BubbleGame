using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using Core.Events;
using Core.Extensions;

using LevelValues;
using Configs;
using Game.Events;
using Game.Bubbles;

namespace Controllers {
    public class LevelController : BaseController<LevelController> {
        public LevelInfo         LevelInfo    { get; private set; }
        public List<LevelTarget> LevelTargets { get; private set; }

        public override void PostInit() {
            LevelInfo = FindCurrentLevelInfo();
            LevelTargets = GetLevelTargets();

            EventManager.Subscribe<DeactivateBubble>(this, OnDeactivateBubble);
            EventManager.Subscribe<DestroyBubble>(this, OnDestroyBubble);
        }

        public override void Reinit() {
            EventManager.Unsubscribe<DeactivateBubble>(OnDeactivateBubble);
            EventManager.Unsubscribe<DestroyBubble>(OnDestroyBubble);
        }

        public bool IsLevelTarget(LevelTarget target) {
            return LevelTargets.Exists(tr => tr == target);
        }

        public bool IsIgnoreBubble(string tag) {
            return LevelInfo.IgnoreBubbles.Exists(b => b.Tag == tag);
        }

        LevelInfo FindCurrentLevelInfo() {
            var levelIndex = SaveController.Instance.GetCurrentProgress().Level;
            var levelConfig = ConfigsController.Instance.FindConfig<LevelConfig>();
            if ( levelConfig == null ) {
                return null;
            }

            var levels = levelConfig.Levels;
            if ( (levels == null) || (levelIndex > levels.Count - 1) ) {
                return null;
            }

            return levels[levelIndex];
        }

        List<LevelTarget> GetLevelTargets() {
            var levelInfo = FindCurrentLevelInfo();
            return levelInfo.Targets;
        }

        void OnDeactivateBubble(DeactivateBubble e) {
            if ( !IsLevelTarget(LevelTarget.IgnoreBubble) ) {
                return;
            }

            if ( !IsIgnoreBubble(e.Bubble.BubbleTag) ) {
                return;
            }

            Debug.Log("Lose");
        }

        void OnDestroyBubble(DestroyBubble e) {
            if ( Bubble.Cache.Exists(item => !item.Value.InGun ) ){
                return;
            }

            Debug.Log("Win");
        }
    }
}