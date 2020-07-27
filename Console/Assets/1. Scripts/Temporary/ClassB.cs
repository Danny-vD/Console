namespace Temporary
{
	public class ClassB
	{
		public static implicit operator ClassA(ClassB other)
		{
			return new ClassA();
		}

		public static explicit operator ClassB(ClassA other)
		{
			return new ClassB();
		}
	}
}