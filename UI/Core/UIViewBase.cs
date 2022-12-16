using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nep.UI
{
    /// <summary>
    /// A UI view holds references to gameobjects and components that make up the UI.
    /// </summary>
    public class UIViewBase
    {
        public UIAnimatorBase AnimIn { get; set; }
        public UIAnimatorBase AnimOut { get; set; }

        public void Init(Transform holder)
        {
            OnInit(holder);
        }

        protected virtual void OnInit(Transform holder)
        {

        }
    }
}