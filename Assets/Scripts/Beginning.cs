using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Beginning : MonoBehaviour {

	//Sounds
	public AudioSource steps;
	private AudioClip[] stepsSound;
	public AudioSource backgroundMusic;

	private bool isShowTutorial; //for IEnumerator TutorialTextShow
	private bool flipX = false; //flip Gary or not
	GameObject Gary;
	private GameObject[] garyLetters;

	void Start () {
		backgroundMusic = GameController.Instance.backgroundMusic;
		StartGame ();
		StartCoroutine (StartGame ());
	}

	private void createName(){
		
		//sort by name
		garyLetters = GameObject.FindGameObjectsWithTag ("garyLetters");
		for (int i = 0; i < garyLetters.Length - 1; i++) {
			for (int j = 0; j < garyLetters.Length - i - 1; j++) {
				if (string.Compare( garyLetters[j].name, garyLetters[j + 1].name) > 0) {
					GameObject temp = garyLetters [j];
					garyLetters [j] = garyLetters [j + 1];
					garyLetters [j + 1] = temp;
				}
			}
		}

		for (int i = 0; i < garyLetters.Length; i++) {
			garyLetters [i].transform.position = 
				new Vector3(garyLetters [i].transform.position.x, Room.yPos + Room.posGary.y - 1.5f);
			var text = garyLetters [i].GetComponent<Text> ();
			switch (i) {
			case 0:
				text.text = "Г";
				break;
			case 1:
				text.text = "а";
				break;
			case 2:
				text.text = "р";
				break;
			case 3:
				text.text = "и";
				break;
			}
			garyLetters [i].SetActive (false);
		}
	}

	private void showLetters(){
		for (int i = 0; i < garyLetters.Length; i++) {
			if (Gary.transform.position.x - garyLetters [i].transform.position.x < 0)
				garyLetters [i].SetActive (true);
			else
				garyLetters [i].SetActive (false);
		}
	}

	public IEnumerator TutorialTextShow(){
		isShowTutorial = true;
		var tutText = GameObject.Find ("TutorialText");
		bool show = false;
		while (true) {
			if (!isShowTutorial) {
				Object.Destroy (tutText);
				yield break;
			}
			if (show)
				tutText.GetComponent<Text> ().text = "ЛЕВАЯ КНОПКА\r\nМЫШИ!";
			else
				tutText.GetComponent<Text> ().text = "";
			show = !show;
			yield return new WaitForSeconds (0.8f);
		}
	}

	public IEnumerator StartGame(){
		createName ();
		StartCoroutine (TutorialTextShow ());

		Gary = new GameObject ("Gary"); 
		Gary.transform.position = new Vector3 (7.4f, Room.yPos + Room.posGary.y);
		Gary.AddComponent<SpriteRenderer> ();
		Animator animGary = Gary.AddComponent<Animator> ();
		animGary.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Gary_9");

		var road = GameObject.Find ("SoundEffect 2").GetComponent<AudioSource>();
		road.clip = Resources.Load<AudioClip>(@"Sounds\RoadNoise");
		road.loop = true;
		StartCoroutine(road.FadeIn (1f));

		steps = GameObject.Find("SoundEffect 1").GetComponent<AudioSource>();
		stepsSound = new AudioClip[4];
		for (int i = 0; i < garyLetters.Length; i++) {
			stepsSound [i] = Resources.Load<AudioClip> (@"Sounds\Steps" + (i + 1));
		}

		float speed = 1f;

		float zDist = 20f;
		float yDist = Room.yPos + Room.posGary.y;
		Vector3 pointB = Gary.transform.position;
		while (true) {

			//exit from loop here
			if ((Gary.transform.position - Room.posDoor - (new Vector3 (Room.startPos, Room.yPos))).x < 0.0001f) {
				Object.Destroy (Gary);
				foreach (var letter in garyLetters)
					Object.Destroy (letter);
				isShowTutorial = false;
				StartCoroutine(road.FadeOut ());
				StartCoroutine (GameController.Instance.LoadRoom9 ());
				yield break;
			}

			//else
			showLetters();
			if (Input.GetMouseButton(0)) {
				var pos = Input.mousePosition;
				pos.z = zDist;
				pointB = Camera.main.ScreenToWorldPoint (pos);
				pointB.y = yDist;
				if ((Gary.transform.position - pointB).x > 0)
					flipX = false;
				else
					flipX = true;

				Gary.GetComponent<SpriteRenderer> ().flipX = flipX;
				animGary.SetBool ("isMove", true);
			}
			if (Mathf.Abs ((Gary.transform.position - pointB).x) < 0.0001f)
				animGary.SetBool ("isMove", false);
			else {
				Gary.transform.position = Vector3.MoveTowards (Gary.transform.position, pointB, Time.deltaTime * speed);
				if (!steps.isPlaying) {
					int rnd = Random.Range (0, stepsSound.Length);
					steps.clip = stepsSound [rnd];
					steps.Play ();
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}
			
}
