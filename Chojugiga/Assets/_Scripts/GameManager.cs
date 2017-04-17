using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject _imagePanel;

	// Use this for initialization
	void Start () {
		Question q = new Question (Const.AnimalType.FROG, Const.Position.LEFT);
		QuestionImage qi = new QuestionImage ();
		qi.id = 3;
		qi.animalType = Const.AnimalType.FROG;
		qi.imagePath = "image_" + qi.id.ToString ();
		q.setQuestionImage (qi);

		_imagePanel.GetComponent<SpriteRenderer>().sprite = q.sprite;
//		Debug.Log ("sprite:" + _imagePanel.GetComponent<SpriteRenderer> ().sprite);
	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
