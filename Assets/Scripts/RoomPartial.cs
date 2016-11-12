using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//all presets and options for rooms here: set colliders, positions, GUI, color of rooms etc.
public abstract partial class Room : MonoBehaviour {

	public static Vector3 posSaler = new Vector3 (-1.98f, -0.92f);
	public static Vector3 posStoika = new Vector3 (-1.17f, -1.2f);
	public static Vector3 posShop = Vector3.zero;
	public static Vector3 posShkaf = new Vector3 (1.41f, -0.2f, -0.2f);
	public static Vector3 posLamp = new Vector3 (-0.05f, 1.4f, -0.3f);
	public static Vector3 posPicture = new Vector3 (-1.72f, 0.53f, -0.4f);
	public static Vector3 posGary = new Vector3 (-0.25f, -1.1f);
	public static Vector3 posTV = new Vector3 (-0.37f, 0.22f, -0.4f);
	public static Vector3 posGrafic = new Vector3 (-1.29f, -1.21f);
	public static Vector3 posCall = new Vector3 (2.58f, 0.84f);
	public static Vector3 posCup = new Vector3 (-1.084f, -0.5f);
	public static Vector3 posDoor = new Vector3 (2.59f, -0.61f);
	public static Vector3 posMoney = new Vector3 (0.5f, 1.75f);
	public static Vector3 posMoneySymbol = new Vector3 (2.307f, 1.65f);
	public static Vector3 posYear = new Vector3 (-1.1f, 1.7f);

	public static Vector3[] posProducts = {
		new Vector3 (0.745f, 0.5f), new Vector3 (1.362f, 0.5f),
		new Vector3 (0.754f, -0.233f), new Vector3 (1.362f, -0.233f),
		new Vector3 (0.745f, -0.954f), new Vector3 (1.362f, -0.954f),
		new Vector3 (0.745f, -1.686f), new Vector3 (1.362f, -1.686f),
		new Vector3 (2.093f, 0.707f), new Vector3 (2.093f, -0.047f),
		new Vector3 (2.093f, -0.78f), new Vector3 (2.093f, -1.502f)
	};

	public virtual void SetPositionsAndColliders(){
		foreach (var obj in objects) {
			switch (obj.Key) {
			case "Saler":
				obj.Value.transform.localPosition = posSaler;
				break;
			case "Stoika":
				obj.Value.transform.localPosition = posStoika;
				break;
			case "Shop":
				obj.Value.transform.localPosition = posShop;
				break;
			case "Lamp":
				obj.Value.transform.localPosition = posLamp;
				break;
			case "Picture":
				obj.Value.transform.localPosition = posPicture;
				break;
			case "Gary":
				obj.Value.transform.localPosition = posGary;
				break;
			case "TV":
				obj.Value.transform.localPosition = posTV;
				break;
			case "Shkaf":
				obj.Value.transform.localPosition = posShkaf;
				break;
			case "Graphic":
				obj.Value.transform.localPosition = posGrafic;
				break;
			case "Call":
				obj.Value.transform.localPosition = posCall;
				break;
			case "Cup":
				obj.Value.transform.localPosition = posCup;
				break;
			case "Door":
			case "DoorHole":
				obj.Value.transform.localPosition = posDoor;
				break;
			}
		}
		MoneyObj.transform.position = transform.position + Room.posMoney;
		MoneySymbol.transform.position = transform.position + Room.posMoneySymbol;
		Year.transform.position = transform.position + Room.posYear;
		Past.transform.localPosition = Room.posShop - objects ["Shop"].GetComponent<SpriteRenderer> ().bounds.extents;
		Past.transform.localPosition = new Vector3 (Past.transform.localPosition.x, 0);

		Future.transform.localPosition = Room.posShop + objects ["Shop"].GetComponent<SpriteRenderer> ().bounds.extents;
		Future.transform.localPosition = new Vector3(Future.transform.localPosition.x, 0);

		int i = 0;
		foreach (var prod in products) {
			prod.Value.Obj.transform.localPosition = posProducts [i];
			i++;
		}
		SetColliders ();
	}

	public virtual void SortInLayers(int numOfYears){
		numOfYear = numOfYears;
		Color yearColor = Color.black;
		if (numOfYears > 14)
			Year.transform.position = new Vector3 (10000f, 10000f);
		else {
			yearColor = new Color (0f, 0f, 0f, 1f / 6 * (15 - numOfYears));	
			Year.GetComponentInChildren<Text> ().color = yearColor; 
			Year.GetComponentInChildren<Text> ().text = numOfYears + " лет";
		}

		MoneySymbol.GetComponent<SpriteRenderer> ().sortingLayerName = "room" + numOfYear;
		MoneySymbol.GetComponent<SpriteRenderer> ().sortingOrder = 2;
		foreach (var obj in objects) {
			SpriteRenderer sprRen = obj.Value.GetComponent<SpriteRenderer> ();
			sprRen.sortingLayerName = "room" + numOfYears.ToString ();
			switch (obj.Key) {
			case "Shop":
				sprRen.sortingOrder = 0;
				break;
			case "TV":
				sprRen.sortingOrder = 1;
				break;
			case "Picture":
				sprRen.sortingOrder = 1;
				break;
			case "Boxes":
				sprRen.sortingOrder = 2;
				break;
			case "Stoika":
				sprRen.sortingOrder = 2;
				break;
			case "Shkaf":
				sprRen.sortingOrder = 2;
				break;
			case "Graphic":
				sprRen.sortingOrder = 3;
				break;
			case "Cup":
				sprRen.sortingOrder = 3;
				break;
			case "TableBook":
				sprRen.sortingOrder = 4;
				break;
			case "Door":
				sprRen.sortingOrder = 4;
				break;
			case "Books":
				sprRen.sortingOrder = 5;
				break;
			case "Call":
				sprRen.sortingOrder = 6;
				break;
			case "DoorHole":
				sprRen.sortingOrder = 6;
				break;
			case "Saler":
				sprRen.sortingOrder = 9;
				break;
			case "Gary":
				sprRen.sortingOrder = 12;
				break;	
			}
		}
		foreach (var prod in products) {
			SpriteRenderer sprRen = prod.Value.Obj.GetComponent<SpriteRenderer> ();
			sprRen.sortingLayerName = "room" + numOfYears.ToString ();
			sprRen.sortingOrder = 5;
		}
	}

	public virtual void InstantiateFromPrevious(Room[] rooms){
		var prevDict = rooms[0].objects;
		foreach (var obj in prevDict)	
			if (obj.Value != null) {
				objects.Add (obj.Key, (GameObject)Object.Instantiate (obj.Value));
			}
		var prevDict2 = rooms[0].products;
		foreach (var prod in prevDict2)	
			if (prod.Value.Obj != null) {
				products.Add (prod.Key, new Products((GameObject)Object.Instantiate (prod.Value.Obj), prod.Value.Price));
			}
	}

	public virtual void SetObjectsPhrases(){
		objectPhrases = new Dictionary<string, string> ();

		objectPhrases.Add ("Shop", "это магазин. довольно мило тут.");
		objectPhrases.Add ("Picture", "картина, на ней закат. надеюсь, не закат общества.");
		objectPhrases.Add ("TV", "это плазма? по ней одни новости, и каналы непонятные - мои родители такие не смотрят.");
		objectPhrases.Add ("Stoika", "железная стойка и так приятно выглядит. под ней, скорей всего, кассовый аппарат и радио.");
		objectPhrases.Add ("Saler", "он что, всегда говорит о политике? странный тип.");
		objectPhrases.Add ("Cup", "кофе, аромат чувствуется даже у двери. у продавца, видимо, хорошее настроение.");
		objectPhrases.Add ("TableBook", "эта книга, о чем она? продавец громко говорит - почти кричит - надеюсь, не из-за этой книги.");
		objectPhrases.Add ("Gary", "это я - единственный адекватный человек. все остальные пытаются навязать свои идеи. и почему-то всегда именно мне!");
		objectPhrases.Add ("Shkaf", "это похоже на шкаф, но на самом деле это коричневый холодильник с прозрачной дверцей. " +
		"он мимикрировал, чтобы не слышать идеи других холодильников. как жизненно.");
		objectPhrases.Add ("Lamp", "люстра, 5 лампочек, 1, 2, 3 плафона. стоп, чего?");
		objectPhrases.Add ("Call", "звонок. выглядит так классически, наверное, и мысли у него самые стандартные. " +
		"just doing my job. вот бы и продавец так говорил, а он не затыкается.");
		objectPhrases.Add ("Door", "это дверь, за ней автомагистраль - там одиноко, а здесь кричащий продавец. " +
		"между молотом и наковальней.");
		objectPhrases.Add ("DoorHole", "небо и крыльцо к первому этажу. продавец говорит, что в жилых домах нельзя " +
		"открывать магазины, но налоговые пока его не докучали.");
		objectPhrases.Add ("Graphic", "распродажа в день открытия! нааайс.");
		objectPhrases.Add ("Gun", "ого, пистолет! игра сразу стала угрожающе опасной. но какой он красивый, все-таки!");
		objectPhrases.Add ("Boxes", "коробки из кеи. все удачливые бизнесмены покупают там мебель. это мода.");
		objectPhrases.Add ("Books", "н-а-м-е-д-н-и. что он читает?");

		objectPhrases.Add ("мед", "хмм, что может поведать этот мед? он чертовски дорогой.");
		objectPhrases.Add ("курица", "это была \"полная жизненных сил\" курица. таких почти сразу убивает общество и " +
		"использует для собственных целей.");
		objectPhrases.Add ("рыба", "похоже на окуня - очень много чешуи, чтобы едоки подавились. " +
		"полезно для косяка рыб, и совершенно беcполезно для съеденного окуня.");
		objectPhrases.Add ("молоко", "из него можно сделать все что угодно - от сливок до творога. " +
			"крафтинг в игре!");
		objectPhrases.Add ("сгущенка", "сладкая до ужаса, всегда беру ее, если не хочется готовить.");
		objectPhrases.Add ("крупа", "8 злаков. стоп, 7? в составе 7. слезы, обманутое человечество. " +
		"как всегда.");
		objectPhrases.Add ("шоколад", "правда, из-за границы? интересно, как там живут люди? " +
		"отец говорит, что там все обманщики и воры, значит, как здесь.");
		objectPhrases.Add ("хлеб", "свежий, несвежий, черный, белый, разных форм и размеров. " +
		"демократия?");
		objectPhrases.Add ("яйца", "ну давай, пошути грубо и пошло. " +
		"президент же так делает, он думает, что это смешно. (вздыхает) первый человек страны.");
		objectPhrases.Add ("яблоки", "они круглые или нет?");
		objectPhrases.Add ("бананы", "самый важный вопрос в этой игре: нужно ли мыть бананы? похоже на начало lucky star.");
		objectPhrases.Add ("масло", "ничего не могу с ним сделать. в этой игре должна быть масленка или типа того.");
		objectPhrases.Add ("торт", "круто, со сметаной и сочными песочными коржиками. люблю такой. " +
			"подарил бы своей девушке, но у меня ее почему-то нет. может, я их всех умней?");
		objectPhrases.Add ("пицца", "я возьму ее, сяду на мопед и уеду по извилистой дорожке в закат, крича \"Чао!\".");
		objectPhrases.Add ("сосиски", "мать сказала купить. их проще готовить, чем обычное мясо - мать говорит так.");
		objectPhrases.Add ("сыр", "так все-таки почему он похож на кусок спанчбоба?");
	
		objectPhrases.Add ("MoneySymbol", "госты. почему они так называются? можно же просто \"деньги\".");
		objectPhrases.Add ("Past", "оглядываясь назад, кажется, что я что-то сделал неправильно.");
		objectPhrases.Add ("Future", "впереди темно и пусто. там автомагистраль, а за ней мой дом, в котором родители " +
		"вечно орут друг на друга и на меня.");
		objectPhrases.Add ("деньги", "");
	}

	public virtual void SetGUI(){

		buttonsObj = GameObject.FindGameObjectsWithTag ("button");
		for (int i = 0; i < buttonsObj.Length - 1; i++) {
			for (int j = 0; j < buttonsObj.Length - i - 1; j++) {
				if (string.Compare( buttonsObj [j].name, buttonsObj [j + 1].name) > 0) {
					GameObject temp = buttonsObj [j];
					buttonsObj [j] = buttonsObj [j + 1];
					buttonsObj [j + 1] = temp;
				}
			}
		}
		buttons = new Dictionary<string, Button> ();
		foreach (var button in buttonsObj) {
			button.GetComponentInChildren<Text> ().text = "";
		}
		exit.gameObject.GetComponentInChildren<Text> ().text = "";
	}

	public virtual void SetGUIActive(bool setTo){
		if (!setTo)
			foreach (var button in buttonsObj) {
				button.GetComponentInChildren<Text>().text = "";
			}
		if (setTo)
			exit.gameObject.GetComponentInChildren<Text> ().text = "Exit";
		else
			exit.gameObject.GetComponentInChildren<Text> ().text = "";
	}

	public virtual void SetProducts (){
		int i = 0;
		foreach (var prod in products) {
			buttons.Add (prod.Key, buttonsObj [i].GetComponent<Button> ());
			buttons [prod.Key].GetComponentsInChildren<Text> ()[0].text = prod.Key + " " + prod.Value.Price;
			i++;
		}
		exit.GetComponentInChildren<Text> ().text = "Exit";
	}

	public virtual void SetParents(){ //for every object in room
		foreach (var prod in products) {
			prod.Value.Obj.transform.SetParent (gameObject.transform);
		}
		foreach (var obj in objects) {
			obj.Value.transform.SetParent (gameObject.transform);
		}
		var canvas = GameObject.Find ("Canvas");
		MoneyObj.transform.SetParent (canvas.transform, false);
		MoneySymbol.transform.SetParent (gameObject.transform);
		Year.transform.SetParent (canvas.transform, false);
		Past.transform.SetParent (gameObject.transform);
		Future.transform.SetParent (gameObject.transform);
	}

	public virtual void SetColliders(){
		foreach (var obj in objects) {
			if (obj.Value.GetComponent<BoxCollider2D>() == null)
				obj.Value.AddComponent<BoxCollider2D> ().size = obj.Value.GetComponent<SpriteRenderer> ().bounds.size;
		}
		foreach (var prod in products) {
			if (prod.Value.Obj.GetComponent<BoxCollider2D>() == null)
				prod.Value.Obj.AddComponent<BoxCollider2D> ().size = 
					prod.Value.Obj.GetComponent<SpriteRenderer> ().bounds.size;
		}
		if (MoneySymbol.GetComponent<BoxCollider2D>() == null)
			MoneySymbol.AddComponent<BoxCollider2D> ().size = MoneySymbol.GetComponent<SpriteRenderer> ().bounds.size;
		MoneyObj.GetComponent<BoxCollider2D> ().offset = new Vector2 (40.8f, -4f);
		Year.GetComponent<BoxCollider2D> ().offset = new Vector2 (-40.8f, -4f);
		Past.GetComponent<BoxCollider2D> ().offset = new Vector2 (- Past.GetComponent<BoxCollider2D> ().size.x / 2, 0);
		var size = objects ["Shop"].GetComponent<SpriteRenderer> ().bounds.size;
		Past.GetComponent<BoxCollider2D> ().size = new Vector2 (Past.GetComponent<BoxCollider2D> ().size.x, size.y);
		Future.GetComponent<BoxCollider2D> ().size = new Vector2 (Future.GetComponent<BoxCollider2D> ().size.x, size.y);
	}

	public virtual void SetDarkness(bool darkIsTrue){

		Color darkness = new Color(0.2f, 0.2f, 0.2f, 0.95f);
		if (!darkIsTrue)
			darkness = Color.white;

		foreach (var obj in objects) {
			obj.Value.GetComponent<SpriteRenderer> ().color = darkness;
		}
		foreach (var prod in products) {
			var ren = prod.Value.Obj.GetComponent<SpriteRenderer> ();
			if (ren != null)
				ren.color = darkness;
		}

		if (darkIsTrue) {
			MoneyObj.SetActive (false);
			MoneySymbol.SetActive (false);
			Year.SetActive (false);
			Object.Destroy (Past);
			Object.Destroy (Future);
			//MoneySymbol.GetComponent<SpriteRenderer> ().color = darkness;
			StartCoroutine (GameController.Instance.backgroundMusic.FadeOut ());
			//GameController.Instance.backgroundMusic.Stop();
			mainText.text = "";
			this.enabled = false;
		}
		lightSound.Play ();
	}
}
