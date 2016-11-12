using UnityEngine;
using System.Collections;

public static class AudioSourceExtension{
	
	public static IEnumerator FadeIn(this AudioSource asource, float volume){
		if (asource.clip != null) {
			if (volume > 1f)
				volume = 1f;
			if (volume < 0)
				volume = 0;
			float step = 0.01f;
			if (asource.volume > volume)
				asource.volume = 0;
			if (!asource.isPlaying)
				asource.Play ();
			while (asource.volume < volume) {
				asource.volume += step;
				yield return new WaitForSeconds (0.04f);
			}
		}
		yield return null;
	}

	public static IEnumerator FadeOut(this AudioSource asource){
		if (asource.clip != null && asource.isPlaying) {
			float step = 0.03f;
			while (asource.volume > 0.04f) {
				asource.volume -= step;
				yield return new WaitForSeconds (0.04f);
			}
			asource.Stop ();
			asource.volume = 1f;
		}
		yield return null;
	}
}
