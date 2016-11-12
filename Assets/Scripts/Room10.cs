using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Room10 : Room {

	public override void Instantiate ()
	{
		Room9[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room9> ();
		InstantiateFromPrevious (rooms);

		Money = 68;
		objects.Add ("Boxes", new GameObject ("Boxes"));
		objects.Add ("Books", new GameObject ("Books"));
		objects.Add ("TableBook", new GameObject ("TableBook"));

		deleteObjects ("Graphic");
		deleteProducts ("яблоки", "крупа", "мед");
	
		products ["шоколад"].Price = 16;
		products ["молоко"].Price = 5;
		products ["хлеб"].Price = 4;
		products ["яйца"].Price = 10;
		products.Add ("курица", new Products (new GameObject ("Chicken"), 39));
		products.Add ("пицца", new Products (new GameObject ("Pizza"), 35));
		products.Add ("сосиски", new Products (new GameObject ("Sausages"), 9));
		products.Add ("сыр", new Products (new GameObject ("Cheese"), 19));
	}

	public override void changeRoom(){

		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler10");
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_11");
		objects ["Boxes"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Boxes");
		objects ["Books"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Books");
		objects ["TableBook"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Book");
		objects ["Picture"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Picture9_10");

		products ["курица"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Chicken");
		products ["пицца"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Pizza");
		products ["сосиски"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Sausages");
		products ["сыр"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Cheese");
	}

	public override void SetPositionsAndColliders ()
	{
		base.SetPositionsAndColliders ();
		objects ["Boxes"].transform.localPosition = new Vector3 (-0.21f, -1.01f);
		objects ["Books"].transform.localPosition = posProducts [2];
		objects ["TableBook"].transform.localPosition = new Vector3 (-1.32f, -0.59f);
		int i = 0;
		foreach (var prod in products) {
			if (i == 0)
				prod.Value.Obj.transform.localPosition = posProducts [9];
			if (i == 2) {
				prod.Value.Obj.transform.localPosition = posProducts [8];
				break;
			}
			i++;
		}
		products ["хлеб"].Obj.transform.localPosition = posProducts [0];
	}

	public override void TimeStart ()
	{
		WriteStartDialog ("p продавец: привет, гари!",
			"продавец: уже год, как ты переехал!",
			"продавец: поздравляю!",
			"p гари: ты что, считал?",
			"продавец: у меня есть записи о каждом клиенте, так проще делать анализ рынка.",
			"продавец: к тому же, наш магазин открылся ровно год назад.",
			"продавец: твоей семье нравится здесь?",
			"гари: не особо, рядом шоссе, по ночам мешает спать.",
			"продавец: да, сумасшедший райончик, я уже написал мэру, чтобы построили шумоподавители.",
			"гари: сомневаюсь, что мэр что-то построит.",
			"продавец: уверен, что построит!",
			"продавец: система обращений к властям стала проще, он точно прочитает письмо.",
			"p продавец: что берешь?");
	}

	public override void TimeEnd ()
	{
		WriteEndDialog ("продавец: уже пошел?",
			"продавец: ладно, буду читать дальше.",
			"p продавец: ты знаешь, как раньше обращались с политзаключенными?",
			"продавец: ужас!",
			"гари: (мне это не нравится.)"
		);
		if (endClicks == 3)
			objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = true;
		if (endClicks == 4)
			objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = false;
	}

	public override void ShopTime(){

		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "молоко":
				WriteShopDialogWithEndPhrase ("продавец: прикольно, да?", 
					"продавец: другие продавцы теперь пытаются украсть у меня телефонную книгу.",
					"продавец: хотят узнать, где я его беру.",
					"продавец: но это честная конкуренция, не более."
				);
				break;
			case "хлеб":
				switch (shopClicks) {
				case 1:
					StartCoroutine(WriteText ("продавец: только осторожно!"));
					break;
				case 2:
					StartCoroutine (bread ());
					StartCoroutine(WriteText ("продавец: черт, теперь книги в хлебных крошках."));
					break;
				case 3:
					StartCoroutine (WriteText ("гари: зачем их держать в шкафу с продуктами?"));
					break;
				case 4:
					StartCoroutine(WriteText ("продавец: я боюсь, что их опять запретят."));
					break;
				case 5:
					StartCoroutine (WriteText ("продавец: наверное это глупо - старый режим никогда не вернется."));
					break;
				case 6:
					StartCoroutine(WriteText ("продавец: и слава богу, да?"));
					buttons [choosenProductName].GetComponentInChildren<Text> ().text = "";
					shopClicks = 0;
					choosenProductName = null;
					break;
				}
				break;
			case "яйца":
				WriteShopDialog( "продавец: как покупать свежие яйца на мальте по 7 центов за штуку, " +
					"а продавать по 5 с выгодой?",
					"гари: ...",
					"p продавец: вот и я не знаю."
				);
				break;
			case "курица":
				WriteShopDialog( "продавец: цыплята бройлер! уже boiler, u know?",
					"гари: о чем ты?",
					"продавец: мода на иностранные слова! мне тоже не нравится, если честно.");
				break;
			case "пицца":
				WriteShopDialog( "гари: можно вопрос?",
					"продавец: да.",
					"гари: представь абстрактную компьютерную игру, в которой действие происходит в продуктовом магазине.",
					"гари: какую бы она мысль могла донести?",
					"продавец: компьютерную? я еще не умею им пользоваться.",
					"продавец: думаю записаться на курсы, говорят, так удобнее вести учет.",
					"p продавец: а вопрос вырван из контекста.");
				break;
			case "шоколад":
				WriteShopDialogWithEndPhrase ( "гари: (с орехом! эта штука никогда не надоест.)");
				break;
			case "сосиски":
				WriteShopDialogWithEndPhrase ( "продавец: самый популярный продукт, righ'?", 
					"продавец: вместе с ним нужно продавать иностранные кеды, кепки и спорткостюмы -", 
					"продавец: люди стояли бы толпами.");
				break;
			case "сыр":
				WriteShopDialog( "гари: почему он похож на кусок спанчбоба?",
					"продавец: ого, ты тоже начал смотреть мультики?");
				break;
			}
		}

		//check if the player can't buy anything
		if (shopClicks == 0)
			IsEndTime();
	}

	private IEnumerator bread(){
		GameObject kroshki = GameObject.Instantiate (Resources.Load<GameObject> ("ParticlesForRoom10"));
		kroshki.transform.SetParent (products["хлеб"].Obj.transform);
		kroshki.transform.localPosition = Vector3.zero;
		var particles = kroshki.GetComponent<ParticleSystem> ();

		particles.Play ();

		yield return new WaitForSeconds (0.4f);
		particles.Stop ();
		shopBell.GetComponent<AudioSource> ().Play ();
		Money -= products [choosenProductName].Price;
		products [choosenProductName].Obj.SetActive (false);
	}

	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue)
			StartCoroutine(GameController.Instance.LoadRoom11 ());
	}


	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["Saler"] = "почему он такой веселый сегодня? я не доверяю ему.";
		objectPhrases ["TV"] = "все взрывы, взрывы. где-то идет война, а мы радуемся победе. нааайс, но только для людей, которые умеют радоваться.";
		objectPhrases ["Picture"] = "\"стройка продолжается\" - новая фотография с подписью.";
		objectPhrases ["Gary"] = "я хотел не приходить сюда, но родители всегда посылают за продуктами.";

	}
}
