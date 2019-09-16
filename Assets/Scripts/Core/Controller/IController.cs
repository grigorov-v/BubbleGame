namespace Core.Controller {
    public interface IController {
        void Init();
        void Load();
        void PostLoad();
        void Save();
    }
}