using System;

namespace Commands
{
    public class Command : AbstractCommand
    {
        private readonly Action callback;

        public Command(string name, Action commandCallback) : base(0)
        {
            Name = name;
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

        public Command(string name, Action<TParam1, TParam2> commandCallback) : base(2)
        {
            Name     = name;
            callback = commandCallback;
        }

        public override void Invoke(params object[] parameters)
        {
            callback.Invoke((TParam1) parameters[0], (TParam2) parameters[1]);
        }
    }
    
    public class Command<TParam1, TParam2, TParam3> : AbstractCommand
    {
        private readonly Action<TParam1, TParam2, TParam3> callback;

        public Command(string name, Action<TParam1, TParam2, TParam3> commandCallback) : base(3)
        {
            Name     = name;
            callback = commandCallback;
        }

        public override void Invoke(params object[] parameters)
        {
            callback.Invoke((TParam1) parameters[0], (TParam2) parameters[1], (TParam3) parameters[2]);
        }
    }
}
