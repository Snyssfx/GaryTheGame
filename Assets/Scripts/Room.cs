using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Products{
	public GameObject Obj;
	public int Price;
	public int MyPrice {
		get { return Price; }
		set { Price = value; }
	}
	public Products(GameObject obj, int price){
		Obj = obj;
		Price = price;
	}
}

public enum timeInRooms
{
	Begin,
	BeforeShop,
	Shop,
	End
}

public abstract partial class Room : MonoBehaviour {

	public Dictionary<string, string> objectPhrases;

	//TODO: how to draw subProducts?
	public int numOfYear;
	public timeInRooms currentTime;

	public GameObject Future, Past;
	protected string clickOnSomething = null;
	protected bool isCoroutineClickStarted = false;

	protected bool firstInput = false;
	protected bool preShopInput = false;
	protected bool isWriteText = false;
	protected Text mainText;
	protected Button exit;

	Vector3 size, pos;
	public static float yPos = 0f;
	public static float xPos = 0.94f;
	public static float startPos = -4.71f;

	public Dictionary<string, Products> products;
	public List<GameObject> subProducts;

	protected GameObject[] buttonsObj; //gameObjects are connected to buttons
	protected Dictionary<string, Button> buttons;

	protected int clicks = 0;
	protected int startClicks = 0;//need for plot in the TimeInRooms.Begin
	protected int shopClicks = 0; //need for plot in the TimeInRooms.Shop
	protected int endClicks = 0; //-- TimeInRooms.End
	protected string choosenProductName = null;

	protected bool isShopTime = false;
	protected bool isPreShopReady = false;
	public bool isPostShopTime = false;

	protected static AudioSource shopBell;
	protected static AudioSource lightSound;

	//[HideInInspector] 
	public GameObject MoneyObj;
	public GameObject MoneySymbol;
	public GameObject Year;
	//[HideInInspector]  
	protected int money = 0;
	protected int Money {
		get{ return money; }
		set{ money = value;
			MoneyObj.GetComponentInChildren<Text> ().text = money.ToString ();
		}
	}

	[HideInInspector] 
	public Dictionary<string, GameObject> objects;

	[HideInInspector] 
	public bool isInstantiate = false;

	public abstract void changeRoom ();
	public abstract void Instantiate();

	public bool isBaseInstantiate = false;

	public virtual void Start(){
		InstantiateBase ();
		Instantiate ();
		changeRoom ();
		SetParents ();
		SetPositionsAndColliders ();
		SortInLayers (GameController.Instance.currentYear);
		SetDarkness (false);
		Show (false);
	}

	public void InstantiateBase(){
		
		if (!isBaseInstantiate) {
			gameObject.tag = "room";
			isBaseInstantiate = true;
			objects = new Dictionary<string, GameObject> ();
			products = new Dictionary<string, Products> ();
			subProducts = new List<GameObject> ();

			GameObject Money = Instantiate((GameObject)Resources.Load("Money"));
			MoneyObj = Money;
			MoneyObj.GetComponent<Text> ().text = money.ToString();
			GameObject monSym = new GameObject ("Money Symbol");
			var ren = monSym.AddComponent<SpriteRenderer> ();
			ren.sprite = Resources.Load<Sprite> ("money");
			MoneySymbol = monSym;
			Year = Instantiate (Resources.Load<GameObject> ("Money"));
			Year.GetComponentInChildren<Text> ().color = Color.black;
			Year.GetComponentInChildren<Text> ().alignment = TextAnchor.LowerLeft;

			SetObjectsPhrases ();

			var text = GameObject.FindWithTag ("main text");
			//text.SetActive (true);
			mainText = text.GetComponent<Text> ();
			mainText.text = "";

			exit = GameObject.FindGameObjectWithTag ("exit button").GetComponent<Button> ();
			exit.gameObject.GetComponentInChildren<Text> ().text = "";

			currentTime = timeInRooms.Begin;

			lightSound = GameObject.Find ("LightSound").GetComponent<AudioSource> ();
			shopBell = GameObject.Find ("ShopBell").GetComponent<AudioSource> ();

			Past = GameObject.Instantiate (Resources.Load<GameObject> ("PastAndFuture"));
			Future = GameObject.Instantiate (Resources.Load<GameObject> ("PastAndFuture"));
		}

	}

	public virtual void Show (bool trueOrFalse) {
		gameObject.SetActive (trueOrFalse);
		foreach(var prod in products)
			prod.Value.Obj.SetActive (trueOrFalse);
		foreach (var obj in objects)
			obj.Value.SetActive (trueOrFalse);
		MoneyObj.SetActive (trueOrFalse);
		MoneySymbol.SetActive (trueOrFalse);
		Year.SetActive (trueOrFalse);
		if (trueOrFalse) {
			shopBell.loop = false;
			shopBell.clip = Resources.Load<AudioClip> (@"Sounds\ShopTime");
			lightSound.clip = Resources.Load<AudioClip> (@"Sounds\Light");
			lightSound.Play ();
			firstInput = true;
		}
	}



	public virtual string getNameOfClickedButton(){
		Vector3 position = Input.mousePosition;
		var camera = Camera.main;
		Ray pos2 = camera.ScreenPointToRay(position);
		var hit = new RaycastHit();
		Physics.Raycast (pos2, out hit);
		foreach (var button in buttons) {
			if (hit.collider != null && button.Value.gameObject == hit.collider.gameObject &&
				button.Value.GetComponentInChildren<Text>().text != "") {
				return button.Key;
			}
		}
		if (hit.collider != null && exit.gameObject == hit.collider.gameObject)
			return "exit";
		return null;
	}

	public virtual void preShopTime(){
		SetGUI ();
		SetProducts();
		SetGUIActive (true);
		isPreShopReady = true;

		currentTime = timeInRooms.Shop;
		preShopInput = false;
	}

	public virtual void ShopTime(){
		
		string name = getNameOfClickedButton ();
		if (name != null && name != "exit" && money >= products[name].Price) {
			Money -= products [name].Price;
			buttons [name].GetComponentInChildren<Text> ().text = "";
			products [name].Obj.SetActive (false);
		}
	}

	public virtual void ChooseProductOrExitIfStart(){
		if (shopClicks == 0) {
			string name = getNameOfClickedButton ();
			if (name == "exit") {
				choosenProductName = "exit";
				currentTime = timeInRooms.End;
				return;
			}
			choosenProductName = name;
		}
	}

	public virtual bool CanBuyProd(){
		if (choosenProductName == null)
			return false;
		if (choosenProductName == "exit")
			return false;
		if (money < products [choosenProductName].Price) {
			StartCoroutine (WriteText ("гари: (у меня не хватает денег.)"));
			choosenProductName = null;
			shopClicks = 0;
			return false;
		}
		return true;
	}

	protected virtual void BuyProduct(){
		shopBell.GetComponent<AudioSource>().Play ();
		Money -= products [choosenProductName].Price;
		buttons [choosenProductName].GetComponentInChildren<Text> ().text = "";
		products [choosenProductName].Obj.SetActive (false);
		shopClicks = 0;
		choosenProductName = null;
	}

	public IEnumerator WriteText(string text){
		//float waitPause = 0.03f;
		float waitPause = 0.8f;
		isWriteText = true;
		int i = 0; 
		mainText.text = "";

		if (text [0] == 'p') { //if pause
			text = text.Remove (0, 2);
			yield return new WaitForSeconds (waitPause);
		}

		//write name of teller
		while (i < text.Length && text [i] != ':' ) {
			i++;
		}
			
		if (i != text.Length) {
			if (text [0] == 'п')
				mainText.text = @"<color=lightblue>";
			if (text [0] == 'г')
				mainText.text = @"<color=lime>";
			for (int j = 0; j < i; j++)
				mainText.text += text [j];
			if (text[0] == 'п' || text[0] == 'г')
				mainText.text += @"</color>";
			yield return new WaitForEndOfFrame ();
			yield return new WaitForEndOfFrame ();
			yield return new WaitForEndOfFrame ();
		} else
			i = 0;

		//write rest of the text
		for (int j = i; j < text.Length; j++) {
			
			if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && clickOnSomething == null) {
				mainText.text += text.Substring(j);
				yield return new WaitForSeconds (0.2f);
				isWriteText = false;
				yield break;
			}
			
			if (isWriteText) {
				mainText.text += text [j];
				yield return new WaitForFixedUpdate ();
			}
		}
		yield return new WaitForFixedUpdate ();
		isWriteText = false;
		yield return null;
	}

	public virtual void WriteDialog(int whoSwitch, params string[] phrases){
		if (whoSwitch <= phrases.Length)
			StartCoroutine (WriteText (phrases [whoSwitch - 1]));
	}

	public virtual void WriteStartDialog(params string[] phrases){
		WriteDialog (startClicks, phrases);
		if (startClicks == phrases.Length) {
			currentTime = timeInRooms.BeforeShop;
			preShopInput = true;
		}
	}

	public virtual void WriteShopDialog(params string[] phrases){
		WriteDialog (shopClicks, phrases);
		if (shopClicks == phrases.Length)
			BuyProduct ();
	}

	public virtual void WriteEndDialog(params string[] phrases){
		WriteDialog (endClicks, phrases);
		if (endClicks == 1) {
			SetGUIActive (false);
			firstInput = false;
		}
		if (endClicks == phrases.Length + 1)
			SetDarkness (true);
	}

	public virtual void WriteShopDialogWithEndPhrase(params string[] phrases){
		WriteDialog (shopClicks, phrases);
		if (shopClicks == phrases.Length + 1){
			string[] endPhrases = {"продавец: что-то еще?", "продавец: отличный выбор!", 
				"продавец: может, еще пакет?", "продавец: качественный продукт.",
				"продавец: да, плюс - недорого.",
				"продавец: хороший выбор."
			};
			int rnd = Random.Range (0, endPhrases.Length);
			StartCoroutine (WriteText (endPhrases [rnd]));
			BuyProduct ();
		}
	}

	public virtual void IsEndTime(){
		currentTime = timeInRooms.End;
		if (choosenProductName == "exit")
			return;
		foreach (var prod in products) {
			if (prod.Value.Obj.activeInHierarchy && prod.Value.Price <= Money) {
				isPostShopTime = false;
				isShopTime = true;
				currentTime = timeInRooms.Shop;
				break;
			}
		}
	}

	public virtual void deleteProducts(params string[] names){
		for (int i = 0; i < names.Length; i++) {
			if (products.ContainsKey (names[i])) {
				Object.Destroy (products [names[i]].Obj);
				products.Remove (names[i]);
			}
		}
	}
	public virtual void deleteObjects(params string[] names){
		for (int i = 0; i < names.Length; i++) {
			if (objects.ContainsKey (names[i])) {
				Object.Destroy (objects [names[i]]);
				objects.Remove (names[i]);
			}
		}
	}

	public virtual string getNameOfClickedObject(){
		Vector3 position = Input.mousePosition;
		position.z = 20f;
		var camera = Camera.main;
		var pos2 = camera.ScreenToWorldPoint (position);
		var cols = Physics2D.OverlapPointAll(new Vector2(pos2.x, pos2.y));
		Collider2D col = null;
		if (cols.Length > 0) {
			col = cols [0];

			for (int i = 0; i < cols.Length; i++) {
				if (cols[i].gameObject == MoneyObj)
					return "деньги " + Money;
				if (cols[i].gameObject == MoneySymbol)
					return "MoneySymbol";
				if (cols[i].gameObject == Year)
					return "лет " + numOfYear;
				if (cols [i].gameObject == Past)
					return "Past";
				if (cols [i].gameObject == Future)
					return "Future";
			}

			foreach (var collider in cols) {
				if (collider.gameObject.GetComponent<SpriteRenderer> ().sortingOrder >
				    col.gameObject.GetComponent<SpriteRenderer> ().sortingOrder)
					col = collider;
			}
		}
		foreach (var obj in objects) {
			if (col != null && obj.Value == col.gameObject) {
				return obj.Key;
			}
		}

		foreach (var prod in products) {
			if (col != null && prod.Value.Obj == col.gameObject) {
				return "продукт " + prod.Key + " " + prod.Value.Price;
			}
		}
		return null;
	}

	public virtual void clickOnRoom(){
		if (Input.GetMouseButton(0) && !isWriteText) {
			clickOnSomething = getNameOfClickedObject ();
			if (clickOnSomething == null)
				return;
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

	public virtual void RoomCycle(){
		if ((Input.GetButtonDown("Click") || choosenProductName == "exit" || firstInput || Input.GetKeyDown(KeyCode.Space) || preShopInput) 
			&& !isWriteText) {
			switch (currentTime) {
			case timeInRooms.Begin:
				startClicks++;
				TimeStart ();
				break;

			case timeInRooms.BeforeShop:
				preShopTime ();
				break;

			case timeInRooms.Shop:
				ShopTime ();
				break;

			case timeInRooms.End:
				endClicks++;
				TimeEnd ();
				break;
			}
		}
	}

	public virtual void TimeStart(){
		if (startClicks == 1)
			currentTime = timeInRooms.BeforeShop;
	}

	public virtual void TimeEnd(){
		if (endClicks == 1)
			SetDarkness (true);
	}
		
	public virtual void Update(){
		clickOnRoom ();

		if (numOfYear != 18 && numOfYear != 19 && numOfYear != 21 && !GameController.Instance.backgroundMusic.isPlaying) {
			StartCoroutine(GameController.Instance.backgroundMusic.FadeIn (0.3f));
			//GameController.Instance.backgroundMusic.Play ();
		}

		if (isWriteText) {
			firstInput = false;
		}

		if (currentTime == timeInRooms.End && isWriteText) {
			choosenProductName = null;
		}

		if (numOfYear < 9){	
			SetGUI ();
			SetDarkness (true);
		}

		RoomCycle ();

	}
}
