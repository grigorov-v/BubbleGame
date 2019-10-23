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
        public override void PostInit() {
        }
    }
}