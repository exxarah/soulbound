﻿using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Unity.Flow
{
    public class FlowManager : SceneSingleton<FlowManager>
    {
        [SerializeField]
        private string m_escapePopup;

        [SerializeField]
        [ReadOnly]
        private View m_currentView;

        [SerializeField]
        private View m_initialView;

        [SerializeField]
        private FlowGraph m_graph;

        private Dictionary<string, View> m_loadedViews;
        private Stack<string> m_popups;
        private string m_currentState;

        private UniTask m_transitionTask = UniTask.CompletedTask;
        public string EscapePopup => m_escapePopup;
        public View CurrentView => m_currentView;
        public bool IsTransitioning => !m_transitionTask.Status.IsCompleted();

        private void OnEnable()
        {
            DontDestroyOnLoad(this);

            m_loadedViews = new Dictionary<string, View>();
            m_popups = new Stack<string>();

            // Mark the initial scene as loaded
            m_transitionTask = OnSceneLoaded(m_initialView.gameObject.scene.name, null).Preserve();
        }

        public event Action<View> OnViewLoaded;
        public event Action<View> OnViewUnloaded;

        public UniTask Trigger(string trigger, string loadingScreenToUse = "", ViewEnterParams @params = null)
        {
            m_transitionTask = _Trigger(trigger, loadingScreenToUse, @params).Preserve();
            return m_transitionTask;
        }

        private async UniTask _Trigger(string trigger, string loadingScreenToUse = "", ViewEnterParams @params = null)
        {
            if (!m_graph.TryGetTrigger(trigger, m_currentState, out FlowGraph.FlowTrigger flowTrigger))
            {
                Log.Warning($"[FlowManager] Invalid Trigger[{trigger}]");
                return;
            }

            if (IsTransitioning)
            {
                Log.Warning($"[FlowManager] Ignoring Trigger[{trigger}] because transition is already in progress");
                return;
            }

            await EnableLoadingScreen(loadingScreenToUse);
            string previousState = m_currentState;

            // Check if this scene has already been loaded, and just re-load that if so
            if (m_loadedViews.TryGetValue(flowTrigger.TargetState, out View view))
            {
                view.gameObject.SetActive(true);
                await OnSceneLoaded(flowTrigger.TargetState, @params);
                return;
            }

            FlowGraph.FlowState flowState = m_graph.GetState(flowTrigger.TargetState);
            await SceneManager.LoadSceneAsync(flowState.Scene.sceneIndex, LoadSceneMode.Additive);
            await OnSceneLoaded(flowTrigger.TargetState, @params);
            
            // Now that the new screen is loaded, we can unload the previous one. Otherwise not having a loading screen causes a broken transition
            await CloseAsync(previousState);
            await DisableLoadingScreen(loadingScreenToUse);
        }

        private void Close(string state)
        {
            // No need for any checks here, we can just assume it's fine to close a view
            CloseAsync(state).Forget();
        }

        private async UniTask CloseAsync(string state)
        {
            await OnSceneDisabled(state);

            FlowGraph.FlowState flowState = m_graph.GetState(state);
            if (flowState == null) { return; }

            if (flowState.UnloadOnExit)
            {
                if (m_loadedViews.ContainsKey(state))
                {
                    m_loadedViews.Remove(state);
                }
                await SceneManager.UnloadSceneAsync(flowState.Scene.sceneIndex);
            }
        }

        public bool ClosePopup()
        {
            if (m_popups.Count == 0)
            {
                Log.Info("[FlowManager] There are no popups to close");
                return false;
            }

            Close(m_popups.Pop());
            return true;
        }

        private async UniTask OnSceneLoaded(string state, ViewEnterParams @params)
        {
            // Pass through the previous screen to the params
            if (@params == null)
            {
                @params = new ViewEnterParams { PreviousScreen = state };
            }
            else
            {
                @params.PreviousScreen = state;
            }

            FlowGraph.FlowState flowState = m_graph.GetState(state);
            Scene scene = SceneManager.GetSceneByBuildIndex(flowState.Scene.sceneIndex);
            View view = scene.GetRootGameObjects()[0].GetComponent<View>();
            switch (view)
            {
                case Popup _:
                    m_popups.Push(state);
                    break;
                case Screen _:
                    m_currentView = view;
                    if (scene.isLoaded) SceneManager.SetActiveScene(scene);
                    break;
            }

            m_loadedViews.TryAdd(state, view);
            m_currentState = state;

            await view.OnViewPreEnter(@params);

            view.OnViewEnter(@params);
            OnViewLoaded?.Invoke(view);
        }

        private async UniTask OnSceneDisabled(string state)
        {
            if (!m_loadedViews.TryGetValue(state, out View view)) return;

            await view.OnViewPreExit();

            view.OnViewExit();
            OnViewUnloaded?.Invoke(view);
            view.gameObject.SetActive(false);
        }

        private async UniTask EnableLoadingScreen(string screenName)
        {
            FlowGraph.LoadingScreenOption loadingScreen = m_graph.GetLoadingScreen(screenName);
            if (loadingScreen != null)
            {
                await SceneManager.LoadSceneAsync(loadingScreen.Scene.sceneIndex, LoadSceneMode.Additive);
                Scene scene = SceneManager.GetSceneByBuildIndex(loadingScreen.Scene.sceneIndex);
                LoadingScreen screen = scene.GetRootGameObjects()[0].GetComponent<LoadingScreen>();
                await screen.OnLoadBegin();
            }
        }

        private async UniTask DisableLoadingScreen(string screenName)
        {
            FlowGraph.LoadingScreenOption loadingScreen = m_graph.GetLoadingScreen(screenName);
            if (loadingScreen != null)
            {
                Scene scene = SceneManager.GetSceneByBuildIndex(loadingScreen.Scene.sceneIndex);
                LoadingScreen screen = scene.GetRootGameObjects()[0].GetComponent<LoadingScreen>();
                await screen.OnLoadEnd();
                if (scene.isLoaded)
                {
                    await SceneManager.UnloadSceneAsync(loadingScreen.Scene.sceneIndex);
                }
            }
        }
    }
}