using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
	// 問題クラス
	public QuestionImage questionImage;
	public Const.AnimalType animalType;
	public Const.Position position;
	public Const.Color color;
	public Sprite sprite;

	public Question(Const.AnimalType pAnimalType = Const.AnimalType.FROG,
					Const.Position pPosition = Const.Position.LEFT,
					Const.Color pColor = Const.Color.NONE) {
		animalType = pAnimalType;
		position = pPosition;
		color = pColor;
	}

	public void setTexture(Sprite pSprite) {
		sprite = pSprite;
	}

	public void setQuestionImage(QuestionImage pQuestionImage) {
		animalType = pQuestionImage.animalType;
		Debug.Log ("path:" + pQuestionImage.imagePath);
		sprite = Resources.Load<Sprite>(pQuestionImage.imagePath);
	}
}
