using UnityEngine;
using System.Collections;

public class Room19 : Room {

	public override void Instantiate () {
		Room18[] rooms = gameObject.transform.parent.GetComponentsInChildren<Room18> ();
		InstantiateFromPrevious (rooms);

		Money = 1000;
		foreach (var prod in products)
			Object.Destroy (prod.Value.Obj);
		products.Clear ();
		deleteObjects ("Shkaf", "Call", "Door", "Saler", "Gas");
		if (!objects.ContainsKey ("DoorHole"))
			objects.Add ("DoorHole", new GameObject ("DoorHole"));
	}

	public override void changeRoom (){
		objects ["Shop"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Shop22");
		objects ["Stoika"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Stoika22_23");
		if (objects["DoorHole"].GetComponent<SpriteRenderer>() == null)
			objects ["DoorHole"].AddComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("DoorWay20");
		else
			objects ["DoorHole"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("DoorWay20");
		objects ["Lamp"].AddComponent<Animator> ().runtimeAnimatorController = 
			Resources.Load<RuntimeAnimatorController> ("BurnedLampController");
		
		objects ["Gary"].AddComponent<Animator> ().runtimeAnimatorController = 
			Resources.Load<RuntimeAnimatorController> ("Gary_21");
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = false;
		objects ["Picture"].GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Picture19_21");


	}

	public override void SetPositionsAndColliders ()
	{
		base.SetPositionsAndColliders ();
		objects ["Gary"].transform.localPosition = new Vector3 (2.13f, -1.05f);
		objects ["Stoika"].transform.localPosition = new Vector3 (-1.75f, -1.47f);
		objects ["Stoika"].transform.localRotation = Quaternion.Euler (new Vector3 (0f, 0f, 90f));
	}

	public override void TimeStart ()
	{
		//
		//StartCoroutine(GameController.Instance.LoadRoom20());
		//this.enabled = false;
		//return;
		//
		if (firstInput) {
			MoneyObj.SetActive (false);
			MoneySymbol.SetActive (false);
			StartCoroutine (walking ());
			firstInput = false;
		}
	}

	private IEnumerator walking(){
		yield return new WaitForSeconds (1.5f);
		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = false;

		objects ["Gary"].GetComponent<Animator> ().SetBool ("Walk", true);

		while ((objects ["Gary"].transform.localPosition - objects ["Stoika"].transform.localPosition).x > 0.3f) {
			objects ["Gary"].transform.localPosition -= new Vector3 (0.02f, 0f);
			yield return new WaitForEndOfFrame ();
		}
		yield return new WaitForSeconds (0.3f);

		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = true;

		while ((objects ["Gary"].transform.localPosition - Room.posGary).x < -0.01f) {
			objects ["Gary"].transform.localPosition += new Vector3 (0.02f, 0f);
			yield return new WaitForEndOfFrame ();
		}
		yield return new WaitForSeconds (0.3f);

		objects ["Gary"].GetComponent<SpriteRenderer> ().flipX = false;
		objects ["Gary"].GetComponent<Animator> ().SetBool ("Walk", false);

		yield return new WaitForSeconds (3f);
		objects ["Gary"].GetComponent<Animator> ().SetBool ("Eye", true);

		yield return new WaitForSeconds (0.9f);

		darkRoom (Color.black);
		lightSound.Play ();

		//setDarkness (true);

		Object.Destroy (Past);
		Object.Destroy (Future);
		StartCoroutine (GameController.Instance.LoadRoom20 ());

		yield return null;
	}

	private void darkRoom(Color darkness){
		//Color darkness = new Color(0.2f, 0.2f, 0.2f, 0.95f);
		foreach (var obj in objects) {
			Object.Destroy (obj.Value);
			//obj.Value.GetComponent<SpriteRenderer> ().color = darkness;
		}
		foreach (var prod in products) {
			var ren = prod.Value.Obj.GetComponent<SpriteRenderer> ();
			if (ren != null)
				Object.Destroy (prod.Value.Obj);
				//ren.color = darkness;
		}
		Object.Destroy (MoneySymbol);//.GetComponent<SpriteRenderer> ().color = darkness;
	}

	public override void clickOnRoom ()
	{
		return;
	}
}
