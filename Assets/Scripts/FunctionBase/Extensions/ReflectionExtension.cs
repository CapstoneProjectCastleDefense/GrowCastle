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
        
        public static List<FieldInfo> GetFieldInfoWithAttribute<T>(this object instance) where T : Attribute
        {
            var fieldInfos = instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            return fieldInfos.Where(field => field.CustomAttributes.Any()).Where(field => field.CustomAttributes.FirstOrDefault()?.AttributeType == typeof(T)).ToList();
        }
        
        public static List<FieldInfo> GetFieldInfo(this object instance) 
        {
            var fieldInfos = instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return fieldInfos.ToList();
        }
    }
}