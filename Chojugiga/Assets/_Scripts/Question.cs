using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
	// 問題クラス
	public QuestionImage questionImage;
	public Const.Position position;
	public Const.Color color;
	public Sprite sprite;

	public void init(QuestionImage pQi, Const.Position pPos, Const.Color pColor) {
		setQuestionImage (pQi);

		position = pPos;
		color = pColor;
	}

	void setQuestionImage(QuestionImage pQuestionImage) {
		questionImage = pQuestionImage;
//		sprite = Resources.Load<Sprite>(pQuestionImage.imagePath);
	}
}
