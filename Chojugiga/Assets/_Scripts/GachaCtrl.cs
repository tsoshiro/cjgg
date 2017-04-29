using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaCtrl : MonoBehaviour {
	GameManager _gameManager;
	public GameObject _cardPrefab;
	public GameObject _cardList;
	public Button _buttonPlayGacha;

	List<Card> _cards;
	bool isLoaded = false;

	// Use this for initialization
	void Start () {
		_gameManager = GameManager.GetInstance ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// 所持コインでガチャが引けるかを判別する
	/// </summary>
	/// <returns><c>true</c>, if gacha affordable was checked, <c>false</c> otherwise.</returns>
	bool checkGachaAffordable() {
		if (checkGachaAffordable(_gameManager._userData.coin)) {
			return true;
		}
		return false;
	}

	public bool checkGachaAffordable(int pCoin) {
		if (pCoin >= Const.GACHA_COST) {
			return true;
		}
		return false;		
	}

	public void playGacha(int pId = 0) {
		if (!checkGachaAffordable ()) {
			DebugLogger.Log ("NO MONEY");
			return;
		}

		string str = getGachaResult (collection);
		_gameManager._userData.unlocked_card = str; // unlocked_cardのリストを更新
		_gameManager._userData.coin -= Const.GACHA_COST; // Gacha Costを消費

		_gameManager._userData.save(); // ユーザーデータ更新

		updateGachaAfford ();
		DebugLogger.Log ("str:" + str);
	}

	/// <summary>
	/// ガチャ結果を反映させたデータをstringで受け取る
	/// </summary>
	/// <returns>The gacha result.</returns>
	/// <param name="pList">リスト</param>
	/// <param name="pId">ガチャ結果IDがあれば指定。基本は0。</param>
	public string getGachaResult(List<int> pList, int pId = 0) {
		int id = (pId != 0) ? pId : gachaalgorithm ();

		List<int> list = pList;

		// すでに持っているかチェック
		if (checkIfAlreadyHaveOne (id, list)) {
			DebugLogger.Log ("SAME ONE...");
		} else {
			// 持っていなければ、コレクションに追加する
			list.Add (id);
			list.Sort ();

			UpdateSprite (id, true);
		}

		string str = getStringFromList (list);
		return str;
	}
		
	// すでに持っているかチェック
	bool checkIfAlreadyHaveOne(int pId, List<int> pList) {
		if (pList.Contains (pId)) {
			return true;
		}
		return false;
	}

	List<int> collection;

	// 文字列をリストに変換する
	public List<int> getListFromString(string pUnlockedCard) {
		List<int> list = new List<int> ();

		if (pUnlockedCard == "") {
			DebugLogger.Log ("NO DATA");
			return list;
		}
			
		string str = pUnlockedCard;
		string[] strs = str.Split (','); // 「,」で区切る

		for (int i = 0; i < strs.Length; i++) {
			strs [i] = strs [i].Trim (); // 空白を削除する
			list.Add( int.Parse(strs[i]));
		}

		return list;
	}

	// リストを文字列に変換する
	public string getStringFromList (List<int> pList) {
		string str = "";
		for (int i = 0; i < pList.Count; i++) {
			str += pList [i].ToString ();
			if (i < pList.Count - 1) {
				str += ",";
			}
		}
		return str;
	}

	int gachaalgorithm() {
		int value = Random.Range (1, 50);
		return value;
	}

	void updateGachaAfford() {
		_buttonPlayGacha.enabled = checkGachaAffordable ();
		DebugLogger.Log ("is affordable:" + checkGachaAffordable ());
	}

	#region Gallery
	public void createCardList() {
		updateGachaAfford ();

		if (isLoaded)
			return;
		
//		string testList = "1,2,3,6,8,10,11,13";
		string testList = _gameManager._userData.unlocked_card;
		collection = getListFromString (testList);

		StartCoroutine (createCardsCoroutine (collection));
	}

	IEnumerator createCardsCoroutine(List<int> pList) {
		// 全てのカードをロードしてCardクラスで表示する
		List<List<Sprite>> spriteList = _gameManager._questionCtrl.spritesList;
		_cards = new List<Card> ();

		int id = 1;
		for (int i = 0; i < spriteList.Count; i++) {
			List<Sprite> sprites = spriteList [i];
			for (int j = 0; j < sprites.Count; j++) {				
				Vector3 position = getPosition (id);
				Sprite sp = sprites [j]; // 所持していない場合、カギ画像
				createCard (id, (Const.AnimalType)i, sp, position, (pList.Contains(id)));
				id++;
				yield return 0;
			}
		}
		isLoaded = true;
	}

	void UpdateSprite(int pId, bool pFlg = true) {
		Card card = _cards [pId - 1];
		Sprite sp = (pFlg) ? card.mySprite : card._defaultSprite;
		card.gameObject.GetComponent<Image> ().sprite = sp;
	}

	float x_start = -160;
	float y_start = 300;
	float x = 320;
	float y = 220; 

	// idからVector3を出力
	Vector3 getPosition(int pId) {
		// 列の設定
		// 奇数 OR 偶数 でX座標が決まる
		float thisX = x_start;
		float thisY = y_start;

		if (pId % 2 == 0) {
			thisX += x;
		}

		int row = (pId + 1 ) / 2; // 行の設定
		thisY -= (row - 1) * y;

		return new Vector3 (thisX, thisY);
	}

	void createCard(int pId, Const.AnimalType pAt, Sprite pSprite, Vector3 pPosition, bool pIsUnlocked = true) {
		GameObject go = (GameObject)Instantiate(_cardPrefab, _cardPrefab.transform.position, _cardPrefab.transform.rotation);
		go.transform.SetParent(_cardList.transform);
		go.transform.localPosition = pPosition;
		go.name = "CARD_" + pId.ToString ("D2");

		Card card = go.GetComponent<Card> ();
		card.id = pId;
		card.at = pAt;
		card.mySprite = pSprite;

		go.GetComponent<Image> ().sprite = (pIsUnlocked) ? card.mySprite : card._defaultSprite;

		_cards.Add (card);
	}
	#endregion
}