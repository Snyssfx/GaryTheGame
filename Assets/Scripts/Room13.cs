using UnityEngine;
using System.Collections;

public class Room13 : Room {

	public override void Instantiate () {
		Room12[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room12> ();
		InstantiateFromPrevious (rooms);

		Money = 20000;
		deleteProducts ("хлеб", "молоко", "яблоки");
		deleteObjects ("Gun");

		products["крупа"].Price = 10000;
		products.Add ("бананы", new Products (new GameObject ("Bananas"), 9500));
		products.Add ("сосиски", new Products (new GameObject ("Sausages"), 8000));
		products.Add ("масло", new Products (new GameObject ("Oil"), 11000));

	}

	public override void changeRoom (){
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_15");
		Object.Destroy (objects ["Saler"].GetComponent<Animator> ());
		objects ["Saler"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Saler13");
		if (objects ["TV"].GetComponent<Animator> () != null)
			objects ["TV"].GetComponent<Animator> ().runtimeAnimatorController = 
				Resources.Load<RuntimeAnimatorController> ("TV13");
		else objects ["TV"].AddComponent<Animator> ().runtimeAnimatorController = 
				Resources.Load<RuntimeAnimatorController> ("TV13");

		products ["бананы"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Bananas");
		products ["сосиски"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Sausages");
		products ["масло"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Oil");

	}

	public override void TimeStart ()
	{
		WriteStartDialog ("p продавец: хало, гари!",
			"продавец: кстати, почему у тебя такое имя?",
			"гари: родители не особо думали над именем.",
			"гари: это отец предложил, что-то языческое.",
			"гари: означает сложное языческое выражение:",
			"гари: \"вечеринка у костра, много историй, а всем вокруг скучно и один ноет о своих страхах.\"",
			"продавец: воу, сложная мифология!",
			"p продавец: я лучше буду думать про современное время.",
			"продавец: сегодня продуктов больше. трудные времена, гари.",
			"продавец: что берешь?");
	}

	public override void TimeEnd ()
	{
		WriteEndDialog ("продавец: бай. все скоро исправится.",
			"гари: слушай, можешь больше не говорить о политике? - сказал гари.",
			"гари: ты меня достал! - сказал гари.",
			"гари: ты несешь околесицу! - сказал гари.",
			"гари: блуждаешь вокруг да около, а потом вставляешь свое мнение ненароком! - сказал гари.",
			"гари: ты меня учить вздумал? - сказал гари.",
			"гари: ты говоришь лютый бред! - сказал гари.", 
			"гари: вы все наседаете на меня со своими дурацкими идеями! - сказал гари.",
			"гари: я уже устал от всех вас! - сказал гари.",
			"p но гари ничего не сказал, он боялся продавца и всех вокруг.",
			"p продавец: бай. все скоро исправится."
		);
	}

	public override void ShopTime(){
		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "бананы":
				WriteShopDialog( "продавец: сейчас не сезон, бананы везут из артзтоцки.", 
					"гари: откуда?",
					"продавец: артзтоцки.",
					"продавец: мы наладили сотрудничество сразу после того, как у них что-то взорвалось на таможне.",
					"продавец: странно, верно?",
					"гари: (он перестанет уже?)"
				);
				break;
			case "сосиски":
				WriteShopDialog( "продавец: сосиски не наши.",
					"продавец: но по телевизору говорят, что мы нашли нефть!",
					"продавец: может, в ней наше призвание?",
					"гари: (я устал.)"
				);
				break;
			case "крупа":
				WriteShopDialog( "продавец: как тебе этот новый президент?",
					"продавец: ему на вид лет 20.",
					"продавец: молодая кровь никому не повредит?",
					"продавец: лишь бы он не чувствовал безнаказанность.",
					"продавец: наказание - то, что должно его сдерживать и заставлять прислушиваться к народу.",
					"продавец: и помогать малому бизнесу, хах!",
					"гари: (это типа шутка? лучше бы он молчал.)"
				);
				break;
			case "масло":
				WriteShopDialog( "продавец: дорого.",
					"продавец: мы нашли нефть, но еще не нашли масло.",
					"продавец: вот увидишь, на нефть мы будем молиться, как на иисуса.",
					"гари: (я не верю в бога.)",
					"гари: (но он явно несет ересь.)"
				);
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
			StartCoroutine (GameController.Instance.LoadRoom14 ());
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["Saler"] = "он покрасил волосы сзади?";
		objectPhrases["Picture"] = "хах, \"построено\" - и что? а шумоподавители у шоссе так и не сделали.";
		objectPhrases ["TV"] = "новый журналист выглядит так, будто каждый день у него траур.";
		objectPhrases ["Gary"] = "я устал.";
		objectPhrases ["Shkaf"] = "продуктов мало. все уже привыкли.";
		objectPhrases ["Shop"] = "магазину нехорошо.";
		objectPhrases ["Stoika"] = "стойке нехорошо.";

		objectPhrases ["деньги"] = "до сих пор много нулей. девальвация? номинация?";

	}
}
