using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongReason : ScriptableObject {
	public string reasonText;
	string holder_bef = "【";
	string holder_aft = "】";


	public void setReason(int pAnswer, Question pQuestion) {
		reasonText = "";
		reasonText += getHold(getPosition (pQuestion.position)) + "に\n";
		reasonText += getHold(getAnimalName (pQuestion.questionImage.animalType)) + "なので\n";
		reasonText += getHold(getAnswer (pAnswer)) + "が正しい";
	}

	string getHold(string str) {
		return holder_bef + str + holder_aft;
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
