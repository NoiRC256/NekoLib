namespace NekoLib.Ioc
{
    public interface IIoC
    {
        /// <summary>
        /// Register type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void Register<T>();

        /// <summary>
        /// Register subtype.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        void Register<TBase, TConcrete>() where TConcrete : TBase;

        /// <summary>
        /// Register singleton instance.
        /// </summary>
        /// <param name="instance"></param>
        void RegisterInstance(object instance);

        /// <summary>
        /// Register singleton instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        void RegisterInstance<T>(object instance);

        /// <summary>
        /// Get an instance of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Inject dependency.
        /// </summary>
        /// <param name="obj"></param>
        void Inject(object obj);
    }
}