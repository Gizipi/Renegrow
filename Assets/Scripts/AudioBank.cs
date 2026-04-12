using System.Collections.Generic;
using UnityEngine;

public class AudioBank
{
	private Dictionary<string, AudioSource> _audioClips = new();
	private AudioVolumes _audioVolumes = new();
	public AudioVolumes AudioVolumes
	{
		get
		{
			return _audioVolumes;
		}
	}
	public void AddAudio(string id, AudioSource audioSource)
	{
		_audioClips.Add(id, audioSource);
	}

	public void PlayAudio(string id, bool loop = false)
	{
		if (_audioClips.TryGetValue(id, out AudioSource audioSource))
		{
			audioSource.loop = loop;
		}
		PlayAudio(id);
	}

	public void PlayAudio(string id)
	{
		if (_audioClips.TryGetValue(id, out AudioSource audioSource))
		{
			audioSource.volume = _audioVolumes.masterVolume;
			audioSource.mute = _audioVolumes.muted;
			audioSource.Play();
		}
	}

	public void StopAudio(string id)
	{
		if (_audioClips.TryGetValue(id, out AudioSource audioSource))
		{
			audioSource.Stop();
		}
	}
}
