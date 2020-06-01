using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class ButtonScripts : MonoBehaviour {

	public void RestartGame(){
        SceneManager.LoadScene(0);
    }

	public void GotoMainmenu(){
	}


	public void NewGame(){
        SceneManager.LoadScene(0);
	}

	public void HighScore(){

	}

	public void Difficulty (){

	}

	public void GooglePlayLogin (){

	}

	public void ShowLeaderboards() {
		//Social.ShowLeaderboardUI();
	}

	public void ShowAchievements (){
		//Social.ShowAchievementsUI();
	}


}
