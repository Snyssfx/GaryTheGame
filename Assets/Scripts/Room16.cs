using UnityEngine;
using System.Collections;

public class Room16 : Room {

	public override void Instantiate () {
		Room15[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room15> ();
		InstantiateFromPrevious (rooms);

		Money = 118;
		deleteProducts ("рыба", "молоко", "яйца", "сгущенка");
		deleteObjects ("TV");

		objects.Add ("TV", GameObject.Instantiate (Resources.Load<GameObject> ("TV16")));

		products.Add ("курица", new Products (new GameObject ("Chicken"), 65));
		products.Add ("торт", new Products (new GameObject ("Pie"), 39));
		products.Add ("пицца", new Products (new GameObject ("Pizza"), 45));
		products.Add ("яблоки", new Products (new GameObject ("Apples"), 22));
		products.Add ("сыр", new Products (new GameObject ("Cheese"), 37));
		products ["хлеб"].Price = 18;
	}

	public override void changeRoom (){
		Object.Destroy (objects ["Saler"].GetComponent<Animator> ());
		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop16_18");
		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler16");
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_18");
		objects ["Picture"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Picture9_14");
		objects ["Lamp"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Lamp_17_19");

		products ["курица"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Chicken");
		products ["торт"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Pie");
		products ["пицца"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Pizza");
		products ["яблоки"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Apples");
		products ["сыр"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Cheese");
	}

	public override void TimeStart ()
	{
		objects ["TV"].GetComponent<SpriteRenderer> ().sortingOrder = 5;
		WriteStartDialog ("p продавец: привет, теперь я в интернете!",
			"продавец: подписывайся на мой канал на ютубе, ставь пальцы вверх и все такое.",
			"продавец: интернет-телевидение такое клевое, до сих пор не могу нарадоваться.",
			"продавец: из-за других законов тут интересней новости.",
			"продавец: и адекватней.",
			"продавец: что берешь?");
	}

	public override void TimeEnd ()
	{
		WriteEndDialog ("продавец: бывай.",
			"продавец: все вокруг будто свихнулись, а?",
			"гари: ..."
		);
	}

	public override void ShopTime(){
		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "курица":
				WriteShopDialogWithEndPhrase("продавец: в итоге все рушится.",
					"продавец: какие-то налоги на малый бизнес.",
					"продавец: вести дела невозможно.",
					"продавец: я работаю в убыток.",
					"продавец: не знаю, что и делать.",
					"гари: (может, он закроется?)"
				);
				break;
			case "торт":
				WriteShopDialogWithEndPhrase ("продавец: эта нефтяная игла нам не помогла.",
					"продавец: пока много сладостей, но скоро это закончится.",
					"гари: (о чем он? все же хорошо. он ненормальный.)"
				);
				break;
			case "пицца":
				WriteShopDialogWithEndPhrase ("продавец: денег на самом деле больше, чем есть у народа?",
					"гари: (ну его к черту, он свихнулся.)"
				);
				break;
			case "хлеб":
				WriteShopDialog( "продавец: люди говорят, что это правительственный заговор.",
					"продавец: в основном те люди, у которых денег хватает только на хлеб.",
					"гари: (да когда же он перестанет?)"
				);
				break;
			case "яблоки":
				WriteShopDialogWithEndPhrase ("продавец: все это странно.",
					"продавец: как круглые яблоки в пиксель-арте.",
					"гари: (он стареет.)"
				);
				break;
			case "сыр":
				WriteShopDialogWithEndPhrase ("продавец: тебе сколько лет?",
					"гари: 16.",
					"продавец: тебя уже могут посадить.",
					"гари: (я надеюсь, не все сошли с ума, как он.)"
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
		objects ["TV"].transform.FindChild ("Laptop").localPosition = new Vector3 (-0.7915f, -0.638f);
	}

	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue)
			StartCoroutine (GameController.Instance.LoadRoom17 ());
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["TV"] = "канал \"3 полоски\". отчим его ненавидит. впрочем, как и все вокруг. хочу убежать.";
		objectPhrases ["Lamp"] = "она показывает странный фалический символ своими лампами.";
		objectPhrases ["Saler"] = "пожалуйста, пусть он перестанет кричать мне в ухо, что страна разваливается! он оглох, что ли.";
		objectPhrases ["Future"] = "самое хорошее место - автомагистраль. там много людей, и никто не рассказывает про свои убеждения.";
		objectPhrases ["Shop"] = "магазину нехорошо.";
		objectPhrases ["Stoika"] = "стойке нехорошо, а музыка та же.";
	}
}
