using System;

namespace Commands
{
	public class Command : AbstractCommand
	{
		private readonly Action callback;

		public Command(string name, Action commandCallback) : base(0)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke();
		}
	}

	public class Command<TParam1> : AbstractCommand
	{
		private readonly Action<TParam1> callback;

		public Command(string name, Action<TParam1> commandCallback) : base(1)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke((TParam1) parameters[0]);
		}
	}

	public class Command<TParam1, TParam2> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2> callback;

		public Command(string name,
			Action<TParam1, TParam2>
				commandCallback)
			: base(2)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3>
				commandCallback)
			: base(3)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4>
				commandCallback)
			: base(4)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5>
				commandCallback)
			: base(5)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6>
				commandCallback)
			: base(6)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7>
				commandCallback)
			: base(7)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8>
				commandCallback)
			: base(8)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7]
			);
		}
	}

	public class
		Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
				TParam5, TParam6, TParam7, TParam8,
				TParam9>
			callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9>
				commandCallback)
			: base(9)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9,
		TParam10> : AbstractCommand
	{
		private readonly
			Action<TParam1, TParam2, TParam3, TParam4,
				TParam5, TParam6, TParam7, TParam8,
				TParam9, TParam10> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10>
				commandCallback)
			: base(10)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8],
				(TParam10) parameters[9]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10,
		TParam11> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11>
				commandCallback)
			: base(11)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8],
				(TParam10) parameters[9],
				(TParam11) parameters[10]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10,
		TParam11, TParam12> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12>
				commandCallback)
			: base(12)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8],
				(TParam10) parameters[9],
				(TParam11) parameters[10],
				(TParam12) parameters[11]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10,
		TParam11, TParam12, TParam13> : AbstractCommand
	{
		private readonly Action<
			TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12,
			TParam13> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12,
					TParam13>
				commandCallback)
			: base(13)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8],
				(TParam10) parameters[9],
				(TParam11) parameters[10],
				(TParam12) parameters[11],
				(TParam13) parameters[12]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10,
		TParam11, TParam12, TParam13, TParam14> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12,
			TParam13, TParam14> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12,
					TParam13, TParam14>
				commandCallback)
			: base(14)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8],
				(TParam10) parameters[9],
				(TParam11) parameters[10],
				(TParam12) parameters[11],
				(TParam13) parameters[12],
				(TParam14) parameters[13]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10,
		TParam11, TParam12, TParam13, TParam14, TParam15> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12,
			TParam13, TParam14, TParam15> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12,
					TParam13, TParam14, TParam15>
				commandCallback)
			: base(15)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8],
				(TParam10) parameters[9],
				(TParam11) parameters[10],
				(TParam12) parameters[11],
				(TParam13) parameters[12],
				(TParam14) parameters[13],
				(TParam15) parameters[14]
			);
		}
	}

	public class Command<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TParam8, TParam9, TParam10,
		TParam11, TParam12, TParam13, TParam14, TParam15, TParam16> : AbstractCommand
	{
		private readonly Action<TParam1, TParam2, TParam3, TParam4,
			TParam5, TParam6, TParam7, TParam8,
			TParam9, TParam10, TParam11, TParam12,
			TParam13, TParam14, TParam15, TParam16> callback;

		public Command(string name,
			Action<TParam1, TParam2, TParam3, TParam4,
					TParam5, TParam6, TParam7, TParam8,
					TParam9, TParam10, TParam11, TParam12,
					TParam13, TParam14, TParam15, TParam16>
				commandCallback)
			: base(16)
		{
			Name     = name;
			callback = commandCallback;
		}

		public override void Invoke(params object[] parameters)
		{
			callback.Invoke
			(
				(TParam1) parameters[0],
				(TParam2) parameters[1],
				(TParam3) parameters[2],
				(TParam4) parameters[3],
				(TParam5) parameters[4],
				(TParam6) parameters[5],
				(TParam7) parameters[6],
				(TParam8) parameters[7],
				(TParam9) parameters[8],
				(TParam10) parameters[9],
				(TParam11) parameters[10],
				(TParam12) parameters[11],
				(TParam13) parameters[12],
				(TParam14) parameters[13],
				(TParam15) parameters[14],
				(TParam16) parameters[15]
			);
		}
	}
}