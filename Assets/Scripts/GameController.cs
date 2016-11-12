using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//using System.Linq;

public class GameController : MonoBehaviour {
	public AudioSource steps;
	private AudioClip[] stepsSound;
	public AudioSource backgroundMusic;

	public int currentYear = 8;

	public Button finalButton;
	public Text finalText;

	private bool isShowTutorial; //for IEnumerator TutorialTextShow
	private bool flipX = false; //flip Gary or not?
	GameObject Gary;
	private AudioSource bellSound, doorSound;
	//private float waitTime = 0.5f;
	private float waitTime = 1.5f;
	private GameObject[] garyName;

	//Singleton realization
	private static GameController instance;
	private GameController(){
	}
	public static GameController Instance {
		get {
			if (instance == null)
				instance = GameObject.FindGameObjectWithTag("game controller").GetComponent<GameController>();
			return instance;
		}
	}

	// Use this for initialization
	void Start () {
		backgroundMusic = GameObject.Find ("BackgroundMusic").GetComponent<AudioSource> ();
		backgroundMusic.loop = true;
		backgroundMusic.clip = Resources.Load<AudioClip> (@"Sounds\bensound-background");

		Instance.finalButton = GameObject.Find ("finalButton").GetComponent<Button>();//need for Room20
		Instance.finalButton.gameObject.SetActive (false);
		Instance.finalText = GameObject.Find ("finalText").GetComponent<Text> ();
		Instance.finalText.text = "";
				
		GameObject.FindGameObjectWithTag ("exit button").GetComponentInChildren<Text> ().text = "";
		GameObject.FindGameObjectWithTag ("main text").GetComponent<Text>().text = "";
		var buttons = GameObject.FindGameObjectsWithTag ("button");
		foreach (var button in buttons) {
			button.GetComponentInChildren<Text> ().text = "";
		}

		StartCoroutine (StartGame ());
		//StartCoroutine (LoadRoom9 ());

	}

	private void createName(){
		garyName = GameObject.FindGameObjectsWithTag ("gary");
		for (int i = 0; i < garyName.Length - 1; i++) {
			for (int j = 0; j < garyName.Length - i - 1; j++) {
				if (string.Compare( garyName[j].name, garyName[j + 1].name) > 0) {
					GameObject temp = garyName [j];
					garyName [j] = garyName [j + 1];
					garyName [j + 1] = temp;
				}
			}
		}

		for (int i = 0; i < 4; i++) {
			garyName [i].transform.position = new Vector3(garyName [i].transform.position.x, Room.yPos + Room.posGary.y - 1.5f);
			var text = garyName [i].GetComponent<Text> ();
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
			garyName [i].SetActive (false);
		}
	}

	private void showLetters(){
		for (int i = 0; i < 4; i++) {
			if (Gary.transform.position.x - garyName [i].transform.position.x < 0)
				garyName [i].SetActive (true);
			else
				garyName [i].SetActive (false);
		}
	}

	public GameObject loadRoom(){
		currentYear++;
		var room = new GameObject ("room" + currentYear.ToString());
		room.transform.parent = gameObject.transform;
		room.transform.position = new Vector3 (Room.startPos + (currentYear - 9) * Room.xPos, Room.yPos);
		return room;
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
		Instance.createName ();
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
		for (int i = 0; i < 4; i++) {
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
				foreach (var letter in garyName)
					Object.Destroy (letter);
				isShowTutorial = false;
				StartCoroutine(road.FadeOut ());
				StartCoroutine (Instance.LoadRoom9 ());
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
					int rnd = Random.Range (0, 4);
					steps.clip = stepsSound [rnd];
					steps.Play ();
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private void playBellAndDoor(){
		doorSound = GameObject.Find ("SoundEffect 2").GetComponent<AudioSource> ();
		doorSound.clip = Resources.Load<AudioClip> (@"Sounds\Door2");
		doorSound.loop = false;
		doorSound.Play ();

		bellSound = GameObject.Find("SoundEffect 3").GetComponent<AudioSource> ();
		bellSound.clip = Resources.Load<AudioClip> (@"Sounds\Bell");
		bellSound.Play();
		if (steps == null) {
			steps = GameObject.Find ("SoundEffect 1").GetComponent<AudioSource> ();
			steps.clip = Resources.Load<AudioClip> (@"Sounds\Steps");
		}
		steps.Play ();
	}

	public IEnumerator LoadRoom9(){
		if (GameObject.Find ("TutorialText") != null)
			Object.Destroy (GameObject.Find ("TutorialText"));
		Instance.playBellAndDoor ();

		var room9 = loadRoom ();
		var script = room9.AddComponent<Room9> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		yield return null;
	}

	public IEnumerator LoadRoom10(){
		var room10 = loadRoom();
		var script = room10.AddComponent<Room10> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		GameObject room9 = GameObject.Find ("room9");
		var script9 = room9.GetComponent<Room> ();
		foreach (var obj in script9.objects) {
			Object.Destroy (obj.Value.GetComponent<BoxCollider2D> ());
		}
		foreach (var prod in script9.products) {
			Object.Destroy (prod.Value.Obj.GetComponent<BoxCollider2D> ());
		}
		yield return null;
	}

	public IEnumerator LoadRoom11(){
		var room11 = loadRoom ();
		var script = room11.AddComponent<Room11> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (10);
		yield return null;
	}

	public IEnumerator LoadRoom12(){
		var room12 = loadRoom ();
		var script = room12.AddComponent<Room12> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (11);
		yield return null;
	}

	public IEnumerator LoadRoom13(){
		var room13 = loadRoom ();
		var script = room13.AddComponent<Room13> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		GameObject room12 = GameObject.Find ("room12");
		var script12 = room12.GetComponent<Room> ();
		foreach (var obj in script12.objects) {
			Object.Destroy (obj.Value.GetComponent<BoxCollider2D> ());
		}
		foreach (var prod in script12.products) {
			Object.Destroy (prod.Value.Obj.GetComponent<BoxCollider2D> ());
		}

		yield return null;
	}

	public IEnumerator LoadRoom14(){
		var room14 = loadRoom ();
		var script = room14.AddComponent<Room14> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (13);
		yield return null;
	}

	public IEnumerator LoadRoom15(){
		var room15 = loadRoom ();
		var script = room15.AddComponent<Room15> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (14);
		yield return null;
	}

	public IEnumerator LoadRoom16(){
		var room16 = loadRoom ();
		var script = room16.AddComponent<Room16> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (15);
		deleteObjectsFromPrevious (9);
		yield return null;
	}

	public IEnumerator LoadRoom17(){
		var room17 = loadRoom ();
		var script = room17.AddComponent<Room17> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (16);
		yield return null;
	}

	public IEnumerator LoadRoom18(){
		var room18 = loadRoom ();
		var script = room18.AddComponent<Room18> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (17);
		yield return null;
	}

	public IEnumerator LoadRoom19(){
		var room19 = loadRoom ();
		var script = room19.AddComponent<Room19> ();

		yield return new WaitForSeconds (waitTime);
		script.Show (true);
		deleteObjectsFromPrevious (18);
		yield return null;
	}

	public IEnumerator LoadRoom20(){
		currentYear = 20;
		var room20 = new GameObject ("room20");
		room20.transform.parent = gameObject.transform;
		room20.transform.position = new Vector3 (Room.startPos + 10 * Room.xPos, Room.yPos);
		//yield return null;
		var script = room20.AddComponent<Room20> ();
		yield return new WaitForSeconds (waitTime);

		var room12 = GameObject.Find ("room12");
		Color darkness = Color.black;
		foreach (var obj in room12.GetComponent<Room>().objects) {
			if (obj.Value != null)
				obj.Value.GetComponent<SpriteRenderer> ().color = darkness;
		}
		foreach (var prod in room12.GetComponent<Room>().products) {
			if (prod.Value.Obj != null) {
				var ren = prod.Value.Obj.GetComponent<SpriteRenderer> ();
				if (ren != null)
					ren.color = darkness;
			}
		}
		GameObject.Find ("LightSound").GetComponent<AudioSource> ().Play ();

		yield return new WaitForSeconds (2 * waitTime);

		script.Show (true);
		deleteObjectsFromPrevious (19);
		yield return null;

	}

	//need for optimization
	public void deleteObjectsFromPrevious(int roomToDelete){
		GameObject room = GameObject.Find ("room" + roomToDelete.ToString ());
		foreach (var obj in room.GetComponent<Room>().objects) {
			if (obj.Value != null && obj.Value.GetComponent<BoxCollider2D>() != null)
				Object.Destroy (obj.Value.GetComponent<BoxCollider2D>());
		}
		foreach (var prod in room.GetComponent<Room>().products) {
			if (prod.Value.Obj != null && prod.Value.Obj.GetComponent<BoxCollider2D>() != null)
				Object.Destroy (prod.Value.Obj);
		}
		foreach (var obj in room.GetComponent<Room>().objects) {
			if (obj.Key != "Saler" && obj.Key != "Shop" && obj.Key != "Picture") {
				Object.Destroy (obj.Value);
			}
		}
		Object.Destroy (room.GetComponent<Room> ().MoneyObj);
		Object.Destroy (room.GetComponent<Room> ().MoneySymbol);
		Object.Destroy (room.GetComponent<Room> ().Year);
	}
}
