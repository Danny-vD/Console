using VDFramework.Singleton;

namespace Console
{
	public class ConsoleManager : Singleton<ConsoleManager>
	{
		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(true);
		}
	}
}
