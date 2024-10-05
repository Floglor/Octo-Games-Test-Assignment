using Naninovel;
using Naninovel.Commands;
using UnityEngine;

namespace CardsMemoryGame.Scripts
{
    [CommandAlias("minigame")]
    public class SwitchToMinigame : Command
    {
        public IntegerParameter CardsNumber;
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            IInputManager inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = false;
            
            IScriptPlayer scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();
            
            HidePrinter hidePrinter = new HidePrinter();
            hidePrinter.ExecuteAsync(asyncToken).Forget();
            
            if (Assigned(CardsNumber))
            {
                CardGameMaster gameMaster = GameObject.Find("Board").GetComponent<CardGameMaster>();
                gameMaster.StartGame(CardsNumber.Value);
            }
            return default;
        }
    }
}