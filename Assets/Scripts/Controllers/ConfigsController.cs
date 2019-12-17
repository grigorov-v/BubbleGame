﻿using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using Core.XML;

using Configs;

namespace Controllers {
    public class ConfigsController : BaseController<ConfigsController> {
        const string PATH_COMMON_CONFIG = "Configs/CommonConfig";

        CommonConfig _commonConfig = new CommonConfig();

        public override void PostInit() {
            LoadHelper.LoadFromResources(_commonConfig, PATH_COMMON_CONFIG, "root");

            var sceneName = SceneManager.GetActiveScene().name;
            var worldInfo = FindWorldInfo(wi => (wi.MapSceneName == sceneName) || (wi.GameSceneName == sceneName));
            foreach (var pair in ConfigsRegistrator.Configs) {
                var name = pair.Key;
                var path = worldInfo.Configs.ContainsKey(name) ? worldInfo.Configs[name] : string.Empty;

                if ( string.IsNullOrEmpty(path) ) {
                    continue;
                }

                var config = pair.Value;
                LoadHelper.LoadFromResources(config, path, "root");
            }
        }

        public T FindConfig<T>() where T: class, IConfig {
            var config = FindConfig(c => c is T);
            if ( config != null ) {
                return config as T;
            }

            return null;
        }

        public LevelInfo GetLevelInfo(int levelIndex) {
            var levelConfig = FindConfig<LevelConfig>();
            if ( levelConfig == null ) {
                return null;
            }

            var levels = levelConfig.Levels;
            if ( (levels == null) || (levelIndex > levels.Count - 1) ) {
                return null;
            }

            return levels[levelIndex];
        }

        IConfig FindConfig(Func<IConfig, bool> factory) {
            foreach (var pair in ConfigsRegistrator.Configs) {
                var config = pair.Value;
                if ( factory(config) ) {
                    return config;
                }
            }

            return null;
        }

        WorldInfo FindWorldInfo(Func<WorldInfo, bool> factory) {
            foreach (var pair in _commonConfig.Worlds) {
                var worldInfo = pair.Value;
                if ( factory(worldInfo) ) {
                    return worldInfo;
                }
            }

            return null;
        }
    } 
}