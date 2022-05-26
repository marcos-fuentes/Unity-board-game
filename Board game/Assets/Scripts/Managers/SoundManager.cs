using System;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        public GameObject ObjectMusic;

        private AudioSource backgroundMusic;

        //Sound effects
        public AudioSource attackSoundEffect;
        public AudioSource hurtSoundEffect;
        public AudioSource walkSoundEffect;
        public AudioSource deadSoundEffect;
        public AudioSource explosionSoundEffect;
        public AudioSource healSoundEffect;
        public AudioSource victoySoundMusic;

        public Slider musicSlider;
        public Slider effectSlider;

        private float MusicVolume = 1f;
        private float EffectsVolume = 1f;

        private const string EFFECT_VOLUME = "EffectVolume";
        private const string BACKGROUND_VOLUME = "BackgroundVolume";


        private void Awake() => Instance = this;

        private void Start()
        {
            ObjectMusic = GameObject.FindWithTag("GameMusic");
            backgroundMusic = ObjectMusic.GetComponent<AudioSource>();


            ChangeAudioSourceVolume(backgroundMusic, PlayerPrefs.GetFloat(BACKGROUND_VOLUME, MusicVolume));
            ChangeAudioSourceVolume(victoySoundMusic, PlayerPrefs.GetFloat(BACKGROUND_VOLUME, MusicVolume));
            ChangeAudioSourceVolume(attackSoundEffect, PlayerPrefs.GetFloat(EFFECT_VOLUME, EffectsVolume));
            ChangeAudioSourceVolume(hurtSoundEffect, PlayerPrefs.GetFloat(EFFECT_VOLUME, EffectsVolume));
            ChangeAudioSourceVolume(walkSoundEffect, PlayerPrefs.GetFloat(EFFECT_VOLUME, EffectsVolume));
            ChangeAudioSourceVolume(deadSoundEffect, PlayerPrefs.GetFloat(EFFECT_VOLUME, EffectsVolume));
            ChangeAudioSourceVolume(explosionSoundEffect, PlayerPrefs.GetFloat(EFFECT_VOLUME, EffectsVolume));
            ChangeAudioSourceVolume(healSoundEffect, PlayerPrefs.GetFloat(EFFECT_VOLUME, EffectsVolume));

            musicSlider.value = PlayerPrefs.GetFloat(BACKGROUND_VOLUME, MusicVolume);
            effectSlider.value = PlayerPrefs.GetFloat(EFFECT_VOLUME, EffectsVolume);

            musicSlider.onValueChanged.AddListener(delegate { MusicVolumeChanged(); });
            effectSlider.onValueChanged.AddListener(delegate { EffectVolumeChanged(); });
        }


        public void setBackgroundVolume(float volume)
        {
            if (backgroundMusic != null) backgroundMusic.volume = volume;
        }

        public void playBackground()
        {
            if (backgroundMusic != null) backgroundMusic.Play();
        }

        public void setattackVolume(float volume)
        {
            if (attackSoundEffect != null) attackSoundEffect.volume = volume;
        }

        public void playAttackSound()
        {
            if (attackSoundEffect != null) attackSoundEffect.Play();
            else Debug.Log("Null attack sound");
        }

        public void sethurtVolume(float volume)
        {
            if (hurtSoundEffect != null) hurtSoundEffect.volume = volume;
        }

        public void playHurtSound()
        {
            if (hurtSoundEffect != null) hurtSoundEffect.Play();
            else Debug.Log("Null hurt sound");
        }

        public void setexplosionVolume(float volume)
        {
            if (explosionSoundEffect != null) explosionSoundEffect.volume = volume;
        }

        public void playExplosionSound()
        {
            if (explosionSoundEffect != null) explosionSoundEffect.Play();
            else Debug.Log("Null explosion sound");
        }

        public void setdeadVolume(float volume)
        {
            if (deadSoundEffect != null) deadSoundEffect.volume = volume;
        }

        public void playDeadSound()
        {
            if (deadSoundEffect != null) deadSoundEffect.Play();
            else Debug.Log("Null dead sound");
        }

        public void sethealVolume(float volume)
        {
            if (healSoundEffect != null) healSoundEffect.volume = volume;
        }

        public void playHealSound()
        {
            if (healSoundEffect != null) healSoundEffect.Play();
            else Debug.Log("Null heal sound");
        }

        public void setwalkVolume(float volume)
        {
            if (walkSoundEffect != null) walkSoundEffect.volume = volume;
        }

        public void playWalkSound()
        {
            if (walkSoundEffect != null) walkSoundEffect.Play();
            else Debug.Log("Null walk sound");
        }

        public void MusicVolumeChanged()
        {
            PlayerPrefs.SetFloat(BACKGROUND_VOLUME, musicSlider.value);
            ChangeAudioSourceVolume(backgroundMusic, musicSlider.value);
            ChangeAudioSourceVolume(victoySoundMusic, musicSlider.value);
        }

        public void EffectVolumeChanged()
        {
            PlayerPrefs.SetFloat(EFFECT_VOLUME, musicSlider.value);

            ChangeAudioSourceVolume(attackSoundEffect, effectSlider.value);
            ChangeAudioSourceVolume(hurtSoundEffect, effectSlider.value);
            ChangeAudioSourceVolume(walkSoundEffect, effectSlider.value);
            ChangeAudioSourceVolume(walkSoundEffect, effectSlider.value);
            ChangeAudioSourceVolume(deadSoundEffect, effectSlider.value);
            ChangeAudioSourceVolume(explosionSoundEffect, effectSlider.value);
            ChangeAudioSourceVolume(healSoundEffect, effectSlider.value);
        }

        private void ChangeAudioSourceVolume(AudioSource audioSource, float volume)
        {
            if (audioSource != null) audioSource.volume = volume;
        }

        public void PlayVictoryMusic()
        {
            if (victoySoundMusic != null)
            {
                backgroundMusic.Stop();
                victoySoundMusic.Play();
            }
            else Debug.Log("Null walk sound");
        }
    }
}