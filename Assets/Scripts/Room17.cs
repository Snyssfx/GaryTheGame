using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Room17 : Room {

	private float timer;
	private GameObject TimerObj;

	private float Timer {
		get { return timer; }
		set {
			timer = value;
			TimerObj.GetComponentInChildren<Text> ().text = "0|" + ((int)timer).ToString ();
		}
	}
		
	public override void Start () {
		base.Start ();
		SetGUI ();
	}

	public override void Instantiate () {
		Room16[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room16> ();
		InstantiateFromPrevious (rooms);

		Money = 590;
		deleteObjects ("TV");
		deleteProducts ("яблоки", "сыр");

		objects.Add ("TV", new GameObject ("TV"));
		objects.Add ("Graphic", new GameObject ("Graphic"));

		products ["курица"].Price = 320;
		products ["торт"].Price = 200;
		products ["пицца"].Price = 250;
		products ["хлеб"].Price = 90;
		products.Add ("бананы", new Products (new GameObject ("Bananas"), 120));
		products.Add ("масло", new Products (new GameObject ("Oil"), 170));

		TimerObj = GameObject.Instantiate (Resources.Load<GameObject> ("Money"));
		TimerObj.name = "Timer";
		var canvas = GameObject.Find ("Canvas");
		TimerObj.transform.SetParent (canvas.transform, false);
		TimerObj.transform.position = transform.position + Room.posYear + new Vector3(-2.45f, 0.5f);
		TimerObj.SetActive (false);
	}

	public override void changeRoom (){
		objects ["TV"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("TV17");
		objects ["Stoika"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Stoika20_21");
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_19");
		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler17");
		objects ["Graphic"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Graphic");
		objects ["Picture"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Picture15_18");

		products ["бананы"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Bananas");
		products ["масло"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Oil");

	}
		
	public override void TimeStart ()
	{
		if (startClicks == 3)
			StartCoroutine (startTimer ());
		WriteStartDialog ("p продавец: привет, покупай быстрее.",
			"продавец: не хватало еще, чтобы ты оказался на улице в комендантский час.",
			"продавец: у тебя около минуты.");
	}

	public override void TimeEnd ()
	{
		WriteEndDialog ("продавец: все, тебе нужно бежать. скорее!");
	}

	private IEnumerator startTimer(){
		float startTime = 59f + Time.time;
		Timer = startTime - Time.time;
		TimerObj.SetActive (true);

		while (true){
			Timer = startTime - Time.time;
			if (Timer < 0) {
				Timer = 0;
				yield return new WaitForSeconds (1f);
				currentTime = timeInRooms.End;
				firstInput = true;
				yield break;
			}
			yield return new WaitForSeconds (0.2f);
		}
	}

	public override void ShopTime(){
		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "курица":
				WriteShopDialog("продавец: ты был на митинге?",
					"гари: слушай, пошел к черту!",
					"гари: какой митинг?",
					"гари: не все тут такие же либералы, как ты!",
					"продавец: что-то еще?");
				break;
			case "торт":
				WriteShopDialogWithEndPhrase ("продавец: он как бы состоит из нефти.",
					"гари: что за бред?",
					"продавец: я пытаюсь объяснить такую цену.",
					"гари: и при чем здесь нефть?",
					"гари: ты можешь перестать повторять это про нефть?",
					"гари: что ты к ней привязался?",
					"гари: у всех вокруг куча навязчивых идей!",
					"гари: а я просто хочу нормально жить!"
				);
				break;
			case "пицца":
				WriteShopDialogWithEndPhrase ("продавец: как тебе это РНД?",
					"продавец: религизиозность, народность, десять.",
					"гари: десять? что за бред?",
					"продавец: не знаю, но все 3 слова похожи на логику последних принятых законов.",
					"гари: это что - такая шутка?",
					"продавец: ну да.",
					"гари: ненавижу, когда ты шутишь. это несмешно, перестань."
				);
				break;
			case "хлеб":
				WriteShopDialogWithEndPhrase ("продавец: я читал, что во всем виноват обристан.",
					"продавец: там есть город скал - никак не могу его найти, но он виноват во всех моих бедах.",
					"p продавец: и так говорит полстраны!",
					"гари: хватит, какая мне разница до того, что думает полстраны?",
					"гари: мне нормально.",
					"продавец: да ты просто их не видишь, а я работник в магазине продуктов - они слоняются всюду.",
					"гари: (как же он мне надоел!)"
				);
				break;
			case "бананы":
				WriteShopDialogWithEndPhrase ("продавец: видал, я выкинул телевизор.",
					"продавец: показывали одну фигню.",
					"гари: и что? мне все равно!",
					"гари: и там показывают нормальные каналы.",
					"продавец: и ты им веришь?",
					"гари: нет, но слушать тебя не лучше."
				);
				break;
			case "масло":
				WriteShopDialogWithEndPhrase ("продавец: ты как думаешь, они бесятся с жиру или действительно скоро будет кризис?",
					"p гари: (господи, когда же он закончит.)"
				);
				break;
			}
		} 

		//check if the player can't buy anything
		IsEndTime ();
	}

	public override void SetPositionsAndColliders ()
	{
		base.SetPositionsAndColliders ();
	}

	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue) {
			Object.Destroy (TimerObj);
			StartCoroutine (GameController.Instance.backgroundMusic.FadeOut ());
			StartCoroutine (GameController.Instance.LoadRoom18 ());
		}
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["TV"] = "хм, телевизора больше нет. я свой еще не выбросил - мать не дает.";
		objectPhrases ["Saler"] = "я чертовски зол сегодня - лучше бы он не говорил ничего.";
		objectPhrases ["Gary"] = "я так устал. даже двигаться не хочу.";
		objectPhrases ["Shop"] = "меня достал этот дурацкий магазин!";
		objectPhrases ["Stoika"] = "она тоже будет навязывать убеждения?!";
		objectPhrases ["Lamp"] = "пошла ты сама к черту, лампа!";
		objectPhrases ["Call"] = "чертов звонок никогда не меняется!";
		objectPhrases ["Door"] = "дверь, хоть и между молотом и наковальней, но всегда в далеке от них обоих. а я? хожу туда-обратно!";
		objectPhrases ["Future"] = "ненавижу будущее!";
		objectPhrases ["Graphic"] = "это что - какая-то зависимость?";
		objectPhrases ["деньги"] = "чертовы девальвации!";
	}
}
