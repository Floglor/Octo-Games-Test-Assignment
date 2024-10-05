using Naninovel;

namespace CardsMemoryGame.Scripts
{
    [CommandAlias("novel")]
    public class SwitchToNovel : Command
    {
        public StringParameter ScriptName;
        public IntegerParameter Mistakes;
        public IntegerParameter Time;


        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            // 3. Load and play specified script (is required).
            if (Assigned(ScriptName))
            {
                IScriptPlayer scriptPlayer = Engine.GetService<IScriptPlayer>();
                ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>();
                int minigameCards;
                variableManager.TryGetVariableValue<int>("MinigameCards", out minigameCards);
                variableManager.TrySetVariableValue<int>("MinigameTimeTaken", Time);
                variableManager.TrySetVariableValue<int>("MinigameMistakes", Mistakes);
                variableManager.TrySetVariableValue<int>("MinigameScore",
                    minigameCards * 600 / (Time / 6 + 6 * (Mistakes + 1)));
                await scriptPlayer.PreloadAndPlayAsync(ScriptName);
            }

            // 4. Enable Naninovel input.
            IInputManager inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = true;
        }
    }
}