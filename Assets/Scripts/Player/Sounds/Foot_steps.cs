using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Foot_steps : MonoBehaviour {

	/*public enum StepsOn {Beton, Wood, Metal, Ground};

	// mainFolder - родительская папка в Resources
	// betonFolder и т.д. дочерние
	private string mainFolder = "Footsteps", betonFolder = "Beton", woodFolder = "Wood", metalFolder = "Metal", groundFolder = "Ground";
	private AudioClip[] Beton, Wood, Metal, Ground;
	private AudioSource source;
	private AudioClip clip;

	void Start()
	{
		source = GetComponent<AudioSource>();
		source.playOnAwake = false;
		source.mute = false;
		source.loop = false;
		LoadSounds();
	}

	void LoadSounds()
	{
		Beton = Resources.LoadAll<AudioClip>(mainFolder + "/" + betonFolder);
		Wood = Resources.LoadAll<AudioClip>(mainFolder + "/" + woodFolder);
		Metal = Resources.LoadAll<AudioClip>(mainFolder + "/" + metalFolder);
		Ground = Resources.LoadAll<AudioClip>(mainFolder + "/" + groundFolder);
	}

	public void PlayStep(StepsOn stepsOn, float volume)
	{
		switch(stepsOn)
		{
		case StepsOn.Beton:
			clip = Beton[Random.Range(0, Beton.Length)];
			break;
		case StepsOn.Wood:
			clip = Wood[Random.Range(0, Wood.Length)];
			break;
		case StepsOn.Metal:
			clip = Metal[Random.Range(0, Metal.Length)];
			break;
		case StepsOn.Ground:
			clip = Ground[Random.Range(0, Ground.Length)];
			break;
		}

		source.PlayOneShot(clip, volume);
	}*/
}

