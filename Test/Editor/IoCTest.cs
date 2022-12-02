using NUnit.Framework;

namespace NekoSystems.Test
{
    public class IoCTest
    {
        [Test]
        public void RegisterAndResolve()
        {
            var container = new IoCContainer();
            container.Register<IoCContainer>();
            var obj = container.Resolve<IoCContainer>();

            Assert.IsNotNull(obj);
            Assert.AreNotEqual(container, obj);
        }

        [Test]
        public void ResolveRegisteredType()
        {
            var container = new IoCContainer();
            var obj = container.Resolve<IoCContainer>();

            Assert.IsNull(obj);
        }

        [Test]
        public void RepeatedRegister()
        {
            var container = new IoCContainer();
            container.Register<IoCContainer>();
            container.Register<IoCContainer>();
        }

        [Test]
        public void RegisterDependency()
        {
            var container = new IoCContainer();
            container.Register<IIoC, IoCContainer>();
            var ioc = container.Resolve<IIoC>();

            Assert.AreEqual(ioc.GetType(), typeof(IoCContainer));
        }

        [Test]
        public void RegisterInstance()
        {
            var container = new IoCContainer();
            container.RegisterInstance(new IoCContainer());
            var instanceA = container.Resolve<IoCContainer>();
            var instanceB = container.Resolve<IoCContainer>();

            Assert.AreEqual(instanceA, instanceB);
        }

        [Test]
        public void RegisterInstanceDependency()
        {
            var container = new IoCContainer();
            container.RegisterInstance<IIoC>(container);
            var iocA = container.Resolve<IIoC>();
            var iocB = container.Resolve<IIoC>();

            Assert.AreEqual(iocA, container);
            Assert.AreEqual(iocA, iocB);
        }

        class DependencyA { }

        class DependencyB { }

        class User
        {
            [Inject]
            public DependencyA A { get; set; }

            [Inject]
            public DependencyB B { get; set; }
        }

        [Test]
        public void Inject()
        {
            var container = new IoCContainer();
            container.RegisterInstance(new DependencyA());
            container.Register<DependencyB>();
            User user = new User();
            container.Inject(user);

            Assert.IsNotNull(user.A);
            Assert.IsNotNull(user.B);
            Assert.AreEqual(user.A.GetType(), typeof(DependencyA));
            Assert.AreEqual(user.B.GetType(), typeof(DependencyB));
        }
    }
}