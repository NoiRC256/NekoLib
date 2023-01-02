# Overview

NekoLib is a library of common utilities for Unity. It encompasses a range of features that can be leveraged to optimize the game development workflow.

#### Base Features
- Pooling - Low-coupling object pools with capacity optimization.
- IoC Container - Auto dependency injection by attributes.
- Global Events - Strongly-typed custom events.
- FSM - Extendable code-based finite statemachine.
- Singleton - MonoBehaviour singleton that provides coroutine support for manager classes.
- Data Structures - Utility data classes that encapsulate commonly-used features.

#### Modules
NekoLib also provides integrated implementations covering some core aspects of game logic.
- UI - Loosely MVC UI system
- Stats - Value modification system
- Movement - Advanced rigidbody movement solution

# Usage
`GameEntry` is a global service locator.
1. Register feature modules by `GameEntry.RegisterModule<T>(module)`.
E.g. Use a MonoBehaviour singleton to register core modules that will be used in the project. `MonoSingleton` instances are `DontDestroyOnLoad` by default.

```Csharp  
    public class GameEngine : MonoSingleton<GameEngine>
    {
        [SerializeField] private SettingsConfig _defaultSettings;
        [SerializeField] private UIConfig _uiConfig;

        protected override void Awake()
        {
            base.Awake();

            // Global object pool.
            var objectPoolManager = new GameObject("Object Pool Manager").AddComponent<ObjectPoolManager>();
            DontDestroyOnLoad(objectPoolManager);
            GameEntry.RegisterModule<IObjectPoolManager>(objectPoolManager);

            // Game settings.
            GameEntry.RegisterModule<Settings>(new Settings(_defaultSettings));

            // UI.
            UIFrame uiFrame = _uiConfig.Setup();
            DontDestroyOnLoad(uiFrame);
            GameEntry.RegisterModule<UIFrame>(uiFrame);
        }

    }
```

2. Use `GameEntry.GetModule<T>()` to access registered modules.
E.g. `BulletFactory` class uses an object pool manager to create pooled bullet instances.

```Csharp
    public static class BulletFactory
    {
        private static IObjectPoolManager _objectPoolManager;

        public static Bullet Instantiate(Bullet prefab, BulletCfg cfg, Vector3 origin, Vector3 direction,
            LayerMask layerMask = default, IBattleActor source = null)
        {
            _objectPoolManager = _objectPoolManager ?? GameEntry.GetModule<IObjectPoolManager>();
            IObjectPool<Bullet> pool = _objectPoolManager.GetPool<Bullet>(prefab);
            Bullet bullet = pool.Get();
            bullet.Init(cfg, origin, direction, layerMask, source, pool);
            return bullet;
        }
    }
```