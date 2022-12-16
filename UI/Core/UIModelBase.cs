using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    public abstract class UIModelBase
    {
        private bool _isStart;

        public void Start()
        {
            OnStart();
        }

        public void ShutDown()
        {
            OnShutDown();
        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnShutDown()
        {

        }
    }
}