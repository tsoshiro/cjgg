using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongReason : ScriptableObject {
	public string reasonText;

	public void setReason(int pAnswer, Question pQuestion) {
		reasonText = "";
		reasonText += "「" + getPosition (pQuestion.position) + "」に";
		reasonText += "「" + getAnimalName (pQuestion.questionImage.animalType) + "」なので";
		reasonText += "「" + getAnswer (pAnswer) + "」";
	}

	string getAnimalName(Const.AnimalType pAnimal) {
		if (pAnimal == Const.AnimalType.FROG) {
			return Const.FROG;
		}
		if (pAnimal == Const.AnimalType.RABBIT) {
			return Const.RABBIT;
		}
		return Const.OTHERS;
	}

	string getPosition(Const.Position pPos) {
		if (pPos == Const.Position.LEFT) {
			return Const.LEFT;
		}
		return Const.RIGHT;
	}

	string getAnswer(int pAnswer) {
		if (pAnswer == 0) {
			return Const.ANS_WRONG;
		}
		return Const.ANS_RIGHT;
	}
}
