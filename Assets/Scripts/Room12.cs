using UnityEngine;
using System.Collections;

public class Room12 : Room {

	public override void Instantiate () {
		Room11[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room11> ();
		InstantiateFromPrevious (rooms);

		Money = 16400;
		deleteProducts ("сыр", "шоколад", "рыба", "масло");
		products.Add ("крупа", new Products (new GameObject ("Krupa"), 9000));
		products.Add ("яблоки", new Products (new GameObject ("Apples"), 8500));
		products ["молоко"].Price = 7700;
		products ["хлеб"].Price = 6000;
	}

	public override void changeRoom (){
		objects ["Picture"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Picture11_12");
		objects ["Gary"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Gary_12");
		objects ["Saler"].AddComponent<Animator> ().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Saler13");
		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop15_19");
		objects ["Stoika"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Stoika15_19");
		Object.Destroy (objects ["TV"].GetComponent<Animator> ());
		objects ["TV"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("TV");


		products ["крупа"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Krupa");
		products ["яблоки"].Obj.AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> (@"Products\Apples");
	}

	public override void TimeStart ()
	{
		WriteStartDialog ("p продавец: гари, заходи.",
			"продавец: хах, из-за кризиса сегодня товаров немного.");
	}

	public override void TimeEnd ()
	{
		WriteEndDialog ("продавец: ты уже слышал? вчера недалеко сгорел магазин.",
			"продавец: ужас, полиция не нашла преступников.",
			"продавец: и свидетелей.",
			"p продавец: и состав преступления.",
			"продавец: лучше иметь при себе оружие.",
			"p гари: ого, и лицензия есть?!",
			"продавец: нет, но...", 
			"гари: дай подержать!",
			"продавец: а у тебя есть лицензия?",
			"гари: ...",
			"продавец: ни за какие вещественные деньги не дам."
		);
		if (endClicks == 5)
			StartCoroutine (TakeGun ());
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
		objects ["Gun"].GetComponent<SpriteRenderer> ().sortingLayerName = "room12";
		objects ["Gun"].GetComponent<SpriteRenderer> ().sortingOrder = 6;
		objects ["Gun"].AddComponent<BoxCollider2D> ();
		yield return null;
	}

	public override void ShopTime(){
		ChooseProductOrExitIfStart ();

		if (CanBuyProd() && !isWriteText) {
			shopClicks++;
			switch (choosenProductName) {
			case "молоко":
				WriteShopDialog( "продавец: иностранное теперь.",
					"продавец: я посчитал на компьютере - оно оказалось дешевле, чем наше.",
					"продавец: как и одежда, бытовые приборы и автомобили.");
				break;
			case "хлеб":
				WriteShopDialog( "продавец: это труднодоставаемый хлеб, если ты понимаешь, о чем я.",
					"продавец: мне приходится покупать его у продуктовых картелей.",
					"продавец: причем черный хлеб они продавать не собираются!",
					"продавец: разве это честный бизнес, хах!");
				break;
			case "крупа":
				WriteShopDialog( "продавец: недавно заходил парень,",
					"продавец: в кроссовках, кожанке, малиновом пиджаке, кепке-лопате, с часами и золотой цепью.",
					"продавец: ему не хватило денег даже на крупу.",
					"продавец: я продал ему взамен на кепку-лопату.",
					"p продавец: это было немного странно.");
				break;
			case "яблоки":
				WriteShopDialog( "гари: представь абстрактную игру про продуктовый магазин.",
					"продавец: а я бы сделал про белочек.",
					"гари: что?",
					"продавец: белочки-убийцы-гонщики с марса. там была бы вторая мировая и машина времени.",
					"продавец: все делают визуальные новеллы про модные темы: политика, отношения, постмодернизм.",
					"продавец: такие игры скучны и однообразны, не думаешь?",
					"гари: ...",
					"продавец: бдыщ, бдыщ! нааайс.");
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
			StartCoroutine (GameController.Instance.LoadRoom13 ());
	}

	public override void SetObjectsPhrases ()
	{
		base.SetObjectsPhrases ();
		objectPhrases ["Saler"] = "продавец измучен. может, он перестанет лезть ко мне с новостями из страны? ...это что, кепка-лопата?";
		objectPhrases ["TV"] = "не работает. что-то сломалось на станции, видимо.";
		objectPhrases ["деньги"] = "я до сих пор не могу поверить - столько нулей! и это за неделю? не, кажется, две.";
		objectPhrases ["Gary"] = "кризис? по мне не скажешь. только нулей больше стало.";
		objectPhrases ["Shop"] = "кризис, серьезно? может, продавец перестанет кричать о положении страны и отстанет от меня?";
		objectPhrases ["Stoika"] = "стойка обветшала. магазину уже 3 года или около того. эй, а музыка все та же!";
		objectPhrases ["Picture"] = "подпись: \"построено.\" и что это? куча офисных зданий в центре столицы! а шумоподавители у шоссе так и не сделали.";
		objectPhrases ["Lamp"] = "лампе нехорошо, она покрылась пылью. будет знать, как обманывать количеством плафонов!";

		objectPhrases ["MoneySymbol"] = "проезд - 600 гостов. что за страна? ужас!";
	}
}
