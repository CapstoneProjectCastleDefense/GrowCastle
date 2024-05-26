namespace FunctionBase.Extensions
{
    using System;

    public static class TypeExtension
    {
        public static T GetCustomAttribute<T>(this object instance) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(instance.GetType(), typeof(T));;
        }
        
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type baseType)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (baseType == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }
    }
}