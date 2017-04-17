using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCtrl : MonoBehaviour {
	public List<Question> questionList;
	public int questionNum = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void init() {
		
	}

	/// <summary>
	/// Creates the questions.
	/// </summary>
	void createQuestions() {
		questionList = new List<Question> ();

		for (int i = 0; i < questionNum; i++) {
			
		}
	}

	void createQuestion() {
		Question q = new Question ();
	}
}
