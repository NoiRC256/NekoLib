# Overview

NekoLib encapsulates a range of features that can be leveraged to optimize the game development workflow. 

NekoLib is not a framework, and does not enforce specific ways of doing things. It's a toolbox of useful features that can help maintain a clean and flexible architecture, so that you can implement and extend logic with relative ease.

#### Base Services
- GameEntry - Global service locator.
- ObjectPoolManager - Object pooling service with capacity optimization.

#### Core
- Pooling - Generic object pooling with easy integration.
- IoC Container - Auto dependency injection by attributes.
- Global Events - Strongly-typed custom events.
- FSM - Extendable code-based finite statemachine.
- Singleton - MonoBehaviour singleton that provides coroutine support for manager classes.
- Data Structures - Utility data classes that encapsulate commonly-used features.

#### Kits
NekoLib also provides integrated implementations covering some aspects of game logic.
- UI - Loosely-MVC UI framework (WIP).
- Movement - Robust rigidbody movement solution.
- Stats - Non-destructive value modification system.
- Playables - Custom behavior system for Unity PlayableGraph.

### Installation

<details>
  <summary>Instructions</summary>

#### Installing via Git URL 
(soon)
#### Installing the old way
Place the source files into your project's assets folder.
</details>

# Quick Start

## Base Services Usage

### GameEntry
`GameEntry` is a global service locator. You can create your own entry point and register game services using `GameEntry.RegisterModule<T>(module)`. Then, you can 
Use `GameEntry.GetModule<T>()` to access registered services.

<details>
  <summary>Example</summary>

Use a MonoBehaviour singleton to register global modules that will be used in the project. `MonoSingleton` instances are `DontDestroyOnLoad` by default.

```Csharp  
    // Entry point of the game. Initializes global game modules.
    [DefaultExecutionOrder(-1)]
    public class GameEngine : MonoSingleton<GameEngine>
    {
        [SerializeField] private GameConfig _gameConfig;

        protected override void Awake()
        {
            base.Awake();

            // Global object pool manager.
            GameEntry.RegisterModule<IObjectPoolManager, ObjectPoolManager>();

            // Register your own services below.

            // Input.
            GameEntry.RegisterModule<Rewired.InputManager>(GameObject.Instantiate(_gameConfig.InputManager));

            // Game settings.
            GameEntry.RegisterModule<Settings>(new Settings(_gameConfig.DefaultSettingsProfile, _gameConfig.SettingsConfig));

            // UI.
            GameEntry.RegisterModule<UIFrame>(_gameConfig.UIConfig.CreateUIFrame());

            // Camera.
            GameEntry.RegisterModule<Camera>(GameObject.Instantiate(_gameConfig.MainCamera));
        }

    }
```
</details>

### Object Pool Manager

`ObjectPoolManager` keeps track of all registered object pools. You can obtain a pool for a specific type by `GetPool<T>()`, or obtain a pool for a specific prefab by `GetPool<T>(T obj) where T : Component`. Both methods will return a `IObjectPool<T>`. 

The corresponding pool will be automatically registered upon the first `GetPool` call. You can also manually register your pools by `RegisterPool<T>()` and `RegisterPool<T>(T obj)`.

With a `IObjectPool pool`, use `pool.Get()` to obtain a pooled object, and `pool.Release(T obj)` to return an object back into the pool.

<details>
  <summary>Example</summary>

A default object pool manager service has been registered by `GameEntry.RegisterModule<IObjectPoolManager, ObjectPoolManager>()`.

Here `BulletFactory` class uses an object pool manager to create pooled bullet instances. `Bullet` is a MonoBehaviour script attached to the root gameobject of each bullet prefab.

```Csharp
    public static class BulletFactory
    {
        private static IObjectPoolManager _objectPoolManager;

        public static Bullet Instantiate(Bullet prefab, BulletCfg cfg, Vector3 origin, Vector3 direction,
            LayerMask layerMask = default, IBattleActor source = null)
        {
            _objectPoolManager = _objectPoolManager ?? GameEntry.GetModule<IObjectPoolManager>();

            IObjectPool<Bullet> pool = _objectPoolManager.GetPool(prefab);
            Bullet bullet = pool.Get();
            bullet.Init(cfg, origin, direction, layerMask, source, pool);
            return bullet;
        }
    }
```
</details>