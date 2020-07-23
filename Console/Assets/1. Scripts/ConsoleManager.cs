using Commands;
using VDFramework.Singleton;

public class ConsoleManager : Singleton<ConsoleManager>
{
	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(true);
	}
}
