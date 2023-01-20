using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NekoLib.IoC
{
    public class IoCContainer : IIoC
    {
        private HashSet<Type> _types = new HashSet<Type>();
        private Dictionary<Type, Type> _dependencies = new Dictionary<Type, Type>();
        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public void Register<T>()
        {
            _types.Add(typeof(T));
        }

        public void Register<TBase, TConcrete>() where TConcrete : TBase
        {
            var baseType = typeof(TBase);
            var concreteType = typeof(TConcrete);
            _dependencies.Add(baseType, concreteType);
        }

        public void RegisterInstance(object instance)
        {
            var type = instance.GetType();
            _instances.Add(type, instance);
        }

        public void RegisterInstance<T>(object instance)
        {
            var type = typeof(T);
            _instances.Add(type, instance);
        }

        public T Resolve<T>() where T : class
        {
            var type = typeof(T);
            return Resolve(type) as T;
        }

        object Resolve(Type type)
        {
            // Use already registered instance of type.
            if (_instances.ContainsKey(type))
            {
                return _instances[type];
            }
            // Create new instance of concrete type by base type.
            if (_dependencies.ContainsKey(type))
            {
                return Activator.CreateInstance(_dependencies[type]);
            }
            // Create new instance of type.
            if (_types.Contains(type))
            {
                return Activator.CreateInstance(type);
            }
            return default;
        }

        public void Inject(object obj)
        {
            foreach(var propertyInfo in obj.GetType().GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(Inject)).Any()))
            {
                var instance = Resolve(propertyInfo.PropertyType);
                if(instance != null)
                {
                    propertyInfo.SetValue(obj, instance);
                }
                else
                {
                    UnityEngine.Debug.LogErrorFormat("Cannot get instance of type {0}", propertyInfo.PropertyType);
                }
            }
        }
    }

    public class Inject : Attribute
    {

    }
}