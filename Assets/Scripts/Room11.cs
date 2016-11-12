using UnityEngine;
using System.Collections;

public class Room11 : Room {

	public override void Instantiate () {
		Room10[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room10> ();
		InstantiateFromPrevious (rooms);

		Money = 70;
		deleteObjects ("Boxes", "TableBook", "Books", "Cup");
		deleteProducts ("курица", "пицца", "сосиски", "яйца");

		products["хлеб"].Price = 5;
		products ["молоко"].Price = 6;
		products ["шоколад"].Price = 19;
		products ["сыр"].Price = 24;
		products.Add("рыба", new Products(new GameObject("Fish"), 46));
		products.Add("масло", new Products(new GameObject("Oil"), 28));

	}

	public override void changeRoom () {
		//Object.Destroy (products ["хлеб"].Obj.transform.GetChild (0).gameObject);
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_12");
		objects ["TV"].GetComponent<Animator> ().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("TV11_1");
		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler11");
		objects ["Lamp"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Lamp_14-16");
		objects ["Shkaf"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shkaf14_18");
		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop11_13");
		objects ["Picture"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Picture11");

		products ["масло"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Oil");
		products ["рыба"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Fish");
	}
		
	public override void TimeStart ()
	{
		WriteStartDialog ("p продавец: привет, гари!",
			"продавец: что-то неясное творится.",
			"продавец: что будешь брать?");
	}

	public override void TimeEnd ()
	{
		WriteEndDialog ("продавец: бывай.",
			"продавец: надеюсь, ничего плохого не случится.");
	}

	public override void ShopTime(){

		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "молоко":
				WriteShopDialog( "продавец: да, дорого, что же делать.",
					"продавец: завод так продает.",
					"продавец: это история про монополию и дачу взяток депутатам.",
					"p гари: все бы брали взятки на их месте.",
					"продавец: хах, я бы не брал. значит, не все.",
					"гари: это ты сейчас так говоришь.",
					"p продавец: где ты этого нахватался?"
				);
				break;
			case "хлеб":
				WriteShopDialog ("продавец: хлеб здесь будет всегда в отличие от журналиста в телеке.",
					"продавец: он стал частным детективом и расследовал выручку рыбных магнатов.",
					"продавец: но подозревать их глупо же?",
					"продавец: я имею в виду, когда твое дело расследуют, не нужно убивать детектива, это подозрительно.",
					"продавец: вот их и не подозревают.");
				break;
			case "масло":
				WriteDialog (shopClicks, "продавец: ого, ты проиграл.",
					"гари: что? почему?",
					"продавец: ты попал в дыру в сюжете.",
					"продавец: хочешь начать заново?",
					"гари: нет!",
					"продавец: интересно почему. тебе не нравится игра?",
					"p гари: не надейся, они все равно не оставят фидбэк.",
					"продавец: тогда ты просто потратил деньги и не узнаешь историю более полно.",
					"гари: а ты то ее знаешь?",
					"p продавец: я этого не говорил."
				);
				if (shopClicks == 10)
					BuyProduct ();
				break;
			case "рыба":
				WriteShopDialog( "продавец: откуда они берут деньги, гари?",
					"продавец: как можно стать миллиардером за пару лет, " +
					"если у тебя есть только завод с рыбными консервами?",
					"гари: может, все любят рыбу?",
					"продавец: ..."
				);
				break;
			case "шоколад":
				WriteDialog (shopClicks, "гари: (буэ, этот шоколад уже достал. не буду его брать.)");
				choosenProductName = null;
				shopClicks = 0;
				break;
			case "сыр":
				WriteShopDialog( "продавец: в законе есть дыра, как в этом сыре.",
					"продавец: магазины организуют картели и поднимают цены.",
					"продавец: они ведут войны против других продавцов, -",
					"продавец: я их боюсь.",
					"гари: (так вот что с ним!)");
				break;
			}
		} 

		//check if the player can't buy anything
		IsEndTime ();
	}

	public override void SetDarkness (bool darkIsTrue)
	{
		base.SetDarkness (darkIsTrue);
		if (darkIsTrue)
			StartCoroutine (GameController.Instance.LoadRoom12 ());
	}

	public override void IsEndTime(){
		currentTime = timeInRooms.End;
		if (choosenProductName == "exit")
			return;
		foreach (var prod in products) {
			if (prod.Value.Obj.activeInHierarchy && prod.Value.Price <= Money && prod.Key != "шоколад") {
				isPostShopTime = false;
				isShopTime = true;
				currentTime = timeInRooms.Shop;
				break;
			}
		}
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["Saler"] = "сегодня он... настороженный, что ли? и кофе пропал.";
		objectPhrases ["TV"] = "воу, журналист. убийство? его же уволили?";
		objectPhrases ["Shop"] = "магазин стал грязнее. и продавец выглядит уставшим.";
		objectPhrases ["Lamp"] = "лампе нехорошо.";
		objectPhrases ["Picture"] = "строят они что-то большое и стеклянное. самое страшное, если они построят, а получится что-то ужасно глупое. " +
		"разрушенные надежды и все такое.";

		objectPhrases ["шоколад"] = "буээ, как я его раньше ел? он такой невкусный!";
	}
}
