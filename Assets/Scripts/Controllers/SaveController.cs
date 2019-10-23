using System.Collections.Generic;
using System.Xml;

using UnityEngine;
using UnityEngine.SceneManagement;

using Core.Controller;
using Core.XML;

using Configs;

namespace Controllers {

    struct XmlSaveInfo {
        public ISave Save;
        public string Key;
        public string NodeName;
    }

    public class SaveController : BaseController<SaveController> {
        List<XmlSaveInfo> _xmlLoadableInfoList = new List<XmlSaveInfo>() {
            new XmlSaveInfo() {
                Save     = null,
                Key      = "{SceneName}",
                NodeName = "root"
            }   
        };

        public override void PostInit() {
        }
    }
}