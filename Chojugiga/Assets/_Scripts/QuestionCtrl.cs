using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCtrl : MonoBehaviour {
	public List<Question> questionList;
	public int questionNum = 10;

	int counter = 0;

	// Use this for initialization
	void Start () {
		init ();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Sprite getQSprite() {
		Debug.Log ("imagePath:" + questionList [counter].questionImage.imagePath);
//		Sprite sp = Resources.Load<Sprite>(questionList [counter].questionImage.imagePath);
		Sprite sp = questionList[counter].sprite;

		counter++;
		if (counter >= questionNum)
			counter = 0;
		return sp;
	}

	public Const.Position getQPos() {
		return questionList [counter].position;
	}

	public int getAnswer () {
		Question q = questionList [counter];
		Const.AnimalType at = q.questionImage.animalType;

		if (at == Const.AnimalType.OTHERS) {
			return 2;
		}
		if (at == Const.AnimalType.FROG) {
//			if (q.position == Const.Position.LEFT) {
//				return 0;
//			}
//			return 1;
			return 0;
		} else if (at == Const.AnimalType.RABBIT) {
//			if (q.position == Const.Position.LEFT) {
//				return 1;
//			}
//			return 0;
			return 1;
		}
		return -1;
	}
		
	void init() {
		createQuestions ();

		readQuestions ();
	}

	/// <summary>
	/// Creates the questions.
	/// </summary>
	void createQuestions() {
		questionList = new List<Question> ();

		for (int i = 1; i <= questionNum; i++) {
			Question q = createQuestion (i);
			questionList.Add (q);
		}
	}

	void readQuestions() {
		for (int i = 0; i < questionList.Count; i++) {
			Question q = questionList [i];
			Debug.Log ("qestionList[" + i + "] id:" + q.questionImage.id + " type:" + q.questionImage.animalType +
				" imagePath:" + q.questionImage.imagePath + " pos:" + q.position+ " sprite:"+q.sprite); 
		}
	}

	Question createQuestion(int pId) {
		QuestionImage qi = new QuestionImage ();
		qi.id = pId;
		qi.animalType = getAnimalType (qi.id);
		qi.imagePath = "image_" + qi.id.ToString ();

		Const.Position pos = getPosition ();
		Const.Color col = getColor ();

		Question q = new Question ();
		q.init (qi, pos, col);

		return q;
	}

	// テスト用
	Const.AnimalType getAnimalType(int pId) {
		if (pId <= 4) {
			return Const.AnimalType.FROG;
		}
		if (pId <= 8) {
			return Const.AnimalType.RABBIT;
		}
		return Const.AnimalType.OTHERS;
	}

	Const.Position getPosition() {
		int n = Random.Range (0, 100);
		n %= 2;
		return (Const.Position)n;
	}

	Const.Color getColor() {
		return Const.Color.NONE;
	}
}
