﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeldaEditor.Scripting;
using ZeldaOracle.Common.Scripting;
using ZeldaOracle.Game.Control.Scripting;
using ZeldaOracle.Game.Tiles;

namespace ZeldaEditor.PropertiesEditor.CustomEditors {
	
	// TODO: update this to reference hidden scripts.
	public class ScriptPropertyEditor : FormPropertyEditor {

		private bool wasGenerated;
		private Script script;

		public override Form CreateForm(object value) {
			string scriptName = (string) value;

			script = null;
			wasGenerated = false;

			// Open the script this property is referencing.
			if (scriptName.Length > 0)
				script = EditorControl.World.GetScript(scriptName);

			// If it is null, create a new hidden script.
			if (script == null) {
				script = EditorControl.GenerateInternalScript();
				wasGenerated = true;
				
				if (PropertyGrid.PropertyObject is IEventObject) {
					Event e = ((IEventObject) PropertyGrid.PropertyObject).Events.GetEvent(property.Name);
					if (e != null)
						script.Parameters = e.Parameters;
				}
			}

			// Open the scrpit editor form.
			ScriptEditor form = new ScriptEditor(script, EditorControl);
			return form;
		}

		public override object OnResultOkay(Form form, object value) {
			return (form as ScriptEditor).Script.ID;
		}

		public override object OnResultCancel(Form form, object value) {
			// If the script was newly generated, delete it completely.
			if (wasGenerated) {
				EditorControl.World.RemoveScript(script);
				return "";
			}
			return value;
		}
	}
}
