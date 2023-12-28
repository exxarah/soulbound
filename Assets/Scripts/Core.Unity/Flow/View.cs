using System;
using Core.Unity.Localisation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Unity.Flow
{
    public class ViewEnterParams
    {
        public string PreviousScreen;
    }

    public abstract class View : MonoBehaviour
    {
        [SerializeField]
        private bool m_overrideText = true;
        
        public virtual bool SetActive { get; } = false;


        private void Awake()
        {
            if (m_overrideText) OverrideText();
        }

        public abstract UniTask OnViewPreEnter(ViewEnterParams viewEnterParams = null);
        public abstract void OnViewEnter(ViewEnterParams viewEnterParams = null);
        public abstract UniTask OnViewPreExit();
        public abstract void OnViewExit();
        public abstract void Back();

        private void OverrideText()
        {
            foreach (Text text in GetComponentsInChildren<Text>(true))
            {
                if (text.gameObject.GetComponent<DoNotLocalise>()) { continue; }
                text.text = "---";
            }

            foreach (TMP_Text text in GetComponentsInChildren<TMP_Text>(true))
            {
                if (text.gameObject.GetComponent<DoNotLocalise>()) { continue; }
                text.text = "---";
            }

            foreach (LocaliseText text in GetComponentsInChildren<LocaliseText>(true)) text.Refresh();
        }
    }

    public abstract class Screen : View
    {
        private string m_previousScreen = String.Empty;

        public override UniTask OnViewPreEnter(ViewEnterParams viewEnterParams = null)
        {
            return UniTask.CompletedTask;
        }

        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            m_previousScreen = viewEnterParams?.PreviousScreen;
        }

        public override UniTask OnViewPreExit()
        {
            return UniTask.CompletedTask;
        }

        public override void OnViewExit()
        {
        }

        public override void Back()
        {
            if (FlowManager.Instance.ClosePopup()) return;
            if (String.IsNullOrEmpty(m_previousScreen))
                FlowManager.Instance.Trigger(FlowManager.Instance.EscapePopup);
            else
                FlowManager.Instance.Trigger(m_previousScreen);
        }
    }

    public abstract class Popup : View
    {
        public override UniTask OnViewPreEnter(ViewEnterParams viewEnterParams = null)
        {
            return UniTask.CompletedTask;
        }

        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
        }

        public override UniTask OnViewPreExit()
        {
            return UniTask.CompletedTask;
        }

        public override void OnViewExit()
        {
        }

        public override void Back()
        {
            FlowManager.Instance.ClosePopup();
        }
    }
}