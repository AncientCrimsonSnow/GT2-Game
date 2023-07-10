using UnityEngine;

namespace DataStructures.Event
{
	public abstract class AudioEvent : ScriptableObject
	{
		public abstract void Play(AudioSource source);
	}
}