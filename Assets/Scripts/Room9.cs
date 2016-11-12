using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Room9 : Room {

	private bool isTutorial = true;

	public override void Instantiate ()
	{
		Money = 24;
		objects.Add ("Saler", new GameObject ("Saler"));
		objects.Add ("Stoika",new GameObject ("Stoika")); 
		objects.Add ("Shop",new GameObject ("Shop")); 
		objects.Add ("Lamp",new GameObject ("Lamp"));
		objects.Add ("Picture",new GameObject ("Picture")); 
		objects.Add ("Gary",new GameObject ("Gary"));
		objects.Add ("TV",new GameObject ("TV"));
		objects.Add ("Shkaf",new GameObject ("Shkaf"));
		objects.Add ("Graphic",new GameObject ("Graphic"));
		objects.Add ("Call",new GameObject ("Call"));
		objects.Add ("Cup",new GameObject ("Cup"));
		objects.Add ("Door", new GameObject ("Door"));

		products.Add ("хлеб", new Products(new GameObject ("Bread"), 2));
		products.Add ("молоко", new Products(new GameObject ("Milk"), 3));
		products.Add ("яйца", new Products(new GameObject ("Eggs"), 6));
		products.Add ("яблоки", new Products(new GameObject ("Apples"), 8));
		products.Add ("шоколад", new Products(new GameObject ("Snickers"), 10));
		products.Add ("крупа", new Products(new GameObject ("Krupa"), 7));
		products.Add ("мед", new Products(new GameObject ("Honey"), 11));
	}

	public override void changeRoom(){

		foreach (var obj in objects) {
			var sprRen = obj.Value.AddComponent<SpriteRenderer> ();
			switch(obj.Key){
			case "Saler":
				sprRen.sprite = Resources.Load<Sprite> ("Saler9");
				break;
			case "Stoika":
				sprRen.sprite = Resources.Load<Sprite> ("Stoika9_14");
				break;
			case "Shop":
				sprRen.sprite = Resources.Load<Sprite> ("Shop2");
				break;
			case "Lamp":
				sprRen.sprite = Resources.Load<Sprite> ("Lamp_9-13");
				break;
			case "Picture":
				sprRen.sprite = Resources.Load<Sprite> ("Picture9");
				break;
			case "Gary":
				sprRen.sprite = Resources.Load<Sprite> ("Gary_9");
				break;
			case "TV":
				sprRen.sprite = Resources.Load<Sprite> ("TV");
				Animator animTV = obj.Value.AddComponent<Animator> ();
				animTV.runtimeAnimatorController = Resources.Load ("TV1") as RuntimeAnimatorController;
				break;
			case "Shkaf":
				sprRen.sprite = Resources.Load<Sprite> ("Shkaf9_13");
				break;
			case "Graphic":
				sprRen.sprite = Resources.Load<Sprite> ("Sale");
				break;
			case "Call":
				sprRen.sprite = Resources.Load<Sprite> ("Call");
				break;
			case "Cup":
				sprRen.sprite = Resources.Load<Sprite> ("Cup");
				break;
			case "Door":
				sprRen.sprite = Resources.Load<Sprite> ("Door");
				break;
			}


		}
		foreach (var prod in products) {
			var sprRen = prod.Value.Obj.AddComponent<SpriteRenderer> ();
			switch (prod.Key) {
			case "молоко":
				sprRen.sprite = Resources.Load<Sprite> (@"Products\Milk");
				break;
			case "хлеб":
				sprRen.sprite = Resources.Load<Sprite> (@"Products\Bread2");
				break;
			case "яйца":
				sprRen.sprite = Resources.Load<Sprite> (@"Products\Eggs");
				break;
			case "яблоки":
				sprRen.sprite = Resources.Load<Sprite> (@"Products\Apples");
				break;
			case "крупа":
				sprRen.sprite = Resources.Load<Sprite> (@"Products\Krupa");
				break;
			case "мед":
				sprRen.sprite = Resources.Load<Sprite> (@"Products\Honey");
				break;
			case "шоколад":
				sprRen.sprite = Resources.Load<Sprite> (@"Products\Snickers");
				break;
			}
		}
	}

	public override void TimeStart ()
	{
		WriteStartDialog ("p продавец: ЛЕВАЯ КНОПКА МЫШИ! нажми ПРОБЕЛ, чтобы слушать дальше.",
			"гари: ПРОБЕЛ? ты о чем?",
			"продавец: это кнопка на клавиатуре компьютера!",
			"гари: но компьютеров еще нет.",
			"продавец: тссссс, начинается!",
			"p продавец: привет, наш магазин только открылся!",
			"продавец: я долго готовился, найти хороших поставщиков не так-то просто!",
			"гари: это 6-ой магазин в квартале?",
			"продавец: да, я так и не понял, откуда другие берут продукты.",
			"продавец: у каждого продавца свои тайны.",
			"продавец: ...",
			"продавец: осмотрись здесь! нажимай ЛЕВОЙ КНОПКОЙ МЫШИ на предметы в магазине!",
			"продавец: ты можешь это делать в любой момент!",
			"продавец: вдруг тебе захочется УЗНАТЬ СЮЖЕТ.",
			"гари: (он ненормальный. это определенно.)",
			"продавец: а у нас скидки в честь открытия! что будешь брать?",
			"продавец: нажимай на кнопки внизу, чтобы ЧИТАТЬ БОЛЬШЕ диалогов! можно нажать на exit, чтобы выйти и сэкономить деньги!"
		);
		if (startClicks == 12)
			isTutorial = false;
	}

	public override void TimeEnd ()
	{
		WriteEndDialog ("продавец: это были трудные времена, малой.",
			"продавец: так приятно начать жизнь с нового листа.",
			"продавец: заходи еще!",
			"продавец: и не забывай про exit!",
			"гари: (господи, еще один постреволюционер. никогда больше сюда не зайду.)");
	}

	public override void ShopTime(){

		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "молоко":
				WriteShopDialogWithEndPhrase ("продавец: смотри, видишь форму бутылки?", 
					"продавец: она не треугольная, как обычно.",
					"продавец: это форма свободы.");
				break;
			case "хлеб":
				WriteShopDialogWithEndPhrase ("продавец: свежий хлеб!", 
					"продавец: после отмены стандарта производители могут " +
					"печь хлеб любой формы и состава.",
					"продавец: и он лучший в этом районе!"
				);
				break;
			case "яйца":
				WriteShopDialog ("продавец: халяльные яйца!",
					"продавец: зато дешевле не найти.",
					"продавец: законы изменились, теперь можно исповедовать что угодно.",
					"продавец: слава богу, я имею в виду!"
				);
				break;
			case "яблоки":
				WriteShopDialog ("продавец: хах, самое интересное в этих яблоках",
					"продавец: то, что они есть.",
					"p продавец: серьезно, так можно про многое сказать!");
				break;
			case "крупа":
				WriteShopDialog( "продавец: эта гречка дорого стоит.",
					"продавец: но законы рынка все устаканят, понимаешь?"
				);
				break;
			case "шоколад":
				WriteShopDialogWithEndPhrase ("продавец: из-за границы. круто, правда?",
					"продавец: потом будет больше - их раскупают.");
				break;
			case "мед":
				WriteShopDialog( "продавец: пока пчелиных ферм мало." +
					" разводить пчел - как разводить бидрилл - очень сложно.", "гари: бидрилл?", 
					"продавец: это из иностранного мультика, ты не смотрел? я специально раньше просыпаюсь."
				);
				break;
			}
		}

		//check if the player can't buy anything
		IsEndTime();
	}

	public override void RoomCycle ()
	{
		if (isTutorial)
			base.RoomCycle ();
	}

	public override void clickOnRoom ()
	{
		if (Input.GetMouseButton(0) && !isWriteText) {
			clickOnSomething = getNameOfClickedObject ();
			if (clickOnSomething == null)
				return;
			//
			isTutorial = true;
			//
			if (clickOnSomething.Contains ("продукт")) {
				var prod = clickOnSomething.Split (' ');
				StartCoroutine (WriteText ("гари: (это " + prod [1] +
					", стоит " + prod [2] + ". " + objectPhrases [prod [1]] + ")"));
				return;
			} 
			if (clickOnSomething.Contains ("деньги")) {
				var mon = clickOnSomething.Split (' ');
				StartCoroutine (WriteText ("гари: (у меня " + mon [1] + " гост/а/ов. " + objectPhrases["деньги"] + ")"));
				return;
			}
			if (clickOnSomething.Contains ("лет")) {
				var years = clickOnSomething.Split (' ');
				StartCoroutine (WriteText ("гари: (мне " + years [1] + " лет.)"));
				return;
			} 
			if (objectPhrases.ContainsKey (clickOnSomething)) {
				StartCoroutine (WriteText ("гари: (" + objectPhrases [clickOnSomething] + ")"));
				return;
			}
		}
		clickOnSomething = null;
	}

	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue)
			StartCoroutine (GameController.Instance.LoadRoom10 ());
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["Saler"] = "продавец. интересно, он в каждом предложении восклицает? странный тип.";
		objectPhrases ["Shkaf"] = "обычный шкаф. но как в нем хранится молоко? это холодильник!?";
		objectPhrases ["Call"] = "звонок для двери. наверное, для него сложно найти прикольные звуки в интернете - вот он и не работает.";
		objectPhrases ["Picture"] = "фотография с подписью. \"строим новую жизнь\" - похоже на рекламный слоган.";
	}
}
