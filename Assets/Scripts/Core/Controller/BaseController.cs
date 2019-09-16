namespace Core.Controller {
    public class BaseController<T>: IController where T: new() {
        public static T Instance {get; private set;}

        public virtual void Init() {
            if (Instance != null) {
                return;
            }

            Instance = new T();
        }

        public virtual void Load() {
        }

        public virtual void PostLoad() { 
        }

        public virtual void Save() {
        }
    }
}