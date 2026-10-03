using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace IRG.Editor
{
    public class MainToolbar
    {
        [MainToolbarElement("IRG/Delayed Pause", defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement DelayedPauseButton()
        {
            var icon = EditorGUIUtility.IconContent("PauseButton").image as Texture2D;
            var content = new MainToolbarContent(icon, "Pause 3 seconds later");
            return new MainToolbarButton(content, () =>
            {
                DelayPauseAsync(3);
            });
        }

        private static async void DelayPauseAsync(float delay)
        {
            await Awaitable.WaitForSecondsAsync(delay);
            EditorApplication.isPaused = true;
        }
    }
}