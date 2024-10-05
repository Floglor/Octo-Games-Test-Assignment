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
                variableManager.TrySetVariableValue("MinigameTimeTaken", Time);
                variableManager.TrySetVariableValue("MinigameMistakes", Mistakes);

                await scriptPlayer.PreloadAndPlayAsync(ScriptName);
            }

            // 4. Enable Naninovel input.
            IInputManager inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = true;
        }
    }
}