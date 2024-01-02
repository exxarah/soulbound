using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace Core.Unity.Flow
{
    [CreateAssetMenu(fileName="FlowGraph", menuName="Flow/New Flow Graph")]
    public class FlowGraph : ScriptableObject
    {
        [Serializable]
        public class FlowState
        {
            [SerializeField]
            private string m_stateName;
            public string StateName => m_stateName;

            [SerializeField]
            private SceneReference m_scene;
            public SceneReference Scene => m_scene;

            [SerializeField]
            private bool m_unloadOnExit = false;
            public bool UnloadOnExit => m_unloadOnExit;

            [SerializeField]
            private bool m_isPopup = false;
            public bool IsPopup => m_isPopup;

            [SerializeField]
            private List<FlowTrigger> m_triggers = new();

            public bool TryGetTrigger(string trigger, out FlowTrigger flowTrigger)
            {
                flowTrigger = m_triggers.Find(flowTrigger => flowTrigger.Trigger == trigger);
                return flowTrigger != null;
            }
        }

        [Serializable]
        public class FlowTrigger
        {
            [SerializeField]
            private string m_trigger;
            public string Trigger => m_trigger;

            [SerializeField]
            private string m_targetState;
            public string TargetState => m_targetState;
        }

        [Serializable]
        public class LoadingScreenOption
        {
            [SerializeField]
            private string m_name;
            public string Name => m_name;

            [SerializeField]
            private SceneReference m_scene;
            public SceneReference Scene => m_scene;
        }

        [SerializeField]
        private List<FlowTrigger> m_globalTriggers = new();
        private IReadOnlyDictionary<string, int> m_globalTriggerLookup;

        [SerializeField]
        private List<FlowState> m_screens = new();
        
        [SerializeField]
        private List<LoadingScreenOption> m_loadingScreens = new();

        private void OnEnable()
        {
            // Add all the global triggers to a lookup
            Dictionary<string, int> globalTriggers = new();
            for (int i = 0; i < m_globalTriggers.Count; i++)
            {
                globalTriggers[m_globalTriggers[i].Trigger] = i;
            }
            m_globalTriggerLookup = globalTriggers;
        }

        public bool TryGetTrigger(string trigger, string currentState, out FlowTrigger flowTrigger)
        {
            if (m_globalTriggerLookup.ContainsKey(trigger))
            {
                flowTrigger = m_globalTriggers[m_globalTriggerLookup[trigger]];
                return true;
            }

            FlowState state = GetState(currentState);
            if (state != null)
            {
                return state.TryGetTrigger(trigger, out flowTrigger);
            }

            flowTrigger = null;
            return false;
        }

        public LoadingScreenOption GetLoadingScreen(string screen)
        {
            if (m_loadingScreens.Count <= 0)
            {
                return null;
            }

            LoadingScreenOption loadingScreen = m_loadingScreens.Find(option => option.Name == screen);
            return loadingScreen;
        }

        public FlowState GetState(string state)
        {
            return m_screens.Find((flowState => flowState.StateName == state));
        }
    }
}