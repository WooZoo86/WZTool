using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WZToolLib.Reflect
{
    public class ProxyHelper : MarshalByRefObject
    {
        public void LoadAssembly(string dll)
        {
            this.assembly = Assembly.LoadFile(dll);
        }

        public object Invoke(string fullClassName, string methodName, params object[] args)
        {
            object result;
            if (this.assembly == null)
            {
                Console.WriteLine("assembly is null");
                result = null;
            }
            else
            {
                Type type = this.assembly.GetType(fullClassName);
                if (type == null)
                {
                    Console.WriteLine("type is null");
                    result = null;
                }
                else
                {
                    MethodInfo method = type.GetMethod(methodName);
                    if (method == null)
                    {
                        Console.WriteLine("method is null");
                        result = null;
                    }
                    else
                    {
                        object obj = Activator.CreateInstance(type);
                        result = method.Invoke(obj, args);
                    }
                }
            }

            return result;
        }

        private Assembly assembly = null;
    }
    
}
