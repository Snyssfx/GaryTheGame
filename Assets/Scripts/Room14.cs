using UnityEngine;
using System.Collections;

public class Room14 : Room {

	private bool isCoroutine = false;
	private ParticleSystem.Particle[] particles;
	private void initializeParticles(){
		var parSys = objects ["Fog"].GetComponent<ParticleSystem> ();
		particles = new ParticleSystem.Particle[parSys.maxParticles];
	}

	public override void Instantiate () {
		Room13[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room13> ();
		InstantiateFromPrevious (rooms);

		Money = 99;
		deleteProducts ("сосиски");

		objects.Add ("Fog", GameObject.Instantiate (Resources.Load<GameObject> ("Room14Fog")));
		products.Add ("торт", new Products (new GameObject ("Pie"), 70));
		products ["крупа"].Price = 43;
		products ["бананы"].Price = 30;
		products ["масло"].Price = 27;

	}

	public override void changeRoom (){
		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop14_15");
		objects ["Picture"].GetComponent<SpriteRenderer> ().sprite = Resources.Load <Sprite> ("Picture14_15");
		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler14");
		objects ["Saler"].AddComponent<Animator> ().runtimeAnimatorController = 
			Resources.Load<RuntimeAnimatorController> ("Saler14");
		objects ["Gary"].AddComponent<Animator> ().runtimeAnimatorController = 
			Resources.Load<RuntimeAnimatorController> ("Gary_14");
		var partSys = GameObject.Instantiate (Resources.Load<GameObject> ("GaryVape"));
		partSys.transform.SetParent (objects ["Gary"].transform);
		partSys.transform.localPosition = new Vector3 (-0.327f, 0.41f);
		//
		objects["Fog"].AddComponent<SpriteRenderer>();
		products ["торт"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Pie");
	}
	
	public override void RoomCycle ()
	{
		if (!isCoroutine)
			base.RoomCycle ();
	}

	public override void TimeStart ()
	{
		WriteDialog (startClicks, "p продавец: привет, гари!",
			"продавец: кхэ-кхэ.",
			"продавец: ты что, тоже куришь?",
			"гари: это пар!",
			"продавец: тебе сколько лет то?",
			"гари: законом не запрещено!",
			"продавец: переставай парить в моем магазине!",
			"продавец: я сейчас проветрю.",
			" ",
			"продавец: эй!",
			"продавец: президент вас разгонит.",
			"продавец: он уже отобрал у старых депутатов право голоса, теперь он может это сделать.",
			"продавец: серьезно, я больше не буду давать тебе мою сегу!",
			"гари: хорошо, я перестану.",
			"продавец: проваливай, ненавижу курильщиков!"
		);
		switch (startClicks) {
		case 1:
			StartCoroutine (vape ());
			break;
		case 2:
			StartCoroutine (ill ());
			break;
		case 9:
			StartCoroutine (freshAir ());
			break;
		case 16:
			SetDarkness (true);
			break;
		}
	}

	private IEnumerator ill(){
		isCoroutine = true;
		objects ["Saler"].GetComponent<Animator> ().SetBool ("isIll", true);
		yield return new WaitForSeconds (0.1f);
		objects ["Saler"].GetComponent<Animator> ().SetBool ("isIll", false);
		isCoroutine = false;
		yield return null;

	}
	private IEnumerator vape(){
		isCoroutine = true;
		var vaping = objects ["Gary"].GetComponentInChildren<ParticleSystem> (true);
		
		objects ["Gary"].GetComponent<Animator> ().SetBool ("Vape", true);
		yield return new WaitForSeconds (1.5f);
		vaping.Play();
		objects ["Gary"].GetComponent<Animator> ().SetBool ("Vape", false);
		isCoroutine = false;
		yield return StartCoroutine(ill());
	}

	private IEnumerator freshAir(){
		isCoroutine = true;
		objects.Add ("DoorHole", new GameObject ("DoorHole"));
		objects ["DoorHole"].transform.SetParent (gameObject.transform);
		objects ["DoorHole"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("DoorWay");
		objects ["DoorHole"].GetComponent<SpriteRenderer> ().sortingLayerName = "room14";
		objects ["DoorHole"].GetComponent<SpriteRenderer> ().sortingOrder = 10;
		objects ["DoorHole"].transform.localPosition = Room.posDoor;

		var doorSound = GameObject.Find ("SoundEffect 1").GetComponent<AudioSource> ();
		doorSound.clip = Resources.Load<AudioClip> (@"Sounds\Door2");
		doorSound.loop = false;
		doorSound.Play ();
		objects["Door"].transform.localScale = new Vector3(-2f, 1f, 1f);
		var road = GameObject.Find ("SoundEffect 2").GetComponent<AudioSource> ();
		road.clip = Resources.Load<AudioClip> (@"Sounds\RoadNoise");
		road.loop = true;
		road.Play ();

		var pSys = objects ["Fog"].GetComponent<ParticleSystem> ();
		initializeParticles ();
		int length = pSys.GetParticles (particles);

		for (float i = 0; i < 3f; i += 0.009f) {
			for (int j = 0; j < length; j++) {
				particles [j].startSize -= 0.05f;
			}
			if (particles [0].startSize < 0.05f)
				break;
			pSys.SetParticles (particles, pSys.maxParticles);
			yield return new WaitForSeconds (i);
		}
		pSys.Stop ();
		pSys.Clear ();

		road.Stop ();
		doorSound.clip = Resources.Load<AudioClip> (@"Sounds\Door");
		doorSound.Play ();
		objects ["DoorHole"].SetActive (false);
		objects ["Door"].transform.localScale = new Vector3 (1f, 1f, 1f);
		isCoroutine = false;
		yield return StartCoroutine(vape());
	}

	public override void SetPositionsAndColliders ()
	{
		base.SetPositionsAndColliders ();
		objects ["Fog"].transform.localPosition = new Vector3 (0f, 0.79f);
	}
	public override void Show (bool trueOrFalse)
	{
		base.Show (trueOrFalse);
		if (trueOrFalse) {
			objects ["Fog"].GetComponent<ParticleSystem> ().playbackSpeed = 10f;
			objects ["Fog"].GetComponent<ParticleSystem> ().Play ();

		}
	}
	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue) {
			StartCoroutine (GameController.Instance.LoadRoom15 ());
		}
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["Gary"] = "я сам крутил намотку! пара много, чтобы продавец подавился!";
		objectPhrases ["Saler"] = "я его уничтожу!";
		objectPhrases ["Stoika"] = "стойка из нержавеющей стали. это не поможет ей против моего супер нового атомайзера!";
		objectPhrases ["Shop"] = "я разнесу этот магазин к черту! курение помогает забыть о всех вокруг!";
		objectPhrases ["деньги"] = "99, кризис закончился.";
		objectPhrases ["Picture"] = "подпись не разобрать из-за пара.";
		objectPhrases ["TV"] = "опять тот же репортер с нефтью. все на ней помешались!";
	}

}
