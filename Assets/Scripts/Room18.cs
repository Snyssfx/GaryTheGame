using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Room18 : Room {
	private bool isCoroutine = false;
	GameObject[] robbers;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		darkRoom (new Color (0.2f, 0.2f, 0.2f));
		MoneySymbol.GetComponent<SpriteRenderer> ().color = Color.white;
	}

	public override void Instantiate () {
		Room17[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room17> ();
		InstantiateFromPrevious (rooms);

		Money = 1200;
		deleteProducts ("торт", "пицца");
		deleteObjects ("Graphic");
		products.Add ("молоко", new Products (new GameObject ("Milk"), 60));

		robbers = new GameObject[3];

	}

	public override void changeRoom (){
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_20");
		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop20_21");
		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler18");
		objects ["Shkaf"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shkaf19_21");

		products ["молоко"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Milk");
		for (int i = 0; i < 3; i++) {
			robbers [i] = new GameObject ("rob" + (i + 1));
			robbers [i].transform.SetParent (gameObject.transform);
			var sprRen = robbers [i].AddComponent<SpriteRenderer> ();
			sprRen.sprite = Resources.Load<Sprite> ("G" + (i + 1) + "_1");
			sprRen.sortingLayerName = "room18";
			sprRen.sortingOrder = 21;
			robbers [i].SetActive (false);
		}
		robbers [1].GetComponent<SpriteRenderer> ().sortingOrder = 22;


	}

	private void darkRoom(Color darkness){
		//Color darkness = new Color(0.2f, 0.2f, 0.2f, 0.95f);
		foreach (var obj in objects) {
			if (obj.Value != null)
				obj.Value.GetComponent<SpriteRenderer> ().color = darkness;
		}
		foreach (var prod in products) {
			if (prod.Value.Obj != null) {
				var ren = prod.Value.Obj.GetComponent<SpriteRenderer> ();
				if (ren != null)
					ren.color = darkness;
			}
		}
		if (MoneySymbol != null)
			MoneySymbol.GetComponent<SpriteRenderer> ().color = darkness;
	}


	public override void RoomCycle ()
	{
		if (!isCoroutine)
			base.RoomCycle ();
	}

	public override void TimeStart ()
	{
		WriteDialog (startClicks, "p гари: темно. что случилось?",
			"продавец: не знаю, отключили свет.",
			"вор 1: привет, приятель.",
			"вор 1: мы заберем все продукты и деньги, друг, ты не против?",
			"вор 2: сложные времена, приятель.",
			"p продавец: я вас знаю?",
			"вор 2: нет.",
			"вор 3: хах, славно ГАРИт.",
			"вор 2: продавец словил ГАРИк.",
			"вор 1: кажется, его бизнес проГАРел. уходим, ребята.");
		switch (startClicks) {
		case 2:
			StartCoroutine (robbersInDaHouse ());
			break;

		case 6:
			foreach(var prod in products)
				prod.Value.Obj.SetActive(false);
			Money = 0;
			shopBell.Play();
			break;

		case 7:
			StartCoroutine (burning ());
			break;

		case 11:
			darkRoom (Color.black);
			MoneyObj.SetActive (false);
			MoneySymbol.SetActive (false);
			//StartCoroutine (backgroundMusic.FadeOut ());
			mainText.text = "";
			foreach (var robber in robbers)
				Object.Destroy (robber);
			Object.Destroy (Past);
			Object.Destroy (Future);
			StartCoroutine (GameObject.Find ("SoundEffect 1").GetComponent<AudioSource> ().FadeOut ());

			StartCoroutine (GameController.Instance.LoadRoom19 ());
			this.enabled = false;
			break;
		}
	}

	private IEnumerator robbersInDaHouse(){
		isCoroutine = true;
		yield return new WaitForSeconds (2f);

		objects.Add ("DoorHole", new GameObject ("DoorHole"));
		var doorHoleSprRen = objects ["DoorHole"].AddComponent<SpriteRenderer> ();
		objects ["DoorHole"].transform.SetParent (gameObject.transform);
		doorHoleSprRen.sprite = Resources.Load<Sprite> ("DoorWay18");
		doorHoleSprRen.sortingLayerName = "room18";
		doorHoleSprRen.sortingOrder = 10;
		objects ["DoorHole"].transform.localPosition = Room.posDoor;

		var doorSound = GameObject.Find ("SoundEffect 1").GetComponent<AudioSource> ();
		doorSound.loop = false;
		doorSound.clip = Resources.Load<AudioClip> (@"Sounds\DoorKick");
		doorSound.volume = 0.3f;
		doorSound.Play ();
		objects["Door"].transform.localScale = new Vector3(-2f, 1f, 1f);

		yield return new WaitForSeconds (1.3f);
		darkRoom (new Color (0f, 0f, 0f));
		MoneyObj.GetComponentInChildren<Text> ().text = "";

		yield return new WaitForSeconds (1.5f);

		objects.Add ("Gas", new GameObject ("Gas"));
		var sprRen = objects ["Gas"].AddComponent<SpriteRenderer> ();
		sprRen.sortingLayerName = "room18";
		sprRen.sortingOrder = 13;
		sprRen.sprite = Resources.Load<Sprite> ("Gas");
		objects ["Gas"].transform.SetParent (gameObject.transform);
		objects ["Gas"].transform.localPosition = new Vector3 (0.19f, -1.57f);

		doorHoleSprRen.sprite = Resources.Load<Sprite> ("DoorWay");
		foreach (var robber in robbers)
			robber.SetActive (true);
		robbers [0].transform.localPosition = new Vector3 (2.01f, -0.97f);
		robbers [1].transform.localPosition = new Vector3 (1.262f, -0.97f);
		robbers [2].transform.localPosition = new Vector3 (0.52f, -0.97f);

		darkRoom (new Color (0.4f, 0.4f, 0.4f));
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = true;
		objects ["Gary"].transform.localPosition -= new Vector3 (0.1f, 0f);
		MoneySymbol.GetComponent<SpriteRenderer> ().color = Color.white;
		MoneyObj.GetComponentInChildren<Text> ().text = Money.ToString ();
		doorHoleSprRen.color = Color.white;

		isCoroutine = false;
		yield return null;
	}

	private IEnumerator burning(){
		isCoroutine = true;
		robbers [1].AddComponent<Animator> ().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("G2_1");
		robbers [1].GetComponent<Animator> ().SetBool ("isBurn", true);

		var fireZippoSound = GameObject.Find ("SoundEffect 1").GetComponent<AudioSource> ();
		fireZippoSound.volume = 1f;
		fireZippoSound.clip = Resources.Load<AudioClip> (@"Sounds\FireZippo");
		fireZippoSound.Play ();
		yield return new WaitForSeconds (0.2f);
		robbers [1].GetComponent<Animator> ().SetBool ("isBurn", false);

		yield return new WaitForSeconds (1f);
		objects.Add ("Fire", new GameObject("Fire"));
		var sprRen = objects ["Fire"].AddComponent<SpriteRenderer> ();
		sprRen.sprite = Resources.Load<Sprite> ("Fire");
		sprRen.sortingLayerName = "room18";
		sprRen.sortingOrder = 15;
		var tran = objects ["Fire"].transform;
		tran.SetParent (gameObject.transform);
		Vector3 center = new Vector3( 0.24f, -1.73f);
		objects ["Fire"].transform.localPosition = new Vector3 (0.99f, -1.46f);
		float radius = Vector3.Magnitude(tran.localPosition - center);
		float angle = 0.2f;

		for (int i = 0; i < 30; i++) {
			tran.localPosition =  new Vector3(radius * Mathf.Cos(angle * Mathf.PI) + center.x,
				radius * Mathf.Sin(angle * Mathf.PI) + center.y);
			angle += (1f - 0.2f) / 30;
			yield return new WaitForEndOfFrame ();
			yield return new WaitForEndOfFrame ();
			yield return new WaitForEndOfFrame ();
		}

		yield return new WaitForSeconds (1.1f);

		objects["Shop"].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("FireShop");
		objects ["Shop"].GetComponent<SpriteRenderer> ().sortingOrder = 20;
		fireZippoSound.loop = true;
		fireZippoSound.clip = Resources.Load<AudioClip> (@"Sounds\Fire");
		fireZippoSound.Play ();
		MoneyObj.SetActive (false);
		MoneySymbol.SetActive (false);
		Object.Destroy (objects ["Fire"]);

		isCoroutine = false;
		yield return new WaitForSeconds (0.5f);
	}

	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue) {
			StartCoroutine (GameObject.Find ("SoundEffect 1").GetComponent<AudioSource> ().FadeOut ());
			Object.Destroy (Past);
			Object.Destroy (Future);
			StartCoroutine (GameController.Instance.LoadRoom19 ());
			foreach (var robber in robbers)
				Object.Destroy (robber);
		}
	}

	public override void clickOnRoom ()
	{
		return;
	}

}
