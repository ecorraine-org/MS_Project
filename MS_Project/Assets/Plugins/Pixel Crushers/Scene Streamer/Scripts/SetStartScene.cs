using UnityEngine;
using System.Collections;

namespace PixelCrushers.SceneStreamer
{

	/// <summary>
	/// Sets the current scene at Start().
	/// </summary>
	/// 
	//[SerializeField, Header("開始時のプレイヤー")] private GameObject _player;
	[AddComponentMenu("Scene Streamer/Set Start Scene")]
	public class SetStartScene : MonoBehaviour
	{

		/// <summary>
		/// The name of the scene to load at Start.
		/// </summary>
		[Tooltip("Load this scene at start")]
		public string startSceneName = "Scene 1";

		public void Awake()
		{
			SceneStreamer.SetCurrentScene(startSceneName);
			//_player = GameObject.Find("Player");
			Destroy(this);
		}

	}

}
