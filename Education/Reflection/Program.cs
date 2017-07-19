using System;
using System.Reflection;

class MemStream
{
    static void Main()
    {
        Type outInterface, classImplementedInterface;
        outInterface = GetOuterInterface();
        classImplementedInterface = GetClassImplementingInterface(outInterface);
        if (classImplementedInterface != null)
        {
            object obj = Activator.CreateInstance(classImplementedInterface);
            Console.WriteLine("Enter the current index (number): ");
            bool isInputCorrect;
            int indexFromInput = 0;
            isInputCorrect = Int32.TryParse(Console.ReadLine(), out indexFromInput);
            if(isInputCorrect)
            {
                SetCurrentIndex(obj, indexFromInput);
                Console.WriteLine("Next index is: ");
                Console.WriteLine((GetNextIndex(obj)));
            }
            else
            {
                Console.WriteLine("Wrong input!");
            }
        }
        Console.ReadKey();
    }

    static Type GetOuterInterface()
    {
        Assembly outAssembly = Assembly.LoadFrom("d:\\Education_dotNet_Reflection_interface.dll");
        Type result = null;
        foreach (var type in outAssembly.GetTypes())
        {
            if (type.Name == "IInterface")
            {
                result = type;
            }
        }
        return result;
    }

    static Type GetClassImplementingInterface(Type outInterface)
    {
        Assembly outAssembly = Assembly.LoadFrom("d:\\Education_dotNet_Reflection_classes.dll");
        Type result = null;
        foreach (var type in outAssembly.GetTypes())
        {
            foreach (var i in type.GetInterfaces())
            {
                if (i == outInterface)
                {
                    result = type;
                }
            }
        }
        return result;
    }

    static void SetCurrentIndex(object classInstance, int valueToSet)
    {
        PropertyInfo currentIndexProperty = classInstance.GetType().GetProperty("CurrentIndex");
        currentIndexProperty.SetValue(classInstance, valueToSet);
    }

    static int GetNextIndex(object classInstance)
    {
        MethodInfo getNextIndexMethod = classInstance.GetType().GetMethod("GetNextIndex");
        object result = getNextIndexMethod.Invoke(classInstance, null);
        return Convert.ToInt32(result);
    }
}