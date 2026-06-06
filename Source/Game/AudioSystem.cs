using FlaxEngine;

namespace Game.Game;

public class AudioSystem : GamePlugin
{
    public static AudioSystem Instance => PluginManager.GetPlugin<AudioSystem>();

    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("AudioSystem initialized");
    }

    public void PlayAudio(Transform location, AudioClip clip)
    {
        Debug.Log("Playing audio at location");
        var audioSource = new AudioSource();
        audioSource.Transform = location;
        audioSource.Parent = Level.GetScene(0);
        audioSource.Clip = clip;
        audioSource.Play();
    }

}
