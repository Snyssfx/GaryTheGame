using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Room15 : Room {

	private GameObject speechGary, speechSaler;
	private bool isSpeakGary = false, isSpeakSaler = false;
	private GameObject[] rooms;
	private GameObject money2;

	public override void Instantiate () {
		Room14[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room14> ();
		InstantiateFromPrevious (rooms);

		Money = 95;
		deleteObjects ("Fog", "DoorHole");
		deleteProducts ("торт", "бананы", "крупа", "масло");

		products.Add ("рыба", new Products (new GameObject ("Fish"), 59));
		products.Add ("молоко", new Products (new GameObject ("Milk"), 19));
		products.Add ("сгущенка", new Products (new GameObject ("ConseledMilk"), 39));
		products.Add ("яйца", new Products (new GameObject ("Eggs"), 29));
		products.Add ("хлеб", new Products (new GameObject ("Bread"), 17));

		speechGary = GameObject.Instantiate (Resources.Load<GameObject> ("Screen15Gary"));
		speechGary.transform.SetParent (gameObject.transform);

		speechSaler = GameObject.Instantiate (Resources.Load<GameObject> ("Screen15Saler"));
		speechSaler.transform.SetParent (gameObject.transform);

	}

	public override void changeRoom (){
		Object.Destroy (objects ["Gary"].GetComponent<Animator> ());
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_16");
		Object.Destroy(objects ["Gary"].transform.GetChild (0).gameObject);
		objects ["TV"].GetComponent<Animator> ().runtimeAnimatorController = 
			Resources.Load<RuntimeAnimatorController> ("TV2");
		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler15");

		products ["рыба"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Fish");
		products ["молоко"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Milk");
		products ["сгущенка"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\ConseledMilk");
		products ["яйца"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Eggs");
		products ["хлеб"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Bread2");
	}

	public override void TimeStart ()
	{
		WriteDialog (startClicks, "p продавец: привет.",
			"продавец: я устал и больше не говорю о политике. устал говорить.",
			"гари: может, сменим формат?",
			"продавец: на что?",
			"гари: как в хотлайн майами!"
		);
		if (startClicks == 6) {
			mainText.text = "";
			mainText = GameObject.Find ("TextRoom15").GetComponent<Text> ();
			writeDialog2 (startClicks - 5, "продавец: о, я недавно играл. вот так?");
		}
		if (startClicks > 6) {
			writeDialog2(startClicks - 6,
				"гари: ага.",
				"продавец: круто, что будешь брать?");
		}
		if (startClicks == 8) {
			currentTime = timeInRooms.BeforeShop;
		}
	}

	public override void TimeEnd ()
	{
		writeDialog2 (endClicks, "продавец: удачи! возвращайся домой и жди звонка.");
		if (endClicks == 1)
			SetGUIActive(false);
		if (endClicks == 2)
			StartCoroutine (setDark ());
	}

	public override void ShopTime(){
		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "рыба":
				writeDialog2 (shopClicks, "продавец: одного парня посадили, потому что он обсуждал коррупцию во власти.",
					"продавец: он оказался директором телевизионного канала.",
					"продавец: теперь на других каналах показывают только сюжеты про кошек.",
					"продавец: как они едят рыбу и пытаются слезть с деревьев.",
					"продавец: пожарные спасли 14 котиков за последние 2 нелели.",
					"продавец: а когда они не спасают котиков - новостей нет."
				);
				if (shopClicks == 7)
					BuyProduct ();
				break;
			case "молоко":
				writeDialog2 (shopClicks, "продавец: да, отлично, сделай себе молочный коктейль!",
					"гари: мать запрещает мне веселиться и разговаривать.",
					"гари: говорит, что так я приличней выгляжу.",
					"гари: отец много смеялся и разговаривал не по делу, в итоге они развелись.",
					"гари: ...",
					"продавец: воу-воу, полегче! слишком много драмы.",
					"продавец: уже можно освободиться от влияния родителей или хотя бы не замечать его.",
					"гари: я все-таки не буду его брать."
				);
				if (shopClicks == 9) {
					shopClicks = 0;
					choosenProductName = null;
					mainText.text = "";
					if (isSpeakGary)
						StartCoroutine (garySpeak (false));
				}
				break;
			case "сгущенка":
				writeDialog2 (shopClicks, "продавец: наша, наконец-то!",
					"продавец: вместе с автомобилями и тяжелой промышленностью."
				);
				if (shopClicks == 3)
					BuyProduct ();
				break;
			case "яйца":
				if (shopClicks == 1) {
					mainText.text = "";
					eggsRoad (true);
					mainText = GameObject.FindGameObjectWithTag ("main text").GetComponent<Text> ();
				}
				WriteDialog (shopClicks, "продавец: халяль!",
					"продавец: зато дешевле не найти.",
					"p гари: что произошло?",
					"продавец: ты переместился на 6 ходов назад, нужно начинать сначала.",
					"гари: этого не было в правилах!",
					"p продавец: хорошо, извини."
				);
				if (shopClicks == 7) {
					eggsRoad (false);
					mainText.text = "";
					mainText = GameObject.Find ("TextRoom15").GetComponent<Text>();
					BuyProduct ();
				}
				break;
			case "хлеб":
				writeDialog2 (shopClicks, "продавец: я рад, что кризис закончился.",
					"продавец: это эпоха спокойного существования,",
					"продавец: когда страна может залатать раны и построить функционирующую систему.",
					"продавец: это легко сделать, у нас огромные запасы внутренних ресурсов.",
					"продавец: нам нужно развивать культуру и науку, малый бизнес и IT.",
					"продавец: будет крайне смешно, если мы не используем этот момент.",
					"продавец: а будем пировать, как раньше, и богатство закончится.",
					"гари: ..."
				);
				if (shopClicks == 9)
					BuyProduct ();
				break;
			}
		}

		//check if the player can't buy anything
		IsEndTime ();
	}

	public override void SetPositionsAndColliders ()
	{
		base.SetPositionsAndColliders ();
		speechGary.transform.position = new Vector3 (9.06f, 0f) + transform.position;
		speechSaler.transform.position = new Vector3 (-10.3f, 0f) + transform.position;
	}

	private IEnumerator garySpeak(bool isStart){
		var oldPos = new Vector3 (9.06f, 0f) + transform.position;
		var newPos = new Vector3 (5.461f, 0f) + transform.position;
		int dir = 1;
		if (isStart)
			isSpeakGary = true;
		if (!isStart)
			dir = -1;
		for (int i = 0; i < 20; i++) {
			speechGary.transform.position += new Vector3 (dir * (newPos - oldPos).x / 20, 0f);
			yield return new WaitForEndOfFrame();
		}
		if (isStart)
			speechGary.transform.position = newPos;
		else
			speechGary.transform.position = oldPos;
		if (!isStart)
			isSpeakGary = false;
	}

	private IEnumerator salerSpeak(bool isStart){
		var oldPos = new Vector3 (-10.22f, 0f) + transform.position;
		var newPos = new Vector3 (-7.328f, 0f) + transform.position;

		if (isStart)
			isSpeakSaler = true;
		int dir = 1;
		if (!isStart)
			dir = -1;
		for (int i = 0; i < 20; i++) {
			speechSaler.transform.position += new Vector3 (dir * (newPos - oldPos).x / 20, 0f);
			yield return new WaitForEndOfFrame();
		}
		if (isStart)
			speechSaler.transform.position = newPos;
		else
			speechSaler.transform.position = oldPos;
		if (!isStart)
			isSpeakSaler = false;
	}

	private void writeDialog2(int whoSwitch, params string[] phrases){
		if (whoSwitch <= phrases.Length) {
			StartCoroutine (WriteText (phrases [whoSwitch - 1]));
			if (!isSpeakGary && (phrases [whoSwitch - 1] [0] == 'г' || phrases [whoSwitch - 1] [2] == 'г')) {
				if (isSpeakSaler)
					StartCoroutine(salerSpeak (false));
				StartCoroutine(garySpeak (true));
			}
			if (!isSpeakSaler && (phrases [whoSwitch - 1] [0] == 'п' || phrases [whoSwitch - 1] [2] == 'п')){
				if (isSpeakGary)
					StartCoroutine(garySpeak(false));
				StartCoroutine(salerSpeak(true));
			}
		}
	}

	protected override void BuyProduct ()
	{
		base.BuyProduct ();
		mainText.text = "";
		if (isSpeakGary)
			StartCoroutine (garySpeak (false));
		if (isSpeakSaler)
			StartCoroutine (salerSpeak (false));
	}

	private void eggsRoad(bool isStart){
		if (isStart) {
			rooms = new GameObject[7];
			rooms [0] = GameObject.Find ("room10");
			rooms [1] = GameObject.Find ("room11");
			rooms [2] = GameObject.Find ("room12");
			rooms [3] = GameObject.Find ("room13");
			rooms [4] = GameObject.Find ("room14");
			rooms [5] = GameObject.Find ("room15");
			rooms [6] = GameObject.Find ("room9");
		}
		for (int i = 0; i < 5; i++)
			rooms [i].SetActive (!isStart);
		
		Color glass = new Color (0f, 0f, 0f, 0f);//for room15
		Color darkness = Color.white;//for room9
		if (!isStart) {
			glass = new Color (1f, 1f, 1f, 1f);
			darkness = new Color(0.2f, 0.2f, 0.2f, 0.95f);
		}
			
		foreach (var obj in objects)
			obj.Value.GetComponent<SpriteRenderer> ().color = glass;
		foreach (var prod in products)
			prod.Value.Obj.GetComponent<SpriteRenderer> ().color = glass;
		/*for (int i = 0; i < gameObject.transform.childCount; i++)
			if (gameObject.transform.GetChild (i) != Past && gameObject.transform.GetChild (i) != Future)
				gameObject.transform.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().color = glass;*/			
		if (isStart) {
			money2 = GameObject.Find ("Money(Clone)");
			lightSound.Play ();
		}
		money2.SetActive (!isStart);
		Year.SetActive (!isStart);
		Past.SetActive (!isStart);
		Future.SetActive (!isStart);
		MoneySymbol.SetActive (!isStart);
		foreach (var obj in rooms[6].GetComponent<Room9>().objects) {
			obj.Value.GetComponent<SpriteRenderer> ().color = darkness;
		}
		foreach (var prod in rooms[6].GetComponent<Room9>().products) {
			var ren = prod.Value.Obj.GetComponent<SpriteRenderer> ();
			if (ren != null)
				ren.color = darkness;
		}
	}

	public override void SetGUIActive (bool setTo)
	{
		base.SetGUIActive (setTo);
		if (setTo == true)
			StartCoroutine (salerSpeak (false));
	}

	public IEnumerator setDark(){
		StartCoroutine (salerSpeak (false));
		yield return new WaitForSeconds (1f);
		SetDarkness (true);
		yield return null;
	}

	public override void IsEndTime(){
		currentTime = timeInRooms.End;
		if (choosenProductName == "exit")
			return;
		foreach (var prod in products) {
			if (prod.Value.Obj.activeInHierarchy && prod.Value.Price <= Money && prod.Key != "молоко") {
				isPostShopTime = false;
				isShopTime = true;
				currentTime = timeInRooms.Shop;
				break;
			}
		}
	}


	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue){
			Object.Destroy (speechGary);
			Object.Destroy (speechSaler);
			//Object.Destroy (mainText.gameObject);
			StartCoroutine (GameController.Instance.LoadRoom16 ());
		}
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["TV"] = "хорошо, что репортаж не про кошек - их показывают последние 2 недели! " +
			"буэ, они противные и пытаются меня обмануть.";
		objectPhrases ["Saler"] = "если честно, я хочу, чтобы он умер - можно будет спокойно ходить за продуктами.";
		objectPhrases ["Shop"] = "ему бы прибраться здесь, что ли.";
		objectPhrases ["Shkaf"] = "good old shkaf. мне нравится, что он такой... тихий.";
		objectPhrases ["Stoika"] = "стойке нехорошо.";
		objectPhrases ["Picture"] = "подпись: \"эта машина настолько занижена, что находится под асфальтом.\" это символ денег.";
		objectPhrases ["Call"] = "вот бы он никогда не менялся.";

		objectPhrases ["Past"] = "я помню лицо продавца - оно всегда становилось хуже.";
		objectPhrases ["Future"] = "будущее темно и пусто. похоже на отношения с девушкой.";
	}
}
