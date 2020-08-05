                                                                                    

						      
using System;
	
namespace Console.Core.Commands.CommandImplementations
{
	public class Command : AbstractCommand
	{
		private readonly Action callback;

		public Command(string name, Action commandCallback) : base(0)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} ";
		}

		public override void Invoke(params object[] parameters)
		{

			callback.Invoke(
			);
		}
	}



	public class Command<TParam0> : AbstractCommand
	{
		private readonly Action<TParam0> callback;

		public Command(string name, Action<TParam0> commandCallback) : base(1)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0])
			);
		}
	}



	public class Command<TParam0, TParam1> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1> callback;

		public Command(string name, Action<TParam0, TParam1> commandCallback) : base(2)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2> commandCallback) : base(3)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3> commandCallback) : base(4)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4> commandCallback) : base(5)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5> commandCallback) : base(6)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> commandCallback) : base(7)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> commandCallback) : base(8)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> commandCallback) : base(9)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9> commandCallback) : base(10)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}, {typeof(TParam9).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");
			if(!IsValidCast<TParam9>(parameters[9])) throw new InvalidCastException("Invalid Cast Parameter: 9");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8]), 
				ConvertTo<TParam9>(parameters[9])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10> commandCallback) : base(11)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}, {typeof(TParam9).Name}, {typeof(TParam10).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");
			if(!IsValidCast<TParam9>(parameters[9])) throw new InvalidCastException("Invalid Cast Parameter: 9");
			if(!IsValidCast<TParam10>(parameters[10])) throw new InvalidCastException("Invalid Cast Parameter: 10");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8]), 
				ConvertTo<TParam9>(parameters[9]), 
				ConvertTo<TParam10>(parameters[10])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11> commandCallback) : base(12)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}, {typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");
			if(!IsValidCast<TParam9>(parameters[9])) throw new InvalidCastException("Invalid Cast Parameter: 9");
			if(!IsValidCast<TParam10>(parameters[10])) throw new InvalidCastException("Invalid Cast Parameter: 10");
			if(!IsValidCast<TParam11>(parameters[11])) throw new InvalidCastException("Invalid Cast Parameter: 11");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8]), 
				ConvertTo<TParam9>(parameters[9]), 
				ConvertTo<TParam10>(parameters[10]), 
				ConvertTo<TParam11>(parameters[11])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12> commandCallback) : base(13)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}, {typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");
			if(!IsValidCast<TParam9>(parameters[9])) throw new InvalidCastException("Invalid Cast Parameter: 9");
			if(!IsValidCast<TParam10>(parameters[10])) throw new InvalidCastException("Invalid Cast Parameter: 10");
			if(!IsValidCast<TParam11>(parameters[11])) throw new InvalidCastException("Invalid Cast Parameter: 11");
			if(!IsValidCast<TParam12>(parameters[12])) throw new InvalidCastException("Invalid Cast Parameter: 12");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8]), 
				ConvertTo<TParam9>(parameters[9]), 
				ConvertTo<TParam10>(parameters[10]), 
				ConvertTo<TParam11>(parameters[11]), 
				ConvertTo<TParam12>(parameters[12])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13> commandCallback) : base(14)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}, {typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}, {typeof(TParam13).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");
			if(!IsValidCast<TParam9>(parameters[9])) throw new InvalidCastException("Invalid Cast Parameter: 9");
			if(!IsValidCast<TParam10>(parameters[10])) throw new InvalidCastException("Invalid Cast Parameter: 10");
			if(!IsValidCast<TParam11>(parameters[11])) throw new InvalidCastException("Invalid Cast Parameter: 11");
			if(!IsValidCast<TParam12>(parameters[12])) throw new InvalidCastException("Invalid Cast Parameter: 12");
			if(!IsValidCast<TParam13>(parameters[13])) throw new InvalidCastException("Invalid Cast Parameter: 13");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8]), 
				ConvertTo<TParam9>(parameters[9]), 
				ConvertTo<TParam10>(parameters[10]), 
				ConvertTo<TParam11>(parameters[11]), 
				ConvertTo<TParam12>(parameters[12]), 
				ConvertTo<TParam13>(parameters[13])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13, TParam14> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13, TParam14> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13, TParam14> commandCallback) : base(15)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}, {typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}, {typeof(TParam13).Name}, {typeof(TParam14).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");
			if(!IsValidCast<TParam9>(parameters[9])) throw new InvalidCastException("Invalid Cast Parameter: 9");
			if(!IsValidCast<TParam10>(parameters[10])) throw new InvalidCastException("Invalid Cast Parameter: 10");
			if(!IsValidCast<TParam11>(parameters[11])) throw new InvalidCastException("Invalid Cast Parameter: 11");
			if(!IsValidCast<TParam12>(parameters[12])) throw new InvalidCastException("Invalid Cast Parameter: 12");
			if(!IsValidCast<TParam13>(parameters[13])) throw new InvalidCastException("Invalid Cast Parameter: 13");
			if(!IsValidCast<TParam14>(parameters[14])) throw new InvalidCastException("Invalid Cast Parameter: 14");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8]), 
				ConvertTo<TParam9>(parameters[9]), 
				ConvertTo<TParam10>(parameters[10]), 
				ConvertTo<TParam11>(parameters[11]), 
				ConvertTo<TParam12>(parameters[12]), 
				ConvertTo<TParam13>(parameters[13]), 
				ConvertTo<TParam14>(parameters[14])
			);
		}
	}



	public class Command<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15> : AbstractCommand
	{
		private readonly Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15> callback;

		public Command(string name, Action<TParam0, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10, TParam11, TParam12, TParam13, TParam14, TParam15> commandCallback) : base(16)
		{
			Name = name;
			callback = commandCallback;
		}

		public override string GetFullName()
		{
			return $"{GetName()} {typeof(TParam0).Name}, {typeof(TParam1).Name}, {typeof(TParam2).Name}, {typeof(TParam3).Name}, {typeof(TParam4).Name}, {typeof(TParam5).Name}, {typeof(TParam6).Name}, {typeof(TParam7).Name}, {typeof(TParam8).Name}, {typeof(TParam9).Name}, {typeof(TParam10).Name}, {typeof(TParam11).Name}, {typeof(TParam12).Name}, {typeof(TParam13).Name}, {typeof(TParam14).Name}, {typeof(TParam15).Name}";
		}

		public override void Invoke(params object[] parameters)
		{
			if(!IsValidCast<TParam0>(parameters[0])) throw new InvalidCastException("Invalid Cast Parameter: 0");
			if(!IsValidCast<TParam1>(parameters[1])) throw new InvalidCastException("Invalid Cast Parameter: 1");
			if(!IsValidCast<TParam2>(parameters[2])) throw new InvalidCastException("Invalid Cast Parameter: 2");
			if(!IsValidCast<TParam3>(parameters[3])) throw new InvalidCastException("Invalid Cast Parameter: 3");
			if(!IsValidCast<TParam4>(parameters[4])) throw new InvalidCastException("Invalid Cast Parameter: 4");
			if(!IsValidCast<TParam5>(parameters[5])) throw new InvalidCastException("Invalid Cast Parameter: 5");
			if(!IsValidCast<TParam6>(parameters[6])) throw new InvalidCastException("Invalid Cast Parameter: 6");
			if(!IsValidCast<TParam7>(parameters[7])) throw new InvalidCastException("Invalid Cast Parameter: 7");
			if(!IsValidCast<TParam8>(parameters[8])) throw new InvalidCastException("Invalid Cast Parameter: 8");
			if(!IsValidCast<TParam9>(parameters[9])) throw new InvalidCastException("Invalid Cast Parameter: 9");
			if(!IsValidCast<TParam10>(parameters[10])) throw new InvalidCastException("Invalid Cast Parameter: 10");
			if(!IsValidCast<TParam11>(parameters[11])) throw new InvalidCastException("Invalid Cast Parameter: 11");
			if(!IsValidCast<TParam12>(parameters[12])) throw new InvalidCastException("Invalid Cast Parameter: 12");
			if(!IsValidCast<TParam13>(parameters[13])) throw new InvalidCastException("Invalid Cast Parameter: 13");
			if(!IsValidCast<TParam14>(parameters[14])) throw new InvalidCastException("Invalid Cast Parameter: 14");
			if(!IsValidCast<TParam15>(parameters[15])) throw new InvalidCastException("Invalid Cast Parameter: 15");

			callback.Invoke(
				ConvertTo<TParam0>(parameters[0]), 
				ConvertTo<TParam1>(parameters[1]), 
				ConvertTo<TParam2>(parameters[2]), 
				ConvertTo<TParam3>(parameters[3]), 
				ConvertTo<TParam4>(parameters[4]), 
				ConvertTo<TParam5>(parameters[5]), 
				ConvertTo<TParam6>(parameters[6]), 
				ConvertTo<TParam7>(parameters[7]), 
				ConvertTo<TParam8>(parameters[8]), 
				ConvertTo<TParam9>(parameters[9]), 
				ConvertTo<TParam10>(parameters[10]), 
				ConvertTo<TParam11>(parameters[11]), 
				ConvertTo<TParam12>(parameters[12]), 
				ConvertTo<TParam13>(parameters[13]), 
				ConvertTo<TParam14>(parameters[14]), 
				ConvertTo<TParam15>(parameters[15])
			);
		}
	}




}