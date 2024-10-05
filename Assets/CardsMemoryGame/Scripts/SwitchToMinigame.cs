using Naninovel;
using Naninovel.Commands;

namespace CardsMemoryGame.Scripts
{
    [CommandAlias("minigame")]
    public class SwitchToMinigame : Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            IInputManager inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = false;
            
            IScriptPlayer scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();
            
            HidePrinter hidePrinter = new HidePrinter();
            hidePrinter.ExecuteAsync(asyncToken).Forget();

            return default;
        }
    }
}