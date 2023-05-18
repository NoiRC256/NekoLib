# Overview

NekoLib is a collection of useful tools and features that can help maintain a clean and flexible architecture. Implement and extend logic with relative ease.

#### Core

- Modules - Global service locator.
- Pooling - Generic object pooling with dynamic optimization.
- Events - Strongly-typed custom events.
- Scriptable Events - Scriptable object events for easy conguration.
- FSM - Extendable code-based finite statemachine.
- IoC Container - Auto dependency injection by attributes.
- Singleton - MonoBehaviour singletons with Coroutine support.
- Data Structures - Utility data classes that encapsulate commonly-used features.
- Math - Helper methods for 3D maths
- Physics - Helper methods for Unity physics

#### Kits
Out-of-the-box implementations for some aspects of game logic
- UI - Lightweight MVC UI framework (Work In Progress).
- Movement - Robust character movement solution with predictive damping.
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

## Setting up a framework for your project
The `Modules` and `Pooling` features can be useful for building a project framework.

### Modules
`Modules` is a global service locator. It can be used to centrally initialize and manage stateful objects you want global access to. 

The term "module" here is the equivalent of "service", but used exclusively to differentiate with network-related remote services a project might use.

You could register anything as a module using `Modules.Register<T>(obj)` at the start. This enables you to use `Modules.Get<T>()` from anywhere in the project to access the registered object.

<details>
  <summary>Example</summary>

In the initial scene, we have a singleton MonoBehaviour `GameEngine` which will register global objects that will be used in the project.

```Csharp  
    // Main game manager singleton.
    [DefaultExecutionOrder(-1)]
    public class GameEngine : MonoBehaviour
    {
        [SerializeField] private UIRoot _uiRoot;
        [SerializeField] private Rewired.InputManager _inputManagerPrefab;

        protected override void Awake()
        {
            base.Awake();

            // Registers an existing MonoBehaviour to make it globally accessible.
            Modules.Register<UIRoot>(_uiRoot);

            // Registers a MonoBehaviour from a manually instantiated instance.
            Modules.RegisterModule<Rewired.InputManager>(
                GameObject.Instantiate(_inputManagerPrefab)
                );
   
            // Registers a MonoBehaviour to an interface.
            // Because no instance is provided,
            // automatically instantiates a new gameobject for it.
            Modules.RegisterModule<IObjectPoolManager, ObjectPoolManager>();

            // Registers self.
            Modules.Register<GameEngine>(this);
        }

    }
```
</details>

### Object Pool Manager
`ObjectPoolManager` centrally manages object pools.

#### Getting a Pool
You can obtain a pool automatically by the following ways:

- `GetPool<T>()` to get or create a pool for a specific type by. 

- `GetPool<T>(T obj)` to get or create a pool for a specific prefab by where `T` is a Unity component.  

Both methods will return a `IObjectPool<T>`.

#### Creating a Pool
When you try to get a pool for the first time, the corresponding pool will be automatically created and registered. 

Alternatively, you could manually create and register pools by `RegisterPool<T>()` and `RegisterPool<T>(T obj)`.

#### Using a Pool
With a `IObjectPool pool`

- `pool.Get()` to obtain a pooled object

- `pool.Release(T obj)` to return an object back into the pool.

<details>
  <summary>Example</summary>

A global object pool manager has already been created by `Modules.RegisterModule<IObjectPoolManager, ObjectPoolManager>()`.

Here, `BulletFactory` class uses an object pool manager to create pooled bullet instances from a prefab.

`Bullet` is a MonoBehaviour component attached to the root gameobject of a bullet prefab.

```Csharp
    public static class BulletFactory
    {
        ObjectPoolManager PoolManager => Modules.GetModule<IObjectPoolManager>();

        public static Bullet Instantiate(Bullet prefab, BulletConfig cfg,
        Vector3 origin, Vector3 direction, LayerMask layerMask = default)
        {
            IObjectPool<Bullet> pool = PoolManager.GetPool(prefab);
            Bullet bullet = pool.Get();
            bullet.Init(cfg, origin, direction, layerMask, pool);
            return bullet;
        }
    }
```
</details>