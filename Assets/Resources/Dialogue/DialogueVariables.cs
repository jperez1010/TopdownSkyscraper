using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set;  }

    public DialogueVariables(string globalsFilePath)
    {
        string inkFileContents = File.ReadAllText(globalsFilePath);
        Ink.Compiler compiler = new Ink.Compiler(inkFileContents);
        Story globalVariablesStory = compiler.Compile();

        //Initialize Dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach(string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
        }
    }
    public void VariableChanged(string name, Ink.Runtime.Object value)
    {
        // only maintains variables that were initialized from the globals ink file
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }
    public void StartListening(Story story)
    {
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }
    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
