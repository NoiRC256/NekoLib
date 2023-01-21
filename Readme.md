# Overview

NekoLib encapsulates a range of features that can be leveraged to optimize the game development workflow. 

NekoLib is not a framework, and does not enforce specific ways of doing things. It's a toolbox that can help maintain a clean and flexible architecture, so that you can implement and extend logic with relative ease.

#### Core
- Pooling - Generic object pooling with capacity optimization and non-invasive integration.
- IoC Container - Auto dependency injection by attributes.
- Global Events - Strongly-typed custom events.
- FSM - Extendable code-based finite statemachine.
- Singleton - MonoBehaviour singleton that provides coroutine support for manager classes.
- Data Structures - Utility data classes that encapsulate commonly-used features.

#### Modules
NekoLib also provides integrated implementations covering some aspects of game logic.
- UI - Loosely MVC UI framework
- Movement - Robust rigidbody movement solution
- Stats - Non-destructive value modification system
- Playables - Custom behavior system for Unity PlayableGraph

# Quick Start

### Installation

<details>
  <summary>Instructions</summary>

#### Installing via Git URL 
(soon)
#### Installing the old way
Place the source files into your project's assets folder.
</details>


### Usage
`GameEntry` is a global service locator. You can create your own entry point and register game services using `GameEntry.RegisterModule<T>(module)`.

E.g. Use a MonoBehaviour singleton to register global modules that will be used in the project. `MonoSingleton` instances are `DontDestroyOnLoad` by default.

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

Use `GameEntry.GetModule<T>()` to access registered services.

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