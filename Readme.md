# Overview

NekoLib is a collection of useful utilities for gameplay programming.

### Key Features

- Pick and choose which features to use.
- Strong separation of editor and runtime code
- Does not rely on any 3rd party assets

#### Core

<details><summary><b>ReactiveProps</b> - Data classes with property binding</summary>

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

<details><summary><b>Events</b> - Strongly-typed custom events</summary>

```csharp
var event = GlobalEvents.Get<LevelSucceedEvt>();
event.Action += OnLevelSucceed;
```

</details>

<details><summary><b>ScriptableEvents</b> - Scriptable objects that invoke events</summary>

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

<details><summary><b>FSM</b> - Extendable code-based finite state machines</summary>

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

#### Modules

<details><summary><b>Stats</b> - Reactive gameplay attributes system.</summary>

```csharp
[System.Serializable]
public class AvatarStatGroup : StatGroup<AvatarStatType>
{
    public enum StatType {
        MoveSpeed,
        KineticResistance,
        BlastResistance,
        MeleeResistance,
        HealthMax,
        Health,
        ShieldMax,
        Shield,
    }

    public Stat MoveSpeed = new Stat();
    public Stat KineticResistance = new Stat();
    public Stat BlastResistance = new Stat();
    public Stat MeleeResistance = new Stat();
    public Stat HealthMax = new Stat();
    public Stat Health = new Stat();
    public Stat ShieldMax = new Stat();
    public Stat Shield = new Stat();

    public void Init(AvatarData avatarData) {
        base.Init();
        RegisterLowerBoundedStat(MoveSpeed, StatType.MoveSpeed, avatarData.MoveSpeed);
        RegisterStat(KineticResistance, StatType.KineticResistance, avatarData.KineticResistance);
        RegisterStat(BlastResistance, StatType.BlastResistance, avatarData.BlastResistance);
        RegisterStat(MeleeResistance, StatType.MeleeResistance, avatarData.MeleeResistance);
        RegisterStat(HealthMax, StatType.HealthMax, avatarData.HealthMax);
        RegisterResourceStat(Health, StatType.Health, avatarData.Health, HealthMax);
        RegisterStat(ShieldMax, StatType.ShieldMax, avatarData.ShieldMax);
        RegisterResourceStat(Shield, StatType.Shield, avatarData.Shield, ShieldMax);
    }
}
```

</details>

### Installation

<details>
  <summary>Instructions</summary>

#### Installing via Git URL

(soon)

#### Installing the old way

Place the source files into your project's assets folder.

</details>
