using UnityEngine;

/* These classes mean you don't need to keep track of matching GUILayout Begin|End Horizontal|Vertical|Area
 * 
 * Usage... 
 * 
 * instead of 
 * 
 * GuiLayout.BeginHorizontal();
 * // do stuff
 * GUILayout.EndHorizontal()
 *
 * you do
 *
 * using (EditorLayoutHelpers.HorizontalGroup.Simple())
 * {
 * // do stuff
 * }
 * 
 * The group is automatically closed by the disposal of the group object when it goes out of scope.
 *
 * See the static creation functions in the HorizontalGroup, VerticalGroup, and Area subclasses classes for more options, and add others you find useful
 * */
namespace RJWS.Core.EditorExtensions
{
	public static class EditorLayoutHelpers
	{
        class FileType
        {
            public string typeName
            {
                get;
                private set;
            }
            public System.Type objectType
            {
                get;
                private set;
            }
            public string defaultPath;
            public string requiredExtn;
            public string defaultName;

            public FileType(string n, System.Type t, string dp, string dn, string re)
            {
                typeName = n;
                objectType = t;
                defaultPath = dp;
                requiredExtn = re;
                defaultName = dn;
            }

            public virtual string Validate(Object obj)
            {
                string result = string.Empty;
                if (obj.GetType() != objectType && false == obj.GetType().IsSubclassOf(objectType))
                {
                    result = "Not a " + objectType.ToString() + " (is a " + obj.GetType().ToString() + ")";
                }
                return result;
            }
        }


        public class BackgroundColourSetter : System.IDisposable
        {
            private Color oldBgColor;
            public BackgroundColourSetter(Color c)
            {
                oldBgColor = GUI.backgroundColor;
                GUI.backgroundColor = c;
            }

            public void Dispose()
            {
                GUI.backgroundColor = oldBgColor;
            }
        }

        static public void FixedWidthLabel(string s, int width)
        {
            GUILayout.Label(s, GUILayout.Width(width));
        }

        public class ScrollBoxPos
        {
            public Vector2 pos = Vector2.zero;
        }

        public class ScrollBox : System.IDisposable
        {
            private ScrollBoxPos _scrollPos;
            private Rect _rect;
            
            static public ScrollBox Simple(Rect r, ScrollBoxPos p)
            {
                ScrollBox s = new ScrollBox();
                s._scrollPos = p;
                s._rect = r;
                s.Init();
                return s;
            }

            private void Init()
            {
                GUILayout.BeginArea(_rect);
                _scrollPos.pos = GUILayout.BeginScrollView(_scrollPos.pos);
            }

            public void Dispose()
            {
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }

        }

        public class HorizontalGroup : System.IDisposable
		{
			private int _space = 0;
			private bool _line = false;
			private string _style = string.Empty;

			static public HorizontalGroup Simple(string st)
			{
				return new HorizontalGroup (0, false, st);
			}

			static public HorizontalGroup Simple()
			{
				return Simple ("");
			}

			static public HorizontalGroup Spaced(int s, string st)
			{
				return new HorizontalGroup (s, false, st);
			}

			static public HorizontalGroup Spaced(int s)
			{
				return Spaced (s, "");
			}

			static public HorizontalGroup SpacedWithLine(int s, string st)
			{
				return new HorizontalGroup (s, true, st);
			}

			static public HorizontalGroup SpacedWithLine(int s)
			{
				return SpacedWithLine (s, "");
			}

			static public HorizontalGroup WithLine(string st)
			{
				return new HorizontalGroup (0, true, st);
			}
			static public HorizontalGroup WithLine()
			{
				return SpacedWithLine (0, "");
			}

			private HorizontalGroup(int s, bool l, string st) 
			{
				Init(s, l, st);
			}

			private void Init(int s, bool l, string st)
			{
				_space = s;
				_line = l;
				_style = st;
				if (_line)
					HorizontalLine ();
				if (_space > 0)
					GUILayout.Space (_space);
				if (_style.Length > 0)
				{
					GUILayout.BeginHorizontal(_style); 
				}
				else
				{
					GUILayout.BeginHorizontal(); 
				}
			}

			public void Dispose() 
			{
				GUILayout.EndHorizontal (); 
				if (_space > 0)
					GUILayout.Space (_space);
			}
		}

		public class VerticalGroup : System.IDisposable
		{
			private bool _line = false;
			private string _style = string.Empty;

			static public VerticalGroup Simple()
			{
				return new VerticalGroup (false, "");
			}

			static public VerticalGroup WithLine()
			{
				return new VerticalGroup (true, "");
			}

			static public VerticalGroup Box()
			{
				return new VerticalGroup (false, "box");
			}

			static public VerticalGroup BoxWithLine()
			{
				return new VerticalGroup (true, "box");
			}

			private VerticalGroup(bool l, string s) { Init(l,s); }

			private void Init(bool l, string s)
			{
				_line = l;
				_style = s;

				if (_style.Length > 0)
				{
					GUILayout.BeginVertical (_style);
					if (_line)
					{
						HorizontalLine ();
					}
				}
				else
				{
					GUILayout.BeginVertical ();
				}
			}
			public void Dispose() {GUILayout.EndVertical (); }
		}

		public class Area : System.IDisposable
		{
			Rect _rect;

			public static Area Simple(Rect r)
			{
				return new Area (r);
			}
			private Area(Rect r) 
			{ 
				Init(r);
			}
			private void Init(Rect r)
			{
				_rect = r;
				GUILayout.BeginArea(_rect); 
			}
			public void Dispose() {GUILayout.EndArea (); }
		}

		static public void SpacedHorizontalLine(int space)
		{
			GUILayout.Space(space);
			HorizontalLine ();
			GUILayout.Space(space);
		}

		static public void HorizontalLine()
		{
			GUILayout.Box("", new GUILayoutOption[] {GUILayout.ExpandWidth(true), GUILayout.Height(1)});
		}
	}
}
