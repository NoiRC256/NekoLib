# Overview

NekoLib provides some useful features you might see in game frameworks.

### Key Features

- Modular - Pick and choose the features you want to use
- Lightweight - GC-friendly solutions with low performance impact
- Easy integration - Does not rely on any 3rd party assets

#### Core

<details><summary><b>ReactiveProps</b> - Subscribe to value change events on data classes</summary>

```csharp
public class UIScoreController : MonoBehaviour {
    [SerializeField] private UIScoreView _view;
    [SerializeField] private PlayerContext _context;

    void OnEnable() {
        _context.Score.ValueChanged += SetScoreText;
    }

    void OnDisable() {
        _context.Score.ValueChanged -= SetScoreText;
    }

    private void SetScoreText(int score) {
        _view.text.SetText(score.ToString());
    }
}
```

</details>

<details><summary>
<b>Pool</b> - Dynamically optimized type safe object pooling
</summary>

```csharp
public static class BulletFactory {
    public static IObjectPoolManager PoolManager;

    public static Bullet Instantiate(Bullet prefab, BulletConfig cfg,
    Vector3 origin, Vector3 direction, LayerMask layerMask = default) {
        var pool = PoolManager.GetPool(prefab);
        Bullet bullet = pool.Get();
        bullet.Init(cfg, origin, direction, layerMask, pool);
        return bullet;
    }
}

public class Bullet : MonoBehaviour {
    ...
    Destroy(){
        if(isPooled) _pool.Push(this);
    }
}
```

</details>

<details><summary><b>Events</b> - Typed custom events</summary>

```csharp
var event = GlobalEvents.Get<LevelSucceedEvt>();
event.Action += HandleLevelSucceed;
```

</details>

<details><summary><b>ScriptableEvents</b> - Use Scriptable Objects as events</summary>

```csharp
public class MyEventListener: MonoBehaviour {
    [SerializeField] ScriptableEventInt _scriptableEvent;

    void OnEnable(){
        _scriptableEvent.Register(MyEventResponse);
    }

    void OnDisable(){
        _scriptableEvent.Unregister(MyEventResponse);
    }

    void MyEventResponse(int value) {
        ...
    }
}
```

</details>

<details><summary><b>FSM</b> - Extendable code-based finite state machine</summary>

```csharp
fsm = new FSMBase<Player>(player);

var idle = new FSMState<Player>();
idle.BindEnterAction(p => p.Idle());
fsm.AddState("Idle", idle);
fsm.SetDefault("Idle");

var move = new FSMState<Player>();
move.BindUpdateAction(p => p.Move(p.MoveInput));
fsm.AddState("Move", move);

var jump = new FSMState<Player>();
jump.BindEnterAction(p => p.Jump());
fsm.AddState("Jump", jump);

var hasMoveInput = new FSMCondition<Player>(p => p.MoveInput.sqrMagnitude > 0.01f);
var hasJumpInput = new FSMCondition<Player>(p => p.JumpInput);
var isJumpAnimEnded = new FSMCondition<Player>(p => p.IsAnimEnded("Jump"));

idle.AddCondition(hasJumpInput, "Jump");
idle.AddCondition(hasMoveInput, "Move");
move.AddCondition(hasJumpInput, "Jump");
move.AddCondition(!hasMoveInput, "Idle");
jump.AddCondition(isJumpAnimEnded, "Idle");
```

</details>

<details><summary><b>Singletons</b> - Simple singleton templates</summary>

```csharp
public class GameManager : MonoSingleton<GameManager> {
    Awake() {
        ...
    }
}

var GameManager gm = GameManager.Instance;
```

</details>

<details><summary><b>ServiceLocator</b> - Global service locator</summary>

```csharp
public class GameEntry : MonoBehaviour {
    void Awake() {
        GameServices.Register<IMyServiceInterface>(new MyServiceType());
        ...
    }
}

var myService = GameServices.Get<IMyServiceInterface>();
```

</details>

**Math** - Utility methods for 3D maths

**Physics** - Utility methods for Unity physics

### Installation

<details>
  <summary>Instructions</summary>

#### Installing via Git URL

(soon)

#### Installing the old way

Place the source files into your project's assets folder.

</details>
