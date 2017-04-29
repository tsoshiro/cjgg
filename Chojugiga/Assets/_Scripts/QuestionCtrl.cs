using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionCtrl : MonoBehaviour {
	public List<Question> questionList;
	public int questionNum = 10;

	int counter = 1;

	// Use this for initialization
	void Start () {
		init ();
		initSprites ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	#region Method_1

	public List<List<Sprite>> spritesList;
	void initSprites() {
		float time = Time.realtimeSinceStartup;

		spritesList = new List<List<Sprite>> ();

		for (int i = 0; i < images_max_count.Length; i++) {
			List<Sprite> sprites = new List<Sprite> ();
			for (int j = 1; j <= images_max_count [i]; j++) {
				Sprite sp = getSprite ((Const.AnimalType)i, j);
				sprites.Add (sp);
			}
			spritesList.Add (sprites);
		}

		time = Time.realtimeSinceStartup - time;
		DebugLogger.Log ("initSprites takes :" + time);
	}

	Sprite getSprite(Const.AnimalType pAt, int pNumber) {
		string path = FOLDER_PATH [(int)pAt] + pNumber.ToString ();
		Sprite sp = Resources.Load<Sprite>(path);
		return sp;
	}



	#endregion

	public Sprite getQSprite(Const.AnimalType pAt, int pId) {
		DebugLogger.Log (pAt + " " + pId);
		Sprite sp = spritesList [(int)pAt] [pId - 1];
		return sp;
	}

	public Sprite getQSprite() {
		counter++;
		if (counter % 5 == 0) {
			addQuestions (5);
		}

		Question q = questionList [counter];
		Sprite sp = getQSprite (q.questionImage.animalType, q.questionImage.number);
		return sp;
	}


	public Sprite _getQSprite() {
		counter++;
		if (counter >= questionNum)
			counter = 1;
		
		DebugLogger.Log ("imagePath:" + questionList [counter].questionImage.imagePath);
		Sprite sp = questionList[counter].sprite;

		return sp;
	}

	public Const.Position getQPos() {
		return questionList [counter].position;
	}

	// 蛙 : Position.Leftなら1、Rightなら0
	// 兎 : Position.Leftなら0、Rightなら1
	// 他 : 2

	// Answer
	// 0 : 左タップ
	// 1 : 右タップ
	// 2 : スワイプ
	public int getAnswer () {
		Question q = questionList [counter];
		Const.AnimalType at = q.questionImage.animalType;
		DebugLogger.Log ("Position:" + q.position + " at:" + at);


		if (at == Const.AnimalType.OTHERS) {
			return 0;
		}
		if (at == Const.AnimalType.FROG) {
			if (q.position == Const.Position.LEFT) {
				return 1;
			} else {
				return 0;
			}
		} else if (at == Const.AnimalType.RABBIT) {
			if (q.position == Const.Position.LEFT) {
				return 0;
			} else {
				return 1;
			}
		}
		return -1;
	}
		
	void init() {
		initQuestions ();
	}

	public void initQuestions() {
		counter = 1;
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

	void addQuestions(int pAddNum) {
		int startNumber = questionList.Count;
		for (int i = 0; i < pAddNum; i++) {
			Question q = createQuestion (startNumber - 1);
			questionList.Add (q);
		}
	}

	void readQuestions() {
		for (int i = 0; i < questionList.Count; i++) {
			Question q = questionList [i];

//			DebugLogger.Log ("qestionList[" + i + "] id:" + q.questionImage.id + " type:" + q.questionImage.animalType +
//				" imagePath:" + q.questionImage.imagePath + " pos:" + q.position+ " sprite:"+q.sprite); 
		}
	}

	Question createQuestion(int pId) {
		QuestionImage qi = new QuestionImage ();
		qi.id = pId;
		qi.animalType = getAnimalType (qi.id);

		// 01_frogs/f_[x] のxの部分を保存しておく
		qi.number = images_counter_array [(int)qi.animalType];

		qi.imagePath = getImagePath(qi.animalType);


		Const.Position pos = getPosition ();
		Const.Color col = getColor ();

		Question q = new Question ();
		q.init (qi, pos, col);

		return q;
	}

	int[] images_max_count = {20,20,10}; // 0:frog 1:rabbit 2:others

	List<List<int>> list_of_images_list; // 0:frog 1:rabbit 2:others
	List<int> frog_images_list;
	List<int> rabbit_images_list;
	List<int> others_images_list;

	// シャッフルされた配列の何番目かを記録しておく
	int[] images_counter_array = {1,1,1}; // 0:frog 1:rabbit 2:others
	string[] FOLDER_PATH = {
		"01_frogs/f_",
		"02_rabbits/r_",
		"03_others/o_"
	};

	void initImagesRandom() {
		list_of_images_list = new List<List<int>> ();

		frog_images_list = new List<int> ();
		rabbit_images_list = new List<int> ();
		others_images_list = new List<int> ();

		for (int i = 1; i <= images_max_count[(int)Const.AnimalType.FROG]; i++) {			
			if (i < images_max_count[(int)Const.AnimalType.OTHERS]) {
				others_images_list.Add (i);
			}
			frog_images_list.Add (i);
			rabbit_images_list.Add (i);
		}

		// シャッフルしてlist_of_images_listに格納する
		frog_images_list.Shuffle ();
		rabbit_images_list.Shuffle ();
		others_images_list.Shuffle ();

		list_of_images_list.Add (frog_images_list);
		list_of_images_list.Add (rabbit_images_list);
		list_of_images_list.Add (others_images_list);
	}

	string getImagePath(Const.AnimalType pAt) {
		string path = "";
		int imagesCounterIndex = (int)pAt;
		path = FOLDER_PATH [imagesCounterIndex];
		path += images_counter_array [imagesCounterIndex].ToString ();

		// max越えたら1に戻す
		images_counter_array [imagesCounterIndex]++;
		if (images_counter_array [imagesCounterIndex] > images_max_count [imagesCounterIndex]) {
			images_counter_array [imagesCounterIndex] = 1;
		}
			
		return path;
	}

	// テスト用
	Const.AnimalType getAnimalType(int pId) {
		int n = Random.Range(0,100);
		if (n < 40) {
			return Const.AnimalType.FROG;
		}
		if (n < 80) {
			return Const.AnimalType.RABBIT;
		}
		return Const.AnimalType.OTHERS;

//		if (pId <= 4) {
//			return Const.AnimalType.FROG;
//		}
//		if (pId <= 8) {
//			return Const.AnimalType.RABBIT;
//		}
//		return Const.AnimalType.OTHERS;
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
