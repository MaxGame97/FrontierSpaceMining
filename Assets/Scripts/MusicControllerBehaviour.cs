using UnityEngine;
using System.Collections;

public class MusicControllerBehaviour : MonoBehaviour {

    [SerializeField] private AudioClip[] calmMusicClips;
    [SerializeField] private AudioClip[] battleMusicClips;

    [SerializeField] private float minDurationBetweenCalmMusic = 60f;
    [SerializeField] private float randomDurationBetweenCalmMusic = 60f;

    [Space(2f)]

    [SerializeField] private float musicFadeTime = 2f;
    [SerializeField] private float battleCooldownTime = 15f;

    [Space(2f)]

    [SerializeField] private float calmMusicVolume = 0.4f;
    [SerializeField] private float battleMusicVolume = 0.55f;

    private AudioSource audioSource;

    private State musicState;
    private MuteState muteState;
    private CalmState calmState;
    private BattleState battleState;

    // State used when no music is being played
    private class MuteState : State
    {
        MusicControllerBehaviour musicController;

        // The time it takes for a calm music clip to start playing
        float currentDurationBetweenCalmMusic;

        public MuteState(MusicControllerBehaviour musicController)
        {
            this.musicController = musicController;
        }

        public override void Entry()
        {
            // Make sure that no audio is being played
            musicController.audioSource.Stop();

            // Make sure that no audio clip is set
            musicController.audioSource.clip = null;

            // Generate the calm music timer
            currentDurationBetweenCalmMusic = musicController.minDurationBetweenCalmMusic + Random.Range(0f, musicController.randomDurationBetweenCalmMusic);
        }

        public override void Update()
        {
            // If the calm music timer has expired, exit to the calm music state
            if (currentDurationBetweenCalmMusic < 0)
                Exit(musicController.calmState);
            // If not, decrease the calm music timer
            else
                currentDurationBetweenCalmMusic -= Time.unscaledDeltaTime;
        }

        public override void Exit(State exitState)
        {
            musicController.musicState = exitState;
            musicController.musicState.Entry();
        }
    }

    private class CalmState : State
    {
        MusicControllerBehaviour musicController;

        public CalmState(MusicControllerBehaviour musicController)
        {
            this.musicController = musicController;
        }

        public override void Entry()
        {
            // Set a random calm music clip and play it
            musicController.audioSource.clip = musicController.calmMusicClips[Random.Range(0, musicController.calmMusicClips.Length)];
            musicController.audioSource.Play();

            // Reset the audio source volume to the defined calm music volume
            musicController.audioSource.volume = musicController.calmMusicVolume;
        }

        public override void Update()
        {
            // If the calm music has stopped playing, exit to the mute state
            if (!musicController.audioSource.isPlaying)
            {
                Exit(musicController.muteState);
            }
        }

        public override void Exit(State exitState)
        {
            musicController.musicState = exitState;
            musicController.musicState.Entry();
        }
    }

    private class BattleState : State
    {
        MusicControllerBehaviour musicController;

        // The queued audio clip (the clip that plays after 
        AudioClip queuedAudioClip;

        float cooldownTime;

        public BattleState(MusicControllerBehaviour musicController)
        {
            this.musicController = musicController;
        }

        public override void Entry()
        {
            // Randomly assign a queued audio clip
            queuedAudioClip = musicController.battleMusicClips[Random.Range(0, musicController.battleMusicClips.Length)];

            // If there is no audio being played
            if(!musicController.audioSource.isPlaying)
            {
                // Set and play the queued audio clip
                musicController.audioSource.clip = queuedAudioClip;
                musicController.audioSource.Play();

                // Set the audio source to loop the battle music
                musicController.audioSource.loop = true;

                // Reset the volume to the defined battle music volume
                musicController.audioSource.volume = musicController.battleMusicVolume;

                // Set the cooldown time to the defined battle cooldown time
                cooldownTime = musicController.battleCooldownTime;
            }
            // If there is already battle music being played
            else if (CheckIfBattleMusicIsPlaying())
            {
                // Reset the volume to the defined battle music volume
                musicController.audioSource.volume = musicController.battleMusicVolume;

                // Set the cooldown time to the defined battle cooldown time
                cooldownTime = musicController.battleCooldownTime;
            }
        }

        public override void Update()
        {
            // If battle music isn't being played
            if (!CheckIfBattleMusicIsPlaying())
            {
                // Fade out the other music
                FadeOutOtherMusic();
            }
            // If battle music is being played
            else
            {
                // If the cooldown time has expired
                if (cooldownTime <= 0)
                {
                    // Fade out the battle music
                    FadeOutBattleMusic();
                }
                // If not, decrease the cooldown time
                else
                    cooldownTime -= Time.unscaledDeltaTime;
            }
        }

        public override void Exit(State exitState)
        {
            musicController.musicState = exitState;
            musicController.musicState.Entry();
        }

        // Fades out the other music and then starts playing the battle music
        void FadeOutOtherMusic()
        {
            // If the audio source's volume is above 0
            if (musicController.audioSource.volume > 0)
                // Decrease it based on the defined music fade time
                musicController.audioSource.volume -= Time.unscaledDeltaTime / (musicController.musicFadeTime / 4);
            // If not, start playing the battle music clip
            else
            {
                // Set and play the queued audio clip
                musicController.audioSource.clip = queuedAudioClip;
                musicController.audioSource.Play();

                // Set the audio source to loop the battle music
                musicController.audioSource.loop = true;

                // Reset the volume to the defined battle music volume
                musicController.audioSource.volume = musicController.battleMusicVolume;

                // Set the cooldown time to the defined battle cooldown time
                cooldownTime = musicController.battleCooldownTime;
            }
        }

        // Fades out the battle music and exits to the mute state
        void FadeOutBattleMusic()
        {
            // If the audio source's volume is above 0
            if (musicController.audioSource.volume > 0)
                // Decrease it based on the defined music fade time
                musicController.audioSource.volume -= Time.unscaledDeltaTime / musicController.musicFadeTime;
            // If not, exit to the mute state
            else
            {
                // Make the audio source stop looping audio clips
                musicController.audioSource.loop = false;

                // Exit to the mute state
                Exit(musicController.muteState);
            }
        }

        // Checks if the audio clip is being played is a battle audio clip
        bool CheckIfBattleMusicIsPlaying()
        {
            // Check all battle audio clips
            for(int i = 0; i < musicController.battleMusicClips.Length; i++)
            {
                // If the current track matches the current audio clip, return true
                if (musicController.audioSource.clip == musicController.battleMusicClips[i])
                    return true;
            }

            // If no match was found, return false
            return false;
        }
    }

    void Awake()
    {
        muteState = new MuteState(this);
        calmState = new CalmState(this);
        battleState = new BattleState(this);
    }

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        musicState = muteState;
        musicState.Entry();
    }

    void Update()
    {
        musicState.Update();
    }
    
    public void TriggerBattleMusic()
    {
        musicState.Exit(battleState);
    }
}
