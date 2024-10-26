using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

namespace Dialogue
{
    public class DialogueVariables
    {
        public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

        private readonly Story globalVariablesStory;
        private const string saveVariablesKey = "INK_VARIABLES";

        public DialogueVariables(TextAsset loadGlobalsJSON) 
        {
            // create the story
            globalVariablesStory = new Story(loadGlobalsJSON.text);
            // to load the save data
            if (PlayerPrefs.HasKey(saveVariablesKey))
            {
                string jsonState = PlayerPrefs.GetString(saveVariablesKey);
                globalVariablesStory.state.LoadJson(jsonState);
            }

            // initialize the dictionary
            variables = new Dictionary<string, Ink.Runtime.Object>();
            foreach (string name in globalVariablesStory.variablesState)
            {
                Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
                variables.Add(name, value);
                Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
            }
        }

        public void SaveVariables() 
        {
            if (globalVariablesStory != null) 
            {
                // Load the current state of all of our variables to the globals story
                VariablesToStory(globalVariablesStory);
                PlayerPrefs.SetString(saveVariablesKey, globalVariablesStory.state.ToJson());
            }
        }

        public void StartListening(Story story) 
        {
            VariablesToStory(story);
            story.variablesState.variableChangedEvent += VariableChanged;
            Debug.Log("Story Listening");
        }

        public void StopListening(Story story) 
        {
            story.variablesState.variableChangedEvent -= VariableChanged;
            Debug.Log("Stop Listening");
        }

        private void VariableChanged(string name, Ink.Runtime.Object value) 
        {
            Debug.Log("Variable changed: " + name + " = " + value);
            if (variables.ContainsKey(name)) 
            {
                variables.Remove(name);
                variables.Add(name, value);
            }
        }

        private void VariablesToStory(Story story) 
        {
            foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables) 
            {
                story.variablesState.SetGlobal(variable.Key, variable.Value);
                Debug.Log("Goes to Variable Story");
            }
        }
    }
}
