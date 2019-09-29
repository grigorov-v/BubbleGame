namespace Core.Singleton {
	public abstract class SingleInstance<IT, T> where T : IT, new() {
		public static T Instance {
			get { return _instance; }
		}
		
		protected static T _instance = new T();

		public static void Reinit() {
			_instance = new T();
		}
	}
	
	public abstract class SingleInstance<T> : SingleInstance<T, T> where T : new() {}
}