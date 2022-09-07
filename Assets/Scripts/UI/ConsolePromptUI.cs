using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsolePrompt
{
    public class Command
    {
        public readonly string name;
        private readonly List<CommandOption> options;

        public Command(
            string name,
            List<CommandOption> options
        )
        {
            this.name = name;
            this.options = options;
        }

        public CommandOption FindOption(string optValue)
        {
            return options.Find(opt => opt.value == optValue);
        }
    }

    public class CommandOption
    {
        public readonly string value;
        public readonly System.Action action;

        public CommandOption(
            string value,
            System.Action action
        )
        {
            this.value = value;
            this.action = action;
        }
    }

    private List<Command> commands = new List<Command>();

    public void AddCommand(string name, List<CommandOption> options)
    {
        var cmd = new Command(
            name: name,
            options: options
        );

        commands.Add(cmd);
    }

    public Command FindCommand(string cmdName)
    {
        return commands.Find(cmd => cmd.name == cmdName);
    }
}

// @todo arrow UP/DOWN to see history
// @todo autocomplete based on available commands
public class ConsolePromptUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private ConsolePrompt consolePrompt;

    public void Setup(ConsolePrompt consolePrompt)
    {
        this.consolePrompt = consolePrompt;

        inputField.onSubmit.AddListener(OnInputFieldSubmit);
    }

    public bool IsOpen()
    {
        return gameObject.activeSelf;
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (IsOpen())
        {
            FocusField();
        }
    }

    void FocusField()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }

    void OnInputFieldSubmit(string value)
    {
        // Debug.Log($"submitted: {value}");
        var res = ProcessMessage(value);

        if (res != "") Debug.Log($"prompt response: {res}");

        FocusField();
    }

    string ProcessMessage(string text)
    {
        // System.String
        var words = text.Trim().Split(" "[0]);
        if (words.Length == 0) return "";

        var cmd = consolePrompt.FindCommand(words[0]);

        if (cmd == null)
        {
            return $"command not found: \"{words[0]}\"";
        }
        else
        {
            if (words.Length == 1)
            {
                return "";
            }

            var opt = cmd.FindOption(words[1]);

            if (opt == null)
            {
                return $"command \"{words[0]}\": option not found: \"{words[1]}\"";
            }
            else
            {
                opt.action();

                return "";
            }
        }

        // if (words[0] == "cam")
        // {
        //     var camType = words[1];

        //     switch (words[1])
        //     {
        //         case "1":
        //             break;
        //         case "2":
        //             break;
        //         case "3":
        //             break;
        //         default:
        //             break;

        //     }
        // }
    }
}
