using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Console.Core.LogSystem;

namespace Console.Core.ILOptimizations
{
    /// <summary>
    /// Can be used to Signal that a Method can be Optimized to IL Code to avoid Reflection
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    public class OptimizeILAttribute : Attribute
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public static class ILTools
    {

        internal static readonly ALogger ILLogger = TypedLogger.CreateTypedWithPrefix("ILOptimizer");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool CanOptimize(MemberInfo info)
        {
            OptimizeILAttribute attrib = info.GetCustomAttribute<OptimizeILAttribute>();
            bool optimize = ConsoleCoreConfig.AggressiveILOptimizations || attrib != null;
            bool wrong = info is MethodInfo mi && !mi.IsPublic || info is FieldInfo fi && !fi.IsPublic;
            if (optimize && wrong)
            {
                if (attrib != null)
                {
                    ILLogger.LogWarning(
                                        $"Can not optimize non-public methods. Make the Method {info} public to be able to optimize."
                                       );
                }

                return false;
            }

            return optimize;
        }

        #region Property

        #region Set

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T SetPropertyDel<T>(PropertyInfo info)
            where T : Delegate
        {
            return GetMethodDel<T>(info.SetMethod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DynamicMethod SetProperty(PropertyInfo info)
        {
            return GetMethod(info.SetMethod);
        }

        #endregion

        #region Get

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T GetPropertyDel<T>(PropertyInfo info)
            where T : Delegate
        {
            return GetMethodDel<T>(info.GetMethod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DynamicMethod GetProperty(PropertyInfo info)
        {
            return GetMethod(info.GetMethod);
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetConstructorDel<T>(string type)
            where T : Delegate
        {
            return GetConstructorDel<T>(Type.GetType(type));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetConstructorDel<T>(Type type)
            where T : Delegate
        {
            DynamicMethod dm = GetConstructor(type);
            return (T) dm.CreateDelegate(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DynamicMethod GetConstructor(Type type)
        {
            ConstructorInfo cinfo = type.GetConstructor(new Type[0]);
            DynamicMethod dm = new DynamicMethod(type.Name + "Ctor", type, new Type[0]);
            ILGenerator gen = dm.GetILGenerator();
            gen.Emit(OpCodes.Newobj, cinfo);
            gen.Emit(OpCodes.Ret);
            return dm;
        }

        #endregion

        #region Field

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DynamicMethod SetField(FieldInfo info)
        {
            DynamicMethod dm = new DynamicMethod(
                                                 "ILSetFieldValue." + info.Name,
                                                 null,
                                                 new[] { typeof(object), info.FieldType }
                                                );
            ILGenerator gen = dm.GetILGenerator();

            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Stfld, info);

            gen.Emit(OpCodes.Ret);
            return dm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DynamicMethod GetField(FieldInfo info)
        {
            DynamicMethod dm = new DynamicMethod(
                                                 "ILGetFieldValue." + info.Name,
                                                 info.FieldType,
                                                 new[] { typeof(object) }
                                                );
            ILGenerator gen = dm.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldfld, info);
            gen.Emit(OpCodes.Ret);
            return dm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T GetFieldDel<T>(FieldInfo info)
            where T : Delegate
        {
            DynamicMethod dm = GetField(info);
            return (T) dm.CreateDelegate(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T SetFieldDel<T>(FieldInfo info)
            where T : Delegate
        {
            DynamicMethod dm = SetField(info);
            return (T) dm.CreateDelegate(typeof(T));
        }

        #endregion

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T GetMethodDel<T>(MethodInfo info)
            where T : Delegate
        {
            DynamicMethod dm = GetMethod(info);
            return (T) dm.CreateDelegate(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static DynamicMethod GetMethod(MethodInfo info)
        {
            if (info.IsStatic)
            {
                return GetStaticMethod(info);
            }

            return GetInstanceMethod(info);
        }

        private static DynamicMethod GetStaticMethod(MethodInfo info)
        {
            List<Type> args = info.GetParameters().Select(x => x.ParameterType).ToList();

            DynamicMethod dm =
                new DynamicMethod("ILStaticMethodCall." + info.Name, info.ReturnType, args.ToArray());
            ILGenerator gen = dm.GetILGenerator();

            for (int i = 0; i < args.Count; i++)
            {
                gen.Emit(OpCodes.Ldarg, i);
            }

            gen.Emit(OpCodes.Call, info);
            gen.Emit(OpCodes.Ret);
            return dm;
        }

        private static DynamicMethod GetInstanceMethod(MethodInfo info)
        {
            List<Type> args = new List<Type> { typeof(object) };
            args.AddRange(info.GetParameters().Select(x => x.ParameterType));

            DynamicMethod dm =
                new DynamicMethod("ILInstanceMethodCall." + info.Name, info.ReturnType, args.ToArray());
            ILGenerator gen = dm.GetILGenerator();

            for (int i = 0; i < args.Count; i++)
            {
                gen.Emit(OpCodes.Ldarg, i);
            }

            gen.Emit(OpCodes.Call, info);
            gen.Emit(OpCodes.Ret);
            return dm;
        }

        #endregion

    }
}