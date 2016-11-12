using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Room20 : Room {

	private float dir, dist, scale;
	private bool oneClick = false;

	public override void Instantiate () {
		Room12[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room12> ();
		InstantiateFromPrevious (rooms);

		Money = 8;
		deleteObjects ("Gun");
		products.Add ("сыр", new Products (new GameObject ("Cheese"), 30));
		products.Add ("пицца", new Products (new GameObject ("Pizza"), 60));
		products.Add ("торт", new Products (new GameObject ("Pie"), 30));
		products.Add ("шоколад", new Products (new GameObject ("Snickers"), 40));
		products.Add ("масло", new Products (new GameObject ("Oil"), 30));
		products.Add ("бананы", new Products (new GameObject ("Bananas"), 30));
		products.Add ("сгущенка", new Products (new GameObject ("ConseledMilk"), 30));
		products.Add ("мед", new Products (new GameObject ("Honey"), 30));
	}

	public override void changeRoom () {
		products ["сыр"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Cheese");
		products ["пицца"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Pizza");
		products ["торт"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Pie");
		products ["шоколад"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Snickers");
		products ["масло"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Oil");
		products ["бананы"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Bananas");
		products ["сгущенка"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\ConseledMilk");
		products ["мед"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Honey");

		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop2");
		objects ["Stoika"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Stoika9_14");
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_21");
		objects ["Gary"].AddComponent<Animator> ().runtimeAnimatorController = 
			Resources.Load<RuntimeAnimatorController> ("Gary_21_2");
		objects ["Lamp"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Lamp_9-13");
	}

	public IEnumerator TakeGun(){
		objects["Saler"].GetComponent<Animator>().SetBool("TakeGun", true);
		yield return new WaitForSeconds (0.5f);
		objects ["Saler"].GetComponent<Animator> ().SetBool ("TakeGun", false);
		yield return new WaitForSeconds (1.8f);
		objects.Add ("Gun", new GameObject ("Gun"));
		objects ["Gun"].transform.SetParent (gameObject.transform);
		objects ["Gun"].transform.localPosition = new Vector3 (-1.36f, -0.62f);
		objects ["Gun"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gun");
		objects ["Gun"].GetComponent<SpriteRenderer> ().sortingLayerName = "room20";
		objects ["Gun"].GetComponent<SpriteRenderer> ().sortingOrder = 7;
		objects ["Gun"].AddComponent<BoxCollider2D> ().size = 
			objects ["Gun"].GetComponent<SpriteRenderer> ().bounds.size;
		yield return null;
	}

	public override void TimeStart ()
	{
		//
		//StartCoroutine (TakeGun ());
		//currentTime = timeInRooms.BeforeShop;
		//return;
		//
		WriteStartDialog ("p гари: Пэ?",
			"продавец: привет, гари!",
			"продавец: слышал, недавно сожгли магазин, полиция не нашла преступников.",
			"продавец: и свидетелей.",
			"p продавец: и состав преступления.",
			"продавец: лучше иметь при себе оружие.",
			"продавец: все хорошо, гари?",
			"продавец: у тебя неважный вид.",
			"p гари: ничего, просто кажется, что это уже было.",
			"продавец: хах, у тебя что - дежавю? у меня нет лицензии, но...",
			"гари: твой магазин...",
			"гари: сгарел?",
			"продавец: ерунда, вот же он. кстати, гари, почему у тебя такое имя?",
			"гари: подожди, кажется, что это уже было.",
			"p продавец: как скажешь, гари. эта молодежь с этими идеями.",
			"гари: стой, я помню.",
			"гари: это же у тебя всегда были какие-то идеи?",
			"продавец: конечно, человеку трудно жить без идеи и дела.",
			"продавец: именно поэтому я открыл магазин.",
			"гари: да, но вокруг всегда было много идей, и они все неправильны.",
			"гари: я нутром это чувствую - они все неправильны!",
			"гари: и я бежал от них.",
			"продавец: но гари, нельзя же сомневаться вообще во всем.",
			"продавец: так и умереть недолго, гари!",
			"продавец: нужно иметь храбрость придерживаться какого-то мнения, даже если ты видишь в нем недостатки.",
			"продавец: если тебе не нравится что-то - измени это.",
			"продавец: это пиар-фраза для рекламы магазина. не слишком пафосно?",
			"гари: я не хочу ничего менять.",
			"гари: пусть они просто отстанут от меня!",
			"гари: я устал.",
			"гари: да и что я мог изменить?",
			"гари: я же был ребенком в то время.",
			"продавец: как же, ты много чего мог.",
			"продавец: просто это долгий путь.",
			"гари: нет, я слишком устал.",
			"p продавец: ну, тогда можешь купить продукты последний раз.",
			"продавец: и отдохнуть потом."
		);
		if (startClicks == 6) {
			StartCoroutine (TakeGun ());
		}
	}

	public override void TimeEnd ()
	{
		WriteDialog (endClicks, "гари: додумывай эту концовку сам.",
			"гари: я сваливаю отсюда.",
			"гари: извини, но ты выбрал неправильный конец.",
			"гари: это же не бомж с дробовиком, в конце концов!",
			"гари: хах!",
			"гари: хах!"
		);
		if (endClicks == 1) {
			StartCoroutine (chooseGun ());
			firstInput = false;
		}
		if (endClicks == 4) {
			StartCoroutine (thxForPlaying ());
		}
	}

	public override void ShopTime(){

		if (shopClicks == 0 || oneClick) {
			string name = getNameOfClickedButton ();
			if (name != null)
				choosenProductName = name;
		}

		if (oneClick && choosenProductName == "exit") {
			shopClicks = 0;
			oneClick = false;
		}
	
		if ((Input.GetButtonDown("Click") || Input.GetKeyDown(KeyCode.Space)) && choosenProductName != null && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "пистолет":
				WriteDialog (shopClicks, "продавец: не, и не проси.", 
					"продавец: ни за какие вещественные деньги не дам.", 
					"p продавец: воу, так много? держи.",
					"гари: это все нереально.",
					"гари: ты сгарел тогда, я видел.",
					"p продавец: когда?",
					"гари: посмотри слева, последнего окна нет.",
					"гари: я появлялся в каждом из них, был там, а теперь тут, это бред.",
					"p продавец: где? я ничего не вижу.",
					"гари: да обернись ты уже!",
					"продавец: зачем, гари?",
					"гари: этого нет! все, что происходило последние 10 лет - лютый бред!",
					"гари: это очень глупо, такой последовательности событий просто не может существовать в природе!",
					"гари: каждый пытается что-то доказать, верит в какую-то ересь.",
					"гари: а верхушке только это и нужно - стайка философов!",
					"гари: пусть спорят друг с другом и остаются в муравейнике.",
					"гари: а я просто хотел нормально жить!",
					"продавец: а что по-твоему нормально?",
					"гари: я не знаю!",
					"гари: я устал.",
					"гари: может, это сон?",
					"гари: я сейчас проснусь?",
					"гари: может, это как игра?",
					"гари: и все вокруг нереально?",
					"продавец: воу воу, гари, ты сейчас будешь верить во все?",
					"продавец: хорошо, ты доказал мне и всем остальным, что ты устал.",
					"продавец: опусти пистолет и пойди поспи чуть-чуть.",
					"продавец: просто успокойся и завтра погуляй с друзьями.",
					"гари: я тебе не верю!",
					"гари: ты все делаешь, чтобы извести меня!",
					"гари: что за бред ты несешь!",
					"гари: каждый указывает мне, как жить!",
					"гари: почему каждый знает лучше меня, как мне жить?",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"p продавец: ты весь дрожишь.",
					"продавец: положи пистолет.",
					"продавец: положи пистолет.",
					"продавец: положи пистолет.",
					"продавец: положи пистолет."
				);
				if (shopClicks == 2) {
					StartCoroutine (switchToInfinity ());
					oneClick = true;
					choosenProductName = null;
				}
				if (shopClicks == 3) {
					MoneyObj.SetActive (false);
					oneClick = false;
					shopBell.Play ();
				}
				if (shopClicks == 4) {
					objects ["Gary"].GetComponent<Animator> ().SetBool ("TakeGun", true);
					objects ["Gun"].SetActive (false);
				}
				if (shopClicks == 34) {
					StartCoroutine (preChooseGun());
				}
				break;
			case "exit":
				WriteDialog (shopClicks, "гари: придумал! я выйду!",
					"продавец: что?",
					"гари: я выйду отсюда и выкину их из кресел правления!",
					"продавец: но для этого нужно пробиться во власть, гари,",
					"продавец: а у тебя даже нет образования.",
					"гари: я переберусь в столицу и поступлю на юридический!",
					"гари: я все исправлю, Пэ!",
					"продавец: а ты не боишься стать таким же, как они?",
					"гари: в таком случае я хотя бы попытаюсь.",
					"гари: послезавтра перееду и сдам экзамены!",
					"гари: сейчас середина лета, я еще успею ко 2-ой волне.",
					"продавец: ты готов?",
					"гари: да! ты достаточно рассказал в свое время, и последние пару лет я много читал. я все исправлю, Пэ!",
					"гари: даже если у меня не получится, я подготовлюсь и поступлю в следующем году.",
					"продавец: и это ради власти? это же займет уйму времени, всю жизнь, возможно.",
					"гари: придется, если они не захотят уйти. они мешают мне жить, Пэ!",
					"гари: все, мне нужно бежать!",
					"продавец: стой! тебе нужны деньги? вот 200 монет, я скину тебе еще завтра.",
					"p гари: спасибо, Пэ!"
				);
				if (shopClicks == 17) {
					objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = true;
					objects ["Gary"].GetComponent<Animator> ().SetBool ("Exit", true);
				}
				if (shopClicks == 19) {
					StartCoroutine (chooseExit ());
				}
				break;
			}
		} 
	}

	public override void preShopTime(){
		var btns = GameObject.FindGameObjectsWithTag ("button");
		foreach (var button in btns)
			Object.Destroy (button);

		buttons = new Dictionary<string, Button> ();
		buttons.Add ("пистолет", GameController.Instance.finalButton);
		buttons ["пистолет"].gameObject.SetActive (true);
		exit.gameObject.GetComponentInChildren<Text> ().text = "Exit";
		currentTime = timeInRooms.Shop;
		preShopInput = false;
	}

	public override string getNameOfClickedButton(){
		Vector3 position = Input.mousePosition;
		var camera = Camera.main;
		Ray pos2 = camera.ScreenPointToRay (position);
		var hit = new RaycastHit ();
		Physics.Raycast (pos2, out hit);
		if (hit.collider != null && buttons ["пистолет"].gameObject == hit.collider.gameObject)
			return "пистолет";
		if (hit.collider != null && exit.gameObject == hit.collider.gameObject)
			return "exit";
		return null;
	}

	public IEnumerator switchToInfinity(){
		yield return new WaitForSeconds (1f);
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = true;

		yield return new WaitForSeconds (0.5f);
		lightSound.Play ();
		MoneyObj.transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, 90f));
		MoneyObj.transform.position = transform.position + new Vector3 (1.7f, 0.2f);
		objectPhrases ["деньги"] = "черт, я подумал о бесконечных деньгах и они сразу же появились. что происходит?";

		yield return new WaitForSeconds (0.5f);
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = false;
		yield return null;
	}

	public IEnumerator preChooseGun(){
		dir = Random.Range (0, 2f) * Mathf.PI;
		dist = 0.001f;

		for (int i = 0; i < 300; i++) {
			if (i % 2 == 1) {
				dir = Mathf.PI + dir;
				dist += 0.0001f;
			} else
				dir = Random.Range (0, 2f) * Mathf.PI;
			objects ["Gary"].transform.localPosition += new Vector3 (Mathf.Cos (dir) * dist, Mathf.Sin (dir) * dist);
			yield return new WaitForEndOfFrame ();
			yield return new WaitForEndOfFrame ();
		}

		yield return new WaitForEndOfFrame ();

		scale = 1.01f;
		dist = 0.001f;
		StartCoroutine (GameController.Instance.backgroundMusic.FadeIn (1f));
		for (int i = 0; i < 700; i++) {
			if (i % 50 == 0) {
				objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop22");
				objects ["Stoika"].SetActive (false);
				objects ["Saler"].SetActive (false);
				objects ["Shkaf"].SetActive (false);
				objects ["Picture"].SetActive (false);
				objects ["Call"].SetActive (false);
				objects ["Door"].SetActive (false);
				objects ["TV"].SetActive (false);
				objects ["Lamp"].SetActive (false);
				MoneySymbol.SetActive (false);
				foreach (var prod in products)
					prod.Value.Obj.SetActive (false);
			} 
			if (i % 50 == 10) {
				objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop2");
				objects ["Stoika"].SetActive (true);
				objects ["Saler"].SetActive (true);
				objects ["Shkaf"].SetActive (true);
				objects ["Picture"].SetActive (true);
				objects ["Call"].SetActive (true);
				objects ["Door"].SetActive (true);
				objects ["TV"].SetActive (true);
				objects ["Lamp"].SetActive (true);
				MoneySymbol.SetActive (true);
				foreach (var prod in products)
					prod.Value.Obj.SetActive (true);
			}
			//if (i == 700)
				//StartCoroutine (thxForPlaying());
			for (int j = 0; j < gameObject.transform.childCount; j++) {
				if (i % 2 == 1) {
					dir = 2 * Mathf.PI - dir;
				} else
					dir = Random.Range (0, 2f) * Mathf.PI;
				var child = gameObject.transform.GetChild (j);
				//if (child == objects["Gary"])
				child.transform.localPosition += new Vector3 (Mathf.Cos (dir) * dist, Mathf.Sin (dir) * dist);
				child.transform.localScale = new Vector3 (Random.Range(scale, scale + 0.01f), Random.Range (scale, scale + 0.01f), 1f);
			}
			dist += 0.0003f;
			scale += 0.003f;
			yield return new WaitForEndOfFrame ();
			yield return new WaitForEndOfFrame ();
		}

		//StartCoroutine (thxForPlaying ());

		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop22");
		objects ["Stoika"].SetActive (false);
		objects ["Shkaf"].SetActive (false);
		objects ["Picture"].SetActive (false);
		objects ["Call"].SetActive (false);
		objects ["Door"].SetActive (false);
		objects ["TV"].SetActive (false);
		objects ["Lamp"].SetActive (false);
		MoneySymbol.SetActive (false);
		foreach (var prod in products)
			prod.Value.Obj.SetActive (false);
		StopCoroutine ("WriteText");
		mainText.text = "";
		isWriteText = false;
		numOfYear = 21;
		StartCoroutine(GameController.Instance.backgroundMusic.FadeOut ());
		yield return new WaitForSeconds (1f);
		firstInput = true;
		currentTime = timeInRooms.End;

		yield return null;
	}

	public IEnumerator chooseGun(){
		/*yield return new WaitForSeconds (0.5f);
		objects ["Gary"].GetComponent<Animator> ().SetBool ("Gun", true);

		yield return new WaitForSeconds (1.5f);*/

		objects ["Gun"].SetActive (true);
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = false;
		objects ["Gary"].GetComponent<Animator> ().runtimeAnimatorController = 
			Resources.Load<RuntimeAnimatorController> ("Gary_21");
		objects ["Gary"].GetComponent<Animator> ().SetBool ("Walk", true);
		var pos = new Vector3 (0, Screen.height / 2, 20f);
		var pos2 = Camera.main.ScreenToWorldPoint (pos);
		while (objects ["Gary"].transform.position.x > pos2.x - 0.8f && endClicks != 4) {
			objects ["Gary"].transform.position -= new Vector3 (0.02f, 0f);
			yield return new WaitForEndOfFrame ();
		}
		objects ["Gary"].GetComponent<Animator> ().SetBool ("Walk", false);
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = true;
		yield return null;
	}

	public IEnumerator chooseExit(){
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = false;
		MoneyObj.transform.localRotation = Quaternion.Euler (Vector3.zero);
		MoneyObj.transform.position = transform.position + Room.posMoney;
		Money += 200;
		shopBell.Play ();
		yield return new WaitForSeconds (1f);

		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = true;

		objects.Add ("DoorHole", new GameObject ("DoorHole"));
		objects ["DoorHole"].transform.SetParent (gameObject.transform);
		objects ["DoorHole"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("DoorWay");
		objects ["DoorHole"].GetComponent<SpriteRenderer> ().sortingLayerName = "room20";
		objects ["DoorHole"].GetComponent<SpriteRenderer> ().sortingOrder = 12;
		objects ["DoorHole"].transform.localPosition = Room.posDoor;

		objects ["Door"].transform.localScale = new Vector3 (-2f, 1f, 1f);


		yield return new WaitForSeconds (1f);
		yield return StartCoroutine (thxForPlaying ());
	}

	public IEnumerator thxForPlaying(){
		yield return new WaitForSeconds (1.5f);

		numOfYear = 21;
		var rooms = GameObject.FindGameObjectsWithTag ("room");
		//darkRoom (Color.black, gameObject);
		for (int i = 0; i < gameObject.transform.childCount; i++){
			var child = gameObject.transform.GetChild (i);
			Object.Destroy (child.gameObject);
		}
		MoneyObj.SetActive (false);
		buttons ["пистолет"].gameObject.SetActive (false);
		exit.gameObject.SetActive (false);
		mainText.gameObject.SetActive (false);
		lightSound.Play ();

		yield return new WaitForSeconds (2f);
		foreach (var room2 in rooms) {
			if (room2.name != "room20")
				room2.SetActive (false);
		}

		StartCoroutine (GameController.Instance.backgroundMusic.FadeOut ());
		GameController.Instance.currentYear++;
		lightSound.Play ();
		yield return new WaitForSeconds (2f);

		GameController.Instance.finalText.text = "автор: Глеб Захаров\r\n" +
			"\r\nблагодарю" +
			"\r\n\tСашу Сятчихина" +
			"\r\n\tСашу Гайдаша" +
			"\r\nза поддержку.\r\n" +
			"музыка: Bensound.com - the jazz piano.";
		yield return new WaitForSeconds (15f);


		GameController.Instance.finalText.gameObject.SetActive (false);
		lightSound.Play ();
		this.enabled = false;

		Application.Quit ();
		yield return null;
	}

	private void darkRoom(Color darkness, GameObject room){
		foreach (var obj in room.GetComponent<Room>().objects) {
			if (obj.Value != null)
				obj.Value.GetComponent<SpriteRenderer> ().color = darkness;
		}
		foreach (var prod in room.GetComponent<Room>().products) {
			if (prod.Value.Obj != null) {
				var ren = prod.Value.Obj.GetComponent<SpriteRenderer> ();
				if (ren != null)
					ren.color = darkness;
			}
		}
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["Gun"] = "пистолет. где-то я его уже видел.";
		objectPhrases.Add ("продукт", "всего так много... кризис прошел?");
		objectPhrases ["Saler"] = "еее, продавец вернулся, найс! его так долго не было, он уезжал... в отпуск? ничего не помню.";
		objectPhrases ["Picture"] = "еее, старая картина! я ее помню!";
		objectPhrases ["Shop"] = "магазин такой чистый.";
		objectPhrases ["TV"] = "телевизор показывает что-то хорошее! впервые за 7 лет!";
		objectPhrases ["Stoika"] = "такая чистая.";
		objectPhrases ["Lamp"] = "новая, как в мечтах.";
		objectPhrases ["Shkaf"] = "милый шкаф. еее.";
		objectPhrases ["Door"] = "что происходит?";
		objectPhrases ["Call"] = "что происходит?";


		objectPhrases ["деньги"] = "сегодня мало. ничего, зато можно поболтать с продавцом.";
		//objectPhrases["
	}

	public override void clickOnRoom(){
		if (Input.GetMouseButton(0) && !isWriteText) {
			clickOnSomething = getNameOfClickedObject ();
			if (clickOnSomething == null)
				return;
			if (clickOnSomething.Contains ("продукт")) {
				StartCoroutine (WriteText ("гари: (" + objectPhrases ["продукт"] + ")"));
				return;
			} 
			if (clickOnSomething.Contains ("деньги")) {
				StartCoroutine (WriteText ("гари: (" + objectPhrases["деньги"] + ")"));
				return;
			}
			if (objectPhrases.ContainsKey (clickOnSomething)) {
				StartCoroutine (WriteText ("гари: (" + objectPhrases [clickOnSomething] + ")"));
				return;
			}
		}
		clickOnSomething = null;
	}
}
