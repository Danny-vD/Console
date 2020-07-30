using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Console.Attributes;

namespace Console.Commands
{
	/// <summary>
	/// Wrapper for the Console.
	/// </summary>
	public class ReflectionCommand : AbstractCommand
	{
		private readonly CommandReflectionData refData;

		public ReflectionCommand(CommandReflectionData refData) : base(refData.AllowedParameterTypes.Length)
		{
			this.refData = refData;

			//Setting the Data from the attributes
			SetName(refData.Attribute.Name);
			SetHelpMessage(refData.Attribute.HelpMessage);
			Aliases.AddRange(refData.Attribute.Aliases);
		}

		public override string GetFullName()
		{
			ParameterInfo[] parameters = refData.Info.GetParameters();

			StringBuilder stringBuilder = new StringBuilder();

			if (parameters.Length != 0)
			{
				stringBuilder.Append(" (");
				stringBuilder.Append(parameters[0].ParameterType.Name);
				
				for (int i = 1; i < parameters.Length; i++)
				{
					stringBuilder.Append($", {parameters[i].ParameterType.Name}");
				}
				
				stringBuilder.Append(")");
			}

			return refData.Attribute.Name + $"{stringBuilder}";
		}

		//Simple Wrapper
		public override void Invoke(params object[] parameters)
		{
			refData.Invoke(parameters);
		}
	}

	/// <summary>
	/// Inner class that contains the reflected data
	/// </summary>
	public class CommandReflectionData
	{
		/// <summary>
		/// Instance is null when the Command Function is static
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="info"></param>
		public CommandReflectionData(object instance, MethodInfo info)
		{
			Instance  = instance;
			Info      = info;
			Attribute = info.GetCustomAttribute<CommandAttribute>();
		}

		public readonly object Instance;
		public readonly MethodInfo Info;

		/// <summary>
		/// Can be used to find name and alias
		/// </summary>
		public readonly CommandAttribute Attribute;

		/// <summary>
		/// Shortcut to get the types of the parameters
		/// </summary>
		public Type[] AllowedParameterTypes => Info.GetParameters().Select(x => x.ParameterType).ToArray();

		/// <summary>
		/// Gets called by the Console System
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public object Invoke(object[] parameter)
		{
			if (!IsAllowedSignature(parameter))
			{
				//Cast
				return Info.Invoke(Instance, Cast(parameter));
			}
			else
			{
				return Info.Invoke(Instance, parameter);
			}
		}

		private object[] Cast(object[] parameter)
		{
			Type[] pt = AllowedParameterTypes;
			object[] ret = new object[pt.Length];

			for (int i = 0; i < pt.Length; i++)
			{
				Type type = pt[i];
				ret[i] = CommandAttributeUtils.ConvertToNonGeneric(parameter[i], type);
			}

			return ret;
		}

		private bool IsAllowedSignature(object[] parameter)
		{
			Type[] pt = AllowedParameterTypes;
			if (pt.Length != parameter.Length) return false;

			bool ret = true;

			for (int i = 0; i < pt.Length; i++)
			{
				Type type = pt[i];
				ret &= type.IsInstanceOfType(parameter[i]);
			}

			return ret;
		}
	}
}