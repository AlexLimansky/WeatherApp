using System;
using System.Reflection;
using educationClasses = Education_dotNet_Reflection_classes;

class MemStream
{
    static void Main()
    {
        Type resultType = null;
        Assembly assembly = typeof(educationClasses.ClassA).Module.Assembly;
        foreach (var type in assembly.GetTypes())
        {
            foreach (var i in type.GetInterfaces())
            {
                if (i.Name == "IInterface")
                {
                    resultType = type;
                }
            }
        }
        if (resultType != null)
        {
            object obj = Activator.CreateInstance(resultType);
            MethodInfo currentIndexMethod = resultType.GetMethod("set_CurrentIndex");
            MethodInfo getNextIndexMethod = resultType.GetMethod("GetNextIndex");
            currentIndexMethod.Invoke(obj, new object[] { 32 });
            object result = getNextIndexMethod.Invoke(obj, null);
            Console.WriteLine((result));
        }
        Console.ReadKey();
    }
}