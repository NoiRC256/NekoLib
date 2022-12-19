using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    public abstract class TransitionBase : MonoBehaviour
    {
        public abstract void PlayAnimation(Transform tr, Action onComplete);
    }
}