using System.Collections;
using Core.Unity.Flow;
using Core.Unity.Utils;
using Dev.ComradeVanti.WaitForAnim;
using Game.Audio;
using Game.Input;
using UnityEngine;
using UnityEngine.UI;
using Screen = Core.Unity.Flow.Screen;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Flow
{
    public class MainMenuScreen : GameScreen
    {
        private const string HIDE_ANIM = "MainMenu_Hide";
        private const string SHOW_ANIM = "MainMenu_Show";
        
        [SerializeField]
        private Animator m_panelAnimator = null;

        [SerializeField]
        private GameObject m_buttonPanel = null;

        [SerializeField]
        private GameObject m_settingsPanel = null;

        [SerializeField]
        private GameObject m_creditsPanel = null;

        [SerializeField]
        private Slider m_sfxSlider = null;

        [SerializeField]
        private Slider m_musicSlider = null;

        public override void OnViewEnter(ViewEnterParams viewEnterParams = null)
        {
            base.OnViewEnter(viewEnterParams);
            
            AudioManager.Instance.Play(MusicAudioDatabase.MusicKey.MenuAmbient);

            m_buttonPanel.SetActiveSafe(true);
            m_settingsPanel.SetActiveSafe(false);
            m_creditsPanel.SetActiveSafe(false);

            m_sfxSlider.value = AudioManager.SFXVolume;
            m_musicSlider.value = AudioManager.MusicVolume;
            
            m_panelAnimator.Play(SHOW_ANIM);
        }

        public void _UpdateSFXVolume(float newVolume)
        {
            AudioManager.SFXVolume = newVolume;
        }
        
        public void _UpdateMusicVolume(float newVolume)
        {
            AudioManager.MusicVolume = newVolume;
        }

        public void _PlayGame()
        {
            StartCoroutine(IEnumerator_PlayGame());
        }
        
        private IEnumerator IEnumerator_PlayGame()
        {
            using (new InputManager.InputDisabledScope())
            {
                m_panelAnimator.Play(HIDE_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, HIDE_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, HIDE_ANIM);
                
                FlowManager.Instance.Trigger("BeginGame", LoadingScreens.CHARACTERS);
            }
        }

        public void _QuitGame()
        {
            StartCoroutine(IEnumerator_QuitGame());
        }
        
        private IEnumerator IEnumerator_QuitGame()
        {
            using (new InputManager.InputDisabledScope())
            {
                m_panelAnimator.Play(HIDE_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, HIDE_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, HIDE_ANIM);
                
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    EditorApplication.ExitPlaymode();
                }
#else
            Application.Quit();
#endif
            }
        }

        public void _OpenSettings()
        {
            StartCoroutine(IEnumerator_OpenSettings());
        }

        private IEnumerator IEnumerator_OpenSettings()
        {
            using (new InputManager.InputDisabledScope())
            {
                m_panelAnimator.Play(HIDE_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, HIDE_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, HIDE_ANIM);
                
                m_buttonPanel.SetActiveSafe(false);
                m_creditsPanel.SetActiveSafe(false);
                m_settingsPanel.SetActiveSafe(true);
                
                m_panelAnimator.Play(SHOW_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, SHOW_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, SHOW_ANIM);
            }
        }
        
        public void _OpenCredits()
        {
            StartCoroutine(IEnumerator_OpenCredits());
        }

        private IEnumerator IEnumerator_OpenCredits()
        {
            using (new InputManager.InputDisabledScope())
            {
                m_panelAnimator.Play(HIDE_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, HIDE_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, HIDE_ANIM);
                
                m_buttonPanel.SetActiveSafe(false);
                m_creditsPanel.SetActiveSafe(true);
                m_settingsPanel.SetActiveSafe(false);
                
                m_panelAnimator.Play(SHOW_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, SHOW_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, SHOW_ANIM);
            }
        }
        
        public void _OpenButtons()
        {
            StartCoroutine(IEnumerator_OpenButtons());
        }

        private IEnumerator IEnumerator_OpenButtons()
        {
            using (new InputManager.InputDisabledScope())
            {
                m_panelAnimator.Play(HIDE_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, HIDE_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, HIDE_ANIM);
                
                m_buttonPanel.SetActiveSafe(true);
                m_creditsPanel.SetActiveSafe(false);
                m_settingsPanel.SetActiveSafe(false);
                
                m_panelAnimator.Play(SHOW_ANIM);
                yield return new WaitForAnimationToStart(m_panelAnimator, SHOW_ANIM);
                yield return new WaitForAnimationToFinish(m_panelAnimator, SHOW_ANIM);
            }
        }
    }
}