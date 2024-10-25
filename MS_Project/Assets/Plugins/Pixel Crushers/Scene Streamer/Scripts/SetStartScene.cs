using UnityEngine;
using System.Collections;

namespace PixelCrushers.SceneStreamer
{

	/// <summary>
	/// Sets the current scene at Start().
	/// </summary>
	[AddComponentMenu("Scene Streamer/Set Start Scene")]
	public class SetStartScene : MonoBehaviour
	{

		/// <summary>
		/// The name of the scene to load at Start.
		/// </summary>
		[Tooltip("Load this scene at start")]
		public string startSceneName = "Scene 1";

		public void Start()
		{
			SceneStreamer.SetCurrentScene(startSceneName);
			//AssetsからCameraPrefabを取得
			//"C:\Users\yuniz\Documents\MS_Project\MS_Project\Assets\Resources\CameraPivot.prefab"

			GameObject cameraPrefab = Resources.Load("CameraPivot") as GameObject;
			//CameraPrefabを元に生成
			Instantiate(cameraPrefab, new Vector3(0, 0, 0), Quaternion.identity);


			Destroy(this);
		}

	}

}
