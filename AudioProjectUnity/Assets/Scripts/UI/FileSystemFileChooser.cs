using UnityEngine;

namespace RJWS.UI
{
    public class FileSystemFileChooser : MonoBehaviour
    {
        public GUISkin guiSkin;

        protected string m_textPath;

        protected RJWS.Core.UI.FileBrowser m_fileBrowser;

		// Check this in windows - seems to have been using / 
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		const string PLATFORM_SEPARATOR = "\\";
#else
		const string PLATFORM_SEPARATOR = "/";
#endif

		private string _path = string.Empty;

		private string _title = "Choose file";

        /// <summary>
        /// Returns full path to file and full path without filename, in that order 
        /// Returns both as null if no file chosen
        /// </summary>
        private System.Action<string, string> _onSelectedCallback;

        private string _filenameExtension;

		public Rect rect = new Rect( 100, 100, 600, 500 );

		public Texture2D directoryImage;
		public Texture2D fileImage;

		private void Awake()
        {
        }

        protected void OnGUI()
        {
            GUI.skin = guiSkin;

            if (m_fileBrowser == null)
            {
                m_fileBrowser = new RJWS.Core.UI.FileBrowser(
                    rect,
                    _title,
                    FileSelectedCallback,
                    _path
                );
				m_fileBrowser.DirectoryImage = directoryImage;
				m_fileBrowser.FileImage = fileImage;
                if (_filenameExtension.Contains(" "))
                {
                    m_fileBrowser.SelectionPattern = "";
                    string[] extns = _filenameExtension.Split(' ');
                    for (int i = 0; i < extns.Length; i++)
                    {
                        if (i > 0)
                        {
                            m_fileBrowser.SelectionPattern = m_fileBrowser.SelectionPattern + " ";
                        }
                        m_fileBrowser.SelectionPattern = m_fileBrowser.SelectionPattern + "*." + extns[i];
                    }
                }
                else
                {
                    m_fileBrowser.SelectionPattern = "*." + _filenameExtension;
                }
            }

            m_fileBrowser.OnGUI();
        }

        protected void FileSelectedCallback(string fullFilePath)
        {
            m_fileBrowser = null;
            m_textPath = fullFilePath;

            string fullPath = null;

            if (!string.IsNullOrEmpty(fullFilePath))
            {
				fullPath = GetPath( fullFilePath );             
                Debug.Log("Selected file: " + fullFilePath + "\n- in path " + fullPath);
            }
            if (_onSelectedCallback != null)
            {
                _onSelectedCallback(fullFilePath, fullPath);
            }
            gameObject.SetActive(false);
        }

		static public string GetPath(string s)
		{
			int sepIndex = s.LastIndexOf( PLATFORM_SEPARATOR );
			if (sepIndex != -1)
			{
				s = s.Substring( 0, sepIndex );
			}
			return s;
		}

		public void Open(string title, string path, string extn, System.Action<string, string> onSelected)
        {
			_title = title;
            _filenameExtension = extn;

            if (!string.IsNullOrEmpty(path))
            {
                _path = path;
            }
            _onSelectedCallback = onSelected;
            gameObject.SetActive(true);
        }
    }    
}
