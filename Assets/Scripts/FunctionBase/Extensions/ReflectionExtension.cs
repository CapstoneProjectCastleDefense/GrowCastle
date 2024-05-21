namespace FunctionBase.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtension
    {
        public static List<Type> GetAllDerivedTypes<T>(bool sameAssembly = false)
        {
            var baseType = typeof(T);
            var baseAsm  = Assembly.GetAssembly(baseType);
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(asm => !asm.IsDynamic && (!sameAssembly || asm == baseAsm))
                .SelectMany(GetTypesSafely)
                .Where(type => type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type)).ToList();
        }

        public static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
            #if UNITY_EDITOR
            IEnumerable<Type> types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types.Where(t => t != null);
            }

            return types;
            #else
                    return assembly.GetTypes();
            #endif
        }
    }
}